# Guide de dépannage

> **Version : v0.1.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | **Français** | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md)

## Problèmes courants

### Construction et compilation

#### Problème : Échec du build, dépendances manquantes

**Symptômes** :
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Solution** :
```bash
dotnet restore
dotnet build
```

#### Problème : SDK .NET non trouvé

**Symptômes** :
```
The .NET SDK could not be found
```

**Solution** :
1. Installer le SDK .NET 9 : https://dotnet.microsoft.com/download/dotnet/9.0
2. Vérifier l'installation :
```bash
dotnet --version
```

---

### Problèmes de connexion IA

#### Problème : Connexion Ollama refusée

**Symptômes** :
```
Failed to connect to Ollama at http://localhost:11434
```

**Solution** :
```bash
# Vérifier si Ollama fonctionne
ollama list

# Démarrer Ollama
ollama serve

# Tester la connexion
curl http://localhost:11434/api/tags
```

#### Problème : Modèle non trouvé

**Symptômes** :
```
model "qwen2.5:7b" not found
```

**Solution** :
```bash
# Télécharger le modèle nécessaire
ollama pull qwen2.5:7b

# Lister les modèles disponibles
ollama list
```

#### Problème : Erreur DashScope 404

**Symptômes** :
```
HTTP 404: Model not found
```

**Solution** :
1. Vérifier la clé API
2. Vérifier le nom du modèle avec le catalogue DashScope
3. Vérifier le point de terminaison régional
4. Vérifier l'accès du compte au modèle

---

### Problèmes d'exécution

#### Problème : Port déjà utilisé

**Symptômes** :
```
HttpListenerException: Address already in use
```

**Solution** :

**Windows** :
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac** :
```bash
lsof -ti:8080 | xargs kill -9
```

**Ou changer le port dans la configuration**.

#### Problème : Le Being ne démarre pas

**Symptômes** :
- Le statut du Being affiche « Error »
- Les journaux montrent des erreurs d'initialisation

**Solution** :
1. Vérifier que le fichier âme existe et est valide
2. Vérifier que le client IA est configuré
3. Examiner les journaux pour des erreurs spécifiques :
```bash
tail -f logs/*.log
```

#### Problème : Mémoire insuffisante

**Symptômes** :
```
OutOfMemoryException
```

**Solution** :
1. **SiliconLife.Default** : Augmenter la taille du heap :
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast** : La version Fast a une consommation mémoire élevée (~500 Mo). Si la mémoire est constamment limitée, il est recommandé de :
   - Réduire le nombre de Silicon Beings fonctionnant simultanément
   - Nettoyer les anciennes données pour libérer de la mémoire

3. Nettoyer les anciennes données :
```bash
# Archiver les anciens journaux
mv logs/ logs-archive/
mkdir logs

# Nettoyer l'ancien stockage
# Via l'interface Web : Gestion du stockage > Nettoyer
```

> **Astuce** : SiliconLife.Default a une faible consommation mémoire (~200 Mo), adapté aux environnements à mémoire limitée ; SiliconLife.Fast a une consommation mémoire plus élevée mais de meilleures performances, adapté aux environnements de production.

---

### Problèmes de permissions

#### Problème : Permission refusée

**Symptômes** :
```
Permission denied: disk:write
```

**Solution** :
1. Vérifier les permissions actuelles :
```bash
curl http://localhost:8080/api/permissions
```

2. Accorder la permission :
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. Ou utiliser l'interface Web : Gestion des permissions

#### Problème : La permission n'expire pas

**Symptômes** :
- La permission est encore active après le délai d'expiration

**Solution** :
1. Vérifier la synchronisation de l'horloge système
2. Vérifier que le champ `expiresAt` est correctement défini
3. Vider le cache des permissions

---

### Problèmes de l'interface Web

#### Problème : Interface Web inaccessible

**Symptômes** :
- Le navigateur affiche « Connection refused »

**Solution** :
1. Vérifier que le serveur fonctionne
2. Vérifier l'URL correcte : `http://localhost:8080`
3. Vérifier les paramètres du pare-feu
4. Examiner les journaux pour les erreurs de démarrage

#### Problème : SSE ne fonctionne pas

**Symptômes** :
- Les mises à jour en temps réel n'apparaissent pas
- Le chat ne se stream pas

**Solution** :
1. Vérifier le support SSE du navigateur
2. Désactiver le buffering du proxy pour SSE
3. Vérifier la stabilité du réseau
4. Essayer un autre navigateur

#### Problème : L'interface semble cassée

**Symptômes** :
- Styles incorrects
- Mise en page brisée

**Solution** :
1. Vider le cache du navigateur
2. Essayer un autre skin : Paramètres > Skin
3. Vérifier la console du navigateur pour les erreurs
4. Désactiver les extensions du navigateur

---

### Problèmes de stockage

#### Problème : Données illisibles/inscriptibles

**Symptômes** :
```
IOException: Access denied
```

**Solution** :
1. Vérifier les permissions de fichiers
2. Vérifier que le chemin de stockage existe
3. Vérifier l'espace disque
4. Exécuter avec les permissions appropriées

#### Problème : Corruption de données

**Symptômes** :
- Erreurs d'analyse JSON
- Perte de données

**Solution** :
1. Restaurer depuis une sauvegarde
2. Vérifier l'intégrité du stockage :
```bash
# Via l'interface Web : Système > Vérification du stockage
```

3. Réparer manuellement les fichiers corrompus

#### Problème : Fichier de stockage SpeedyPack corrompu (version Fast)

**Symptômes** :
- Le fichier `.spk` ne peut pas être chargé
- L'initialisation de SpeedyStorage échoue

**Solution** :
1. Utiliser l'outil `SiliconLife.Speedy.Manager` pour vérifier et réparer les fichiers `.spk`
2. Vérifier que le fichier d'index `.spk.idx` correspond au fichier `.spk`
3. Si le fichier d'index est corrompu, supprimer le fichier `.spk.idx` — le système recréera l'index automatiquement
4. Restaurer le fichier `.spk` depuis une sauvegarde

#### Problème : Échec de l'auto-compaction SpeedyPack (version Fast)

**Symptômes** :
- Le fichier `.spk` grandit continuellement
- L'espace disque devient insuffisant

**Solution** :
1. Vérifier que `SpeedyPackAutoCompactor` fonctionne correctement
2. Déclencher manuellement l'opération de compaction
3. Vérifier la configuration du seuil de compaction
4. Utiliser l'outil `SiliconLife.Speedy.Manager` pour la compaction manuelle

---

### Problèmes d'exécution d'outils

#### Problème : Outil non trouvé

**Symptômes** :
```
Tool "xyz" not found
```

**Solution** :
1. Vérifier que le nom de l'outil est correct
2. Vérifier l'outil dans le répertoire des outils
3. Reconstruire le projet
4. Vérifier que l'outil est correctement implémenté

#### Problème : L'outil retourne une erreur

**Symptômes** :
```
Tool execution failed: ...
```

**Solution** :
1. Examiner les journaux de l'outil
2. Vérifier les paramètres d'entrée
3. Tester l'outil indépendamment
4. Vérifier les permissions

---

### Problèmes de plugins

#### Problème : Échec du chargement du plugin

**Symptômes** :
```
Plugin load failed: Security check failed
```

**Solution** :
1. Vérifier si le plugin référence des espaces de noms interdits (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Vérifier que le plugin ne référence que des assemblies de la liste blanche de confiance
3. Vérifier que le plugin implémente correctement l'interface `IPlugin`
4. Examiner les journaux pour les détails des erreurs de vérification de sécurité

#### Problème : Outils du plugin non enregistrés

**Symptômes** :
- Le plugin est chargé avec succès, mais les outils n'apparaissent pas dans la liste

**Solution** :
1. Confirmer que la classe d'outil dans le plugin implémente correctement l'interface `ITool`
2. Vérifier que la classe d'outil est publique
3. Vérifier que `ToolManager.ScanAllPluginAssemblies()` a été appelé
4. Reconstruire le plugin et redémarrer l'application

---

### Problèmes de notes de travail

#### Problème : Impossible de créer une note de travail

**Symptômes** :
```
Failed to create work note
```

**Solution** :
1. Vérifier que le Being existe et fonctionne
2. Vérifier que le chemin de stockage a les droits d'écriture
3. Vérifier que le contenu n'est pas vide (contenu requis)
4. Examiner les journaux pour les détails de l'erreur

#### Problème : Recherche de notes sans résultats

**Symptômes** :
- La recherche par mot-clé retourne des résultats vides
- Mais des notes pertinentes existent certainement

**Solution** :
1. Vérifier l'orthographe du mot-clé
2. Essayer un mot-clé plus générique
3. Vérifier que la note contient le mot-clé (sensible à la casse)
4. Augmenter la valeur du paramètre `max_results`

#### Problème : Génération du répertoire des notes lente

**Symptômes** :
- Temps de réponse long pour la génération du répertoire
- Le Being a beaucoup de notes (>1000 pages)

**Solution** :
1. C'est normal, doit parcourir toutes les notes
2. Archiver régulièrement les anciennes notes
3. Utiliser la fonction de recherche au lieu du parcours du répertoire
4. Optimisation prévue : mécanisme de cache du répertoire

---

### Problèmes du réseau de connaissances

#### Problème : La requête de connaissances retourne des résultats vides

**Symptômes** :
```
No knowledge triples found
```

**Solution** :
1. Vérifier l'orthographe du sujet et du prédicat
2. Vérifier que la connaissance a été ajoutée au réseau
3. Utiliser la fonction de recherche pour la correspondance floue :
```json
{
  "action": "search",
  "query": "mot-clé"
}
```

#### Problème : Échec de la recherche de chemin de connaissance

**Symptômes** :
```
No path found between concepts
```

**Solution** :
1. Vérifier que les deux concepts existent dans le réseau de connaissances
2. Vérifier qu'un chemin de connexion existe (peut-être pas de relation directe/indirecte)
3. Ajouter plus de connaissances pour établir une connexion
4. Réduire la limite de longueur de chemin (si définie)

#### Problème : Échec de la validation de connaissance

**Symptômes** :
```
Knowledge validation failed
```

**Solution** :
1. Vérifier que le format du triplet est correct (sujet, prédicat, objet requis)
2. Vérifier que la valeur de confiance est dans la plage 0.0-1.0
3. Vérifier les triplets dupliqués
4. Examiner les détails de l'erreur de validation pour le problème spécifique

#### Problème : Statistiques du réseau de connaissances inexactes

**Symptômes** :
- Les chiffres de statistiques ne sont pas ceux attendus
- Les statistiques ne sont pas mises à jour après l'ajout de connaissances

**Solution** :
1. Les statistiques peuvent prendre quelques secondes pour se mettre à jour (cache)
2. Vérifier que l'opération de suppression a été exécutée avec succès
3. Redémarrer l'application pour forcer la mise à jour des statistiques
4. Requérir les statistiques via l'API

---

### Problèmes de gestion de projet

#### Problème : Impossible de créer un projet

**Symptômes** :
```
Failed to create project
```

**Solution** :
1. Vérifier que le nom du projet n'est pas vide (requis)
2. Vérifier que le nom du projet n'est pas dupliqué
3. Vérifier que le chemin de stockage a les droits d'écriture
4. Examiner les journaux pour les détails de l'erreur

#### Problème : Données de projet perdues

**Symptômes** :
- Les informations du projet ne sont pas chargeables
- Les fichiers du projet sont corrompus

**Solution** :
1. Vérifier que le répertoire de stockage du projet existe
2. Restaurer les données du projet depuis une sauvegarde
3. Vérifier que le format du fichier JSON est correct
4. Réparer manuellement les fichiers de projet corrompus

---

## Débogage

### Activer les journaux détaillés

Modifier la configuration :
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Examiner les journaux

Les journaux sont stockés dans :
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Affichage en temps réel :
```bash
tail -f logs/*.log
```

### Utiliser le débogueur

**SiliconLife.Default (implémentation standard)** :
```bash
# Exécuter avec le débogueur
dotnet run --project src/SiliconLife.Default --configuration Debug

# Attacher le débogueur
# Via l'IDE : Attacher au processus > SiliconLife.Default
```

**SiliconLife.Fast (version haute performance)** :
```bash
# Exécuter avec le débogueur
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Attacher le débogueur
# Via l'IDE : Attacher au processus > SiliconLife.Fast
```

> **Recommandation** : Pour le débogage de développement, utiliser SiliconLife.Default est recommandé. Après vérification architecturale réussie, utiliser SiliconLife.Fast pour le déploiement en production.

---

## Problèmes de performance

### Temps de réponse lents

**Optimiser** :
1. Réduire la complexité du modèle IA
2. Activer le cache
3. Nettoyer les anciennes données
4. Augmenter les ressources système

### Utilisation CPU élevée

**Vérifier** :
- Trop de Beings en cours d'exécution
- Boucle infinie dans les outils
- Exécution fréquente des minuteries

**Solution** :
- Réduire les Beings parallèles
- Optimiser le code des outils
- Ajuster l'intervalle des minuteries

### Utilisation mémoire élevée

**Surveiller** :
```bash
# Via l'interface Web : Tableau de bord > Mémoire
```

**Optimiser** :
- Nettoyer les anciens souvenirs
- Réduire la taille du contexte
- Implémenter la pagination

---

## Obtenir de l'aide

### Consulter la documentation

- [Guide de démarrage rapide](getting-started.md)
- [Guide de développement](development-guide.md)
- [Référence API](api-reference.md)
- [Guide d'architecture](architecture.md)

### Examiner les journaux

Toujours examiner d'abord les journaux pour les détails des erreurs.

### Support communautaire

- GitHub Issues : Signaler des bugs
- Discussions : Poser des questions
- Documentation : Rechercher des solutions

---

## Procédures d'urgence

### Crash du système

1. Examiner les journaux pour la cause
2. Redémarrer l'application :

**SiliconLife.Default (implémentation standard)** :
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (version principale de production)** :
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Restaurer depuis une sauvegarde si nécessaire

### Perte de données

1. Arrêter immédiatement l'application
2. Vérifier les fichiers de sauvegarde
3. Restaurer les données
4. Vérifier l'intégrité

### Faille de sécurité

1. Arrêter tous les Beings
2. Révoquer toutes les permissions
3. Examiner les journaux d'audit
4. Vérifier le contrôle d'accès
5. Redémarrer avec des permissions restreintes

---

## Prévention

### Bonnes pratiques

1. **Sauvegardes régulières**
   - Sauvegarder le répertoire de données
   - Sauvegarder la configuration
   - Tester le processus de restauration

2. **Surveiller les ressources**
   - Surveiller l'utilisation CPU/mémoire
   - Surveiller l'espace disque
   - Vérifier la connexion réseau

3. **Rester à jour**
   - Mettre à jour le SDK .NET
   - Mettre à jour les dépendances
   - Appliquer les correctifs de sécurité

4. **Tester les modifications**
   - Tester d'abord en développement
   - Utiliser le contrôle de version
   - Documenter les modifications

---

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 🚀 Voir le [guide de démarrage rapide](getting-started.md)
- 🔒 Consulter la [documentation de sécurité](security.md)
