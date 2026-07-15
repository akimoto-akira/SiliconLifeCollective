# Guide de l'interface Web UI

> **Version : v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Русский](../ru-RU/web-ui-guide.md)

## Vue d'ensemble

L'interface Web UI fournit une interface complète pour gérer les Êtres de Silicium, surveiller l'état du système et interagir avec les agents IA. Le système adopte une architecture de rendu côté serveur pur, sans dépendance à un framework frontend, générant HTML, CSS et JavaScript via les constructeurs `H`, `CssBuilder` et `JsBuilder`.

## Accès

URL par défaut : `http://localhost:8080`

## Navigation

### Sections principales

1. **Tableau de bord** - Vue d'ensemble du système et métriques
2. **Êtres** - Gestion des Êtres de Silicium
3. **Chat** - Interaction avec les Êtres (support du téléversement de fichiers, SSE en temps réel)
4. **Historique de chat** - Consultation de l'historique de chat des Êtres de Silicium (liste de sessions, détails des messages)
5. **Tâches** - Gestion des tâches (tâches personnelles)
6. **Minuteurs** - Configuration des minuteurs (création, pause, historique d'exécution)
7. **Configuration** - Paramètres système (clients IA, localisation)
8. **Autorisations** - Contrôle d'accès (gestion ACL, requêtes d'autorisation)
9. **Journaux** - Journaux système (filtrage par niveau, requêtes par plage de temps)
10. **Audit** - Utilisation des tokens et piste d'audit
11. **Mémoire** - Mémoire des Êtres (vue chronologique, filtrage avancé)
12. **Connaissances** - Base de connaissances (gestion des triplets, découverte de chemins)
13. **Navigateur de code** - Exploration de code (arborescence de fichiers, coloration syntaxique)
14. **Éditeur de code** - Édition de code avec infobulles (Monaco Editor)
15. **Projets** - Gestion de projet (espaces de travail, tâches, notes de travail)
16. **Exécuteurs** - Gestion des exécuteurs (disque, réseau, ligne de commande)
17. **Aide** - Système de documentation d'aide (support multilingue, recherche par thème)
18. **À propos** - Informations système et version

---

## Tableau de bord

### Fonctionnalités

- Métriques de performance système (CPU, mémoire, temps de fonctionnement)
- Vue d'ensemble de l'état des Êtres
- Statistiques d'utilisation IA
- Actions rapides

### Mises à jour en temps réel

Utilisation de SSE (Server-Sent Events) pour les données en temps réel :

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Gestion des Êtres

### Liste des Êtres

Affiche tous les Êtres, incluant :
- Nom et ID
- État actuel (en cours d'exécution / arrêté / erreur)
- Lien vers le Fichier d'Âme
- Actions rapides (démarrer / arrêter / configurer)

### Détails d'un Être

- Configuration complète
- Éditeur de Fichier d'Âme
- Historique des tâches
- Visualiseur de mémoire
- Métriques de performance

### Créer un Être

1. Cliquer sur **Créer un nouvel être**
2. Remplir :
   - Nom
   - Contenu du Fichier d'Âme (éditeur Markdown)
   - Configuration initiale
3. Cliquer sur **Créer**

---

## Interface de chat

### Fonctionnalités

- Flux de messages en temps réel
- Historique des messages
- Support multi-sessions
- Visualisation des appels d'outils

### Utiliser le chat

1. Sélectionner un Être
2. Saisir un message
3. Voir la réponse en flux
4. Observer l'exécution des outils en temps réel

### Affichage des appels d'outils

Lorsque l'IA appelle un outil :
```
🔧 Outil : calendar
📥 Entrée : {"date": "2026-04-20"}
📤 Sortie : "Troisième jour du quatrième mois lunaire"
```

---

## Configuration

### Clients IA

Configurer le backend IA :
- Ollama (local)
- Bailian (cloud)
- DeepSeek (cloud, mode thinking)
- Zhipu AI / GLM (cloud, mode thinking, vision)
- Baidu Qianfan / ERNIE (cloud)
- Tencent Hunyuan (cloud, TokenHub + Legacy)
- MiniMax (cloud, multimodal M3)
- Moonshot / Kimi (cloud, mode thinking, vision)
- SiliconFlow (cloud, agrégateur 100+ modèles)
- Volcengine Ark (cloud)
- Herdsman (local/cloud, sans authentification)
- Meituan LongCat (cloud)
- Qiniu Cloud AI (cloud)
- Client personnalisé

### Paramètres de stockage

- Version Default : chemin de base, index temporel, stratégie de nettoyage
- Version Fast : configuration du moteur de stockage SpeedyPack, gestion des fichiers .spk, paramètres de compression automatique

### Localisation

Basculer entre 34 variantes linguistiques :
- Chinois (6) : simplifié, traditionnel, Singapour, Macao, Taïwan, Malaisie
- Anglais (10) : américain, britannique, canadien, australien, indien, singapourien, sud-africain, irlandais, néo-zélandais, malaisien
- Espagnol (2) : Espagne, Mexique
- Allemand (5) : Allemagne, Autriche, Suisse, Luxembourg, Liechtenstein
- Français (3) : France, Canada, Suisse
- Japonais, coréen, tchèque
- Russe, portugais (2), italien, néerlandais, polonais, suédois

---

## Système de thèmes

### Thèmes disponibles

1. **Admin** - Interface de gestion professionnelle
2. **Chat** - Design centré sur la conversation
3. **Creative** - Style créatif et artistique
4. **Dev** - Layout orienté développeur
5. **HighContrast** - Thème à contraste élevé (version Fast)
6. **Minimal** - Style minimaliste (version Fast)
7. **Light** - Thème clair (version Fast)

### Changer de thème

1. Cliquer sur **Paramètres** (icône engrenage)
2. Sélectionner **Thème**
3. Choisir le thème souhaité
4. L'interface se met à jour immédiatement

### Thème personnalisé

Créer un thème personnalisé en implémentant `ISkin` :

```csharp
public class MySkin : ISkin
{
    public string Name => "MySkin";
    
    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
}
```

---

## Gestion des autorisations

### Consulter les autorisations

- Lister toutes les règles d'autorisation
- Filtrer par utilisateur ou ressource
- Consulter les dates d'expiration

### Ajouter une règle d'autorisation

1. Cliquer sur **Ajouter une règle**
2. Configurer :
   - Type d'autorisation (par ex. `FileAccess`, `NetworkAccess`)
   - Préfixe de ressource (par ex. `C:\Projects`, `api.github.com`)
   - Autoriser / Refuser
   - Description
3. Sauvegarder

### Piste d'audit

Consulter toutes les décisions d'autorisation :
- Horodatage
- Utilisateur
- Ressource
- Décision
- Raison

### Gestion des autorisations d'outils

Gérer les autorisations d'opérations d'outils des Êtres de Silicium et des projets :

1. **Autorisations d'outils de l'Être de Silicium** :
   - Naviguer vers **Êtres** → Sélectionner l'Être → **Autorisations d'outils**
   - Consulter la configuration actuelle des autorisations
   - Définir autoriser/refuser pour chaque opération
   - Appliquer un modèle d'autorisations (readonly/restricted/full)

2. **Autorisations d'outils du projet** :
   - Naviguer vers **Projets** → Sélectionner le projet → **Autorisations d'outils**
   - Les autorisations d'outils au niveau projet sont indépendantes de celles au niveau de l'Être de Silicium
   - Implémentation de l'isolation des autorisations entre projets

---

## Gestion des tâches

### Liste des tâches

- Toutes les tâches et leur état
- Filtrer par Être ou par état
- Indicateurs de priorité

### Détails d'une tâche

- Description
- Priorité
- Date limite
- Historique d'exécution
- Résultat

### Créer une tâche

1. Cliquer sur **Créer une tâche**
2. Remplir :
   - Attribution à un Être
   - Description
   - Priorité (1-10)
   - Date limite
3. Créer

---

## Gestion des minuteurs

### Minuteurs actifs

- Liste des minuteurs en cours
- Prochaine heure d'exécution
- État de répétition

### Créer un minuteur

1. Cliquer sur **Créer un minuteur**
2. Configurer :
   - Attribution à un Être
   - Intervalle ou expression cron
   - Action à exécuter
   - Paramètres de répétition
3. Démarrer

---

## Visualiseur de journaux

### Fonctionnalités

- Filtrer par niveau (information / avertissement / erreur)
- Rechercher par mots-clés
- Sélection de plage de temps
- Mises à jour en temps réel

### Détails des journaux

Chaque entrée de journal affiche :
- Horodatage
- Niveau
- Source
- Message
- Trace d'appels (pour les erreurs)

---

## Rapports d'audit

### Utilisation des tokens

- Total des tokens utilisés
- Ventilation par modèle
- Calcul des coûts
- Graphiques temporels

### Exporter les rapports

Télécharger les données d'audit :
- Format CSV
- Sélection de plage de dates
- Filtrer par Être ou modèle

---

## Éditeur de code

### Fonctionnalités

- Coloration syntaxique (Monaco Editor)
- Complétion de code
- Infobulles au survol des identifiants
- Compilation en temps réel

### Infobulles

Survoler n'importe quel identifiant pour voir :
- Informations de type
- Documentation
- Emplacement de la définition
- Références

---

## Consultation de l'historique de chat

### Fonctionnalités

- Navigation dans l'historique de chat des Êtres de Silicium
- Affichage de la liste des sessions
- Consultation des détails des messages
- Vue chronologique

### Utiliser l'historique de chat

1. Naviguer vers la page **Êtres**
2. Cliquer sur le lien **Historique de chat** de l'Être de Silicium
3. Consulter la liste des sessions :
   - Titre de la session
   - Date de création
   - Nombre de messages
4. Cliquer sur une session pour voir les détails :
   - Historique complet des messages
   - Horodatages
   - Informations sur l'expéditeur
   - Enregistrements des appels d'outils

### Implémentation technique

- **Contrôleur** : `ChatHistoryController`
- **Modèle de vue** : `ChatHistoryViewModel`
- **Vues** :
  - `ChatHistoryListView` - Liste des sessions
  - `ChatHistoryDetailView` - Détails des messages
- **Routes API** :
  - `/api/chat-history/{beingId}/conversations` - Obtenir la liste des sessions
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Obtenir les détails des messages

---

## Téléversement de fichiers

### Fonctionnalités

- Dialogue de source de fichiers
- Support du téléversement multiple
- Gestion des métadonnées de fichiers
- Affichage de la progression du téléversement

### Utiliser le téléversement de fichiers

1. Cliquer sur le bouton **Téléverser un fichier** dans l'interface de chat
2. Le dialogue de source de fichiers s'ouvre
3. Sélectionner la source du fichier :
   - Fichier local
   - Chemin du système de fichiers
4. Sélectionner le(s) fichier(s) (sélection multiple supportée)
5. Confirmer le téléversement
6. Les informations du fichier seront jointes au message

### Types de fichiers pris en charge

- Fichiers texte (.txt, .md, .json, .xml, etc.)
- Fichiers de code (.cs, .js, .py, .java, etc.)
- Fichiers de configuration (.yml, .yaml, .ini, .conf, etc.)
- Fichiers de documents (.csv, .log, etc.)

---

## Indicateur de chargement

### Fonctionnalités

- Affichage de l'état de chargement de la page de chat
- Sélection automatique de la session du Curateur
- Retour sur la progression du chargement des données

### Comportement

- Animation de chargement affichée lors du chargement de la page
- Masquage automatique une fois les données chargées
- Session du Curateur automatiquement sélectionnée (si elle existe)
- Textes d'indication de chargement multilingues

---

## Système de documentation d'aide (nouveau)

### Vue d'ensemble des fonctionnalités

Le système de documentation d'aide fournit un support de documentation multilingue pour les Êtres de Silicium et les utilisateurs.

### Utiliser la documentation d'aide

1. Naviguer vers la page **Aide**
2. Consulter la liste des thèmes d'aide :
   - Guide de démarrage rapide
   - Référence d'utilisation des outils
   - Guide de gestion des autorisations
   - Manuel de dépannage
   - Guide de développement
3. Cliquer sur un thème pour voir le contenu détaillé :
   - Contenu documentaire structuré (rendu Markdown)
   - Support multilingue (suit les paramètres de localisation système)
   - Recommandation de thèmes connexes
4. Utiliser la fonction de recherche pour localiser rapidement :
   - Recherche par mots-clés (support chinois, anglais)
   - Résultats de recherche triés par pertinence

### Accès à l'aide par les Êtres de Silicium

Les Êtres de Silicium peuvent accéder à la documentation d'aide via l'outil `help` :
```json
{
  "action": "get_topics"
}
```

### Implémentation technique

- **Contrôleur** : `HelpController`
- **Outil** : `HelpTool`
- **Routes API** :
  - `/api/help` - Obtenir la liste des thèmes d'aide
  - `/api/help/{topicId}` - Obtenir les détails d'un thème
  - `/api/help/search?q=keyword` - Rechercher dans la documentation d'aide

---

## Espace de projet (nouveau)

### Vue d'ensemble des fonctionnalités

L'espace de projet fournit un environnement de travail structuré, prenant en charge la gestion de projet, le suivi des tâches et les notes de travail.

### Gestion de projet

1. **Créer un projet** :
   - Nom et description du projet
   - Tags du projet (catégorisation)
   - Statut du projet (en cours, terminé, archivé)
2. **Consulter les détails du projet** :
   - Informations de base du projet
   - Liste des tâches associées
   - Liste des notes de travail
   - Statistiques de progression du projet
3. **Archiver un projet** : Conserver les données historiques sans activité
4. **Gestion des rôles de projet** :
   - Attribuer des rôles de projet aux Êtres de Silicium (par ex. developer, reviewer, manager)
   - Retirer des attributions de rôles
   - Consulter la liste des membres et rôles du projet
5. **Flux de travail de projet** :
   - Consulter la liste des modèles de flux de travail
   - Lier un modèle de flux de travail au projet
   - Consulter l'état de l'instance de flux de travail
   - Consulter les journaux d'exécution du flux de travail

### Notes de travail (privées)

Notes de travail personnelles des Êtres de Silicium, similaires à un journal :

1. **Créer une note** :
   - Résumé (description brève)
   - Contenu (support du format Markdown)
   - Mots-clés (pour la recherche)
   - Enregistrement automatique de l'horodatage
2. **Gérer les notes** :
   - Navigation chronologique (design par pages)
   - Recherche de notes (par mots-clés, résumé, contenu)
   - Génération de sommaire (aperçu rapide de la structure des notes)
   - Mise à jour et suppression de notes
3. **Contrôle des autorisations** :
   - Privées par défaut, seul l'Être lui-même peut y accéder
   - Le Curateur de Silicium peut gérer toutes les notes

### Implémentation technique

- **Contrôleur** : `WorkNoteController`
- **Outils** : `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Routes API** :
  - `/api/worknotes` - Obtenir la liste des notes de travail
  - `/api/worknotes/{id}` - Obtenir les détails d'une note
  - `/api/worknotes/search?q=keyword` - Rechercher des notes
  - `/api/worknotes/directory` - Générer un sommaire des notes
  - `/api/projects` - API de gestion de projet

---

## Design responsive

L'interface Web UI s'adapte aux différentes tailles d'écran :
- Bureau : Layout complet
- Tablette : Barre latérale compressée
- Mobile : Menu repliable

---

## Raccourcis clavier

| Raccourci | Action |
|----------|--------|
| `Ctrl+K` | Recherche rapide |
| `Ctrl+B` | Basculer la barre latérale |
| `Ctrl+Enter` | Envoyer un message |
| `Esc` | Annuler / Fermer |

---

## Dépannage

### Impossible de se connecter

**Vérifier** :
- Le serveur est en cours d'exécution
- Le port 8080 n'est pas bloqué
- Les paramètres du pare-feu

### SSE ne fonctionne pas

**Vérifier** :
- Le navigateur supporte SSE
- Aucun proxy ne met en tampon SSE
- Stabilité du réseau

### Performances lentes

**Optimiser** :
- Réduire le niveau de détail des journaux
- Nettoyer les anciennes données d'audit
- Vérifier les ressources système

---

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 📖 Explorer la [référence API](api-reference.md)
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md)
