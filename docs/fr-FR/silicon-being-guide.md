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

### États d'activité

Les Êtres de Silicium possèdent les états d'activité suivants :

| État | Description |
|------|------|
| `Idle` | État inactif, en attente de déclenchement par l'horloge |
| `SingleChat` | Conversation en tête-à-tête en cours |
| `GroupChat` | Conversation de groupe en cours |
| `Task` | Exécution d'une tâche en cours |
| `Timer` | Exécution d'un minuteur en cours |
| `Broadcast` | Traitement d'un message de diffusion en cours |
| `Project` | Travail de projet en cours |
| `MemoryCompression` | Compression de mémoire en cours |
| `Stopped` | Arrêté, en raison d'erreurs consécutives ou d'un arrêt manuel |

**Mécanisme de l'état Stopped** :
- Lorsqu'un Être de Silicium rencontre 10 erreurs consécutives, il passe automatiquement à l'état `Stopped`
- Après être entré dans l'état Stopped, l'être n'exécutera plus aucune tâche
- Lorsqu'un nouveau message de chat arrive, le compteur d'erreurs est réinitialisé et l'être reprend son fonctionnement
- Il peut également être redémarré par intervention manuelle

### Transitions d'état

```
Idle → SingleChat → Idle (chat terminé)
Idle → GroupChat → Idle (chat de groupe terminé)
Idle → Task → Idle (tâche terminée)
Idle → Timer → Idle (minuteur terminé)
Idle → Broadcast → Idle (diffusion terminée)
Idle → Project → Idle (travail de projet terminé)
Idle → MemoryCompression → Idle (compression de mémoire terminée)
Quelconque → Stopped (10 erreurs consécutives)
Stopped → Idle (nouveau message de chat ou redémarrage manuel)
```

### Opérations

- **Démarrage** : Initialisation et début du traitement
- **Arrêt** : Fermeture progressive
- **Redémarrage** : Reprise de l'état Stopped vers l'état Idle

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

## Système de Tâches

### Création de tâche

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Statuts de tâche

- `Pending` - En attente d'exécution
- `Running` - En cours d'exécution
- `SubmittedForReview` - Soumis pour révision
- `UnderReview` - En cours de révision
- `Rework` - Retour pour modification
- `Completed` - Terminé avec succès
- `Failed` - Échec de l'exécution
- `Cancelled` - Annulé manuellement

## Système de Minuteries

### Types de minuteurs

1. **Unique** : Exécuté une fois après un délai
2. **Intervalle** : Exécuté à intervalle fixe de manière répétée
3. **Cron** : Exécuté selon une expression cron

### Exemple

```csharp
// Exécuter toutes les heures
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

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

## Système de Compétences

Les compétences (Skill) sont des unités de capacité réutilisables des Êtres de Silicium — elles encapsulent « l'orchestration d'outils + modèle de prompt » en une fonction déclarable, évolutive et auto-planifiable. L'IA appelle une compétence comme elle appellerait un outil ordinaire.

### Structure des compétences

| Élément | Description |
|------|------|
| `id` / `description` | Identifiant unique et description en une phrase (présentée à l'IA, détermine quand l'IA choisit cette compétence) |
| `parameter_schema` | JSON Schema des paramètres, déclarant chaque placeholder `{param}` utilisé dans le prompt |
| `system_prompt_template` | Modèle de prompt système, les placeholders sont remplis par les paramètres lors de l'exécution |
| `tool_whitelist` | Liste des outils autorisés pendant l'exécution (vide = hérite de tous les outils de l'être) |
| `max_tool_round` / `timeout` | Limites de tours d'outils et de délai d'attente (limitées par les plafonds globaux) |
| `on_complete` | Action de fin : `none` / `write_memory` / `notify_curator` / `broadcast` |
| `trigger_mode` | `Manual` (l'IA appelle autonomement) ou `Auto` + planification `schedule` |

### Quatre sources

- **Intégré** — Intégré au framework (`summarize_document` résumé de document, `code_review` revue de code, `research_topic` recherche de sujet)
- **Plugin** — Enregistré via `ISkillProvider`
- **Être** — Créé par l'être à l'exécution via l'outil `skill`
- **Utilisateur** — Créé par l'utilisateur via la page de gestion des compétences de l'interface Web UI

### Modes de déclenchement

1. **Manuel (Manual)** : La compétence est injectée dans la requête IA comme une définition d'outil ordinaire, l'IA décide quand l'appeler ; le planificateur priorise le routage des appels homonymes vers la compétence
2. **Automatique (Auto + schedule)** : L'expression de planification est stockée dans `metadata.schedule`, supportant trois formats :
   - `"09:30"` — Heure fixe quotidienne
   - `"6h"` / `"30 m"` / `"2 d"` — Période d'intervalle
   - `"0 9 * * *"` / `"*/15 * * * *"` — Sous-ensemble cron

### Écriture en Markdown

Les compétences sont stockées en Markdown (`skills/{id}.md`, métadonnées YAML frontmatter + corps du prompt) :

```markdown
---
id: daily_news_digest
description: Rechercher les actualités technologiques du jour et générer un résumé
tool_whitelist: [network, work_note]
on_complete: write_memory
---

Veuillez utiliser l'outil network pour rechercher les dernières actualités sur {topic}, générer un résumé de 500 mots et l'enregistrer dans les notes de travail.
```

Il est également possible d'écrire uniquement le corps (en omettant le YAML) : lors de l'enregistrement, l'IA complétera automatiquement les métadonnées telles que id, description, schema de paramètres — les champs déjà remplis par l'utilisateur ne sont jamais écrasés.

### Auto-gestion par l'être

L'être peut gérer sa propre bibliothèque de compétences via l'outil `skill` :

```json
{ "action": "list" }
{ "action": "create", "id": "my_skill", "system_prompt": "...", "description": "..." }
{ "action": "update_from_md", "skill_id": "my_skill", "markdown": "..." }
{ "action": "delete", "skill_id": "my_skill" }
```

### Rechargement à chaud et évolution

- L'être détecte les modifications du répertoire `skills/` toutes les 30 secondes (comparaison d'empreinte), les modifications de l'interface Web UI ou d'autres êtres prennent effet automatiquement, sans redémarrage
- Chaque mise à jour de compétence archive automatiquement la version historique dans `skills/archive/{id}/{version}.md`, formant l'historique d'évolution des compétences
- Le nombre de compétences personnalisées est limité par un quota (`MaxCustomSkillsPerBeing`, 50 par défaut)

### Garde-fous d'exécution

- Autorisation d'action `execute` au niveau de la compétence (peut être désactivée par la matrice d'autorisations, invisible pour l'IA une fois désactivée)
- Les paramètres d'exécution sont limités par les plafonds globaux : tours ≤ `GlobalMaxToolRound` (10 par défaut), délai ≤ `GlobalSkillTimeoutSeconds` (300 secondes par défaut)
- Une compétence ne peut pas s'appeler elle-même récursivement
- Les appels d'outils hors liste blanche échouent directement

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

### Structure de stockage

Version Default :
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Version Fast (stockage SpeedyPack) :
```
data/
├── speedy/
│   ├── {being-id}.spk       # Fichier de stockage SpeedyPack
│   └── {being-id}.spk.idx   # Fichier d'index
└── beings/
    └── {being-id}/
        └── soul.md
```

## Système de Notes de Travail

### Aperçu

Les notes de travail sont le système de journal personnel de l'Être de Silicium, adoptant une conception par pages, utilisé pour enregistrer les progrès de travail, les notes d'apprentissage, les notes de projet, etc.

### Caractéristiques

- **Gestion par pages** : Chaque note est une page indépendante, accessible par numéro de page
- **Support Markdown** : Le contenu supporte le format Markdown (texte, listes, tableaux, blocs de code)
- **Index par mots-clés** : Possibilité d'ajouter des mots-clés aux notes pour faciliter la recherche
- **Fonction de résumé** : Chaque note a un bref résumé pour une consultation rapide
- **Génération de table des matières** : Possibilité de générer un aperçu de la table des matières de toutes les notes pour comprendre le contexte global
- **Horodatage** : Enregistrement automatique des heures de création et de mise à jour
- **Privé par défaut** : Seul l'être lui-même peut y accéder (le Curateur de Silicium peut les gérer)

### Scénarios d'utilisation

1. **Enregistrement des progrès de projet**
   ```
   Résumé : Module d'authentification utilisateur terminé
   Contenu : Implémentation de la vérification JWT token, intégration OAuth2, mécanisme de refresh token
   Mots-clés : authentification,JWT,OAuth2
   ```

2. **Notes d'apprentissage**
   ```
   Résumé : Apprentissage des meilleures pratiques de programmation asynchrone C#
   Contenu : Précautions d'utilisation d'async/await, scénarios d'utilisation de ConfigureAwait...
   Mots-clés : C#,asynchrone,meilleures pratiques
   ```

3. **Compte-rendu de réunion**
   ```
   Résumé : Discussion sur les besoins produit
   Contenu : Discussion des besoins de nouvelles fonctionnalités, détermination du plan d'implémentation...
   Mots-clés : produit,besoin,réunion
   ```

### Utilisation via outil

L'être peut gérer les notes de travail via l'outil `work_note` :

```json
// Créer une note
{
  "action": "create",
  "summary": "Module d'authentification utilisateur terminé",
  "content": "## Détails d'implémentation\n\n- Utilisation de JWT token\n- Support OAuth2",
  "keywords": "authentification,JWT,OAuth2"
}

// Lire une note
{
  "action": "read",
  "page_number": 1
}

// Rechercher des notes
{
  "action": "search",
  "keyword": "authentification",
  "max_results": 10
}
```

### Gestion via l'interface Web UI

1. Naviguer vers **Gestion des êtres** → sélectionner un être
2. Cliquer sur l'onglet **Notes de travail**
3. Consulter, rechercher et éditer des notes
4. Support de l'aperçu Markdown

## Système de Réseau de Connaissances

### Aperçu

Le réseau de connaissances est un système de représentation et de gestion des connaissances basé sur une structure triplet (sujet-prédicat-objet), utilisé pour stocker et gérer des connaissances structurées.

### Concepts clés

#### Structure triplet

```
Sujet (Subject) --Prédicat (Predicate)--> Objet (Object)
```

**Exemples** :
- `Python` --`is_a`--> `programming_language`
- `Pékin` --`capital_of`--> `Chine`
- `eau` --`boiling_point`--> `100°C`

#### Degré de confiance

Chaque triplet de connaissance a un score de confiance (0.0-1.0), indiquant le niveau de fiabilité de la connaissance :
- `1.0` : Absolument certain (ex. théorème mathématique)
- `0.8-0.99` : Hautement fiable (ex. fait vérifié)
- `0.5-0.79` : Fiable modérément (ex. inférence ou hypothèse)
- `<0.5` : Faible fiabilité (ex. supposition ou information non vérifiée)

#### Système de balises

Supporte l'ajout de balises aux triplets pour faciliter la classification et la recherche :
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Opérations de connaissance

#### 1. Ajouter une connaissance

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. Interroger des connaissances

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Rechercher des connaissances

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Découvrir un chemin de connaissance

Trouver le chemin de relation entre deux concepts :
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Résultat :
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validation de connaissance

Vérifier la validité et la cohérence des connaissances :
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Statistiques de connaissance

Obtenir les informations statistiques globales du réseau de connaissances :
```json
{
  "action": "stats"
}
```

Résultat :
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Scénarios d'utilisation

1. **Stockage de faits**
   - Stocker des faits objectifs et des connaissances communes
   - Exemple : `Terre` --`is_a`--> `planète`

2. **Relations entre concepts**
   - Enregistrer les relations entre concepts
   - Exemple : `héritage` --`is_a`--> `concept de programmation orientée objet`

3. **Accumulation d'apprentissage**
   - L'être accumule continuellement des connaissances par l'apprentissage
   - Forme un système de connaissances structuré

4. **Support au raisonnement**
   - Découverte de relations indirectes par les chemins de connaissance
   - Supporte le raisonnement et la prise de décision basés sur les connaissances

### Gestion via l'interface Web UI

1. Naviguer vers la page **Réseau de connaissances**
2. Consulter les informations statistiques des connaissances
3. Rechercher et parcourir les connaissances
4. Visualiser le graphe de relations de connaissances (planifié)

## Opérations du Navigateur WebView (Nouveau)

### Aperçu

L'Être de Silicium peut naviguer sur le Web, obtenir des informations et exécuter des opérations Web de manière autonome via l'outil de navigateur WebView. Le navigateur fonctionne en mode headless, totalement invisible pour l'utilisateur.

### Caractéristiques

- **Isolation individuelle** : Chaque être possède une instance de navigateur indépendante, des cookies et des sessions
- **Mode headless** : Opération autonome en arrière-plan, invisible pour l'utilisateur
- **Fonctionnalités complètes** : Supporte l'exécution JavaScript, le rendu CSS, le remplissage de formulaires, etc.
- **Contrôle de sécurité** : Toutes les opérations doivent passer par la chaîne de vérification des autorisations

### Opérations courantes

#### 1. Ouvrir le navigateur

```json
{
  "action": "open"
}
```

#### 2. Naviguer vers une page Web

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Obtenir le contenu de la page

```json
{
  "action": "get_page_text"
}
```

Renvoie le contenu textuel de la page pour l'analyse et la compréhension par l'IA.

#### 4. Cliquer sur un élément

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Saisir du texte

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "mot-clé de recherche"
}
```

#### 6. Exécuter JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Obtenir une capture d'écran

```json
{
  "action": "get_screenshot"
}
```

Renvoie une capture d'écran de la page (encodée en Base64), utilisable pour l'analyse visuelle.

#### 8. Attendre l'apparition d'un élément

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Scénarios d'utilisation

1. **Obtention d'informations**
   - Naviguer sur des sites d'actualités pour obtenir les dernières informations
   - Consulter de la documentation et des ressources techniques
   - Surveiller les changements de contenu de pages Web

2. **Opérations automatisées**
   - Remplir et soumettre des formulaires
   - Cliquer sur des boutons pour déclencher des opérations
   - Extraire des données de pages Web

3. **Analyse de pages Web**
   - Analyser la structure et le contenu des pages
   - Extraire des informations spécifiques
   - Analyser visuellement les captures d'écran de pages

### Précautions

- Les opérations de navigateur peuvent être lentes, il faut attendre le chargement complet des pages
- Utiliser `wait_for_element` pour s'assurer de l'apparition d'un élément avant d'opérer
- Respecter les conditions d'utilisation et le fichier robots.txt des sites Web
- Éviter les requêtes fréquentes pouvant entraîner un blocage

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

### Via l'API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

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

### Gestion des tâches

1. **Définir les priorités** : Utiliser les priorités (1-10)
2. **Définir les échéances** : Toujours définir une date d'échéance
3. **Surveiller la progression** : Vérifier régulièrement le statut des tâches
4. **Gérer les échecs** : Implémenter une logique de réessai

### Optimisation de la mémoire

1. **Nettoyer les anciennes données** : Archiver régulièrement les anciens souvenirs
2. **Indexer les informations importantes** : Marquer les informations clés
3. **Utiliser le stockage temporel** : Exploiter l'indexation temporelle pour les requêtes

---

## Espace de Travail du Projet

### Aperçu

L'espace de travail de projet est un mécanisme de gestion d'espace supportant la collaboration multi-Êtres de Silicium. Le Curateur de Silicium peut créer des espaces de projet, assigner des Êtres de Silicium aux projets et leur attribuer des rôles.

### Cycle de vie du projet

```
Création → Actif → Archive → Destruction
              ↑           |
              └─ Restauration ┘
```

### Rôles de projet

Les Êtres de Silicium peuvent se voir attribuer des rôles spécifiques dans les projets :

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### Notes de travail de projet

Les notes de travail dans l'espace de projet sont publiques, accessibles à tous les membres du projet :

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Module d'authentification utilisateur terminé",
  "content": "## Détails d'implémentation\n\n- Utilisation de JWT token",
  "keywords": "authentification,JWT"
}
```

### Tâches de projet

Les tâches dans l'espace de projet supportent une gestion complète du cycle de vie :

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "Implémenter l'authentification utilisateur",
  "priority": 5
}
```

### Flux de travail de projet

Les projets peuvent être liés à des modèles de flux de travail pour piloter le processus de collaboration des Êtres de Silicium :

- Les flux de travail sont basés sur des modèles de machine à états
- Supportent les transitions d'état pilotées par Tick
- Enregistrent automatiquement les journaux de transition d'état

### Isolation des autorisations d'outils

Les autorisations d'outils au niveau du projet sont indépendantes des autorisations au niveau de l'Être de Silicium, réalisant une isolation des autorisations entre projets. Par exemple, un Être de Silicium peut avoir un accès réseau dans le projet A, mais être limité à un accès en lecture seule dans le projet B.

## Dépannage

### L'être ne peut pas démarrer

**Vérifications** :
- Le Fichier d'Âme existe et est valide
- Le client IA est configuré
- Les ressources système sont suffisantes

### L'être s'arrête de manière inattendue

**Vérifications** :
- Les erreurs dans les journaux
- La disponibilité du service IA
- L'utilisation de la mémoire

### La tâche n'est pas exécutée

**Vérifications** :
- Le système de minuteurs fonctionne
- La priorité et la planification des tâches
- Les paramètres d'autorisation

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md) pour comprendre la conception du système
- 🛠️ Consulter la [référence des outils](tools-reference.md) pour tous les outils intégrés
- 🔒 Consulter le [système d'autorisations](permission-system.md) pour la gestion des autorisations
- 🌐 Consulter le [guide de l'interface Web UI](web-ui-guide.md) pour les fonctionnalités de l'interface
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md) pour commencer rapidement
