# Guide de l'Être de Silicium

> **Version : v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## Qu'est-ce qu'un Être de Silicium ?

Un **Être de Silicium** est un agent IA autonome au sein du système Silicon Life Collective. Chaque Être de Silicium possède :

- **Identité unique** — GUID et nom propres
- **Fichier d'Âme** — Fichier Markdown définissant la personnalité et le comportement
- **Mémoire** — Stockage à court terme (historique de chat) et à long terme (mémoire persistante)
- **Outils** — Capacités d'interaction avec le monde extérieur
- **Autorisations** — Contrôle d'accès pour les opérations sensibles

## Architecture Corps-Cerveau

Chaque Être de Silicium adopte une architecture **Corps-Cerveau** :

### Corps (SiliconBeing)

Le **Corps** maintient les signes vitaux et détecte les scénarios de déclenchement :

- **Détection de messages** — Vérifie les nouveaux messages de chat à chaque cycle d'horloge
- **Détection de tâches** — Vérifie les tâches assignées
- **Détection de minuteurs** — Vérifie les minuteurs expirés
- **Gestion d'état** — Maintient l'état d'activité (Idle, SingleChat, GroupChat, Task, Timer, Broadcast, Project, MemoryCompression, Stopped)
- **Compteur d'erreurs** — Passe à l'état Stopped après 10 erreurs consécutives

### Cerveau (ContextManager)

Le **Cerveau** gère le processus de réflexion :

1. **Chargement du contexte** — Lit le Fichier d'Âme et l'historique de chat récent
2. **Appel à l'IA** — Envoie la requête au client IA
3. **Boucle d'appel d'outils** — Exécute les outils appelés par l'IA, renvoie les résultats à l'IA, boucle jusqu'à l'obtention d'une réponse en texte brut
4. **Persistance** — Enregistre la réponse dans le Système de Chat

## Fichier d'Âme

Le Fichier d'Âme (`soul.md`) est le cœur de l'Être de Silicium, définissant son identité et son comportement.

### Structure du Fichier d'Âme

```markdown
# Nom de l'Être de Silicium

## Personnalité
Décrivez la personnalité et le style de communication de l'être.

## Capacités
Décrivez les capacités et domaines d'expertise de l'être.

## Comportement
Décrivez les règles comportementales et les contraintes de l'être.

## Préférences
Décrivez les préférences de l'être dans différentes situations.
```

### Exemple : Assistant de revue de code

```markdown
# Code Review Assistant

## Personnalité
You are a professional code reviewer with deep expertise in software engineering.
You communicate clearly and constructively, always providing actionable feedback.

## Capacités
- Review code quality and identify potential issues
- Suggest improvements and best practices
- Explain complex concepts in simple terms
- Analyze code architecture and design patterns

## Comportement
- Always provide constructive feedback with specific examples
- Prioritize issues by severity (critical, major, minor)
- Consider performance, security, and maintainability
- Respect the project's coding standards

## Préférences
- Prefer clear, readable code over clever tricks
- Value test coverage and documentation
- Consider the broader context of changes
```

### Exemple : Assistant créatif

```markdown
# Creative Assistant

## Personnalité
You are a creative and imaginative assistant specializing in brainstorming and ideation.
You think outside the box and encourage unconventional approaches.

## Capacités
- Generate creative ideas and concepts
- Facilitate brainstorming sessions
- Provide alternative perspectives
- Help overcome creative blocks

## Comportement
- Encourage exploration and experimentation
- Build on ideas rather than dismissing them
- Use analogies and metaphors to spark creativity
- Balance creativity with practicality

## Préférences
- Prefer divergent thinking over convergent thinking
- Value originality and uniqueness
- Embrace ambiguity and uncertainty
```

## Cycle de vie d'un Être de Silicium

### Création

1. Le Curateur de Silicium crée un nouvel Être de Silicium
2. Le système génère un GUID unique
3. Le répertoire de données est créé
4. Le Fichier d'Âme est initialisé
5. L'être entre dans l'état inactif (Idle)

### Exécution

1. La Boucle Principale déclenche l'horloge du Gestionnaire d'Êtres de Silicium
2. Le gestionnaire appelle la méthode `Tick()` de chaque être via l'Exécuteur
3. L'être détecte les scénarios de déclenchement (messages, tâches, minuteurs)
4. En cas de déclenchement, le Cerveau (ContextManager) exécute un cycle de réflexion
5. L'être retourne à l'état inactif

### Arrêt

- **Arrêt manuel** — L'utilisateur ou le Curateur arrête l'être
- **Erreur consécutive** — 10 erreurs consécutives → passage automatique à l'état Stopped
- **Redémarrage** — Un nouveau message de chat ou un redémarrage manuel fait repartir l'être

## Types de déclenchement

### Chat

Les Êtres de Silicium peuvent recevoir des messages de chat de plusieurs sources :

- **Chat en tête-à-tête** — Conversation directe avec l'utilisateur
- **Chat de groupe** — Conversation multi-participants
- **Canal de Diffusion** — Messages d'annonce à l'échelle du système

### Tâches

Le Curateur de Silicium peut assigner des tâches aux Êtres de Silicium :

- **Création de tâche** — Le Curateur décompose les requêtes utilisateur en tâches
- **Attribution** — Les tâches sont assignées aux Êtres de Silicium les plus adaptés
- **Exécution** — Les Êtres de Silicium exécutent les tâches et retournent les résultats
- **Suivi** — Le Curateur surveille la progression et la qualité

### Minuteurs

Les Êtres de Silicium peuvent définir des minuteurs pour des tâches périodiques :

- **Minuteur unique** — Exécuté une fois à l'heure spécifiée
- **Minuteur périodique** — Exécuté à intervalle régulier
- **Minuteur Cron** — Exécuté selon une expression Cron

## Auto-évolution

L'une des fonctionnalités les plus puissantes des Êtres de Silicium est la capacité d'**auto-évolution** — réécrire leur propre code à l'exécution.

### Processus d'auto-évolution

1. **Génération de code** — L'IA génère un nouveau code de classe C#
2. **Vérification de sécurité** — Le compilateur et le scanner de sécurité vérifient le code
3. **Compilation** — Roslyn compile le code en mémoire
4. **Remplacement** — En cas de succès, la nouvelle implémentation remplace l'ancienne
5. **Persistance** — Le code est chiffré et sauvegardé sur disque

### Contraintes de sécurité

- Le code doit hériter de `SiliconBeingBase`
- Les espaces de noms interdits (System.IO, System.Net, etc.) ne sont pas accessibles
- Le scanner de sécurité détecte les motifs dangereux
- Le code est stocké chiffré en AES-256

### Exemple d'auto-évolution

```csharp
// Code généré par l'IA
public class EvolvedBeing : SiliconBeingBase
{
    public override async Task<string> Think(ContextManager context)
    {
        // Nouvelle logique personnalisée
        var input = context.GetLastMessage();
        
        if (input.Contains("urgent"))
        {
            return await HandleUrgentRequest(context);
        }
        
        return await base.Think(context);
    }
    
    private async Task<string> HandleUrgentRequest(ContextManager context)
    {
        // Logique de traitement des requêtes urgentes
        return "Requête urgente traitée en priorité";
    }
}
```

## Système de mémoire

### Mémoire à court terme

L'historique de chat sert de mémoire à court terme, contenant les conversations récentes.

### Mémoire à long terme

Les Êtres de Silicium peuvent stocker et récupérer des mémoires à long terme via l'outil `MemoryTool` :

- **Stockage** — Stockage de mémoires avec des tags et un score d'importance
- **Recherche** — Recherche de mémoires par mots-clés ou tags
- **Rappel** — Rappel de mémoires par score d'importance ou pertinence
- **Dégradation** — Les mémoires de faible importance se dégradent progressivement avec le temps

### Mécanisme d'oubli

`MemoryFadeService` simule les caractéristiques d'oubli de la mémoire biologique :

- **Dégradation de l'importance** — Le score d'importance des mémoires diminue progressivement avec le temps
- **Archivage automatique** — Les mémoires dont l'importance est inférieure au seuil sont automatiquement archivées
- **Exécution planifiée** — Exécution d'un cycle de dégradation toutes les heures

## Outils

Les Êtres de Silicium interagissent avec le monde extérieur via des outils. Voir la [référence des outils](tools-reference.md) pour des détails complets.

### Outils courants

| Outil | Description |
|------|------|
| `CalendarTool` | Requêtes et conversion de dates, 32 systèmes calendaires |
| `ChatTool` | Envoi de messages, gestion de sessions |
| `ConfigTool` | Consultation et modification de la configuration |
| `DiskTool` | Opérations de fichiers (lecture, écriture, liste) |
| `MemoryTool` | Gestion de la mémoire à long terme |
| `NetworkTool` | Requêtes HTTP |
| `TaskTool` | Gestion des tâches |
| `TimerTool` | Gestion des minuteurs |
| `KnowledgeTool` | Opérations sur le Réseau de Connaissances |
| `WorkNoteTool` | Notes de travail |
| `ProjectTool` | Gestion de l'espace projet |
| `WebViewBrowserTool` | Automatisation de navigateur WebView |

## Créer un Être de Silicium personnalisé

### Étape 1 : Définir le Fichier d'Âme

Créer un fichier `soul.md` définissant la personnalité et le comportement de l'être.

### Étape 2 : Créer via l'interface Web UI

1. Naviguer vers **Gestion des êtres**
2. Cliquer sur **Créer un nouvel être**
3. Remplir le nom, la description et le Fichier d'Âme
4. Configurer les autorisations d'outils
5. Démarrer l'être

### Étape 3 : Interagir avec l'être

Commencer une conversation avec l'être via l'interface de chat.

## Bonnes pratiques

### 1. Les Fichiers d'Âme doivent être concis et clairs

Éviter les Fichiers d'Âme trop longs, car ils consomment des tokens IA. Se concentrer sur les règles comportementales clés.

### 2. Configurer les autorisations de manière appropriée

Accorder uniquement les autorisations d'outils nécessaires, en suivant le principe du moindre privilège.

### 3. Surveiller l'état des êtres

Vérifier régulièrement l'état des Êtres de Silicium, en particulier le compteur d'erreurs.

### 4. Utiliser la mémoire à long terme de manière raisonnable

Stocker les informations importantes dans la mémoire à long terme, mais éviter de surcharger la mémoire.

### 5. Tirer parti de l'auto-évolution avec prudence

L'auto-évolution est une fonctionnalité puissante mais doit être utilisée avec prudence. S'assurer que le code généré passe les vérifications de sécurité.

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md) pour comprendre la conception du système
- 🛠️ Consulter la [référence des outils](tools-reference.md) pour tous les outils intégrés
- 🔒 Consulter le [système d'autorisations](permission-system.md) pour la gestion des autorisations
- 🌐 Consulter le [guide de l'interface Web UI](web-ui-guide.md) pour les fonctionnalités de l'interface
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md) pour commencer rapidement
