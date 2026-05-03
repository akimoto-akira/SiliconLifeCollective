# Guide de l'interface Web

> **Version : v0.1.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | **Français** | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md)

## Aperçu

L'interface Web fournit une interface complète pour gérer les Silicon Beings, surveiller l'état du système et interagir avec les agents IA. Le système utilise une architecture de rendu côté serveur pur sans dépendances à un framework frontend, générant HTML, CSS et JavaScript via les builders `H`, `CssBuilder` et `JsBuilder`.

## Accès

URL par défaut : `http://localhost:8080`

## Navigation

### Sections principales

1. **Tableau de bord** - Vue d'ensemble du système et métriques
2. **Beings** - Gestion des Silicon Beings
3. **Chat** - Interaction avec les Beings (avec upload de fichiers, SSE en direct)
4. **Historique de chat** - Afficher l'historique de chat des Silicon Beings (liste des sessions, détails des messages)
5. **Tâches** - Gestion des tâches (tâches personnelles)
6. **Minuteries** - Configuration des minuteries (créer, pause, historique d'exécution)
7. **Configuration** - Paramètres système (clients IA, localisation)
8. **Permissions** - Contrôle d'accès (gestion ACL, requêtes de permissions)
9. **Journaux** - Journaux système (filtrer par niveau, requêtes par plage temporelle)
10. **Audit** - Utilisation des tokens et piste d'audit
11. **Mémoire** - Mémoire des Beings (vue chronologique, filtrage avancé)
12. **Connaissances** - Base de connaissances (gestion des triplets, recherche de chemins)
13. **Navigateur de code** - Exploration du code (arborescence de fichiers, coloration syntaxique)
14. **Éditeur de code** - Édition de code avec info-bulles de survol (Monaco Editor)
15. **Projets** - Gestion de projets (espaces de travail, tâches, notes de travail)
16. **Exécuteurs** - Gestion des exécuteurs (disque, réseau, ligne de commande)
17. **Aide** - Système de documentation d'aide (support multilingue, recherche par sujet)
18. **À propos** - Informations système et version

---

## Tableau de bord

### Fonctionnalités

- Métriques de performance système (CPU, mémoire, temps de fonctionnement)
- Vue d'ensemble du statut des Beings
- Statistiques d'utilisation de l'IA
- Actions rapides

### Mises à jour en temps réel

Utilise SSE (Server-Sent Events) pour les données en temps réel :

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Gestion des Beings

### Liste des Beings

Affiche tous les Beings avec :
- Nom et ID
- Statut actuel (En cours/Arrêté/Erreur)
- Lien vers le fichier âme
- Actions rapides (Démarrer/Arrêter/Configurer)

### Détails d'un Being

- Configuration complète
- Éditeur de fichier âme
- Historique des tâches
- Visualiseur de mémoire
- Métriques de performance

### Créer un Being

1. Cliquer sur **Créer un nouveau Being**
2. Remplir :
   - Nom
   - Contenu de l'âme (éditeur Markdown)
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

1. Sélectionner un Being
2. Taper un message
3. Voir la réponse en streaming
4. Observer l'exécution des outils en temps réel

### Affichage des appels d'outils

Lorsque l'IA appelle un outil :
```
🔧 Outil : calendar
📥 Entrée : {"date": "2026-04-20"}
📤 Sortie : "农历四月初三"
```

---

## Configuration

### Clients IA

Configurer le backend IA :
- Ollama (local)
- DashScope (cloud)
- Clients personnalisés

### Paramètres de stockage

- Version Default : Chemin de base, index temporel, stratégie de nettoyage
- Version Fast : Configuration du moteur de stockage SpeedyPack, gestion des fichiers .spk, paramètres d'auto-compaction

### Localisation

Basculer entre 24 variantes linguistiques :
- Chinois (6) : Simplifié, Traditionnel, Singapour, Macao, Taïwan, Malaisie
- Anglais (10) : US, UK, Canadien, Australien, Indien, Singapour, Sud-Africain, Irlandais, Néo-Zélandais, Malaisien
- Espagnol (2) : Espagne, Mexique
- Français (3) : France, Canada, Suisse
- Japonais, Coréen, Tchèque

---

## Système de skins

### Skins disponibles

1. **Admin** - Interface d'administration professionnelle
2. **Chat** - Design centré sur la conversation
3. **Creative** - Style créatif et artistique
4. **Dev** - Disposition orientée développeur
5. **HighContrast** - Contraste élevé (version Fast)
6. **Minimal** - Style minimaliste (version Fast)
7. **Light** - Thème clair (version Fast)

### Changer de skin

1. Cliquer sur **Paramètres** (icône engrenage)
2. Sélectionner **Skin**
3. Choisir le skin souhaité
4. L'interface se met à jour immédiatement

### Skin personnalisé

Créer des skins personnalisés en implémentant `ISkin` :

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

## Gestion des permissions

### Afficher les permissions

- Lister toutes les règles de permissions
- Filtrer par utilisateur ou ressource
- Afficher la date d'expiration

### Ajouter une règle de permission

1. Cliquer sur **Ajouter une règle**
2. Configurer :
   - Utilisateur
   - Ressource (ex. `disk:read`)
   - Autoriser/Refuser
   - Durée
3. Sauvegarder

### Piste d'audit

Afficher toutes les décisions de permissions :
- Horodatage
- Utilisateur
- Ressource
- Décision
- Raison

---

## Gestion des tâches

### Liste des tâches

- Toutes les tâches avec statut
- Filtrer par Being ou statut
- Indicateurs de priorité

### Détails d'une tâche

- Description
- Priorité
- Date d'échéance
- Historique d'exécution
- Résultat de sortie

### Créer une tâche

1. Cliquer sur **Créer une tâche**
2. Remplir :
   - Assignation du Being
   - Description
   - Priorité (1-10)
   - Date d'échéance
3. Créer

---

## Gestion des minuteries

### Minuteries actives

- Liste des minuteries en cours
- Prochaine heure d'exécution
- Statut de répétition

### Créer une minuterie

1. Cliquer sur **Créer une minuterie**
2. Configurer :
   - Assignation du Being
   - Intervalle ou expression Cron
   - Action à exécuter
   - Paramètres de répétition
3. Démarrer

---

## Visualiseur de journaux

### Fonctionnalités

- Filtrer par niveau (Info/Avertissement/Erreur)
- Rechercher par mot-clé
- Sélection de plage temporelle
- Mises à jour en temps réel

### Détails des journaux

Chaque entrée de journal affiche :
- Horodatage
- Niveau
- Source
- Message
- Trace de la pile (pour les erreurs)

---

## Rapports d'audit

### Utilisation des tokens

- Total des tokens utilisés
- Répartition par modèle
- Calcul des coûts
- Graphiques temporels

### Exporter les rapports

Télécharger les données d'audit :
- Format CSV
- Sélection de plage de dates
- Filtrer par Being ou modèle

---

## Éditeur de code

### Fonctionnalités

- Coloration syntaxique (Monaco Editor)
- Complétion de code
- Info-bulles de survol pour les identifiants
- Compilation en temps réel

### Info-bulles de survol

Passer la souris sur un identifiant pour :
- Informations de type
- Documentation
- Emplacement de la définition
- Références

---

## Affichage de l'historique de chat

### Fonctionnalités

- Parcourir l'historique de chat des Silicon Beings
- Affichage de la liste des sessions
- Voir les détails des messages
- Vue chronologique

### Utiliser l'historique de chat

1. Naviguer vers la page **Beings**
2. Cliquer sur le lien **Historique de chat** du Silicon Being
3. Afficher la liste des sessions :
   - Titre de la session
   - Heure de création
   - Nombre de messages
4. Cliquer sur une session pour les détails :
   - Historique complet des messages
   - Horodatages
   - Informations sur l'expéditeur
   - Journal des appels d'outils

### Implémentation technique

- **Contrôleur** : `ChatHistoryController`
- **ViewModel** : `ChatHistoryViewModel`
- **Vues** :
  - `ChatHistoryListView` - Liste des sessions
  - `ChatHistoryDetailView` - Détails des messages
- **Routes API** :
  - `/api/chat-history/{beingId}/conversations` - Obtenir la liste des sessions
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Obtenir les détails des messages

---

## Upload de fichiers

### Fonctionnalités

- Dialogue de sélection de fichiers
- Support d'upload multi-fichiers
- Gestion des métadonnées de fichiers
- Indicateur de progression de l'upload

### Utiliser l'upload de fichiers

1. Cliquer sur le bouton **Upload de fichier** dans le chat
2. Le dialogue de sélection de fichiers s'ouvre
3. Choisir la source du fichier :
   - Fichier local
   - Chemin du système de fichiers
4. Sélectionner les fichiers (sélection multiple supportée)
5. Confirmer l'upload
6. Les informations du fichier sont jointes au message

### Types de fichiers supportés

- Fichiers texte (.txt, .md, .json, .xml, etc.)
- Fichiers de code (.cs, .js, .py, .java, etc.)
- Fichiers de configuration (.yml, .yaml, .ini, .conf, etc.)
- Fichiers de documents (.csv, .log, etc.)

---

## Indicateur de chargement

### Fonctionnalités

- Affichage du statut de chargement sur la page de chat
- Sélection automatique de la session du Curator
- Retour de progression du chargement des données

### Comportement

- Animation de chargement au chargement de la page
- Masqué automatiquement après le chargement des données
- Session du Curator automatiquement sélectionnée (si présente)
- Textes de chargement multilingues

---

## Système de documentation d'aide (Nouveau)

### Aperçu des fonctionnalités

Le système de documentation d'aide fournit un support d'aide multilingue pour les Silicon Beings et les utilisateurs.

### Utiliser l'aide

1. Naviguer vers la page **Aide**
2. Afficher la liste des sujets d'aide :
   - Guide de démarrage rapide
   - Référence d'utilisation des outils
   - Guide de gestion des permissions
   - Guide de dépannage
   - Guide de développement
3. Cliquer sur un sujet pour les détails :
   - Contenu de documentation structuré (rendu Markdown)
   - Support multilingue (suit la localisation système)
   - Recommandations de sujets connexes
4. Fonction de recherche pour une localisation rapide :
   - Recherche par mot-clé (supporte le chinois, l'anglais)
   - Résultats de recherche triés par pertinence

### Aide pour les Silicon Beings

Les Silicon Beings peuvent accéder à l'aide via l'outil `help` :
```json
{
  "action": "get_topics"
}
```

### Implémentation technique

- **Contrôleur** : `HelpController`
- **Outil** : `HelpTool`
- **Routes API** :
  - `/api/help` - Obtenir la liste des sujets d'aide
  - `/api/help/{topicId}` - Obtenir les détails du sujet
  - `/api/help/search?q=keyword` - Rechercher dans la documentation d'aide

---

## Espace de projet (Nouveau)

### Aperçu des fonctionnalités

L'espace de projet fournit un environnement de travail structuré avec gestion de projets, suivi de tâches et notes de travail.

### Gestion de projets

1. **Créer un projet** :
   - Nom et description du projet
   - Tags du projet (catégorisation)
   - Statut du projet (Actif, Terminé, Archivé)
2. **Afficher les détails du projet** :
   - Informations de base du projet
   - Liste des tâches associées
   - Liste des notes de travail
   - Statistiques de progression du projet
3. **Archiver un projet** : Conserver les données historiques mais ne plus être actif

### Notes de travail (Privé)

Notes de travail personnelles des Silicon Beings, similaires à un journal :

1. **Créer une note** :
   - Résumé (description courte)
   - Contenu (support Markdown)
   - Mots-clés (pour la recherche)
   - Horodatage automatique
2. **Gérer les notes** :
   - Navigation chronologique (design par pages)
   - Rechercher des notes (par mot-clé, résumé, contenu)
   - Générer une table des matières (affichage rapide de la structure)
   - Mettre à jour et supprimer des notes
3. **Contrôle des permissions** :
   - Privé par défaut, accessible uniquement au Being lui-même
   - Le Silicon Curator peut gérer toutes les notes

### Implémentation technique

- **Contrôleur** : `WorkNoteController`
- **Outils** : `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Routes API** :
  - `/api/worknotes` - Obtenir la liste des notes de travail
  - `/api/worknotes/{id}` - Obtenir les détails d'une note
  - `/api/worknotes/search?q=keyword` - Rechercher des notes
  - `/api/worknotes/directory` - Générer le répertoire des notes
  - `/api/projects` - API de gestion de projets

---

## Design responsive

L'interface Web s'adapte à différentes tailles d'écran :
- Bureau : Disposition complète
- Tablette : Barre latérale compressée
- Mobile : Menu repliable

---

## Raccourcis clavier

| Raccourci | Action |
|-----------|--------|
| `Ctrl+K` | Recherche rapide |
| `Ctrl+B` | Basculer la barre latérale |
| `Ctrl+Enter` | Envoyer le message |
| `Esc` | Annuler/Fermer |

---

## Dépannage

### Impossible de se connecter

**Vérifier** :
- Le serveur fonctionne
- Le port 8080 n'est pas bloqué
- Les paramètres du pare-feu

### SSE ne fonctionne pas

**Vérifier** :
- Le navigateur supporte SSE
- Aucun proxy ne bufferise SSE
- Stabilité du réseau

### Performance lente

**Optimiser** :
- Réduire le niveau de détail des journaux
- Nettoyer les anciennes données d'audit
- Vérifier les ressources système

---

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 📖 Explorer la [référence API](api-reference.md)
- 🚀 Voir le [guide de démarrage rapide](getting-started.md)
