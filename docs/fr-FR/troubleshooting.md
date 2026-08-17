# Guide de dépannage

> **Version : v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Русский](../ru-RU/troubleshooting.md)

## Problèmes courants

### Construction et compilation

#### Problème : Échec de la construction, dépendances manquantes

**Symptômes** :
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Solution** :
```bash
dotnet restore
dotnet build
```

#### Problème : SDK .NET introuvable

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
# Vérifier si Ollama est en cours d'exécution
ollama list

# Démarrer Ollama
ollama serve

# Tester la connexion
curl http://localhost:11434/api/tags
```

#### Problème : Modèle introuvable

**Symptômes** :
```
model "qwen2.5:7b" not found
```

**Solution** :
```bash
# Télécharger le modèle requis
ollama pull qwen2.5:7b

# Lister les modèles disponibles
ollama list
```

#### Problème : Erreur 404 DashScope (Bailian)

**Symptômes** :
```
HTTP 404: Model not found
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Vérifier que le nom du modèle correspond au catalogue DashScope
3. Vérifier que le point de terminaison régional est correct
4. Vérifier que le compte a accès au modèle

#### Problème : Échec de connexion Volcengine Ark

**Symptômes** :
```
HTTP 401: Unauthorized
ou
HTTP 404: Endpoint not found
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Vérifier que le format de l'URL du point de terminaison est correct (par défaut : `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Confirmer que le paramètre Model utilise l'ID du point de terminaison d'inférence (par ex. `ep-20241212123456-abcde`), et non le nom du modèle
4. Vérifier que le compte a accès au point de terminaison

#### Problème : Erreur de connexion DeepSeek

**Symptômes** :
```
HTTP 401: Unauthorized
ou
HTTP 429: Rate limit exceeded
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Vérifier que l'endpoint est correct (`https://api.deepseek.com`)
3. En cas d'erreur 429, attendre et réessayer (limite de débit atteinte)
4. Vérifier que le compte dispose de crédits suffisants

#### Problème : Erreur de connexion Zhipu AI (GLM)

**Symptômes** :
```
HTTP 401: Unauthorized
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Vérifier que l'endpoint est correct (`https://open.bigmodel.cn/api/paas/v4`)
3. Vérifier que le modèle est disponible pour votre compte

#### Problème : Erreur de connexion Baidu Qianfan (ERNIE)

**Symptômes** :
```
HTTP 401: Unauthorized
ou
HTTP 403: Forbidden
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Vérifier que l'endpoint est correct (`https://qianfan.baidubce.com/v2`)
3. Vérifier que l'application est autorisée à utiliser le modèle demandé

#### Problème : Erreur de connexion Tencent Hunyuan

**Symptômes** :
```
HTTP 401: Unauthorized
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Essayer l'autre endpoint : TokenHub (`https://tokenhub.tencentmaas.com/v1`) ou Legacy (`https://api.hunyuan.cloud.tencent.com/v1`)
3. Vérifier que le modèle est disponible pour votre compte

#### Problème : Erreur de connexion MiniMax

**Symptômes** :
```
HTTP 401: Unauthorized
ou
base_resp status_code non nul
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Vérifier que l'endpoint est correct (`https://api.minimaxi.com/v1`)
3. Vérifier le champ `base_resp` dans la réponse pour plus de détails sur l'erreur

#### Problème : Erreur de connexion Moonshot (Kimi)

**Symptômes** :
```
HTTP 401: Unauthorized
ou
HTTP 429: Rate limit exceeded
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Vérifier que l'endpoint est correct (`https://api.moonshot.cn/v1`)
3. En cas de timeout, augmenter le délai (les modèles thinking peuvent nécessiter plus de temps)

#### Problème : Erreur de connexion SiliconFlow

**Symptômes** :
```
HTTP 401: Unauthorized
ou
HTTP 404: Model not found
```

**Solution** :
1. Vérifier que la clé API est correcte
2. Vérifier que l'endpoint est correct (`https://api.siliconflow.cn/v1`)
3. Vérifier que le nom du modèle inclut le préfixe du fournisseur (par ex. `deepseek-ai/DeepSeek-V3.2`)
4. Consulter la liste des modèles disponibles sur la plateforme SiliconFlow

---

### Problèmes d'exécution

#### Problème : Port déjà occupé

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

**Ou modifier le port dans la configuration**.

#### Problème : L'être ne peut pas démarrer

**Symptômes** :
- L'état de l'être affiche « Error »
- Les journaux indiquent un échec d'initialisation

**Solution** :
1. Vérifier que le Fichier d'Âme existe et est valide
2. Vérifier que le client IA est configuré
3. Consulter les journaux pour l'erreur spécifique :
```bash
tail -f logs/*.log
```

#### Problème : Mémoire insuffisante

**Symptômes** :
```
OutOfMemoryException
```

**Solution** :
1. **SiliconLife.Default** : Augmenter la taille du tas :
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast** : La version Fast a une consommation mémoire intrinsèquement plus élevée (~500 Mo). Si la mémoire reste insuffisante, il est recommandé de :
   - Réduire le nombre d'Êtres de Silicium concurrents
   - Nettoyer les anciennes données pour libérer de la mémoire

3. Nettoyer les anciennes données :
```bash
# Archiver les anciens journaux
mv logs/ logs-archive/
mkdir logs

# Nettoyer les anciens souvenirs
# Via l'UI Web : Gestion de la mémoire > Nettoyage
```

> **Astuce** : SiliconLife.Default a une consommation mémoire plus faible (~200 Mo), adaptée aux environnements à mémoire limitée ; SiliconLife.Fast a une consommation mémoire plus élevée mais de meilleures performances, adaptée aux environnements de production.

---

### Problèmes d'autorisations

#### Problème : Autorisation refusée

**Symptômes** :
```
Permission denied: FileAccess C:\Windows
```

**Solution** :
1. Vérifier les autorisations actuelles :
```bash
curl http://localhost:8080/api/permissions/list
```

2. Accorder une autorisation :
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

3. Ou utiliser l'UI Web : Gestion des autorisations

#### Problème : Les autorisations n'expirent pas

**Symptômes** :
- Les autorisations restent valides après l'heure d'expiration

**Solution** :
1. Vérifier la synchronisation de l'horloge système
2. Vérifier que le champ `expiresAt` est correctement défini
3. Vider le cache des autorisations

---

### Problèmes de l'UI Web

#### Problème : Impossible d'accéder à l'UI Web

**Symptômes** :
- Le navigateur affiche « Connection refused »

**Solution** :
1. Vérifier que le serveur est en cours d'exécution
2. Vérifier l'URL correcte : `http://localhost:8080`
3. Vérifier les paramètres du pare-feu
4. Consulter les journaux pour les erreurs de démarrage

#### Problème : SSE ne fonctionne pas

**Symptômes** :
- Les mises à jour en temps réel n'apparaissent pas
- Le chat ne se diffuse pas en continu

**Solution** :
1. Vérifier que le navigateur prend en charge SSE
2. Désactiver la mise en tampon du proxy pour SSE
3. Vérifier la stabilité du réseau
4. Essayer un autre navigateur

#### Problème : L'UI semble endommagée

**Symptômes** :
- Styles incorrects
- Mise en page cassée

**Solution** :
1. Vider le cache du navigateur
2. Essayer un autre habillage : Paramètres > Habillage
3. Vérifier les erreurs dans la console du navigateur
4. Désactiver les extensions du navigateur

---

### Problèmes de stockage

#### Problème : Impossible de lire/écrire des données

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
# Via l'UI Web : Système > Vérification du stockage
```

3. Réparer manuellement les fichiers corrompus

#### Problème : Fichier de stockage SpeedyPack corrompu (version Fast)

**Symptômes** :
- Impossible de charger le fichier `.spk`
- Échec de l'initialisation de SpeedyStorage

**Solution** :
1. Utiliser l'outil `SiliconLife.Speedy.Manager` pour vérifier et réparer les fichiers `.spk`
2. Vérifier que le fichier d'index `.spk.idx` correspond au fichier `.spk`
3. Si le fichier d'index est corrompu, supprimer le fichier `.spk.idx`, le système reconstruira automatiquement l'index
4. Restaurer le fichier `.spk` depuis une sauvegarde

#### Problème : Échec de la compression automatique SpeedyPack (version Fast)

**Symptômes** :
- Le fichier `.spk` continue de croître
- Espace disque insuffisant

**Solution** :
1. Vérifier que le `SpeedyPackAutoCompactor` fonctionne correctement
2. Déclencher manuellement l'opération de compression
3. Vérifier la configuration du seuil de compression
4. Utiliser l'outil `SiliconLife.Speedy.Manager` pour compresser manuellement

---

### Problèmes d'exécution d'outils

#### Problème : Outil introuvable

**Symptômes** :
```
Tool "xyz" not found
```

**Solution** :
1. Vérifier que le nom de l'outil est correct
2. Vérifier que l'outil est dans le répertoire Tools
3. Reconstruire le projet
4. Vérifier que l'outil est correctement implémenté

#### Problème : L'outil renvoie une erreur

**Symptômes** :
```
Tool execution failed: ...
```

**Solution** :
1. Consulter les journaux de l'outil
2. Vérifier les paramètres d'entrée
3. Tester l'outil indépendamment
4. Vérifier les autorisations

---

### Problèmes de plugins

#### Problème : Échec du chargement du plugin

**Symptômes** :
```
Plugin load failed: Security check failed
```

**Solution** :
1. Vérifier que le plugin ne référence pas d'espaces de noms interdits non déclarables (ex. : `System.Runtime.InteropServices`, `System.Reflection.Emit`, `Microsoft.CodeAnalysis`)
2. Si le plugin nécessite `System.IO` ou `System.Net.Http`, vérifier qu'il a déclaré les capacités `FileIO` ou `Network` via `[PluginCapability]`
3. Vérifier que le plugin ne référence que des assemblys dans la liste blanche des assemblys de confiance
4. Vérifier que le plugin implémente correctement l'interface `IPlugin`
5. Consulter les journaux pour les détails de l'échec de la vérification de sécurité

#### Problème : Outils du plugin non enregistrés

**Symptômes** :
- Le plugin se charge avec succès mais les outils n'apparaissent pas dans la liste

**Solution** :
1. Confirmer que la classe d'outil du plugin implémente correctement l'interface `ITool`
2. Vérifier que la classe d'outil est publique
3. Vérifier que `ToolManager.ScanAllPluginAssemblies()` est appelé
4. Reconstruire le plugin et redémarrer l'application

---

### Problèmes avec les Compétences

#### Problème : La compétence n'apparaît pas dans la liste des compétences ou n'est pas visible par l'IA

**Symptômes** :
- La page des compétences de l'UI Web enregistre avec succès, mais la liste ne l'affiche pas / l'IA n'appelle pas la compétence

**Solution** :
1. Vérifier que l'`id` et la `description` de la compétence ne sont pas vides (les brouillons ne sont pas exposés à l'IA)
2. Les compétences dont les métadonnées sont incomplètes (`NeedsCompletion`) ne sont pas injectées dans l'IA — compléter les métadonnées du front-matter YAML ou laisser l'IA les compléter avant de sauvegarder
3. Vérifier si la matrice d'autorisations a désactivé `{skillId}:execute` (les compétences désactivées sont invisibles pour l'IA)
4. Confirmer que le commutateur global `SkillEnabled` est à true
5. Le rechargement à chaud prend au maximum 30 secondes pour prendre effet, attendre puis rafraîchir ou redémarrer

#### Problème : Échec de l'exécution de la compétence avec le message "not in whitelist"

**Symptômes** :
```
Tool 'xxx' is not available in skill 'yyy' (not in whitelist)
```

**Solution** :
- Ajouter l'outil à la `tool_whitelist` de la compétence, ou vider la liste blanche pour hériter de tous les outils de l'Être de Silicium

#### Problème : Limite du nombre de compétences atteinte

**Symptômes** :
```
Custom skill limit reached (50)
```

**Solution** :
1. Supprimer les compétences personnalisées inutilisées
2. Ou augmenter la configuration `MaxCustomSkillsPerBeing`

---

### Problèmes MCP

#### Problème : Échec de connexion au serveur MCP

**Symptômes** :
- Le statut du serveur affiche `error` ou `disconnected`, `lastError` n'est pas vide

**Solution** :
1. Serveur stdio : confirmer que `command` est exécutable (ex. : `npx` dans le PATH), `arguments` corrects
2. Serveur http : vérifier que l'URL `endpoint` est accessible (pare-feu, proxy)
3. Cliquer sur **Reconnecter** sur la page /mcp
4. Consulter les détails de `lastError`, les causes courantes étant : commande inexistante, version incompatible, point de terminaison 404

#### Problème : Les outils MCP ne sont pas injectés dans l'Être de Silicium

**Symptômes** :
- Le serveur est connecté (`connected`) mais l'IA ne peut pas appeler l'outil `mcp_xxx_yyy`

**Solution** :
1. Confirmer que `enabled` du serveur est à true
2. Confirmer que le commutateur global `McpEnabled` est à true
3. Vérifier la matrice d'autorisations : `mcp_{serverId}_{toolName}:execute` n'est pas désactivé
4. Dans la conversation de l'Être de Silicium, utiliser l'outil `mcp` (`list_tools`) pour vérifier les noms d'outils réellement injectés

#### Problème : L'ajout de serveur renvoie une erreur de format d'ID

**Symptômes** :
```
Server id must contain only lowercase letters, digits and underscores
```

**Solution** :
- L'ID du serveur ne permet que les minuscules, les chiffres et les tirets bas (ex. : `filesystem`, `github_tools`)

---

### Problèmes avec la Plateforme IM

#### Problème : Les messages Feishu ne sont pas reçus

**Solution** :
1. Vérifier l'adresse de rappel et le port configurés dans l'abonnement aux événements de la plateforme ouverte Feishu (`listenPort` + `callbackPath`)
2. Confirmer que l'`Encrypt Key` / `Verification Token` correspond à la configuration
3. En développement local, l'assistant d'autorisation OAuth peut être utilisé (autorisation en un clic sur la page de configuration) ; le rappel d'événements nécessite un accès au réseau public ou un tunneling
4. Consulter les journaux pour les erreurs de vérification de signature / déchiffrement

#### Problème : Délai d'expiration de l'autorisation OAuth

**Symptômes** :
- La page d'autorisation affiche le statut `timeout`

**Solution** :
1. La session d'autorisation est valide 5 minutes, après expiration cliquer à nouveau sur le bouton d'autorisation
2. Confirmer que l'adresse de rappel `/im/feishu/callback` est accessible par Feishu (`redirectBaseUrl` configuré correctement)
3. L'affichage de l'état côté front-end dépend de SSE, en cas de déconnexion SSE, le sondage `/im/{platform}/status` peut servir de solution de repli

#### Problème : L'espace réservé `${ENV_VAR}` n'est pas résolu

**Symptômes** :
- La connexion à la plateforme IM échoue, la valeur de configuration est toujours le texte de l'espace réservé

**Solution** :
1. Confirmer que la variable d'environnement a été définie avant le démarrage du processus (redémarrer l'application pour prise en compte)
2. Vérifier l'orthographe du nom de variable (seuls `[A-Za-z_][A-Za-z0-9_]*` sont pris en charge)
3. Note : conserver les espaces réservés dans config.json est un comportement de conception, la résolution se fait sur la copie en mémoire

#### Problème : Un seul des plusieurs plateformes IM reçoit les messages

**Solution** :
- Les messages sortants sont diffusés vers toutes les plateformes activées, l'échec d'envoi sur une plateforme est isolé silencieusement — vérifier si le jeton de cette plateforme a expiré (réautoriser ou mettre à jour la clé)

---

### Problèmes de notes de travail

#### Problème : Impossible de créer une note de travail

**Symptômes** :
```
Failed to create work note
```

**Solution** :
1. Vérifier que l'être existe et est en cours d'exécution
2. Vérifier que le chemin de stockage a les permissions d'écriture
3. Vérifier que le contenu n'est pas vide (contenu obligatoire)
4. Consulter les journaux pour les détails de l'erreur

#### Problème : La recherche de notes ne renvoie aucun résultat

**Symptômes** :
- La recherche par mot-clé renvoie des résultats vides
- Mais vous êtes certain qu'il existe des notes pertinentes

**Solution** :
1. Vérifier l'orthographe du mot-clé
2. Essayer un mot-clé plus général
3. Vérifier que la note contient le mot-clé (sensible à la casse)
4. Augmenter la valeur du paramètre `max_results`

#### Problème : Génération lente du répertoire des notes

**Symptômes** :
- Temps de réponse long lors de la génération du répertoire
- L'être a un grand nombre de notes (>1000 pages)

**Solution** :
1. C'est un comportement normal, nécessitant le parcours de toutes les notes
2. Envisager d'archiver régulièrement les anciennes notes
3. Utiliser la fonction de recherche au lieu de la navigation par répertoire
4. Optimisation planifiée : ajout d'un mécanisme de cache de répertoire

---

### Problèmes du Réseau de Connaissances

#### Problème : La requête de connaissances renvoie des résultats vides

**Symptômes** :
```
No knowledge triples found
```

**Solution** :
1. Vérifier l'orthographe du sujet et du prédicat
2. Vérifier que la connaissance a été ajoutée au réseau
3. Utiliser la fonction de recherche pour une correspondance floue :
```json
{
  "action": "search",
  "query": "mot-clé"
}
```

#### Problème : Échec de la recherche de chemin de connaissances

**Symptômes** :
```
No path found between concepts
```

**Solution** :
1. Vérifier que les deux concepts existent dans le Réseau de Connaissances
2. Vérifier s'il existe un chemin d'association (il peut ne pas y avoir de relation directe ou indirecte)
3. Essayer d'ajouter plus de connaissances pour établir des connexions
4. Réduire la limite de longueur de chemin (si définie)

#### Problème : Échec de la validation des connaissances

**Symptômes** :
```
Knowledge validation failed
```

**Solution** :
1. Vérifier que le format du triplet est correct (sujet, prédicat, objet obligatoires)
2. Vérifier que la confiance est dans la plage 0.0-1.0
3. Vérifier s'il existe des triplets en double
4. Consulter les détails de l'erreur de validation pour comprendre le problème spécifique

#### Problème : Statistiques du Réseau de Connaissances inexactes

**Symptômes** :
- Les chiffres statistiques ne correspondent pas aux attentes
- Les statistiques ne sont pas mises à jour après l'ajout de connaissances

**Solution** :
1. Les statistiques peuvent nécessiter quelques secondes pour se mettre à jour (cache)
2. Vérifier si des opérations de suppression n'ont pas été exécutées avec succès
3. Redémarrer l'application pour forcer le rafraîchissement des statistiques
4. Requêter à nouveau les statistiques via l'API

---

### Problèmes de gestion de projet

#### Problème : Impossible de créer un projet

**Symptômes** :
```
Failed to create project
```

**Solution** :
1. Vérifier que le nom du projet n'est pas vide (obligatoire)
2. Vérifier que le nom du projet n'est pas en double
3. Vérifier que le chemin de stockage a les permissions d'écriture
4. Consulter les journaux pour les détails de l'erreur

#### Problème : Perte de données du projet

**Symptômes** :
- Impossible de charger les informations du projet
- Fichier de projet corrompu

**Solution** :
1. Vérifier que le répertoire de stockage du projet existe
2. Restaurer les données du projet depuis une sauvegarde
3. Vérifier que le format du fichier JSON est correct
4. Réparer manuellement le fichier de projet corrompu

#### Problème : Échec de l'attribution de rôle au projet

**Symptômes** :
```
Failed to assign role
```

**Solution** :
1. Confirmer que l'Être de Silicium a rejoint le projet
2. Vérifier que le nom du rôle est valide
3. Vérifier que l'opérateur est le Curateur de Silicium
4. Consulter les journaux pour les détails de l'erreur

#### Problème : Le flux de travail ne peut pas démarrer

**Symptômes** :
- Échec de la création de l'instance de flux de travail
- Les transitions d'état ne s'exécutent pas

**Solution** :
1. Vérifier que le modèle de flux de travail est défini
2. Vérifier que l'état initial est correctement défini
3. Confirmer que le projet est lié à un modèle de flux de travail
4. Consulter les journaux du flux de travail pour les erreurs de transition

---

### Problèmes d'autorisations d'outils

#### Problème : Opération d'outil refusée

**Symptômes** :
```
Tool operation denied: network:post
```

**Solution** :
1. Vérifier la configuration des autorisations d'outil de l'Être de Silicium :
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. Mettre à jour les autorisations d'outil :
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. Ou utiliser l'UI Web : Êtres → Autorisations d'outil

#### Problème : Les autorisations d'outil du projet ne prennent pas effet

**Symptômes** :
- Les autorisations d'outil au niveau du projet ne fonctionnent pas comme prévu

**Solution** :
1. Confirmer que les autorisations au niveau du projet sont correctement configurées
2. Vérifier s'il y a un conflit entre les autorisations au niveau de l'Être de Silicium et au niveau du projet
3. Les autorisations au niveau du projet sont indépendantes du niveau Être de Silicium, les deux sont intersectées
4. Consulter le journal d'audit pour confirmer les résultats de la vérification des autorisations

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

### Consulter les journaux

Les journaux sont stockés dans :
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Consultation en temps réel :
```bash
tail -f logs/*.log
```

### Utiliser le débogueur

**SiliconLife.Default (implémentation par défaut)** :
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

> **Recommandation** : Utiliser SiliconLife.Default pendant la phase de développement et de débogage, puis passer à SiliconLife.Fast pour le déploiement en production après validation de l'architecture.

---

## Problèmes de performance

### Temps de réponse lent

**Optimisation** :
1. Réduire la complexité du modèle IA
2. Activer le cache
3. Nettoyer les anciennes données
4. Augmenter les ressources système

### Utilisation CPU élevée

**Vérifications** :
- Trop d'êtres en cours d'exécution
- Boucle infinie dans un outil
- Exécution fréquente des minuteurs

**Solutions** :
- Réduire les êtres concurrents
- Optimiser le code des outils
- Ajuster les intervalles des minuteurs

### Utilisation mémoire élevée

**Surveillance** :
```bash
# Via l'UI Web : Tableau de bord > Mémoire
```

**Optimisation** :
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

### Consulter les journaux

Vérifiez toujours les journaux en premier pour les détails des erreurs.

### Support communautaire

- GitHub Issues : Signaler des bugs
- Discussions : Poser des questions
- Documentation : Rechercher des solutions

---

## Procédures d'urgence

### Crash système

1. Consulter les journaux pour identifier la cause
2. Redémarrer l'application :

**SiliconLife.Default (implémentation par défaut)** :
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (version de production recommandée)** :
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

1. Arrêter tous les êtres
2. Révoquer toutes les autorisations
3. Consulter les journaux d'audit
4. Vérifier les contrôles d'accès
5. Redémarrer avec des autorisations restreintes

---

## Prévention

### Bonnes pratiques

1. **Sauvegardes régulières**
   - Sauvegarder le répertoire de données
   - Sauvegarder la configuration
   - Tester les procédures de restauration

2. **Surveiller les ressources**
   - Surveiller l'utilisation CPU/mémoire
   - Surveiller l'espace disque
   - Vérifier les connexions réseau

3. **Maintenir à jour**
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
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md)
- 🔒 Consulter la [documentation de sécurité](security.md)
