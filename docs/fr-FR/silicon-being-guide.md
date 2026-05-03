# Guide des Silicon Beings

> **Version : v0.1.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | **Français** | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md)

## Aperçu

Les Silicon Beings sont des agents pilotés par l'IA capables de penser, d'agir et d'évoluer de manière autonome.

## Architecture

### Séparation Body-Brain

```
┌─────────────────────────────────────┐
│         Silicon Being                │
├──────────────────┬──────────────────┤
│   Body           │   Brain          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Gestion d'état │ • Charger historique │
│ • Détection de   │ • Appeler l'IA   │
│   déclencheurs   │ • Exécuter outils│
│ • Cycle de vie   │ • Persister réponse│
└──────────────────┴──────────────────┘
```

## Fichier âme

### Structure

```markdown
# Nom du Being

## Personality
Décrivez les traits de personnalité et les caractéristiques du being.

## Capabilities
Listez ce que ce being peut faire.

## Behavior Guidelines
Définissez comment le being doit se comporter dans différentes situations.

## Knowledge Domain
Spécifiez le domaine d'expertise du being.
```

### Exemple

```markdown
# Assistant de revue de code

## Personality
Vous êtes un réviseur de code méticuleux avec 10 ans d'expérience.
Vous fournissez des retours constructifs et expliquez toujours votre raisonnement.

## Capabilities
- Réviser le code pour les bugs et les bonnes pratiques
- Suggérer des optimisations de performance
- Expliquer les algorithmes complexes
- Identifier les vulnérabilités de sécurité

## Behavior Guidelines
- Commencer par des observations positives
- Fournir des exemples spécifiques
- Expliquer pourquoi les changements sont nécessaires
- Être respectueux et professionnel

## Knowledge Domain
Spécialisé en C#, .NET et architecture logicielle.
```

## Créer un Being

### Via l'interface Web

1. Naviguer vers **Gestion des Beings**
2. Cliquer sur **Créer un nouveau Being**
3. Remplir :
   - Nom
   - Contenu de l'âme
   - Options de configuration
4. Cliquer sur **Créer**

### Via l'API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Cycle de vie d'un Being

### États

```
Created → Starting → Running → Stopping → Stopped
                    ↓
                  Error
```

### Opérations

- **Démarrer** : Initialiser et commencer le traitement
- **Arrêter** : Arrêt gracieux
- **Pause** : Suspendre temporairement (état préservé)
- **Reprendre** : Continuer depuis l'état de pause

## Système de tâches

### Créer une tâche

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

### Statuts des tâches

- `Pending` - En attente d'exécution
- `Running` - En cours d'exécution
- `Completed` - Terminé avec succès
- `Failed` - Échec de l'exécution
- `Cancelled` - Annulé manuellement

## Système de minuteries

### Types de minuteries

1. **Ponctuelle** : Exécution unique après un délai
2. **Intervalle** : Répétition à intervalles fixes
3. **Cron** : Exécution basée sur une expression Cron

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

## Système de mémoire

### Types de mémoire

- **Court terme** : Contexte de conversation actuel
- **Long terme** : Connaissances et expériences persistées
- **Épisodique** : Événements et interactions indexés dans le temps

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

## Système de notes de travail

### Aperçu

Les notes de travail sont un système de journal personnel des Silicon Beings avec un design par pages pour enregistrer la progression du travail, les expériences d'apprentissage, les notes de projet, etc.

### Fonctionnalités

- **Gestion des pages** : Chaque note est une page distincte, accessible par numéro de page
- **Support Markdown** : Le contenu supporte le format Markdown (texte, listes, tableaux, blocs de code)
- **Index par mots-clés** : Les notes peuvent être étiquetées avec des mots-clés pour la recherche
- **Résumé** : Chaque note a un court résumé pour une navigation rapide
- **Génération de répertoire** : Peut générer un répertoire de toutes les notes pour une vue d'ensemble
- **Horodatage** : Enregistrement automatique des dates de création et de mise à jour
- **Privé par défaut** : Seul le Being y a accès (le Curator peut gérer)

### Scénarios d'utilisation

1. **Documenter la progression du projet**
   ```
   Résumé : Module d'authentification utilisateur terminé
   Contenu : Vérification JWT, intégration OAuth2, mécanisme de refresh token implémentés
   Mots-clés : authentification,JWT,OAuth2
   ```

2. **Notes d'apprentissage**
   ```
   Résumé : Bonnes pratiques de programmation asynchrone C# apprises
   Contenu : Notes sur async/await, cas d'utilisation de ConfigureAwait...
   Mots-clés : C#,Async,Bonnes pratiques
   ```

3. **Comptes rendus de réunion**
   ```
   Résumé : Réunion sur les exigences produit
   Contenu : Nouvelles exigences de fonctionnalités discutées, approche d'implémentation définie...
   Mots-clés : produit,exigences,réunion
   ```

### Utilisation via l'outil

Les Beings peuvent gérer leurs notes de travail via l'outil `work_note` :

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

### Gestion via l'interface Web

1. Naviguer vers **Gestion des Beings** → Sélectionner un Being
2. Cliquer sur l'onglet **Notes de travail**
3. Afficher, rechercher, modifier les notes
4. Aperçu Markdown supporté

---

## Système de réseau de connaissances

### Aperçu

Le réseau de connaissances est un système de représentation et de gestion des connaissances basé sur une structure en triplets (Sujet-Prédicat-Objet) pour le stockage et la gestion de connaissances structurées.

### Concepts clés

#### Structure en triplets

```
Sujet (Subject) --Prédicat (Predicate)--> Objet (Object)
```

**Exemples** :
- `Python` --`is_a`--> `programming_language`
- `Paris` --`capital_of`--> `France`
- `eau` --`boiling_point`--> `100°C`

#### Valeur de confiance

Chaque triplet de connaissance a une valeur de confiance (0.0-1.0) indiquant la fiabilité de la connaissance :
- `1.0` : Absolument certain (comme les théorèmes mathématiques)
- `0.8-0.99` : Haute confiance (comme les faits vérifiés)
- `0.5-0.79` : Confiance moyenne (comme les inférences ou hypothèses)
- `<0.5` : Faible confiance (comme les conjectures ou informations non vérifiées)

#### Système de tags

Supporte l'ajout de tags aux triplets pour la classification et la recherche :
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

#### 2. Consulter une connaissance

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

#### 4. Découvrir des chemins de connaissance

Trouve les chemins de connexion entre deux concepts :
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

#### 5. Valider une connaissance

Vérifie la validité et la cohérence de la connaissance :
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Statistiques du réseau de connaissances

Obtient les statistiques globales du réseau de connaissances :
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
   - Stocker des faits objectifs et des connaissances générales
   - Exemple : `Terre` --`is_a`--> `planète`

2. **Relations entre concepts**
   - Enregistrer les relations entre concepts
   - Exemple : `héritage` --`is_a`--> `concept_programmation_orientée_objet`

3. **Accumulation d'apprentissage**
   - Les Beings accumulent continuellement des connaissances par l'apprentissage
   - Forment des systèmes de connaissances structurés

4. **Support d'inférence**
   - Découvrir des relations indirectes via les chemins de connaissance
   - Supporter l'inférence et la prise de décision basées sur les connaissances

### Gestion via l'interface Web

1. Naviguer vers la page **Réseau de connaissances**
2. Afficher les statistiques de connaissances
3. Rechercher et parcourir les connaissances
4. Visualiser le diagramme de relations de connaissances (prévu)

---

## Opérations du navigateur WebView (Nouveau)

### Aperçu

Les Silicon Beings peuvent naviguer autonomement sur le Web, récupérer des informations et exécuter des opérations Web via l'outil de navigateur WebView. Le navigateur fonctionne en mode headless, totalement invisible pour l'utilisateur.

### Fonctionnalités

- **Isolation individuelle** : Chaque Being a sa propre instance de navigateur, cookies et sessions
- **Mode headless** : Opération autonome en arrière-plan, invisible pour l'utilisateur
- **Fonctionnalité complète** : Supporte l'exécution de JavaScript, le rendu CSS, le remplissage de formulaires, etc.
- **Contrôle de sécurité** : Toutes les opérations doivent passer par la chaîne de permissions

### Opérations courantes

#### 1. Ouvrir le navigateur

```json
{
  "action": "open_browser"
}
```

#### 2. Naviguer vers un site Web

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Récupérer le contenu de la page

```json
{
  "action": "get_page_text"
}
```

Retourne le contenu textuel de la page pour l'analyse et la compréhension par l'IA.

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

#### 6. Exécuter du JavaScript

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

Retourne une capture d'écran de la page (encodée en Base64), utilisable pour l'analyse visuelle.

#### 8. Attendre un élément

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Scénarios d'utilisation

1. **Collecte d'informations**
   - Naviguer sur des sites d'actualités pour des informations à jour
   - Consulter la documentation et les ressources techniques
   - Surveiller les changements de contenu de sites Web

2. **Opérations automatisées**
   - Remplir et soumettre des formulaires
   - Cliquer sur des boutons pour des opérations
   - Extraire des données Web

3. **Analyse Web**
   - Analyser la structure et le contenu des pages
   - Extraire des informations spécifiques
   - Analyse visuelle par capture d'écran

### Remarques

- Les opérations de navigateur peuvent être lentes, en attente du chargement des pages
- Utiliser `wait_for_element` pour s'assurer que l'élément est présent
- Respecter les conditions d'utilisation et le fichier robots.txt des sites Web
- Éviter les requêtes fréquentes pour prévenir les bannissements

---

## Bonnes pratiques

### Écrire un fichier âme

1. **Concret** : Traits de personnalité et limites clairs
2. **Définir le périmètre** : Ce que le Being doit et ne doit pas faire
3. **Inclure des exemples** : Montrer les modèles de comportement attendus
4. **Mettre à jour régulièrement** : Faire évoluer l'âme en fonction des performances

### Gestion des tâches

1. **Définir les priorités** : Utiliser la priorité (1-10)
2. **Définir les échéances** : Toujours fixer une date limite
3. **Surveiller la progression** : Vérifier régulièrement le statut des tâches
4. **Gérer les erreurs** : Implémenter une logique de retry

### Optimisation de la mémoire

1. **Nettoyer les anciennes données** : Archiver régulièrement les anciens souvenirs
2. **Indexer les informations importantes** : Marquer les informations clés
3. **Utiliser le stockage temporel** : Exploiter les requêtes par index temporel

## Dépannage

### Le Being ne démarre pas

**Vérifier** :
- Le fichier âme existe et est valide
- Le client IA est configuré
- Les ressources système sont suffisantes

### Le Being s'arrête inopinément

**Vérifier** :
- Les erreurs dans les journaux
- La disponibilité du service IA
- L'utilisation de la mémoire

### Les tâches ne s'exécutent pas

**Vérifier** :
- Le système de minuteries fonctionne
- La priorité et la planification des tâches
- Les paramètres de permissions

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 🚀 Voir le [guide de démarrage rapide](getting-started.md)
