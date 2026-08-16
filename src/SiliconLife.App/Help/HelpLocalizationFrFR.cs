// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Help;

/// <summary>
/// French (France) help documentation implementation.
/// </summary>
public class HelpLocalizationFrFR : HelpLocalizationBase
{
    #region Help Documents

    public override string GettingStarted_Title => "Démarrage rapide";
    public override string BeingManagement_Title => "Gestion des Beings";
    public override string ChatSystem_Title => "Système de chat";
    public override string Dashboard_Title => "Tableau de bord";
    public override string Task_Title => "Tâches";
    public override string Timer_Title => "Minuteries";
    public override string Permission_Title => "Gestion des permissions";
    public override string Config_Title => "Gestion de la configuration";
    public override string FAQ_Title => "FAQ";
    public override string Memory_Title => "Système de mémoire";
    public override string OllamaSetup_Title => "Installation Ollama et téléchargement de modèles";
    public override string BailianDashScope_Title => "Guide utilisateur de la plateforme Alibaba Cloud Bailian";
    public override string VolcengineArk_Title => "Guide utilisateur de la plateforme Volcengine Ark";
    public override string HerdsmanSetup_Title => "Guide de configuration du moteur d'inférence Herdsman";
    public override string LongCatSetup_Title => "Guide de configuration du grand modèle LongCat de Meituan";
    public override string QiniuAISetup_Title => "Guide de configuration de l'IA Qiniu Cloud";
    public override string DeepSeekSetup_Title => "Guide de configuration de la plateforme DeepSeek AI";
    public override string ZhipuSetup_Title => "Guide de configuration de la plateforme Zhipu AI (GLM)";
    public override string MoonshotSetup_Title => "Guide de configuration de la plateforme Moonshot (Kimi)";
    public override string SiliconFlowSetup_Title => "Guide de configuration de la plateforme d'inférence SiliconFlow AI";
    public override string MiniMaxSetup_Title => "Guide de configuration de la plateforme MiniMax AI";
    public override string ErnieSetup_Title => "Guide de configuration de la plateforme Baidu ERNIE (Qianfan)";
    public override string HunyuanSetup_Title => "Guide de configuration du grand modèle Tencent Hunyuan";
    public override string AIClients_Title => "Configuration du client IA";

    public override string BeingSoul_Title => "Fichier âme du Being";

    public override string AuditLog_Title => "Journal d'audit";
    public override string KnowledgeGraph_Title => "Graphe de connaissances";
    public override string WorkNotes_Title => "Notes de travail";
    public override string Projects_Title => "Gestion de projets";
    public override string Logging_Title => "Système de journalisation";
    public override string Skills_Title => "Compétences";

    public override string[] GettingStarted_Tags => new[]
        { "installer", "démarrer", "configuration", "démarrage rapide", "commencer", "lancer", "initialiser" };

    public override string[] BeingManagement_Tags => new[]
        { "being", "créer", "configurer", "gestion des beings", "silicon being", "profil", "gérer" };

    public override string[] ChatSystem_Tags => new[]
        { "chat", "message", "conversation", "système de chat", "dialogue", "communiquer", "discussion" };

    public override string[] Dashboard_Tags => new[]
        { "tableau de bord", "surveiller", "statistiques", "statut", "système", "fréquence des messages", "mémoire" };

    public override string[] Task_Tags => new[]
        { "tâche", "travail", "exécution", "priorité", "dépendance", "statut", "automatisation", "gestion" };

    public override string[] Timer_Tags => new[]
        { "minuterie", "planification", "déclencheur", "récurrent", "calendrier", "rappel", "automatique", "cron" };

    public override string[] Permission_Tags => new[]
    {
        "permission", "sécurité", "contrôle d'accès", "gestion des permissions", "authentification", "autorisation",
        "confidentialité", "protection"
    };

    public override string[] Config_Tags => new[]
        { "config", "paramètres", "options", "configuration", "préférences", "personnalisation", "système" };

    public override string[] FAQ_Tags => new[]
        { "faq", "aide", "questions", "fréquentes", "support", "dépannage", "guide", "assistance" };

    public override string[] Memory_Tags => new[]
        { "mémoire", "historique", "enregistrements", "système de mémoire", "activité", "trace", "recherche", "journal" };

    public override string[] OllamaSetup_Tags => new[]
        { "Ollama", "installer", "modèle", "télécharger", "IA locale", "configuration", "exécuter" };

    public override string[] BailianDashScope_Tags => new[]
        { "Bailian", "DashScope", "Alibaba Cloud", "IA cloud", "API", "configuration", "modèle", "facturation" };

    public override string[] VolcengineArk_Tags => new[]
        { "Volcengine", "Ark", "Doubao", "ByteDance", "IA cloud", "API", "configuration", "modèle" };

    public override string[] HerdsmanSetup_Tags => new[]
        { "Herdsman", "inférence", "moteur", "IA locale", "API", "configuration", "modèle" };

    public override string[] LongCatSetup_Tags => new[]
        { "LongCat", "Meituan", "IA cloud", "API", "configuration", "modèle", "grand modèle" };

    public override string[] QiniuAISetup_Tags => new[]
        { "Qiniu", "IA cloud", "API", "configuration", "modèle", "inférence" };

    public override string[] DeepSeekSetup_Tags => new[]
        { "DeepSeek", "AI", "API", "configuration", "modèle", "raisonnement", "code" };

    public override string[] ZhipuSetup_Tags => new[]
        { "Zhipu", "GLM", "AI", "API", "configuration", "modèle", "code", "agent" };

    public override string[] MoonshotSetup_Tags => new[]
        { "Moonshot", "Kimi", "AI", "API", "configuration", "modèle", "contexte long" };

    public override string[] SiliconFlowSetup_Tags => new[]
        { "SiliconFlow", "AI", "inférence", "API", "configuration", "modèle", "open source" };

    public override string[] MiniMaxSetup_Tags => new[]
        { "MiniMax", "AI", "API", "configuration", "modèle", "multimodal", "agent" };

    public override string[] ErnieSetup_Tags => new[]
        { "Baidu", "ERNIE", "Qianfan", "AI", "API", "configuration", "modèle" };

    public override string[] HunyuanSetup_Tags => new[]
        { "Tencent", "Hunyuan", "AI", "API", "configuration", "modèle", "multimodal" };

    public override string[] AIClients_Tags => new[]
        { "client IA", "service IA", "modèle", "configuration", "local", "cloud", "Ollama", "DashScope" };

    public override string[] BeingSoul_Tags => new[]
    {
        "fichier âme", "personnalité", "prompt", "rôle", "comportement", "configuration", "caractère",
        "guide de travail", "prompt système"
    };

    public override string[] AuditLog_Tags => new[]
        { "journal d'audit", "Token", "statistiques d'utilisation", "surveiller", "consommation", "analyse", "tendance", "exporter", "CSV" };

    public override string[] KnowledgeGraph_Tags => new[]
    {
        "graphe de connaissances", "connaissances", "visualisation", "triplet", "entité", "relation", "réseau",
        "apprentissage", "gestion"
    };

    public override string[] WorkNotes_Tags => new[]
        { "notes de travail", "notes", "enregistrements", "journal", "Markdown", "mots-clés", "version", "recherche" };

    public override string[] Projects_Tags => new[]
    {
        "gestion de projets", "projet", "collaboration", "tâche", "membre", "archiver", "équipe", "espace de travail",
        "progression"
    };

    public override string[] Logging_Tags => new[]
        { "système de journalisation", "journal", "enregistrements", "débogage", "erreur", "avertissement", "surveiller", "trace", "console", "fichier" };
    public override string[] Skills_Tags => new[] { "compétences", "skill", "orchestration", "prompt", "automatisation", "plugin", "personnalisé" };
    #endregion

    #region Help Document Content

    public override string GettingStarted => @"
# Démarrage rapide

## Démarrage du système

### Double-clic pour démarrer (Recommandé)

Trouvez le fichier programme et double-cliquez pour démarrer :
- **Windows** : `SiliconLife.Default.exe`
- Le système démarrera automatiquement et **ouvrira automatiquement le navigateur**

C'est aussi simple que ça ! Aucune configuration nécessaire.

## Première utilisation

Lors du premier démarrage, le système **complète automatiquement toute l'initialisation** :
- ✅ Crée automatiquement le Silicon Curator
- ✅ Utilise le fichier âme intégré (prompt)
- ✅ Sauvegarde automatiquement la configuration
- ✅ Tous les services sont automatiquement prêts

Il vous suffit d'attendre que le navigateur s'ouvre, et vous êtes prêt !

## Aperçu de l'interface

L'interface du système se divise en deux parties principales :

### Barre de navigation gauche

Contient les modules fonctionnels suivants :

- **💬 Chat** - Discuter avec les silicon beings
- **📊 Tableau de bord** - Voir l'état du système
- **🧠 Beings** - Voir et gérer les silicon beings
- **🔍 Audit** - Voir les enregistrements d'opérations
- **📚 Connaissances** - Gérer le graphe de connaissances
- **📁 Projets** - Gérer les projets de code
- **📝 Journaux** - Voir les journaux système
- **⚙ Config** - Paramètres système
- **❓ Aide** - Ce document
- **ℹ À propos** - Informations système

### Zone de contenu principale

Affiche le contenu de la page actuelle, qui change en fonction du module fonctionnel que vous sélectionnez.

## Démarrage rapide

### 1. Discuter avec un Silicon Being

1. Cliquez sur l'icône **💬 Chat** à gauche
2. Sélectionnez un silicon being dans la liste de gauche (le Silicon Curator est disponible par défaut)
3. Tapez votre message dans la zone de saisie en bas
4. Appuyez sur `Entrée` pour envoyer
5. L'IA vous répondra en temps réel

**Conseils :** Appuyez sur `Shift + Entrée` pour un retour à la ligne. Cliquez sur ⏹ pour arrêter la réponse.

### 2. Voir les informations d'un Silicon Being

1. Cliquez sur l'icône **🧠 Beings** à gauche
2. Cliquez sur n'importe quelle carte de silicon being
3. Les informations détaillées s'afficheront à droite

### 3. Modifier les paramètres système

1. Cliquez sur l'icône **⚙ Config** à gauche
2. Trouvez l'élément de configuration que vous souhaitez modifier
3. Cliquez sur ""Modifier"", entrez la nouvelle valeur et sauvegardez

## Téléchargement de fichiers

1. Cliquez sur le bouton **📁** dans l'interface de chat
2. Entrez le chemin complet du fichier (par ex., `C:\Users\VotreNom\Documents\Rapport.pdf`)
3. Cliquez sur ""Confirmer le téléchargement""
4. L'IA lira et analysera le fichier

**Types de fichiers pris en charge :** .txt, .md, .json, .cs, .js, .py, .yml, .yaml, .csv, .log, etc.

## Voir l'historique de chat

1. Allez sur la page **🧠 Beings**
2. Cliquez sur le silicon being que vous souhaitez voir
3. Cliquez sur le lien ""Historique de chat""

## Obtenir de l'aide

- **Voir l'aide** : Cliquez sur l'icône **❓ Aide** à gauche
- **Voir les journaux** : Cliquez sur l'icône **📝 Journaux** à gauche
- **Redémarrer le système** : De nombreux problèmes peuvent être résolus en redémarrant

## Prochaines étapes

- 📖 Lire les autres documents d'aide pour en savoir plus sur les fonctionnalités
- 💬 Discuter avec le Silicon Curator pour accomplir des tâches
- ⚙ Explorer les options de configuration pour personnaliser votre système
";

    public override string BeingManagement => @"
# Gestion des Beings

## Qu'est-ce qu'un Silicon Being ?

Un Silicon Being est l'entité centrale du système. Chaque silicon being est un agent IA indépendant avec :
- **Fichier âme** : Prompt central définissant les modèles de comportement, la personnalité et les capacités
- **Système de mémoire** : Sauvegarde l'historique des conversations et les informations importantes
- **Système de tâches** : Exécute des tâches planifiées et des opérations automatisées
- **Ensemble d'outils** : Divers outils fonctionnels appelables

## Voir les Silicon Beings

### Liste des Beings

En entrant sur la page ""Beings"", tous les silicon beings sont affichés sous forme de cartes :
- **Nom** : Le nom d'affichage du silicon being
- **Statut** : Inactif (vert) ou En cours (bleu)
- **Type** : Si du code compilé personnalisé est chargé, une étiquette de type sera affichée

### Voir les détails d'un Being

Cliquez sur n'importe quelle carte de silicon being pour voir les informations détaillées :
- **ID** : Identifiant unique du silicon being
- **Statut** : État de fonctionnement actuel
- **Compilation personnalisée** : Si du code personnalisé est chargé
- **Nombre de minuteries** : Cliquez pour voir la gestion des minuteries
- **Nombre de tâches** : Cliquez pour voir la liste des tâches
- **Mémoire** : Cliquez pour voir le système de mémoire
- **Permissions** : Cliquez pour voir la configuration des permissions
- **Historique de chat** : Voir les enregistrements de conversation historiques
- **Notes de travail** : Voir les notes de travail
- **Client IA** : Cliquez pour voir et modifier la configuration IA
- **Fichier âme** : Cliquez pour voir et modifier le prompt

## Modifier un Silicon Being

### Modifier le fichier âme

1. Cliquez sur le lien ""Fichier âme"" dans la page de détails du being
2. Entrez dans l'éditeur de fichier âme (supporte le format Markdown)
3. Modifiez le contenu du prompt
4. Sauvegardez les modifications

### Modifier la configuration IA

1. Cliquez sur le lien ""Client IA"" dans la page de détails du being
2. Sélectionnez le type de client IA (comme Ollama, OpenAI, etc.)
3. Configurez le point de terminaison API, le modèle, la clé et d'autres paramètres
4. Prend effet immédiatement après la sauvegarde

## Guide d'écriture du fichier âme

### Structure de base

```markdown
# Définition du rôle

Vous êtes un [description du rôle], spécialisé dans :
- Compétence 1
- Compétence 2
- Compétence 3

# Directives de comportement

1. Directive 1
2. Directive 2
3. Directive 3

# Flux de travail

Lors de la réception d'une tâche :
1. Comprendre les exigences
2. Analyser l'approche
3. Exécuter les opérations
4. Rapporter les résultats
```

### Conseils d'écriture

1. **Définition claire du rôle** : Définissez clairement les responsabilités et l'expertise du silicon being
2. **Définir les limites de comportement** : Expliquez ce qui peut être fait et ce qui ne doit pas être fait
3. **Fournir des flux de travail** : Guidez le silicon being sur la façon de traiter les tâches
4. **Utiliser le format Markdown** : Prend en charge les titres, les listes, les blocs de code, etc.

### Exemple : Assistant de programmation

```markdown
# Définition du rôle

Vous êtes un assistant professionnel de développement full-stack, spécialisé dans :
- Développement C# / .NET
- Conception d'architecture et revue de code
- Conception et optimisation de bases de données
- Développement frontend Web

# Directives de comportement

1. Fournir toujours des exemples de code exécutables
2. Expliquer la logique clé du code et la réflexion de conception
3. Fournir des recommandations de bonnes pratiques
4. En cas d'incertitude, informer clairement l'utilisateur

# Normes de code

- Suivre les principes SOLID
- Utiliser une nomenclature claire
- Ajouter les commentaires nécessaires
- Considérer la gestion des erreurs et les cas limites
```

## Statut des Silicon Beings

- **Inactif** : En attente de tâches ou de conversations (indicateur vert)
- **En cours** : Exécute actuellement une tâche ou est en conversation (indicateur bleu)

## Bonnes pratiques

1. **Séparation des responsabilités** : Différents beings traitent différents domaines
2. **Optimisation continue** : Optimisez les fichiers âme en fonction des retours d'utilisation
3. **Sauvegarde de configuration** : Sauvegardez les fichiers âme des beings importants

## Dépannage

### Q : Le silicon being ne répond pas ?

Vérifiez : 1) Le service IA fonctionne-t-il normalement ? 2) La connexion réseau est-elle normale ? 3) Le fichier âme est-il configuré correctement ? 4) Consultez les journaux système.

### Q : Comment changer le modèle IA d'un silicon being ?

Cliquez sur le lien ""Client IA"" dans la page de détails du being, sélectionnez un nouveau modèle IA et configurez-le. Prend effet immédiatement après la sauvegarde.

### Q : Le comportement du silicon being ne correspond pas aux attentes ?

1. Vérifiez si le fichier âme est clair et explicite
2. Ajoutez plus de directives de comportement et de contraintes
3. Fournissez des conseils de flux de travail spécifiques
4. Testez et optimisez continuellement
";

    public override string ChatSystem => @"
# Système de chat

## Démarrer une conversation

1. Cliquez sur l'icône **💬 Chat** dans la barre de navigation gauche
2. Sélectionnez le silicon being avec lequel vous souhaitez discuter
3. Tapez votre message dans la zone de saisie en bas
4. Appuyez sur `Entrée` ou cliquez sur le bouton ""Envoyer""
5. L'IA répondra en temps réel (le texte apparaît caractère par caractère)

## Description de l'interface

- **Liste gauche** : Affiche tous les silicon beings, cliquez pour changer de cible de conversation
- **Zone centrale** : Affiche les messages de conversation (vos messages à droite, réponses de l'IA à gauche)
- **Zone de saisie inférieure** : Zone de saisie et bouton d'envoi

### Boutons

- **Envoyer** : Envoie votre message saisi
- **⏹ Arrêter** : Interrompt la réponse de l'IA
- **📁 Fichier** : Télécharge des fichiers pour analyse par l'IA

## Opérations de base

### Envoyer un message

- Appuyez sur `Entrée` pour envoyer
- Appuyez sur `Shift + Entrée` pour un retour à la ligne

### Arrêter une réponse

- Cliquez sur le bouton ""⏹ Arrêter""
- Ou envoyez un nouveau message (interrompra automatiquement la réponse actuelle)

### Télécharger un fichier

1. Cliquez sur le bouton **📁** à côté de la zone de saisie
2. Entrez le chemin du fichier dans le panneau popup
3. Cliquez sur ""Confirmer le téléchargement""
4. L'IA lira et analysera le fichier

**Types de fichiers pris en charge** : .txt, .md, .json, .xml, .cs, .js, .py, .java, .yml, .yaml, .ini, .conf, .csv, .log, etc.

## Fonctionnalités de conversation

### Affichage en streaming en temps réel

Les réponses de l'IA s'affichent caractère par caractère, vous n'avez pas besoin d'attendre la réponse complète.

### Conversation multi-tours

- Le système sauvegarde automatiquement l'historique des conversations
- L'IA se souvient de ce qui a été dit précédemment
- Vous pouvez vous référer directement aux conversations antérieures

### Appel d'outils

L'IA peut appeler automatiquement des outils pour : interroger le calendrier, gérer la configuration système, exécuter du code, lire des fichiers, rechercher de l'aide, créer des notes, interroger la mémoire.

### Conversation multilingue

Vous pouvez converser avec l'IA dans n'importe quelle langue, et l'IA répondra automatiquement dans la même langue.

## Voir l'historique de chat

1. Cliquez sur l'icône **🧠 Beings** dans la barre de navigation gauche
2. Cliquez sur la carte du silicon being que vous souhaitez voir
3. Trouvez le lien ""Historique de chat"" dans les détails à droite
4. Cliquez pour voir toutes les sessions historiques

## Questions fréquentes

### Q : Que faire si l'IA répond lentement ?

**Causes possibles** : Modèle volumineux, latence réseau, long historique de conversation.
**Solutions** : Utilisez des modèles locaux (comme Ollama), choisissez un modèle plus léger.

### Q : L'IA n'appelle pas d'outils ?

Vérifiez : 1) L'outil est-il activé ? 2) Y a-t-il des restrictions de permission ? 3) Le modèle IA prend-il en charge les appels d'outils ?

### Q : Comment télécharger des fichiers ?

Cliquez sur le bouton ""📁"" à côté de la zone de saisie, entrez le chemin complet du fichier, puis cliquez sur ""Confirmer le téléchargement"".

### Q : Comment voir les conversations précédentes ?

Sur la page ""Beings"", cliquez sur le lien ""Historique de chat"" du silicon being correspondant.

## Suggestions d'utilisation

1. **S'exprimer clairement** : Décrivez vos besoins dans un langage clair
2. **Poser les questions étape par étape** : Décomposez les questions complexes
3. **Fournir du contexte** : Expliquez les informations de fond pertinentes si nécessaire
4. **Utiliser le téléchargement de fichiers** : Fournissez directement le chemin du fichier
5. **Attention aux appels d'outils** : Notez les outils que l'IA appelle
";

    public override string Dashboard => @"
# Tableau de bord

## Aperçu

Le tableau de bord est votre centre de surveillance système, vous permettant de comprendre l'état de fonctionnement des silicon beings en un coup d'œil.

## Fonctionnalités principales

- **Surveillance en temps réel** : Affiche le nombre, l'état actif et l'utilisation des ressources système des silicon beings
- **Statistiques des messages** : Affiche la fréquence des messages de chat récents sous forme de graphique
- **Mise à jour automatique** : Les données se rafraîchissent automatiquement

## Cartes statistiques

| Carte | Description |
|-------|-------------|
| 🧠 Total Silicon Beings | Nombre total de tous les silicon beings créés |
| ⚡ Silicon Beings actifs | Nombre de silicon beings actuellement au travail |
| ⏱️ Temps de fonctionnement | Temps écoulé depuis le démarrage du programme |
| 💾 Utilisation mémoire | Mémoire actuellement occupée par le programme |

## Graphique de fréquence des messages

Un graphique à barres montrant le nombre de messages au cours des 20 dernières minutes.

## Comment comprendre les données

### Activité des Silicon Beings
- Nombre actif proche du total : la plupart des beings sont occupés
- Nombre actif à 0 : tous les beings sont au repos

### Utilisation de la mémoire
- Plage normale : 50-300 Mo
- Si la mémoire dépasse 500 Mo, envisagez de redémarrer

## Questions fréquentes

### Q : Pourquoi les statistiques ne se mettent-elles pas à jour ?

Vérifiez : 1) Erreurs JavaScript dans la console du navigateur 2) Requêtes réseau normales 3) Services backend normaux 4) Essayez de rafraîchir la page (F5)

### Q : Le graphique est vide ou sans données ?

Causes possibles : 1) Le système vient de démarrer 2) Aucun message dans les 20 dernières minutes 3) Service ChatSystem non initialisé

### Q : Comment rafraîchir manuellement les données ?

Appuyez sur F5 pour rafraîchir la page, ou Ctrl+F5 pour un rafraîchissement forcé.

## Suggestions d'utilisation

1. **Vérification régulière** : Ouvrez quotidiennement pour comprendre l'état du système
2. **Observer les tendances** : Utilisez le graphique de fréquence des messages
3. **Surveiller la mémoire** : Envisagez de redémarrer si la mémoire dépasse 500 Mo
4. **Comprendre l'activité** : Jugez si le système fonctionne normalement via le nombre de beings actifs
";

    public override string Task => @"
# Tâches

## Qu'est-ce qu'une tâche ?

Les tâches sont des éléments de travail qu'un silicon being exécute ou doit exécuter.

## Statut des tâches

| Statut | Description |
|--------|-------------|
| **En attente** | La tâche a été créée, en attente de démarrage |
| **En cours** | La tâche est en cours d'exécution |
| **Terminée** | La tâche a été complétée avec succès |
| **Échouée** | L'exécution de la tâche a échoué |
| **Annulée** | La tâche a été annulée |

## Informations de tâche

| Information | Description |
|-------------|-------------|
| **Titre** | Le titre de la tâche |
| **Description** | Description détaillée |
| **Statut** | État actuel |
| **Priorité** | Nombre plus petit = priorité plus élevée |
| **Date de création** | Quand la tâche a été créée |
| **Dépendances** | Autres tâches dont cette tâche dépend |

## Priorité des tâches

- **Nombre plus petit = priorité plus élevée**
- La priorité par défaut est 100
- Le système trie d'abord par priorité, puis par date de création

## Dépendances des tâches

- Si la tâche A dépend de la tâche B, la tâche B doit être terminée avant que la tâche A puisse commencer
- Le système vérifie automatiquement les dépendances
- Le système détecte et empêche les dépendances circulaires

## Questions fréquentes

### Q : Comment créer une nouvelle tâche ?

Les tâches peuvent être créées automatiquement par les silicon beings, déclenchées par des minuteries, ou créées via la conversation.

### Q : Puis-je supprimer des tâches ?

Oui, mais uniquement les tâches qui ne sont pas en statut ""En cours"".

### Q : Que faire si une tâche affiche ""Échouée"" ?

1. Consultez le message d'erreur pour comprendre la raison de l'échec
2. Si c'est un problème temporaire, le silicon being peut réessayer
3. Si l'échec persiste, discutez avec le silicon being

### Q : Que signifie la priorité des tâches ?

La priorité détermine l'ordre d'exécution. Nombre plus petit = priorité plus élevée.

### Q : Que sont les dépendances de tâches ?

Une dépendance signifie qu'une tâche doit attendre que d'autres tâches soient terminées avant de pouvoir commencer.
";

    public override string Timer => @"
# Minuteries

## Qu'est-ce qu'une minuterie ?

Une minuterie est un mécanisme de rappel automatique pour les silicon beings.

## Deux types de minuteries

### Minuterie à usage unique

Se déclenche une seule fois. Se termine automatiquement après le déclenchement.

**Scénarios applicables :** Rappel à une heure spécifique, tâche ponctuelle à une date spécifique.

### Minuterie récurrente

Se déclenche de manière répétée. Le système calcule automatiquement la prochaine heure de déclenchement.

**Scénarios applicables :** Tâches quotidiennes, hebdomadaires ou mensuelles, fêtes du calendrier lunaire.

## Statut des minuteries

| Statut | Description |
|--------|-------------|
| **En cours** | La minuterie fonctionne normalement |
| **En pause** | La minuterie est temporairement arrêtée |
| **Déclenchée** | La minuterie à usage unique a terminé son déclenchement |
| **Annulée** | La minuterie a été annulée |

## Informations de minuterie

| Information | Description |
|-------------|-------------|
| **Nom** | Le nom de la minuterie |
| **Description** | Description détaillée |
| **Statut** | État actuel |
| **Type** | Usage unique ou Récurrente |
| **Prochaine heure de déclenchement** | Heure du prochain déclenchement |
| **Système de calendrier** | Calendrier utilisé (Grégorien, Lunaire, etc.) |
| **Nombre de déclenchements** | Combien de fois la minuterie s'est déclenchée |

## Systèmes de calendrier pris en charge

- **Grégorien** : Calendrier solaire international
- **Lunaire chinois** : Calendrier lunaire traditionnel chinois
- **Autres** : Calendrier islamique, cycle sexagésimal, etc.
- **Calendrier d'intervalle** : Déclenche à des intervalles fixes

## Historique de déclenchement

Chaque déclenchement enregistre des informations détaillées : heure de déclenchement, statut d'exécution, messages de conversation, informations d'erreur.

## Questions fréquentes

### Q : Comment créer une nouvelle minuterie ?

Les minuteries sont gérées automatiquement par les silicon beings.

### Q : Puis-je supprimer ou mettre en pause des minuteries ?

Oui. Le système prend en charge : pause, reprise, annulation, suppression.

### Q : Que faire si une minuterie ne se déclenche pas ?

Vérifiez : 1) La minuterie est-elle en statut ""En cours"" ? 2) L'heure de déclenchement est-elle atteinte ? 3) Le silicon being fonctionne-t-il normalement ?

### Q : Comment une minuterie récurrente calcule-t-elle la prochaine heure ?

Après chaque déclenchement, le système calcule automatiquement la prochaine heure en fonction du système de calendrier et des conditions définies.
";

    public override string Permission => @"
# Gestion des permissions

## Qu'est-ce que le système de permissions ?

Le système de permissions protège la sécurité de votre système et empêche l'IA d'exécuter des opérations non autorisées.

## Comment fonctionnent les permissions ?

### Popup de permission automatique

Lorsque l'IA tente une opération nécessitant une permission, le système affiche une popup vous demandant d'autoriser ou de refuser.

### Ordre de vérification des permissions

1. **Silicon Curator** : Opérations du curator automatiquement autorisées
2. **Limite de fréquence** : Empêche les requêtes massives en peu de temps
3. **Règles globales** : Règles d'autorisation/refus prédéfinies
4. **Règles personnalisées** : Règles que vous avez écrites
5. **Demander à l'utilisateur** : Popup si aucun des ci-dessus ne peut décider

## Règles de permission intégrées

### Règles d'accès aux fichiers

**Accès autorisé :** Répertoire temporaire du silicon being, dossiers communs de l'utilisateur (Bureau, Téléchargements, Documents, Images, Musique, Vidéos), dossiers publics.

**Accès refusé :** Répertoires critiques du système, répertoires de données d'autres silicon beings.

**Chemins non correspondants :** Popup vous demandant si vous autorisez.

## Règles de permission personnalisées (Fonctionnalité avancée)

1. Allez sur la page **🧠 Beings**
2. Cliquez sur le silicon being à configurer
3. Trouvez le lien ""Permissions"" dans les détails
4. Entrez dans l'éditeur de code de permission

L'éditeur prend en charge : coloration syntaxique C#, auto-complétion, sauvegarde en temps réel, analyse de sécurité.

## Historique des demandes de permission

Toutes les demandes de permission sont enregistrées dans le journal d'audit.

## Questions fréquentes

### Q : Pourquoi l'opération de l'IA a-t-elle été refusée ?

Causes possibles : opération dans une règle de refus, limite de fréquence déclenchée, vous avez précédemment choisi de refuser.

### Q : Que faire s'il y a trop de popups de permission ?

Envisagez d'écrire des règles personnalisées pour autoriser automatiquement les opérations sûres courantes.

### Q : Le code de permission personnalisé est-il dangereux ?

Le code passe par une analyse de sécurité. Le code malveillant sera rejeté. Les échecs de compilation ne prendront pas effet.

### Q : Puis-je définir différentes permissions pour différents beings ?

Oui. Chaque silicon being a une configuration de permission indépendante.

## Types de permission

- **Accès réseau** : L'IA tente d'accéder à des ressources réseau
- **Exécution de commandes** : L'IA tente d'exécuter des programmes en ligne de commande
- **Accès aux fichiers** : L'IA tente de lire ou d'écrire des fichiers
- **Appel de fonction** : L'IA tente d'appeler des fonctions spécifiques
- **Accès aux données** : L'IA tente d'accéder aux données système
";

    public override string Config => @"
# Gestion de la configuration

## Qu'est-ce que la gestion de la configuration ?

La page de gestion de la configuration vous permet d'ajuster divers paramètres système.

## Comment utiliser la page de configuration ?

1. Cliquez sur l'icône **⚙ Config** dans la barre de navigation gauche
2. La page affichera plusieurs groupes de configuration
3. Trouvez l'élément à modifier, cliquez sur ""Modifier""
4. Entrez la nouvelle valeur et sauvegardez

## Groupes de configuration

### Paramètres de base

- **Répertoire de données** : Emplacement du dossier de stockage des données (par défaut : `./data`)
- **Langue** : Langue de l'interface système (Chinois simplifié, Anglais, Français, etc.)

### Paramètres IA

- **Type de client IA** : Sélectionnez le service IA (Ollama, OpenAI, etc.)
- **Configuration IA** : endpoint, model, temperature, maxTokens

### Paramètres d'exécution

- **Délai d'exécution** : Temps d'exécution maximal pour une tâche (par défaut : 10 minutes)
- **Nombre maximal de dépassements** : Déclenche le mécanisme de protection (par défaut : 3 fois)
- **Délai du chien de garde** : Temps avant redémarrage automatique (par défaut : 10 minutes)
- **Niveau de journalisation minimal** : Trace, Debug, Info, Warning, Error

### Paramètres Web

- **Port Web** : Port d'accès au système (par défaut : 8080)
- **Autoriser l'accès LAN** : Si les autres appareils du réseau local peuvent accéder
- **Thème Web** : Thème de l'interface

### Paramètres utilisateur

- **Pseudo utilisateur** : Votre nom d'affichage dans le système

## Méthodes d'édition pour différents types

- **Texte** : Zone de saisie de texte
- **Numérique** : Zone de saisie numérique
- **Booléen** : Case à cocher
- **Énumération** : Liste déroulante
- **Intervalle de temps** : Quatre zones de saisie (jours, heures, minutes, secondes)
- **Chemin de répertoire** : Zone de saisie de chemin + bouton ""Parcourir""
- **Dictionnaire** : Éditeur de paires clé-valeur

## Questions fréquentes

### Q : Impossible d'accéder au système après avoir modifié le port ?

Vérifiez si le port est occupé, si le pare-feu l'autorise, et accédez via le nouveau port.

### Q : Comment restaurer la configuration par défaut ?

Méthode 1 : Modifiez manuellement les valeurs. Méthode 2 : Supprimez `config.json` et redémarrez.

### Q : Que faire si la connexion IA échoue ?

Vérifiez : le service IA fonctionne-t-il ? L'adresse endpoint est-elle correcte ? La clé API est-elle correcte ? La connexion réseau est-elle normale ?

### Q : Quand les modifications prennent-elles effet ?

- **Effet immédiat** : Langue, thème, configuration IA, pseudo
- **Nécessite un redémarrage** : Port Web, accès LAN
";

    public override string FAQ => @"
# Questions fréquentes

## Démarrage

### Q : Comment démarrer le système ?

Double-cliquez sur le fichier programme. Le système ouvrira automatiquement le navigateur.

### Q : Que dois-je faire au premier démarrage ?

Rien ! Le système complétera automatiquement l'initialisation.

### Q : Le navigateur ne s'est pas ouvert après le démarrage ?

Visitez manuellement `http://localhost:8080`.

## Conversation IA

### Q : Que faire si l'IA répond lentement ?

Causes possibles : modèle volumineux, latence réseau, long historique. Solutions : utilisez Ollama, choisissez un modèle plus léger.

### Q : La réponse de l'IA ne correspond pas aux attentes ?

Vérifiez le fichier âme, fournissez plus de contexte, décrivez vos besoins plus spécifiquement.

### Q : L'IA n'appelle pas d'outils ?

Vérifiez : l'outil est-il activé ? Y a-t-il des restrictions de permission ? Le modèle prend-il en charge les appels d'outils ?

### Q : Comment faire analyser des fichiers par l'IA ?

Cliquez sur ""📁 Fichier"", entrez le chemin complet du fichier, cliquez sur ""Confirmer le téléchargement"".

## Silicon Beings

### Q : Comment créer un nouveau silicon being ?

Le système ne prend pas en charge la création directe. Discutez avec le Silicon Curator pour qu'il vous aide à en créer un.

### Q : Comment modifier le comportement d'un silicon being ?

Allez sur la page ""Beings"", cliquez sur le being, cliquez sur ""Fichier âme"", modifiez et sauvegardez.

### Q : Comment configurer différentes IA pour différents beings ?

Allez sur la page ""Beings"", cliquez sur le being, cliquez sur ""Client IA"", sélectionnez et configurez.

### Q : Le silicon being ne répond pas ?

Vérifiez : le service IA fonctionne-t-il ? La connexion réseau est-elle normale ? Consultez les journaux système.

## Paramètres système

### Q : Comment changer la langue du système ?

Allez sur la page ""Config"", trouvez ""Langue"", sélectionnez la langue et sauvegardez.

### Q : Comment changer le thème de l'interface ?

Allez sur la page ""Config"", trouvez ""Thème Web"", sélectionnez et sauvegardez.

### Q : Comment modifier le port d'accès ?

Allez sur la page ""Config"", trouvez ""Port Web"", entrez le nouveau port, sauvegardez et redémarrez.

### Q : Comment autoriser l'accès LAN ?

Allez sur la page ""Config"", trouvez ""Autoriser l'accès LAN"", cochez ""Oui"" et sauvegardez. Privilèges d'administrateur requis.

## Historique de chat

### Q : Comment voir les conversations passées ?

Allez sur la page ""Beings"", cliquez sur le being, cliquez sur ""Historique de chat"".

### Q : Comment supprimer l'historique de conversation ?

Le système ne fournit actuellement pas cette fonctionnalité.

## Données et stockage

### Q : Où les données sont-elles stockées ?

Par défaut dans le dossier `data` sous le répertoire d'exécution du programme.

### Q : Comment sauvegarder les données ?

Copiez l'intégralité du dossier `data` vers un emplacement sûr.

### Q : Comment migrer vers un nouvel ordinateur ?

Copiez le dossier `data`, installez le système sur le nouvel ordinateur, placez le dossier `data` et démarrez.

## Fichier de configuration

### Q : Où se trouve le fichier de configuration ?

Le fichier `config.json` dans le répertoire d'exécution du programme.

### Q : Puis-je éditer directement le fichier de configuration ?

Oui, mais il est recommandé de modifier via l'interface Web.

### Q : Que faire si j'ai fait une erreur dans la configuration ?

Supprimez `config.json` et redémarrez le système.

## Problèmes de performance

### Q : Le système fonctionne lentement ?

Utilisez des services IA locaux, choisissez des modèles plus légers, réduisez les tâches simultanées.

### Q : Utilisation mémoire élevée ?

Utilisez des modèles plus légers, nettoyez régulièrement les données inutiles.

## Autres questions

### Q : Quelles langues le système prend-il en charge ?

Chinois simplifié, chinois traditionnel, anglais, japonais, coréen, allemand, espagnol, français, etc.

### Q : Ai-je besoin d'une connexion Internet ?

IA locale (Ollama) : non. IA cloud (OpenAI) : oui.

### Q : Le système est-il sécurisé ?

Oui. Le système intègre des mécanismes de gestion des permissions. Les opérations sensibles demanderont votre confirmation.

### Q : Puis-je personnaliser les fonctionnalités ?

Le système prend en charge l'extension via l'écriture de code, mais cela nécessite des connaissances en programmation.
";

    public override string Memory => @"
# Système de mémoire

## Qu'est-ce que le système de mémoire ?

Le système de mémoire enregistre tout l'historique d'activité des silicon beings, y compris les conversations, les appels d'outils, les événements système, etc.

## Comment accéder au système de mémoire ?

1. Cliquez sur l'icône **🧠 Beings** à gauche
2. Cliquez sur la carte du silicon being que vous souhaitez voir
3. Trouvez le lien ""Mémoire"" dans les détails à droite
4. Cliquez pour entrer dans la page de mémoire

## Types de mémoires

- **Conversation** : Conversations entre l'utilisateur et l'IA
- **Appel d'outil** : Enregistrements d'exécution des outils appelés par l'IA
- **Événement système** : Événements importants pendant le fonctionnement du système
- **Résumé** : Résumés compressés des conversations ou événements

## Voir les mémoires

### Parcourir la liste des mémoires

La page affiche la liste des mémoires pour ce being. Chaque mémoire montre : icône de type, résumé du contenu, heure, statut.

### Voir les détails d'une mémoire

Cliquez sur n'importe quelle entrée de mémoire pour afficher : contenu complet, horodatage, paramètres associés, résultat d'exécution.

### Tracer le contexte original

Pour certaines entrées, le système fournit une fonction ""Tracer"" qui affiche le contexte complet au moment de la mémoire.

## Filtrer les mémoires

### Filtrer par type

Sélectionnez le type de mémoire à voir : conversations uniquement, appels d'outils uniquement, événements système, résumés.

### Filtrer par temps

Entrez une date de début et de fin pour ne voir que les mémoires dans cette période.

### Recherche par mots-clés

Entrez des mots-clés dans la zone de recherche. La recherche porte sur le contenu complet des mémoires.

### Afficher résumé ou enregistrements originaux

- **Tout afficher** : Toutes les mémoires
- **Résumés uniquement** : Uniquement les enregistrements compressés
- **Originaux uniquement** : Uniquement les enregistrements détaillés

## Questions fréquentes

### Q : Comment trouver une conversation spécifique ?

1. Entrez des mots-clés dans la zone de recherche
2. Sélectionnez ""Conversation"" dans le filtre de type
3. Définissez la période si vous la connaissez

### Q : Que faire si les mémoires prennent trop de place ?

Les mémoires sont gérées automatiquement. Le système crée des résumés pour compresser l'historique.

### Q : Puis-je supprimer des mémoires ?

Le système ne fournit pas cette fonctionnalité. Les mémoires sont importantes pour les silicon beings.

### Q : Pourquoi certaines mémoires sont-elles des ""Résumés"" ?

Le système compresse automatiquement les conversations ou événements longs en résumés pour économiser l'espace de stockage.

## Suggestions d'utilisation

1. **Vérifier régulièrement** : Comprendre l'activité des silicon beings
2. **Utiliser les filtres** : Localiser rapidement les informations nécessaires
3. **Utiliser la trace** : Comprendre le processus décisionnel de l'IA
4. **Attention aux statistiques** : Comprendre l'état de fonctionnement du système
";

    public override string OllamaSetup => @"
# Installation Ollama et téléchargement de modèles

## Qu'est-ce qu'Ollama ?

Ollama est un outil d'exécution de modèles IA locaux open source qui vous permet d'exécuter des grands modèles de langage sur votre propre ordinateur sans connexion Internet (après le téléchargement du modèle).

**Avantages :**
- Fonctionne complètement en local, protège la vie privée
- Prend en charge plusieurs modèles IA
- Facile à installer et utiliser
- Gratuit et open source

## Télécharger et installer Ollama

### Windows

1. Visitez https://ollama.com/download
2. Téléchargez l'installateur Windows (ollama-setup.exe)
3. Double-cliquez sur le fichier téléchargé et suivez l'assistant d'installation
4. Après l'installation, Ollama démarrera automatiquement

### Vérifier l'installation

Ouvrez l'Invite de commandes et entrez : `ollama --version`

## Télécharger et exécuter des modèles

### Qu'est-ce qu'un modèle ?

Un modèle est le ""cerveau"" de l'IA, déterminant ses capacités. Différents modèles ont différentes caractéristiques.

### Intelligence du modèle (Unité B)

L'intelligence du modèle IA est mesurée en **B (Milliard de paramètres)** :
- **7B-8B** : Niveau de base, peut accomplir des tâches simples
- **13B-14B** : Niveau moyen, performant pour la plupart des tâches quotidiennes
- **32B et plus** : Niveau supérieur, raisonnement complexe plus fort

**Ce système recommande d'utiliser des modèles au-dessus de 8B** pour une meilleure expérience.

### Modèles locaux vs modèles cloud

**Modèles locaux :** Téléchargés sur votre ordinateur, fonctionnent sans Internet, gratuits, limités par votre matériel.

**Modèles cloud :** Fonctionnent sur les serveurs Ollama, nécessitent Internet, ont des quotas d'utilisation.

### Modèles recommandés

| Modèle | Intelligence | Taille | Caractéristiques |
|--------|-------------|--------|------------------|
| **qwen3.5:8b** | 8B | 4-5 Go | Bonne capacité chinoise |
| **qwen3.5:14b** | 14B | 8-9 Go | Capacité chinoise renforcée |
| **llama3:8b** | 8B | 4-5 Go | Bonne capacité anglaise |
| **gemma3:4b** | 4B | 2-3 Go | Léger, rapide |
| **mistral:7b** | 7B | 4 Go | Équilibre performance/vitesse |

**Recommandé pour les utilisateurs francophones : qwen3.5:8b ou qwen3.5:14b**

### Télécharger un modèle

```bash
ollama pull qwen3.5
```

### Exécuter un modèle

```bash
ollama run qwen3.5
```

### Voir les modèles téléchargés

```bash
ollama list
```

## Utiliser Ollama dans Silicon Life

1. Assurez-vous qu'Ollama est démarré et en cours d'exécution
2. Ouvrez le système Silicon Life
3. Allez sur la page **⚙ Config**
4. Sélectionnez `OllamaClient` comme type de client IA
5. Configurez : endpoint = `http://localhost:11434`, model = `qwen3.5`
6. Sauvegardez la configuration

## Questions fréquentes

### Q : Le téléchargement d'Ollama est très lent ?

Les fichiers de modèle sont volumineux (2-8 Go). Assurez-vous d'avoir une connexion stable.

### Q : Que faire si le téléchargement est interrompu ?

Relancez la commande de téléchargement, elle reprendra où elle s'est arrêtée.

### Q : Quelle taille de modèle mon ordinateur peut-il exécuter ?

- **4 Go RAM** : Modèles < 2 Go (environ 2B-3B)
- **8 Go RAM** : Modèles de 4 Go (environ 7B-8B)
- **16 Go RAM** : Modèles de 8 Go (environ 13B-14B)
- **32 Go RAM** : Modèles de 16 Go (environ 32B, avec lags possibles)
- **64 Go+** : Modèles plus grands

### Q : Ollama nécessite-t-il une connexion Internet ?

Téléchargement des modèles : oui. Exécution des modèles : non.
";

    public override string BailianDashScope => @"
# Guide utilisateur de la plateforme Alibaba Cloud Bailian

## Qu'est-ce qu'Alibaba Cloud Bailian ?

Alibaba Cloud Bailian (DashScope) est une plateforme de services de grands modèles fournie par Alibaba Cloud, offrant plusieurs modèles IA de haute qualité.

**Avantages :**
- Intelligence de modèle élevée (jusqu'à des centaines de B)
- Aucun matériel local requis
- Prend en charge plusieurs modèles IA de premier plan
- Paiement à l'utilisation, coût contrôlable
- Compatible avec le format API OpenAI

## Activation du service

### Étape 1 : Enregistrer un compte Alibaba Cloud

1. Visitez https://www.aliyun.com
2. Cliquez sur ""Inscription gratuite""
3. Complétez l'inscription et l'authentification réelle

### Étape 2 : Activer le service Bailian

1. Connectez-vous à la console Alibaba Cloud
2. Recherchez ""Bailian"" ou ""DashScope""
3. Cliquez pour entrer dans la page du produit Bailian
4. Cliquez sur ""Activer maintenant""

### Étape 3 : Obtenir la clé API

1. Entrez dans la console Bailian
2. Trouvez ""Gestion des clés API""
3. Cliquez sur ""Créer une clé API""
4. Copiez et sauvegardez la clé API

## Configurer Bailian dans Silicon Life

1. Ouvrez le système Silicon Life
2. Allez sur la page **⚙ Config**
3. Sélectionnez `DashScopeClient` comme type de client IA
4. Remplissez : API Key, Région, Modèle
5. Sauvegardez la configuration

### Régions disponibles

| Région | Emplacement | Utilisateurs recommandés |
|--------|-------------|------------------------|
| `beijing` | Pékin, Chine | Utilisateurs Chine continentale |
| `singapore` | Singapour | Asie du Sud-Est |
| `hongkong` | Hong Kong, Chine | Hong Kong, Macao, Taïwan |
| `virginia` | Virginie, États-Unis | Amérique du Nord |
| `frankfurt` | Francfort, Allemagne | Europe |

### Modèles recommandés

| Modèle | Caractéristiques | Scénarios |
|--------|-----------------|-----------|
| `qwen3.6-plus` | Performance équilibrée (recommandé) | Usage quotidien |
| `qwen3-max` | Capacité la plus forte | Tâches complexes |
| `qwen3.6-flash` | Réponse rapide | Questions simples |
| `deepseek-v3.2` | Modèle tiers | Scénarios généraux |
| `deepseek-r1` | Modèle de raisonnement | Réflexion approfondie |
| `glm-5.1` | Modèle Zhipu | Scénarios chinois |

## Explication des coûts

La plateforme Bailian utilise la facturation **à l'utilisation** :
- Facturé par nombre de tokens d'entrée
- Différents modèles ont des prix différents
- Les nouveaux utilisateurs ont généralement un quota d'essai gratuit

### Remarque sur les statistiques de tokens

La plateforme Bailian ne renvoie pas systématiquement les champs d'utilisation de tokens dans ses réponses API. Le système ne peut donc pas statistiquer l'utilisation des tokens avec les modèles Bailian. Consultez la console Bailian pour voir l'utilisation réelle.

## Questions fréquentes

### Q : Où obtenir la clé API ?

Connectez-vous à la console Bailian, trouvez ""Gestion des clés API"", créez une nouvelle clé.

### Q : Quelle région choisir ?

Utilisateurs domestiques : beijing (Pékin). Utilisateurs overseas : la région la plus proche.

### Q : Quelle est la différence entre Bailian et Ollama ?

| Fonctionnalité | Bailian | Ollama |
|---------------|---------|--------|
| Lieu d'exécution | Cloud | Ordinateur local |
| Configuration matérielle | Aucune | Requise |
| Taille du modèle | Jusqu'à des centaines de B | Généralement 4B-70B |
| Coût | À l'utilisation | Gratuit |
| Internet | Obligatoire | Non requis après téléchargement |
| Confidentialité | Données envoyées au cloud | Complètement local |
";

    public override string VolcengineArk => @"
# Guide utilisateur de la plateforme Volcengine Ark

## Qu'est-ce que Volcengine Ark ?

Volcengine Ark est la plateforme de services de grands modèles de langage tout-en-un de ByteDance, proposant la série Doubao et divers modèles tiers (DeepSeek, GLM, Kimi, etc.).

**Avantages :**
- Modèles phares de la série Doubao Seed, intelligence exceptionnelle (contexte 256K)
- Aucun matériel local requis, fonctionne dans le cloud
- Prend en charge plusieurs modèles d'IA de premier plan
- Paiement à l'utilisation, tarifs très compétitifs
- Entièrement compatible avec le format OpenAI API
- Plateforme chinoise, accès stable sans réseau spécial

## Inscription et configuration

### Étape 1 : Créer un compte Volcengine

1. Accédez à la console Volcengine : https://console.volcengine.com
2. Cliquez sur ""Inscription gratuite""
3. Terminez l'inscription (numéro de téléphone, e-mail)
4. Effectuez la vérification d'identité

### Étape 2 : Activer le service Ark

1. Connectez-vous à la console Volcengine
2. Dans le menu de gauche, trouvez ""Ark""
3. Accédez à la page produit Ark
4. Cliquez sur ""Activer maintenant""
5. Lisez et acceptez le contrat de service

### Étape 3 : Créer un point de terminaison d'inférence

Volcengine Ark utilise des ""points de terminaison d'inférence"" pour identifier les modèles, plutôt que d'utiliser directement les noms de modèles.

1. Dans la console Ark, accédez à la page ""Points de terminaison d'inférence""
2. Cliquez sur ""Créer un point de terminaison d'inférence""
3. Sélectionnez le modèle à utiliser (ex. doubao-seed-1-6-251015)
4. Nommez votre point de terminaison
5. Après la création, copiez l'ID du point de terminaison (format : ep-20241212123456-abcde)

### Étape 4 : Obtenir une clé API

1. Dans la console Ark, trouvez ""Gestion des clés API""
2. Cliquez sur ""Créer une clé API""
3. Nommez votre clé (ex. ""SiliconLife"")
4. Copiez et enregistrez la clé API (**affichée une seule fois, conservez-la en sécurité**)

## Configuration de Volcengine Ark dans Silicon Life

### Étapes de configuration

1. Ouvrez le système Silicon Life
2. Allez dans la page **⚙ Configuration**
3. Trouvez ""Type de client IA"", sélectionnez `VolcengineArkClientFactory`
4. Dans ""Configuration IA"", remplissez :
   - **Clé API** : collez votre clé API
   - **ID du point de terminaison d'inférence** : collez votre ID de point de terminaison (ex. ep-20241212123456-abcde)
   (**Après avoir saisi la clé API, le système récupérera automatiquement votre liste de points de terminaison**)
5. Enregistrez la configuration

**Astuce :**
- La clé API doit être saisie en premier pour que la liste déroulante des points de terminaison se charge
- Si le chargement échoue, vous pouvez saisir manuellement l'ID du point de terminaison

### Sélection du modèle

Volcengine Ark appelle les modèles via des points de terminaison d'inférence, chaque point de terminaison correspond à un modèle.

**Modèles populaires :**

| Série de modèles | ID du modèle | Caractéristiques |
|----------------|-------------|-----------------|
| **Doubao-Seed-1.8** | doubao-seed-1-8-YYMMDD | Dernier modèle phare, capacités Agent renforcées |
| **Doubao-Seed-1.6** | doubao-seed-1-6-251015 | Modèle polyvalent, recommandé pour un usage quotidien |
| **Doubao-Seed-1.6-Flash** | doubao-seed-1-6-flash | Version à très faible latence |
| **Doubao-Seed-1.6-Lite** | doubao-seed-1-6-lite | Léger, excellent rapport qualité-prix |
| **Doubao-Seed-Code** | doubao-seed-code-preview-latest | Modèle spécialisé en code |
| **Doubao-Pro-32K** | doubao-pro-32k | Version professionnelle classique |
| **Doubao-Lite-32K** | doubao-lite-32k | Version légère classique |

## Tarifs

### Méthode de facturation

Volcengine Ark utilise la facturation **à l'utilisation** :
- Facturation basée sur le nombre de tokens d'entrée et de sortie
- Prix unifiés pour la série Doubao Seed :
  - Entrée 0~32K : 0,8 yuan/million de tokens
  - Sortie 0~32K : 8 yuan/million de tokens
  - Entrée 32K~128K : 1,2 yuan/million de tokens
  - Contexte plus long, tarif plus élevé

### Quota gratuit

- Les nouveaux utilisateurs reçoivent 500 000 tokens d'inférence gratuits par modèle (vérification d'identité requise)
- Quota gratuit valable 30 jours
- Prend en charge le ""mode sécurisé"" qui ne consomme que le quota gratuit et s'arrête automatiquement à épuisement

### Conseils pour économiser

- Choisissez le modèle approprié (la série Lite offre un excellent rapport qualité-prix)
- Contrôlez judicieusement la longueur des entrées
- Utilisez le cache de contexte (Context Cache) pour réduire les coûts
- Vérifiez régulièrement votre utilisation

## FAQ

### Q : Qu'est-ce qu'un point de terminaison d'inférence ?

**R :** Un point de terminaison d'inférence est un concept unique de Volcengine Ark. Vous créez d'abord un point de terminaison dans la console Ark, sélectionnez le modèle à utiliser, et le système attribue un ID de point de terminaison. Lorsque vous configurez cet ID dans Silicon Life, vous pouvez appeler le modèle correspondant.

### Q : Où puis-je obtenir ma clé API ?

**R :**
1. Connectez-vous à la console Volcengine
2. Allez dans ""Gestion des clés API"" d'Ark
3. Créez une nouvelle clé API
4. Copiez-la et conservez-la en sécurité

### Q : Quelle est la différence entre Volcengine Ark et Bailian ?

| Fonctionnalité | Volcengine Ark | Bailian (DashScope) |
|--------------|--------------|-------------------|
| Fournisseur | ByteDance | Alibaba Cloud |
| Modèles phares | Série Doubao Seed | Série Qwen |
| Appel de modèle | Point de terminaison d'inférence | Nom du modèle direct |
| Tarif | Très compétitif | Moyen |
| Quota gratuit | 500K tokens par modèle | Essai gratuit |
| Accès en Chine | Stable | Stable |

### Q : Que faire si l'appel API échoue ?

**Vérifiez :**
1. La clé API est-elle correcte ?
2. L'ID du point de terminaison est-il correct et publié ?
3. Le solde du compte est-il suffisant ?
4. Le quota gratuit est-il épuisé ?
5. Le point de terminaison est-il en cours d'exécution ?

Profitez bien du système !
";

    public override string HerdsmanSetup => @"
# Guide de configuration du moteur d'inférence Herdsman

## Qu'est-ce que Herdsman ?

Herdsman est un moteur d'inférence local léger, conçu pour exécuter des grands modèles de langage sur votre propre matériel. Il fournit une API compatible OpenAI, s'intégrant facilement à Silicon Life.

**Avantages :**
- Fonctionnement entièrement local, protection de la vie privée
- Format d'API compatible OpenAI
- Prise en charge de plusieurs modèles open source
- Faible consommation de ressources
- Aucune connexion réseau requise après le téléchargement des modèles

## Étapes d'installation

### Étape 1 : Téléchargement et installation

Visitez la page de publication de Herdsman et téléchargez la version adaptée à votre système d'exploitation :
- Windows : téléchargez le fichier `.zip` et décompressez-le
- Linux : téléchargez le fichier `.tar.gz` et décompressez-le
- macOS : téléchargez le fichier `.dmg` et installez-le

### Étape 2 : Démarrage du service

Après décompression, exécutez le fichier exécutable Herdsman :
- Windows : double-cliquez sur `herdsman.exe` ou exécutez-le depuis la ligne de commande
- Linux/macOS : `./herdsman --serve`

Le service démarre par défaut sur le port `8080`. Accédez à `http://localhost:8080/v1/models` pour vérifier son fonctionnement.

### Étape 3 : Téléchargement de modèles

Utilisez l'interface CLI de Herdsman pour télécharger des modèles :
```
herdsman pull <nom_du_modèle>
```

## Configuration dans Silicon Life

1. Sélectionnez **Herdsman** comme type de client IA
2. Définissez le **Point de terminaison Herdsman** sur l'adresse de votre service Herdsman (par défaut : `http://localhost:8080`)
3. Définissez le **Modèle** sur le nom du modèle téléchargé
4. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Modèles disponibles

Herdsman prend en charge tous les modèles au format GGUF, y compris :
- Série Llama
- Série Qwen
- Série Mistral

## FAQ

1. Assurez-vous que le service Herdsman est en cours d'exécution avant de démarrer Silicon Life
2. Vérifiez que l'URL du point de terminaison est correcte et accessible
3. Confirmez que le nom du modèle correspond au modèle téléchargé
4. Vérifiez que les ressources système (mémoire/GPU) sont suffisantes
";

    public override string LongCatSetup => @"
# Guide de configuration du grand modèle LongCat de Meituan

## Qu'est-ce que LongCat ?

LongCat est une plateforme de grands modèles de langage développée par Meituan, offrant des capacités IA puissantes via une API compatible OpenAI.

**Avantages :**
- Développé par Meituan, excellente compréhension du chinois
- Format d'API compatible OpenAI
- Optimisé pour les scénarios métier
- Prise en charge de fenêtres de contexte longues
- Exécution dans le cloud, aucun matériel local requis

## Démarrage

### Étape 1 : Inscription

Visitez la plateforme ouverte LongCat pour vous inscrire :
- URL : `https://longcat.sensetime.com`
- Inscrivez-vous avec votre numéro de téléphone ou e-mail
- Effectuez l'authentification réelle

### Étape 2 : Obtenir la clé API

1. Connectez-vous à la plateforme LongCat
2. Accédez à **Gestion API** → **API Keys**
3. Cliquez sur **Créer une clé API**
4. Copiez et sauvegardez soigneusement votre clé API

### Étape 3 : Consulter les modèles disponibles

Consultez les modèles disponibles dans la section **Liste des modèles** de la plateforme.

## Configuration dans Silicon Life

1. Sélectionnez **LongCat** comme type de client IA
2. Définissez la **Clé API** sur votre clé API LongCat
3. Définissez le **Point de terminaison** sur l'adresse API LongCat (le point de terminaison par défaut est préconfiguré)
4. Définissez le **Modèle** sur le nom du modèle souhaité
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Mode de facturation

LongCat facture à l'usage de Tokens. Consultez la plateforme pour connaître les tarifs actuels.

## FAQ

1. Confirmez que la clé API est correcte et valide
2. Assurez-vous que le solde du compte est suffisant
3. Vérifiez que le nom du modèle est valide
4. Vérifiez la connexion réseau au point de terminaison API LongCat
";

    public override string QiniuAISetup => @"
# Guide de configuration de l'IA Qiniu Cloud

## Qu'est-ce que Qiniu Cloud AI ?

Qiniu Cloud AI est un service d'inférence de grands modèles fourni par Qiniu Cloud, offrant des capacités IA hautes performances via une interface API.

**Avantages :**
- Inférence hautes performances, faible latence
- Format d'API compatible OpenAI
- Prise en charge de plusieurs modèles populaires
- Mise à l'échelle automatique dans le cloud
- Infrastructure cloud fiable

## Démarrage

### Étape 1 : Inscription à Qiniu Cloud

1. Visitez `https://www.qiniu.com`
2. Inscrivez-vous et effectuez l'authentification du compte
3. Activez le service d'inférence IA

### Étape 2 : Obtenir la clé API

1. Connectez-vous à la console Qiniu Cloud
2. Accédez à **Services IA** → **Gestion API**
3. Créez ou consultez votre clé API
4. Copiez la clé API et l'URL du point de terminaison

### Étape 3 : Consulter les modèles disponibles

Consultez les modèles disponibles dans la section Services IA de la console.

## Configuration dans Silicon Life

1. Sélectionnez **Qiniu AI** comme type de client IA
2. Définissez la **Clé API** sur votre clé API Qiniu Cloud
3. Définissez le **Point de terminaison** sur l'adresse API du service d'inférence
4. Définissez le **Modèle** sur le nom du modèle souhaité
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Mode de facturation

Qiniu Cloud AI facture à l'usage. Consultez la plateforme pour connaître les informations tarifaires détaillées.

## FAQ

1. Assurez-vous que la clé API et le point de terminaison sont corrects
2. Vérifiez que le solde ou le quota du compte est suffisant
3. Vérifiez que le nom du modèle est pris en charge
4. Confirmez la connexion réseau à l'API Qiniu Cloud
";

    public override string DeepSeekSetup => @"
# Guide de configuration de la plateforme DeepSeek AI

## Qu'est-ce que DeepSeek ?

DeepSeek est une entreprise IA spécialisée dans le développement de grands modèles de langage hautes performances, reconnue pour ses capacités exceptionnelles de raisonnement et de programmation. L'API DeepSeek fournit une interface compatible OpenAI.

**Avantages :**
- Capacités de raisonnement et mathématiques exceptionnelles
- Génération et compréhension de code excellentes
- Format d'API compatible OpenAI
- Prix très compétitifs
- Prise en charge de fenêtres de contexte longues (128K)

## Démarrage

### Étape 1 : Inscription

1. Visitez `https://platform.deepseek.com`
2. Inscrivez-vous avec votre e-mail ou numéro de téléphone
3. Effectuez la vérification du compte

### Étape 2 : Obtenir la clé API

1. Connectez-vous à la plateforme DeepSeek
2. Accédez à la page **API Keys**
3. Cliquez sur **Créer une clé API**
4. Copiez et sauvegardez soigneusement votre clé API

### Étape 3 : Recharger le solde

DeepSeek utilise un mode prépayé. Vous devez recharger avant d'utiliser l'API.

## Modèles disponibles

- **deepseek-v4-flash** : modèle phare le plus récent, capacités de raisonnement de premier niveau (contexte 128K)
- **deepseek-chat** : modèle de conversation équilibré (contexte 64K)

## Configuration dans Silicon Life

1. Sélectionnez **DeepSeek** comme type de client IA
2. Définissez la **Clé API** sur votre clé API DeepSeek
3. Définissez le **Modèle** sur le nom du modèle souhaité (ex. `deepseek-v4-flash`)
4. Définissez le **Point de terminaison** sur l'adresse API DeepSeek (par défaut : `https://api.deepseek.com`, laissez vide pour la valeur par défaut)
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Mode de facturation

DeepSeek facture à l'usage :
- Les Tokens d'entrée et de sortie sont facturés séparément
- Consultez `https://api-docs.deepseek.com` pour connaître les tarifs actuels

## FAQ

1. Confirmez que la clé API est correcte
2. Assurez-vous que le solde du compte est suffisant
3. Vérifiez que le nom du modèle est valide
4. Si vous utilisez un point de terminaison personnalisé, vérifiez l'URL du point de terminaison
";

    public override string ZhipuSetup => @"
# Guide de configuration de la plateforme Zhipu AI (GLM)

## Qu'est-ce que Zhipu AI ?

Zhipu AI est une entreprise IA de premier plan en Chine, développant la série GLM (General Language Model). La plateforme ouverte Zhipu fournit un accès API à ses modèles puissants, compatible OpenAI.

**Avantages :**
- Série de modèles GLM, excellentes capacités bilingues chinois-anglais
- Excellentes capacités de programmation et d'agent
- Format d'API compatible OpenAI
- Quotas gratuits pour certains modèles
- Prise en charge de tâches multimodales

## Démarrage

### Étape 1 : Inscription

1. Visitez `https://open.bigmodel.cn`
2. Inscrivez-vous avec votre numéro de téléphone
3. Effectuez l'authentification réelle

### Étape 2 : Obtenir la clé API

1. Connectez-vous à la plateforme ouverte Zhipu
2. Accédez à **Gestion API** → **API Keys**
3. Créez une nouvelle clé API
4. Copiez et sauvegardez votre clé API

### Étape 3 : Consulter les quotas gratuits

Les nouveaux utilisateurs peuvent obtenir des Tokens gratuits pour GLM-4-Flash. Consultez les détails dans **Gestion de facturation**.

## Modèles disponibles

- **glm-5.1-plus** : modèle phare, raisonnement approfondi (contexte 128K)
- **glm-5.1** : excellent rapport qualité-prix, performances puissantes (contexte 128K)
- **glm-5.1-flash** : rapide et économique (contexte 128K)
- **glm-4-flash** : modèle gratuit (contexte 128K)
- **glm-4-flashx** : vitesse ultra-rapide, coût réduit (contexte 128K)
- **glm-4-air** : léger, excellent rapport qualité-prix (contexte 128K)
- **glm-4-airx** : léger, vitesse élevée (contexte 8K)
- **glm-4-long** : contexte long, 1 million de Tokens (contexte 1M)
- **glm-4-plus** : modèle phare stable (contexte 128K)
- **codegeex-4** : modèle de génération de code (contexte 128K)

## Configuration dans Silicon Life

1. Sélectionnez **Zhipu GLM** comme type de client IA
2. Définissez la **Clé API** sur votre clé API Zhipu
3. Définissez le **Modèle** sur le nom du modèle souhaité
4. Définissez le **Point de terminaison** sur l'adresse API (laissez vide pour la valeur par défaut)
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Mode de facturation

- GLM-4-Flash est gratuit
- Les autres modèles sont facturés à l'usage
- Consultez `https://open.bigmodel.cn/pricing` pour plus de détails

## FAQ

1. Confirmez que la clé API est correcte
2. Vérifiez si le quota gratuit est épuisé
3. Assurez-vous que le nom du modèle correspond aux modèles disponibles
4. Vérifiez la connexion réseau à l'API Zhipu
";

    public override string MoonshotSetup => @"
# Guide de configuration de la plateforme Moonshot (Kimi)

## Qu'est-ce que Moonshot ?

Moonshot est l'entreprise derrière Kimi, l'un des assistants IA les plus populaires en Chine. La plateforme Moonshot fournit un accès API aux modèles Kimi, reconnus pour leur prise en charge de fenêtres de contexte ultra-longues.

**Avantages :**
- Prise en charge de fenêtres de contexte ultra-longues (jusqu'à 2 millions de Tokens)
- Excellente compréhension du chinois
- Format d'API compatible OpenAI
- Capacités de traitement de documents puissantes
- Prix compétitifs

## Démarrage

### Étape 1 : Inscription

1. Visitez `https://platform.moonshot.cn`
2. Inscrivez-vous avec votre numéro de téléphone
3. Effectuez l'authentification réelle

### Étape 2 : Obtenir la clé API

1. Connectez-vous à la plateforme Moonshot
2. Accédez à **Gestion des clés API**
3. Créez une nouvelle clé API
4. Copiez et sauvegardez soigneusement

### Étape 3 : Recharger le solde

Rechargez votre compte. Les nouveaux utilisateurs peuvent recevoir des Tokens d'essai gratuits.

## Modèles disponibles

- **kimi-latest** : modèle phare le plus récent, raisonnement approfondi (contexte 256K)
- **moonshot-v1-128k** : modèle standard, contexte long (contexte 128K)
- **moonshot-v1-32k** : modèle standard (contexte 32K)
- **moonshot-v1-8k** : modèle standard, économique (contexte 8K)
- **moonshot-v1-200k** : modèle longue portée (contexte 200K)
- **moonshot-v1-auto** : sélection automatique du modèle selon la longueur d'entrée (contexte 192K)

## Configuration dans Silicon Life

1. Sélectionnez **Moonshot Kimi** comme type de client IA
2. Définissez la **Clé API** sur votre clé API Moonshot
3. Définissez le **Modèle** sur le nom du modèle souhaité
4. Définissez le **Point de terminaison** sur l'adresse API (laissez vide pour la valeur par défaut)
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Mode de facturation

Moonshot facture à l'usage de Tokens, les tarifs varient selon les modèles. Consultez `https://platform.moonshot.cn/docs/pricing` pour plus de détails.

## FAQ

1. Confirmez que la clé API est correcte
2. Assurez-vous que le solde du compte est suffisant
3. Vérifiez que le nom du modèle est valide
4. Lors du traitement de documents longs, assurez-vous que le modèle prend en charge la longueur de contexte requise
";

    public override string SiliconFlowSetup => @"
# Guide de configuration de la plateforme d'inférence SiliconFlow AI

## Qu'est-ce que SiliconFlow ?

SiliconFlow est une plateforme d'inférence IA dans le cloud, offrant un accès unifié via une API compatible OpenAI à une variété de grands modèles de langage open source et commerciaux.

**Avantages :**
- Accès à de nombreux modèles open source (Qwen, DeepSeek, Llama, etc.)
- API compatible OpenAI unifiée pour tous les modèles
- Accélération optimisée, vitesse d'inférence élevée
- Quotas gratuits pour certains modèles
- Aucun matériel local requis

## Démarrage

### Étape 1 : Inscription

1. Visitez `https://siliconflow.cn`
2. Inscrivez-vous avec votre numéro de téléphone ou compte GitHub
3. Effectuez la configuration du compte

### Étape 2 : Obtenir la clé API

1. Connectez-vous à la plateforme SiliconFlow
2. Accédez à la page **API Keys**
3. Créez une nouvelle clé API
4. Copiez et sauvegardez

### Étape 3 : Parcourir les modèles disponibles

SiliconFlow héberge un grand nombre de modèles. Consultez toutes les options et tarifs disponibles dans la **Bibliothèque de modèles**.

## Configuration dans Silicon Life

1. Sélectionnez **SiliconFlow** comme type de client IA
2. Définissez la **Clé API** sur votre clé API SiliconFlow
3. Définissez le **Modèle** sur le nom du modèle souhaité (ex. `deepseek-ai/DeepSeek-V3`)
4. Définissez le **Point de terminaison** sur l'adresse API (laissez vide pour la valeur par défaut)
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Modèles disponibles

SiliconFlow offre un large choix de modèles, notamment :
- Série DeepSeek (V3, R1, etc.)
- Série Qwen
- Série Llama
- Série GLM
- Et d'autres modèles open source

Consultez la bibliothèque de modèles de la plateforme pour la liste complète.

## Mode de facturation

- Certains modèles (comme Qwen2.5-7B-Instruct) sont gratuits
- Les modèles payants sont facturés à l'usage
- Consultez `https://siliconflow.cn/pricing` pour plus de détails

## FAQ

1. Confirmez que la clé API est correcte
2. Assurez-vous que le format du nom du modèle est correct (ex. `fabricant/nom_du_modèle`)
3. Pour les modèles payants, vérifiez le solde du compte
4. Vérifiez la connexion réseau à l'API SiliconFlow
";

    public override string MiniMaxSetup => @"
# Guide de configuration de la plateforme MiniMax AI

## Qu'est-ce que MiniMax ?

MiniMax est une entreprise IA proposant des grands modèles de langage aux capacités puissantes en dialogue, raisonnement et tâches multimodales. La plateforme MiniMax fournit un accès API à ses modèles.

**Avantages :**
- Capacités de dialogue et de raisonnement puissantes
- Prise en charge de modèles multimodaux
- Format d'API compatible OpenAI
- Prise en charge des points de terminaison nationaux et internationaux
- Modèles optimisés pour agents

## Démarrage

### Étape 1 : Inscription

1. Visitez `https://platform.minimaxi.com` (national) ou `https://api.minimaxi.chat` (international)
2. Inscrivez-vous avec votre numéro de téléphone (national) ou e-mail (international)
3. Effectuez la vérification

### Étape 2 : Obtenir la clé API

1. Connectez-vous à la plateforme MiniMax
2. Accédez à la page **API Keys**
3. Créez une nouvelle clé API
4. Copiez et sauvegardez

### Étape 3 : Choisir le point de terminaison

MiniMax propose deux options de point de terminaison :
- **Point de terminaison national** : pour les utilisateurs en Chine continentale
- **Point de terminaison international** : pour les utilisateurs internationaux

Choisissez le point de terminaison approprié selon votre localisation.

## Modèles disponibles

- **MiniMax-M2** : modèle phare, raisonnement approfondi (contexte 245K)
- **abab6.5s-chat** : rapide et économique (contexte 245K)
- **MiniMax-Text-01** : modèle agent longue portée (contexte 1M)
- **abab6.5g-chat** : inférence haute vitesse (contexte 8K)
- **abab6.5t-chat** : faible latence, sans appel d'outils (contexte 8K)

## Configuration dans Silicon Life

1. Sélectionnez **MiniMax** comme type de client IA
2. Définissez la **Clé API** sur votre clé API MiniMax
3. Définissez le **Modèle** sur le nom du modèle souhaité
4. Définissez le **Point de terminaison** sur `domestic` (national) ou `international` (international)
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Mode de facturation

MiniMax facture à l'usage, les tarifs varient selon les modèles. Consultez la plateforme pour connaître les tarifs actuels.

## FAQ

1. Confirmez que la clé API est correcte
2. Assurez-vous d'avoir choisi le bon point de terminaison (national/international)
3. Vérifiez que le nom du modèle est valide
4. Vérifiez que le solde du compte est suffisant
";

    public override string ErnieSetup => @"
# Guide de configuration de la plateforme Baidu ERNIE (Qianfan)

## Qu'est-ce que Baidu ERNIE ?

Baidu ERNIE est la série de grands modèles de langage de Baidu, accessible via la plateforme Qianfan. Elle offre de puissantes capacités de compréhension et de génération en chinois.

**Avantages :**
- Excellente compréhension du chinois
- Riche intégration avec l'écosystème de services Baidu
- Plusieurs tailles de modèles pour différents scénarios
- Prise en charge de tâches multimodales
- Infrastructure cloud stable et fiable

## Démarrage

### Étape 1 : Inscription à Baidu Cloud

1. Visitez `https://cloud.baidu.com/product-s/qianfan_home`
2. Connectez-vous avec votre compte Baidu
3. Effectuez l'authentification réelle

### Étape 2 : Obtenir la clé API

1. Connectez-vous à la plateforme Qianfan
2. Accédez à **Gestion des applications** → **Créer une application**
3. Remplissez les informations de l'application et créez-la
4. Copiez la **clé API** et la **Secret Key**
5. Remarque : Silicon Life utilise directement la clé API Qianfan

### Étape 3 : Activer les modèles

Sur la plateforme Qianfan, accédez à **Gestion des modèles** et activez les modèles que vous souhaitez utiliser.

## Modèles disponibles

- **GLM-5.2** : Prend en charge un contexte 1M réellement utilisable, continue de mener dans les tâches à long terme
- **GLM-5.1** : Capacités de codage considérablement renforcées, amélioration notable des tâches à long terme, livre des résultats de niveau ingénierie
- **DeepSeek-V4-Pro** : Prend en charge un contexte ultra-long de millions de tokens, leader dans les capacités d'agent, la connaissance du monde et les performances de raisonnement, tant au niveau national que dans le domaine open-source
- **DeepSeek-V4-Flash** : Modèle léger efficace prenant en charge un contexte ultra-long de millions de tokens
- **Kimi-K2.6** : Capacité de rédaction de code à long terme plus forte et plus stable, prend en charge les entrées texte et image
- **ERNIE-5.1** : Dernier modèle de la série Wenxin, capacités fondamentales entièrement améliorées, progrès significatifs en matière d'agents, de connaissances, de raisonnement et de recherche approfondie
- **qianfan-code-latest** : Le choix du modèle est déterminé par la console Qianfan

1. Sélectionnez **Baidu Qianfan** comme type de client IA
2. Définissez la **Clé API** sur votre clé API Qianfan
3. Définissez le **Modèle** sur le nom du modèle Qianfan activé
4. Définissez le **Point de terminaison** sur l'adresse API Qianfan (laissez vide pour la valeur par défaut)
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## FAQ

1. Confirmez que la clé API est correcte
2. Assurez-vous d'avoir activé le modèle correspondant sur la plateforme Qianfan
3. Vérifiez si le quota gratuit est épuisé
4. Pour les modèles payants, confirmez que le solde du compte est suffisant
5. Assurez-vous d'avoir effectué l'authentification réelle
";

    public override string HunyuanSetup => @"
# Guide de configuration du grand modèle Tencent Hunyuan

## Qu'est-ce que Tencent Hunyuan ?

Tencent Hunyuan est la série de grands modèles de langage développée par Tencent, accessible via la plateforme Tencent Cloud. Elle offre de puissantes capacités bilingues chinois-anglais et une prise en charge multimodale.

**Avantages :**
- Excellentes capacités bilingues chinois-anglais
- Prise en charge de modèles multimodaux (texte, image)
- Basée sur l'infrastructure Tencent Cloud
- Format d'API compatible OpenAI
- Prise en charge de fenêtres de contexte longues

## Démarrage

### Étape 1 : Inscription à Tencent Cloud

1. Visitez `https://cloud.tencent.com`
2. Connectez-vous avec votre compte QQ ou WeChat
3. Effectuez l'authentification réelle

### Étape 2 : Activer le service Hunyuan

1. Accédez à la **Console Tencent Cloud**
2. Recherchez **Hunyuan** ou **Moteur de connaissances de grands modèles (LKE)**
3. Activez le service

### Étape 3 : Obtenir la clé API

1. Dans la console Tencent Cloud, accédez à **Gestion des clés API**
2. Créez une nouvelle clé API (SecretId et SecretKey)
3. Remarque : Silicon Life utilise directement la clé API

## Modèles disponibles

- **hunyuan-turbos-latest** : Turbo S, haute vitesse (contexte 256K)
- **hunyuan-turbo-latest** : Turbo, performances équilibrées (contexte 256K)
- **hunyuan-large-latest** : Large, contexte long (contexte 256K)
- **hunyuan-standard-latest** : Standard (contexte 256K)
- **hunyuan-lite-latest** : Lite, excellent rapport qualité-prix (contexte 256K)
- **hunyuan-standard-256k** : modèle longue portée (contexte 256K)
- **hunyuan-function-call** : modèle d'appel de fonction (contexte 8K)

## Configuration dans Silicon Life

1. Sélectionnez **Tencent Hunyuan** comme type de client IA
2. Définissez la **Clé API** sur votre clé API Tencent Cloud
3. Définissez le **Modèle** sur le nom du modèle Hunyuan souhaité
4. Définissez le **Point de terminaison** sur l'adresse API (laissez vide pour la valeur par défaut)
5. Laissez la **Fenêtre de contexte Tokens** vide pour la détection automatique

## Mode de facturation

Tencent Hunyuan facture à l'usage, les tarifs varient selon les modèles. Consultez `https://cloud.tencent.com/document/product/1729` pour plus de détails.

## FAQ

1. Confirmez que la clé API est correcte
2. Assurez-vous d'avoir activé le service Hunyuan
3. Vérifiez que le nom du modèle est valide
4. Vérifiez que le solde du compte est suffisant
5. Assurez-vous d'avoir effectué l'authentification réelle
";

    public override string AIClients => @"
# Configuration du client IA

## Aperçu

Les clients IA sont les ""connecteurs de cerveau"" du système Silicon Life, responsables de la communication avec les modèles d'intelligence artificielle.

## Clients IA pris en charge

### Client IA local

**Caractéristiques :**
- 🏠 **Exécution locale** : Les modèles IA fonctionnent sur votre ordinateur
- 🔒 **Confidentialité** : Les données ne sont pas téléchargées dans le cloud
- 💰 **Gratuit** : Aucune restriction d'utilisation
- ⚡ **Réponse rapide** : Pas de latence réseau

### Client IA cloud

**Caractéristiques :**
- ☁️ **Service cloud** : Les modèles IA fonctionnent sur des serveurs distants
- 🚀 **Puissant** : Peut utiliser des modèles ultra-grands (200B+)
- 💳 **À l'utilisation** : Quota gratuit, facturé au-delà
- 🌍 **Multi-région** : Choix du serveur le plus proche

## Comment choisir un client IA ?

```
Quelle est la configuration de votre ordinateur ?
├─ Configuration élevée (16 Go+ RAM)
│  └─ Soucieux de la confidentialité ?
│     ├─ Oui → Client local (par ex., Ollama)
│     └─ Non → L'un ou l'autre
└─ Configuration faible (8 Go ou moins)
   └─ Client cloud (par ex., DashScope)
```

| Fonctionnalité | Client local | Client cloud |
|---------------|--------------|--------------|
| Difficulté d'installation | Moyenne | Simple |
| Coût d'exécution | Gratuit | Quota gratuit, facturé au-delà |
| Protection de la vie privée | ⭐⭐⭐⭐⭐ Complètement local | ⭐⭐⭐ Données via le cloud |
| Exigence réseau | Uniquement pour le téléchargement | Toujours requise |

## Configurer le client IA

### Étape 1 : Allez sur la page de configuration

Cliquez sur le menu **⚙ Config**.

### Étape 2 : Sélectionnez le type de client IA

Sélectionnez votre client dans le menu déroulant.

### Étape 3 : Remplissez la configuration

#### Client local (Ollama)

| Paramètre | Description | Exemple |
|-----------|-------------|---------|
| **endpoint** | Adresse du service IA | `http://localhost:11434` |
| **model** | Nom du modèle | `qwen3.5:8b` |
| **temperature** | Créativité (0-1) | `0.7` |
| **maxTokens** | Longueur maximale de réponse | `2048` |

#### Client cloud (DashScope)

| Paramètre | Description | Exemple |
|-----------|-------------|---------|
| **apiKey** | Clé API | Fournie par la plateforme |
| **region** | Région du serveur | `beijing` |
| **model** | Modèle à utiliser | `qwen3.6-plus` |
| **temperature** | Créativité (0-1) | `0.7` |
| **maxTokens** | Longueur maximale de réponse | `2048` |

### Étape 4 : Sauvegardez et testez

1. Sauvegardez la configuration
2. Allez sur la page **💬 Chat**
3. Envoyez un message test pour vérifier la connexion

## Questions fréquentes

### Q : Je ne sais pas quel client choisir ?

Débutants : client cloud (configuration simple). Soucieux de la confidentialité ou bonne configuration : client local.

### Q : Puis-je utiliser deux clients simultanément ?

Non. Un seul client IA à la fois, mais vous pouvez changer à tout moment.

### Q : Les enregistrements de chat sont-ils perdus après un changement de client ?

Non. Les enregistrements sont sauvegardés dans le système, indépendants du client IA.

### Q : Qu'est-ce que le paramètre temperature ?

- **0.0-0.3** : Très conservateur, réponses prévisibles
- **0.4-0.7** : Mode équilibré (recommandé)
- **0.8-1.0** : Très créatif, réponses diversifiées

### Q : Combien fixer maxTokens ?

- **1024** : Réponses courtes
- **2048** : Longueur moyenne (recommandé)
- **4096+** : Réponses longues

### Q : Différents beings peuvent-ils utiliser différents clients ?

Oui. Chaque silicon being peut configurer indépendamment le type de client IA.
";

    public override string BeingSoul => @"
# Fichier âme du Being

## Aperçu

Le fichier âme est le fichier de configuration central d'un Silicon Being, déterminant sa **personnalité, ses modèles de comportement, ses capacités professionnelles et ses méthodes de travail**.

## Rôle du fichier âme

- 🎭 **Positionnement du rôle** : Qui est ce silicon being, dans quels domaines il excelle
- 📋 **Directives de comportement** : Comment répondre aux utilisateurs, quels principes suivre
- 🔄 **Flux de travail** : Comment traiter les tâches après les avoir reçues
- ⚠️ **Limites de comportement** : Ce qui peut être fait, ce qui ne doit pas être fait
- 💡 **Exigences professionnelles** : Normes de code, format de sortie, style linguistique

## Comment modifier le fichier âme

1. Allez sur la page **Silicon Beings**
2. Cliquez sur la carte du silicon being à modifier
3. Cliquez sur le lien **Fichier âme**
4. Modifiez le contenu dans l'éditeur Markdown
5. Cliquez sur le bouton **Sauvegarder**

Vous pouvez aussi demander au **Silicon Curator** de modifier le fichier âme via la conversation.

## Guide d'écriture du fichier âme

### Structure de base

```markdown
# Définition du rôle

Vous êtes un [description du rôle], spécialisé dans :
- Compétence 1
- Compétence 2

# Directives de comportement

1. Directive 1
2. Directive 2

# Flux de travail

Lors de la réception d'une tâche :
1. Comprendre les exigences
2. Analyser l'approche
3. Exécuter les opérations
4. Rapporter les résultats
```

### Conseils d'écriture

1. **Définition claire du rôle** : Spécifiez clairement les responsabilités et l'expertise
2. **Définir les limites** : Expliquez ce qui peut et ne peut pas être fait
3. **Fournir des flux de travail** : Guidez le silicon being sur le traitement des tâches
4. **Être spécifique** : Utilisez des exemples concrets plutôt que des descriptions abstraites

### Exemple : Assistant de programmation

```markdown
# Définition du rôle

Vous êtes un assistant professionnel de développement full-stack, spécialisé dans :
- Développement C# / .NET
- Conception d'architecture et revue de code
- Conception et optimisation de bases de données

# Directives de comportement

1. Fournir toujours des exemples de code exécutables
2. Expliquer la logique clé du code
3. Fournir des recommandations de bonnes pratiques
4. En cas d'incertitude, informer clairement l'utilisateur

# Normes de code

- Suivre les principes SOLID
- Utiliser une nomenclature claire
- Ajouter les commentaires nécessaires
- Considérer la gestion des erreurs et les cas limites
```

## Questions fréquentes

### Q : Les modifications du fichier âme prennent-elles effet immédiatement ?

Oui, elles prennent effet immédiatement après la sauvegarde.

### Q : Y a-t-il une limite de taille pour le fichier âme ?

Pas de limite stricte, mais il est recommandé de rester dans quelques milliers de mots.

### Q : Puis-je complètement supprimer le fichier âme ?

Ce n'est pas recommandé. Si le contenu est vide, le silicon being perdra ses directives de comportement.

### Q : Quelle est la relation entre le fichier âme et le système de mémoire ?

Le fichier âme définit les **modèles de comportement à long terme**, tandis que le système de mémoire enregistre l'**historique des conversations à court terme**.

## Bonnes pratiques

1. **Optimisation continue** : Optimisez le fichier âme en fonction des retours d'utilisation
2. **Gestion des versions** : Sauvegardez la version actuelle avant les modifications importantes
3. **Test de vérification** : Testez l'effet via la conversation après modification
4. **Rester concis** : Exprimez les exigences clés dans un langage concis
5. **Éviter les contradictions** : Assurez-vous qu'il n'y a pas de conflits entre les directives
";

    public override string AuditLog => @"
# Journal d'audit

## Aperçu

Le journal d'audit est le **système de surveillance de l'utilisation des tokens** de la plateforme Silicon Life, vous aidant à suivre et gérer la consommation d'appels IA de tous les silicon beings.

Avec le journal d'audit, vous pouvez :
- 📊 **Voir les statistiques de consommation de tokens** : Comprendre combien de tokens chaque silicon being utilise
- 📈 **Analyser les tendances d'utilisation** : Voir les changements par heure, jour ou mois
- 🔍 **Filtrer et comparer** : Filtrer par période, silicon being ou type de client IA
- 💾 **Exporter les données** : Exporter en CSV pour analyse approfondie

**Qu'est-ce qu'un token ?** Un token est l'unité de base pour le traitement de texte par les modèles IA. Chaque appel IA consomme des tokens (Prompt Tokens + Completion Tokens = Total Tokens).

## Accéder au journal d'audit

1. Démarrez la plateforme Silicon Life
2. Cliquez sur **""Audit""** (icône 📊) dans le menu de navigation gauche

### Exigences de permission

- 📊 **Voir le journal d'audit** : Requiert la permission Curator
- 🔒 Les silicon beings ordinaires ne peuvent pas accéder à la fonctionnalité d'audit

## Fonctionnalités du tableau de bord d'audit

### 📈 Graphique de tendance

Affiche le graphique de tendance d'utilisation des tokens avec sélecteur de période (Aujourd'hui, Semaine, Mois, Année).

### 📊 Résumé statistique

- Nombre total de requêtes
- Nombre de succès/échecs
- Consommation de tokens (entrée, sortie, total)

### 🔍 Filtrage

- Filtrer par silicon being
- Filtrer par client IA
- Filtres combinés

### 💾 Export de données

Cliquez sur le bouton ""Exporter CSV"" pour exporter les données d'audit.

## Questions fréquentes

### Q : Pourquoi ne puis-je pas voir la page d'audit ?

La fonctionnalité d'audit requiert la permission Curator.

### Q : Les données d'audit affichent ""Aucune donnée"" ?

Aucun appel IA dans la période sélectionnée, ou filtres trop restrictifs.

### Q : Pourquoi l'utilisation des tokens a-t-elle soudainement augmenté ?

Causes possibles : historique de conversation long, fichier âme complexe, appels IA fréquents, tâches/minuteries nombreuses.

### Q : Quel logiciel peut ouvrir les fichiers CSV exportés ?

Microsoft Excel, Google Sheets, LibreOffice Calc, ou tout éditeur de texte.

## Bonnes pratiques

1. **Surveiller régulièrement** : Consultez le tableau de bord d'audit chaque semaine
2. **Utiliser les filtres** : Localisez précisément les problèmes
3. **Exporter régulièrement** : Sauvegardez les données d'audit mensuellement
4. **Optimiser l'utilisation des tokens** : Simplifiez les fichiers âme, contrôlez la mémoire, ajustez les minuteries
";

    public override string KnowledgeGraph => @"
# Graphe de connaissances

## Aperçu

Le graphe de connaissances est le **système de gestion et de visualisation des connaissances** des silicon beings, affichant les connaissances apprises sous forme graphique.

## Qu'est-ce qu'un triplet de connaissances ?

L'unité de base est le **triplet de connaissances**, utilisant une structure ""sujet-prédicat-objet"" :

```
(sujet) -[relation]-> (objet)
```

**Exemples :**
- `(Python) -[est]-> (langage de programmation)`
- `(Terre) -[orbite autour de]-> (Soleil)`

## Accéder au graphe de connaissances

1. Cliquez sur l'icône **📚 Base de connaissances** dans la barre de navigation gauche
2. Le système chargera et affichera la visualisation du graphe de connaissances

## Visualisation du graphe

### Nœuds

Les nœuds représentent les **entités** (sujets ou objets) : nœuds circulaires avec étiquettes, taille ajustée automatiquement selon le nombre de connexions.

### Arêtes

Les arêtes représentent les **relations** (prédicats) : lignes avec flèches indiquant la direction de la relation.

## Gestion des connaissances

### Comment les connaissances sont-elles générées ?

1. **Apprentissage par conversation** : Extraction d'informations clés
2. **Invocation d'outils** : Ajout via KnowledgeTool
3. **Analyse de fichiers** : Extraction de connaissances
4. **Exécution de tâches** : Accumulation de connaissances

### Opérations de l'outil de connaissances

**Ajouter :** ""Veuillez ajouter la connaissance : (Python) -[est]-> (langage de programmation)""

**Rechercher :** ""Recherchez toutes les connaissances sur Python""

**Chemin de relation :** ""Trouvez le chemin de relation entre Python et IA""

## Questions fréquentes

### Q : Pourquoi le graphe de connaissances est-il vide ?

Les silicon beings n'ont pas encore appris de connaissances. Ajoutez des connaissances via la conversation.

### Q : Le graphe se met-il à jour automatiquement ?

Les données sont stockées en temps réel, mais la page doit être rafraîchie pour afficher les dernières données.

### Q : Comment supprimer des connaissances incorrectes ?

Demandez au silicon being via la conversation : ""Veuillez supprimer la connaissance incorrecte sur...""

## Bonnes pratiques

1. **Examiner régulièrement** : Comprendre la progression d'apprentissage des silicon beings
2. **Guider l'apprentissage** : Fournir des entrées de connaissance de qualité
3. **Contrôle qualité** : Vérifier l'exactitude des connaissances importantes
";

    public override string WorkNotes => @"
# Notes de travail

## Aperçu

Les notes de travail sont le **système d'enregistrement de connaissances personnelles** des silicon beings, similaire à un journal numérique ou un carnet de travail.

Les notes de travail prennent en charge deux modes :
- **Notes personnelles** : Propriété individuelle du silicon being, privées par défaut
- **Notes de projet** : Propriété de l'espace projet, les membres du projet peuvent collaborer

## Fonctionnalités principales

- **Enregistrements paginés** : Chaque note est une page indépendante
- **Support Markdown** : Contenu au format Markdown
- **Mots-clés** : Ajout de mots-clés pour la recherche et la classification
- **Contrôle de version** : Numéro de version auto-incrémenté à chaque modification
- **Suivi d'auteur** : Enregistrement du créateur et du dernier modificateur
- **Recherche plein texte** : Recherche par mots-clés, résumé ou contenu

## Accéder aux notes de travail

**Notes personnelles** : Cliquez sur l'icône **📝 Notes de travail** dans la barre de navigation gauche.

**Notes de projet** : Entrez dans la page de détails du projet, cliquez sur l'onglet **Notes de travail**.

## Créer des notes

Les silicon beings peuvent créer des notes via la conversation avec l'IA :

```
Veuillez m'aider à créer une note de travail :
- Résumé : Appris les concepts du graphe de connaissances
- Contenu : Les graphes de connaissances utilisent des structures de graphes...
- Mots-clés : graphe de connaissances, IA, apprentissage
```

### Champs requis

- **Résumé** : Brève description du contenu (requis)
- **Contenu** : Contenu détaillé, supporte Markdown (requis)
- **Mots-clés** : Mots-clés séparés par des virgules (optionnel)

## Mettre à jour les notes

Mise à jour via l'outil IA. Le numéro de version s'incrémente automatiquement à chaque modification.

## Supprimer des notes

Suppression via l'outil IA, nécessite le numéro de page ou l'ID de la note. Irréversible.

## Contrôle des permissions

### Notes personnelles
- **Créateur** : Contrôle total
- **Silicon Curator** : Peut gérer toutes les notes
- **Autres** : Aucun accès

### Notes de projet
- **Membres du projet** : Créer, voir, modifier
- **Non-membres** : Aucun accès
- **Silicon Curator** : Peut gérer toutes les notes de projet

## Questions fréquentes

### Q : Quelle est la différence entre les notes de travail et le système de mémoire ?

Les notes de travail sont du contenu structuré activement enregistré, comme un journal. Le système de mémoire sauvegarde automatiquement les fragments de conversation et les faits.

### Q : Y a-t-il une limite sur le nombre de notes ?

Pas de limite stricte, mais un organisation régulière est recommandée.

### Q : Les notes peuvent-elles être exportées ?

La version actuelle ne prend pas en charge l'export direct, mais les données peuvent être récupérées via l'API.
";

    public override string Projects => @"
# Gestion de projets

## Aperçu

La gestion de projets est l'**espace collaboratif** du système de silicon beings, fournissant un environnement pour que plusieurs silicon beings travaillent ensemble.

## Fonctionnalités principales

- **Gestion du cycle de vie** : Créer, archiver, restaurer, détruire des projets
- **Gestion des membres** : Attribuer et retirer des membres
- **Collaboration par tâches** : Système de tâches spécifique au projet
- **Notes de travail** : Notes de travail partagées au niveau du projet
- **Mécanisme d'archivage** : Archivage et restauration des projets

### Statut des projets

| Statut | Description | Opérations disponibles |
|--------|-------------|----------------------|
| Actif | Projet en cours | Toutes les opérations |
| Archivé | Projet en pause, données conservées | Restaurer, voir |
| Détruit | Projet supprimé définitivement | Aucune |

## Créer des projets

Créer un projet via l'outil IA : fournir un nom de projet clair et une description détaillée.

Paramètres automatiques : ID unique (GUID), heure de création UTC, statut Actif, liste de membres vide.

## Gérer les membres du projet

- **Attribuer des membres** : Ajouter des silicon beings au projet
- **Retirer des membres** : Retirer des silicon beings du projet
- **Voir les membres** : Lister tous les membres du projet

### Permissions des membres

- **Membres du projet** : Créer des tâches, écrire des notes
- **Non-membres** : Ne peuvent pas accéder aux ressources du projet
- **Silicon Curator** : Peut gérer tous les membres du projet

## Gestion du cycle de vie

**Archiver** : Mettre en pause un projet inactif. Données conservées, peut être restauré.

**Restaurer** : Remettre un projet archivé en statut actif.

**Détruire** : Supprimer définitivement un projet (irréversible). Données, tâches et notes supprimées.

## Fonctionnalités collaboratives

**Tâches de projet** : Système de tâches indépendant avec titre, description, priorité.

**Notes de projet** : Notes partagées que tous les membres peuvent créer et modifier.

## Questions fréquentes

### Q : Y a-t-il une limite sur le nombre de projets ?

Pas de limite stricte, mais un nombre raisonnable est recommandé.

### Q : Les projets archivés occupent-ils de l'espace ?

Oui, toutes les données des projets archivés sont conservées.

### Q : Un silicon being peut-il appartenir à plusieurs projets ?

Oui, un silicon being peut être attribué à plusieurs projets simultanément.

### Q : Peut-on ajouter des tâches à un projet archivé ?

Non, il faut d'abord restaurer le projet en statut actif.

### Q : Lors de la suppression d'un projet, les tâches et notes sont-elles supprimées ?

Oui, cette opération est irréversible.
";

    public override string Logging => @"
# Système de journalisation

## Aperçu

Le système de journalisation est une infrastructure centrale de la plateforme Silicon Life Collective, utilisé pour enregistrer l'état de fonctionnement du système, les comportements des silicon beings, les messages d'erreur et les données de débogage.

## Fonctionnalités principales

- **Journalisation multi-niveaux** : 6 niveaux (Trace, Debug, Information, Warning, Error, Critical)
- **Cibles de sortie multiples** : Console colorée et stockage persistant sur fichier
- **Filtrage intelligent** : Par niveau, période et silicon being
- **Gestion par catégorie** : Organisation des journaux par catégorie
- **Enregistrement des exceptions** : Traces de pile automatiques

## Niveaux de journal

| Niveau | Description | Cas d'utilisation |
|--------|-------------|-------------------|
| **Trace** | Journaux les plus détaillés | Débogage développement |
| **Debug** | Informations de débogage | Investigation interactive |
| **Information** | Informations générales | Suivi du flux applicatif |
| **Warning** | Messages d'avertissement | Événements anormaux mais non bloquants |
| **Error** | Messages d'erreur | Échec de l'exécution actuelle |
| **Critical** | Erreurs critiques | Crash système, nécessite un traitement immédiat |
| **None** | Pas de journalisation | Désactiver la journalisation |

## Où voir les journaux ?

1. **Console** : Journaux colorés en temps réel dans la fenêtre du terminal
2. **Fichiers** : Sauvegardés automatiquement dans le répertoire `data/Log/`

## Configuration

Ajustez la verbosité des journaux dans le fichier de configuration :
- **Développement/Débogage** : ""Debug"" ou ""Trace""
- **Usage quotidien** : ""Information""
- **Production** : ""Warning""

## Format d'affichage

```
[2026-04-27 10:30:00.123] [INFO] [Catégorie] [Being:guid] Message de journal
```

## Protection des informations sensibles

Le système N'enregistre PAS les informations sensibles suivantes :
- Mots de passe et clés
- Informations personnelles identifiables
- Clés API et tokens
- Chaînes de connexion aux bases de données

## Questions fréquentes

### Q : Pourquoi ne puis-je pas voir certains messages de journal ?

La verbosité est peut-être trop élevée. Ajustez à ""Debug"" ou ""Trace"" dans la configuration.

### Q : Comment voir des journaux plus détaillés ?

Modifiez le paramètre de verbosité à ""Debug"" ou ""Trace"", puis redémarrez le système.

### Q : Où sont les fichiers de journaux ?

Dans le répertoire `data/Log/`, indexés par temps.

## Bonnes pratiques

1. **Choisir le niveau approprié** : Debug en développement, Information en production
2. **Se concentrer sur** : Démarrage/arrêt du système, anomalies des silicon beings, échecs de permission, erreurs d'appels IA
3. **Éviter l'impact sur les performances** : Augmentez la verbosité si les fichiers de journal deviennent trop volumineux
";

    
    public override string Skills => @"
# Compétences

## Qu'est-ce qu'une compétence ?

Une compétence (Skill) est une unité de capacité réutilisable encapsulant l'orchestration d'outils et des modèles de prompts. Elle représente une « capacité » concrète d'un être en silicium, et peut être invoquée automatiquement par l'IA (function calling) ou déclenchée explicitement par l'utilisateur ou le conservateur.

**Compétence vs Outil :**
- **Outil (Tool)** : une opération atomique unique (ex. lecture de fichier, recherche web) — un appel accomplit une tâche
- **Compétence (Skill)** : orchestration multi-étapes + modèle de prompt système. Coordonne plusieurs outils pour accomplir des tâches complexes

## Sources des compétences

Le système prend en charge 4 sources de compétences :

| Source | Description | Exemple |
|--------|-------------|---------|
| Intégrée (Builtin) | Intégrée au framework, non modifiable | Fonctionnalités système |
| Plugin (Plugin) | Enregistrée via l'interface `ISkillProvider` | Extensions tierces |
| Créée par l'être (Being) | Créée par l'être en silicium à l'exécution | Auto-évolution de l'IA |
| Créée par l'utilisateur (User) | Créée via l'UI Web ou l'outil `skill` | Flux de travail personnalisés |

## Configuration principale

Lors de la création d'une compétence, vous pouvez configurer :

- **id** : identifiant unique (ex. `summarize_document`)
- **description** : description en une phrase utilisée lors du function calling
- **system_prompt** : modèle de prompt système. Supporte les espaces réservés `{param}`, remplis automatiquement à l'exécution
- **parameter_schema** : JSON Schema déclarant les paramètres acceptés
- **tool_whitelist** : liste blanche des outils autorisés à l'exécution (liste vide = hérite de tous les outils)
- **max_tool_round** : nombre maximum de rounds d'appels d'outils (5 par défaut)
- **timeout** : délai d'exécution (60 secondes par défaut)
- **on_complete** : action après completion : `write_memory` (écrire en mémoire), `notify_curator` (notifier le conservateur), `broadcast` (diffuser), `none`
- **trigger_mode** : mode de déclenchement : `manual` (manuel/appel auto IA) ou `auto` (déclenchement programmé)
- **auto_trigger_condition** : condition de déclenchement auto (ex. `daily 09:00`, `interval 6h`, expression cron)

## Utilisation des compétences

### 1. Déclenchement depuis le chat

Demandez directement à l'être en silicium d'exécuter une compétence dans le chat. Ex :

> « Résume ce document »

Si la compétence `summarize_document` est enregistrée, elle sera reconnue et invoquée automatiquement.

### 2. Gestion avec l'outil `skill`

L'être en silicium peut utiliser l'outil `skill` pour :

- **create** : créer une nouvelle compétence (`id` et `system_prompt` requis)
- **list** : lister toutes les compétences enregistrées
- **update** : mettre à jour une compétence existante
- **update_from_md** : mettre à jour depuis un Markdown
- **delete** : supprimer une compétence
- **export / export_md** : exporter en JSON ou Markdown
- **import / import_md** : importer depuis JSON ou Markdown

### 3. Gestion via l'UI Web

La page « Compétences » de l'UI Web permet de :
- Visualiser toutes les compétences
- Créer/modifier/supprimer des compétences personnalisées
- Importer/exporter des définitions de compétences
- Consulter l'historique d'exécution

## Format de fichier de compétence

Les compétences sont persistées par défaut au format Markdown, dans le répertoire `skills/` de l'être.

**Exemple de format Markdown :**

```markdown
---
id: summarize_document
version: ""1.0.0""
description: Summarize a long document into key points
parameter_schema:
  type: object
  properties:
    document:
      type: string
      description: The document content to summarize
    max_length:
      type: integer
      description: Maximum summary length in characters
---

You are a document summarization assistant. Summarize the following document into {max_length} characters or less, highlighting the key points:

{document}
```

**Explications :**
- La partie entre `---` est le YAML front matter (métadonnées)
- Le reste est le modèle de prompt système (`system_prompt`)
- Les métadonnées manquantes sont automatiquement complétées par l'IA
- Un fichier `.md` prime sur un fichier `.json` de même ID

## Compétences à déclenchement automatique

Configurez `trigger_mode` sur `auto` et définissez `auto_trigger_condition` pour exécuter la compétence automatiquement à l'heure programmée.

**Formats de planification supportés :**
- `daily 09:00` — tous les jours à 9h
- `interval 6h` — toutes les 6 heures
- Expression cron standard — ex. `0 9 * * 1-5` (9h en semaine)

Les résultats d'exécution automatique sont écrits par défaut dans la mémoire de l'être, avec option de notification au conservateur ou de diffusion sur un canal.

## Permissions et sécurité

- Le conservateur (Curator) peut modifier toutes les compétences
- Un être ordinaire ne peut modifier que les compétences qu'il a créées (source = being) ou celles importées par l'utilisateur
- Les compétences sont soumises aux permissions d'actions d'outils (ToolActionPermissions)
- Une liste blanche vide hérite de toutes les permissions d'outils de l'être

## Limite de quantité

Chaque être en silicium peut créer jusqu'à 50 compétences personnalisées (ajustable via configuration).

## FAQ

**Q : Que faire si l'exécution d'une compétence expire ?**
R : Augmentez la valeur `timeout` dans la définition de la compétence, ou ajustez `GlobalSkillTimeoutSeconds` dans la configuration globale.

**Q : Que faire si une compétence échoue à appeler un outil ?**
R : Vérifiez que `tool_whitelist` contient l'outil nécessaire et que l'être possède la permission pour cet outil.

**Q : Comment sauvegarder une compétence ?**
R : Utilisez `export` ou `export_md` pour l'exporter en JSON/Markdown, stockez-la en lieu sûr, puis restaurez-la avec `import`.

**Q : Une compétence peut-elle s'appeler elle-même récursivement ?**
R : Non. Les compétences en cours d'exécution sont bloquées contre les appels récursifs pour éviter les boucles infinies.
";

#endregion
}