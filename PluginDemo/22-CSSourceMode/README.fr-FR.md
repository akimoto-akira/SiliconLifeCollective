# Démo du Mode de Chargement par Compilation de Code Source CS

Un plugin chargé à partir de fichiers source `.cs` bruts au lieu d'une DLL précompilée, démontrant le mode de compilation de code source CS de PluginLoader (introduit par task-389).

## Fonctionnement du Mode Code Source CS

Lorsque PluginLoader scanne un répertoire de plugin et ne trouve **aucune DLL**, il passe automatiquement en mode code source CS :

```
1. PluginLoader scanne le répertoire → aucune DLL
2. Passe en mode code source CS
3. cs.txt trouvé → lecture ligne par ligne, chargement uniquement des fichiers .cs listés
   (Pas de cs.txt → chargement de tous les fichiers *.cs du répertoire)
4. Scan des DLL voisines → DLLs de confiance ajoutées directement comme références ;
   les DLLs non fiables doivent passer ScanForbiddenReferences
5. CompilationCore (mode restreint) compile les fichiers .cs en DLL en mémoire
6. Les octets de la DLL en mémoire sont écrits dans un fichier temporaire pour le scan ScanForbiddenReferences
7. Scan réussi → réflexion trouve l'implémentation IPlugin → instanciation
8. Journal affiche : "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — Liste Blanche de Chargement Sélectif

Le fichier `cs.txt` spécifie quels fichiers `.cs` compiler, un nom de fichier par ligne :

```
Plugin.cs
```

- **Fichiers listés** : Compilés et chargés (ex: `Plugin.cs`)
- **Fichiers non listés** : Ignorés par le compilateur (ex: `Helpers.cs`)
- **Lignes commençant par `#`** : Traitées comme commentaires
- **Lignes vides** : Ignorées
- **Pas de cs.txt** : Tous les fichiers `*.cs` du répertoire sont chargés

## Mode Code Source CS vs Mode DLL

| Aspect | Mode DLL | Mode Code Source CS |
|--------|----------|-------------------|
| Format du plugin | DLL précompilée `.dll` | Fichiers source `.cs` bruts |
| Déclencheur de chargement | DLL trouvée dans le répertoire | Pas de DLL, fichiers `.cs` présents |
| Compilation | À la compilation | Au chargement par PluginLoader |
| Performance | Pas de surcharge de compilation | Surcharge de compilation Roslyn au démarrage |
| Scan de sécurité | Scan direct des métadonnées PE | Compilation → DLL temporaire → Scan métadonnées PE |
| Préfixe de journal | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| Recommandé pour | Déploiement en production | Itération de développement |

## Gestion des Erreurs

| Scénario | Comportement |
|----------|-------------|
| Pas de DLL, pas de fichiers .cs | Avertissement : "No DLL and no CS source files found" |
| Erreurs de compilation | Erreur : Messages de diagnostic détaillés journalisés |
| Échec du scan de sécurité | Erreur : Toutes les violations listées, plugin rejeté |
| Entrée cs.txt introuvable | Avertissement : "cs.txt entry not found or not a .cs file" |
| Échec du scan de DLL voisine | Avertissement : DLL non ajoutée comme référence, compilation continue |

## Note de Sécurité

Les plugins en mode code source CS subissent le **même scan de sécurité** que les plugins en mode DLL. L'assembly compilé est écrit dans un fichier DLL temporaire et scanné avec `ScanForbiddenReferences` — le même scan que celui appliqué aux DLL précompilées. Toutes les règles d'espaces de noms/types/membres/chaînes interdits s'appliquent de manière identique.

Les plugins sont toujours chargés dans un contexte isolé et scannés pour les références d'espaces de noms interdits (ex: `System.IO`, `System.Net.Http`). Voir la [Documentation de Sécurité](../../docs/fr-FR/security.md) pour plus de détails.
