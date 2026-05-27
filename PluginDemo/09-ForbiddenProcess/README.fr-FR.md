# PluginDemo-09 : Anti-pattern des opérations de processus interdites

## Aperçu

Ce plugin démontre les opérations d'exécution de processus **interdites** dans le système de plugins SiliconLife. Il sert de référence anti-pattern, montrant ce qu'il ne faut PAS faire et fournissant des alternatives correctes pour chaque violation.

## Pourquoi les types Process sont-ils interdits ?

`System.Diagnostics.Process` et `ProcessStartInfo` sont bloqués dans les plugins car l'exécution directe de processus présente des risques de sécurité graves :

1. **Exécution de commandes arbitraires** : Les plugins pourraient exécuter n'importe quelle commande sans audit ni vérification de permissions
2. **Lancement de malware** : Des plugins malveillants pourraient exécuter des applications ou scripts indésirables
3. **Accès aux ressources système** : Les processus pourraient accéder à des ressources sensibles en dehors du bac à sable du plugin
4. **Pas de validation de commande** : Process.Start direct n'a pas de protection intégrée contre l'injection de commandes
5. **Pas de piste d'audit** : Les opérations directes de processus contournent le système d'audit de sécurité des plugins
6. **Escalade de privilèges** : Pourrait créer des processus avec des privilèges plus élevés que ceux du plugin

## Quels types sont interdits ?

Seuls les types liés à Process sont interdits, **PAS l'ensemble de l'espace de noms System.Diagnostics** :

| Type interdit | Méthode bloquée | Niveau de risque |
|--------------|----------------|-----------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Critique |
| `ProcessStartInfo` | Constructeur, toutes les propriétés | 🔴 Critique |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Critique |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 Élevé |

## Quels types sont autorisés ?

Les autres types `System.Diagnostics` qui n'impliquent pas l'exécution de processus restent disponibles :

| Type autorisé | Utilisation | Pourquoi c'est sûr |
|--------------|------------|-------------------|
| `Stopwatch` | Mesure du temps | Pas d'exécution de processus |
| `Debug` | Sortie de débogage | Pas de risque de sécurité |
| `Trace` | Traçage/journalisation | Pas de risque de sécurité |
| `PerformanceCounter` | Surveillance des performances | Lecture seule, audité |

## Comment exécuter des commandes en toute sécurité ?

### Utiliser CommandLineExecutor (la seule méthode sûre)

`CommandLineExecutor` est le **point d'entrée contrôlé** pour l'exécution de commandes dans les plugins :

```csharp
// ✅ CORRECT : Exécuter une commande
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Erreur : {result.Error}");
}
```

**Ce que CommandLineExecutor fournit :**
1. **Protection contre l'injection de commandes** : Bloque les séparateurs dangereux (`||`, `&&`, `|`, `&`, `;`)
2. **Application du délai d'attente** : Délai par défaut de 30 secondes (configurable)
3. **Journalisation d'audit** : Toutes les exécutions de commandes sont enregistrées pour révision
4. **Capture de sortie** : Capture automatique de stdout et stderr
5. **Support multiplateforme** : Utilise `cmd.exe` sous Windows, `/bin/bash` sous Unix
6. **Gestion des erreurs** : Retourne un résultat structuré avec statut succès/échec

## Violations démontrées

Ce plugin montre 5 violations courantes d'exécution de processus :

### Violation 1 : Process.Start

```csharp
// ❌ INTERDIT
Process.Start("notepad.exe");

// ✅ CORRECT
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**TypeRef bloqué** : `System.Diagnostics.Process::Start(System.String)`

### Violation 2 : ProcessStartInfo

```csharp
// ❌ INTERDIT
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ CORRECT
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**TypeRef bloqué** : `System.Diagnostics.ProcessStartInfo::.ctor()`

### Violation 3 : Process avec arguments

```csharp
// ❌ INTERDIT
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ CORRECT
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**TypeRef bloqué** : `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Violation 4 : Redirection de sortie de processus

```csharp
// ❌ INTERDIT
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ CORRECT
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**TypeRef bloqué** : `System.Diagnostics.Process::StandardOutput`

### Violation 5 : Process.Kill

```csharp
// ❌ INTERDIT
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ CORRECT
// Pour des raisons de sécurité, CommandLineExecutor ne prend pas en charge la terminaison de processus.
// Contactez l'administrateur système si nécessaire.
```

**TypeRef bloqué** : `System.Diagnostics.Process::Kill()`

## Pourquoi seulement Process et non tout System.Diagnostics ?

Le système de plugins adopte une approche **chirurgicale** de la sécurité :

- **Bloquer uniquement les types dangereux** : Process/ProcessStartInfo permettent l'exécution de code arbitraire
- **Autoriser les types sûrs** : Stopwatch, Debug, Trace n'ont pas d'implications de sécurité
- **Minimiser l'impact** : Les développeurs peuvent continuer à utiliser des outils de diagnostic sans risque
- **Frontière claire** : Seuls les types capables de créer/tuer des processus sont interdits

## Mécanisme de sécurité PluginLoader

Lorsque PluginLoader analyse ce plugin :

1. **Scan TypeRef** : Détecte les références aux types interdits `Process`/`ProcessStartInfo`
2. **Scan MemberRef** : Détecte les appels aux méthodes bloquées (ex: `Process.Start`)
3. **Scan IL String** : Détecte les tentatives de réflexion basées sur les chaînes
4. **Rejet** : Le plugin est rejeté lors du chargement avec un message d'erreur détaillé

## Bonnes pratiques

1. **Toujours utiliser CommandLineExecutor** : Ne jamais utiliser `Process.Start` directement
2. **Définir des délais raisonnables** : Empêcher les commandes de bloquer indéfiniment
3. **Vérifier les résultats** : Toujours vérifier `result.Success` avant d'utiliser la sortie
4. **Assainir les entrées** : Ne jamais passer les entrées utilisateur directement aux commandes
5. **Déclarer Capability si nécessaire** : Si une exécution de processus sans restriction est nécessaire, déclarer `Capability.Process` (voir 15-CapabilityProcess)

## Fichiers

- `Plugin.cs` - Plugin de démonstration anti-pattern
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Ce fichier (Français)
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Exemples connexes

- **08-ForbiddenNetwork** : Opérations réseau interdites
- **15-CapabilityProcess** : Permission Process déclarative
- **10-ForbiddenReflection** : Opérations de réflexion interdites
- **12-ForbiddenStringBypass** : Tentatives de contournement par réflexion basée sur les chaînes
