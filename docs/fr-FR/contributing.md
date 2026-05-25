# Guide de contribution

> **Version : v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | **Français** | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

Merci de votre intérêt pour contribuer à SiliconLifeCollective !

## Contributions en double version

Ce projet a deux versions d'implémentation. Vous pouvez contribuer selon vos intérêts :

### SiliconLife.Default (Version standard)
- **Stack technologique** : Application console .NET 9
- **Direction de contribution** : Développement de fonctionnalités principales, implémentation d'outils, localisation, documentation
- **Public cible** : Tous les développeurs

### SiliconLife.Fast (Version haute performance)
- **Stack technologique** : Application Windows Forms .NET 9
- **Direction de contribution** : Optimisation des performances, stockage SpeedyPack, barre d'état système, concurrence sans verrou
- **Public cible** : Développeurs avec expérience Windows et intérêt pour l'optimisation des performances

> **Note importante** : Les deux versions partagent les projets SiliconLife.Core et SiliconLife.Common. Les améliorations des interfaces principales affectent les deux versions.

## Code de conduite

Ce projet suit la licence Apache 2.0. Restez respectueux et professionnel dans toutes les interactions.

---

## Démarrage rapide

### 1. Forker le dépôt

Cliquez sur le bouton « Fork » sur GitHub pour créer votre propre copie.

### 2. Cloner votre fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Configurer l'environnement de développement

```bash
# Installer .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Restaurer les dépendances
dotnet restore

# Construire le projet
dotnet build

# Exécuter les tests
dotnet test
```

### 4. Créer une branche de fonctionnalité

```bash
git checkout -b feature/nom-de-votre-fonctionnalité
```

### 5. Choisir le projet de développement

Selon votre type de contribution, choisissez le projet approprié :

- **Interfaces principales/classes abstraites** → Modifier `SiliconLife.Core`
- **Implémentations partagées** → Modifier `SiliconLife.Common`
- **Spécifique à la version Default** → Modifier `SiliconLife.Default`
- **Spécifique à la version Fast** → Modifier `SiliconLife.Fast`
- **Moteur de stockage** → Modifier `SiliconLife.Speedy`
- **Outil de gestion du stockage** → Modifier `SiliconLife.Speedy.Manager`
- **Développement de plugins** → Modifier `SiliconLife.Core/Plugins`
- **Documentation multilingue** → Modifier le répertoire `docs/`

---

## Flux de travail de développement

### Style de code

- Suivre les conventions C#
- Noms de classes en PascalCase
- Paramètres de méthodes en camelCase
- Champs privés en `_camelCase`
- Toutes les API publiques doivent avoir une documentation XML

### Messages de commit

Suivre le format des **commits conventionnels** :

```
<type>(<portée>): <description>
```

**Types** :
- `feat` : Nouvelle fonctionnalité
- `fix` : Correction de bug
- `docs` : Modification de documentation
- `style` : Formatage de code
- `refactor` : Refactorisation de code
- `test` : Modification de tests
- `chore` : Modification de build/outils

**Exemples** :
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Effectuer des modifications

1. **Écrire le code**
   - Suivre les modèles existants
   - Ajouter des tests pour les nouvelles fonctionnalités
   - Mettre à jour la documentation

2. **Tester vos modifications**
   ```bash
   # Exécuter tous les tests
   dotnet test
   
   # Construire en mode release
   dotnet build --configuration Release
   ```

3. **Formater le code**
   ```bash
   dotnet format
   ```

4. **Committer les modifications**
   ```bash
   git add .
   git commit -m "feat(portée): description"
   ```

5. **Pousser vers votre fork**
   ```bash
   git push origin feature/nom-de-votre-fonctionnalité
   ```

6. **Créer une Pull Request**
   - Aller au dépôt original
   - Cliquer sur « Compare & pull request »
   - Remplir le modèle de PR
   - Soumettre

---

## Guide des Pull Requests

### Titre de PR

Utiliser le même format que les messages de commit :
```
feat(localization): add Korean language support
```

### Description de PR

Inclure :

1. **Quoi** - Que fait cette PR ?
2. **Pourquoi** - Pourquoi ce changement est-il nécessaire ?
3. **Comment** - Comment l'avez-vous implémenté ?
4. **Tests** - Comment a-t-il été testé ?

### Exemple de description de PR

```markdown
## Quoi
Ajout de la localisation coréenne pour tous les composants UI et la documentation.

## Pourquoi
Élargir l'accessibilité du projet aux utilisateurs coréens.

## Comment
- Création du fichier de localisation KoKR.cs
- Ajout de 500+ clés de traduction
- Mise à jour de toutes les vues pour utiliser la localisation
- Création de la documentation coréenne dans docs/ko-KR/

## Tests
- Vérification que tous les éléments UI affichent correctement le coréen
- Test de la fonctionnalité de changement de langue
- Revue des traductions avec un locuteur natif
```

---

## Types de contributions

### 1. Correction de bugs

**Processus** :
1. Vérifier les issues existantes
2. Créer une issue si elle n'existe pas
3. Corriger le bug
4. Ajouter des cas de test
5. Soumettre une PR

**Exigences** :
- Description claire du bug
- Étapes de reproduction
- Tests pour prévenir les régressions

### 2. Nouvelles fonctionnalités

**Processus** :
1. Discuter de la fonctionnalité dans Issues/Discussions
2. Obtenir l'approbation des mainteneurs
3. Implémenter la fonctionnalité
4. Ajouter des tests complets
5. Mettre à jour la documentation
6. Soumettre une PR

**Exigences** :
- Proposition de fonctionnalité approuvée
- Couverture de test complète
- Documentation mise à jour
- Compatibilité ascendante

### 3. Documentation

**Processus** :
1. Identifier les lacunes dans la documentation
2. Écrire/mettre à jour la documentation
3. Soumettre une PR

**Exigences** :
- Clair et concis
- Inclure des exemples
- Support multilingue si applicable

### 4. Refactorisation de code

**Processus** :
1. Proposer la refactorisation dans une Issue
2. Obtenir l'approbation
3. Refactoriser le code
4. S'assurer que tous les tests passent
5. Soumettre une PR

**Exigences** :
- Aucun changement de fonctionnalité
- Tous les tests passent
- Amélioration de la qualité du code
- Explication claire

---

## Guide des tests

### Tests unitaires

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Arrangement
    var service = new MyService();
    
    // Exécution
    var result = service.DoSomething();
    
    // Assertion
    Assert.IsTrue(result.Success);
}
```

### Tests d'intégration

Tester les flux de travail complets :
- Interaction IA
- Exécution d'outils
- Validation des permissions
- Opérations de stockage

### Tests manuels

Pour les modifications UI :
- Tester dans plusieurs navigateurs
- Vérifier le design responsive
- Vérifier l'accessibilité

---

## Guide de la documentation

### Commentaires de code

- Utiliser les commentaires XML pour toutes les API publiques
- Utiliser des commentaires en ligne pour la logique complexe
- Les commentaires de code doivent être en anglais

### Fichiers de documentation

- Placer dans `docs/{langue}/`
- Mettre à jour toutes les versions linguistiques
- Suivre la structure existante

### Documentation multilingue

Lors de l'ajout de documentation :
1. Créer d'abord la version anglaise
2. Traduire dans les autres langues
3. Maintenir le contenu synchronisé

---

## Processus de revue

### Ce que les mainteneurs vérifient

1. **Qualité du code**
   - Suit les conventions
   - Clair et lisible
   - Bien documenté

2. **Tests**
   - Couverture adéquate
   - Tous les tests passent
   - Couvre les cas limites

3. **Documentation**
   - Mise à jour
   - Explications claires
   - Multilingue

4. **Compatibilité**
   - Compatible avec les versions antérieures
   - Pas de changements cassants (sauf notification)
   - Suit la gestion sémantique de version

### Délai de revue

- Revue initiale : 1-3 jours
- Intégration des retours : selon les besoins
- Fusion : après approbation

---

## Questions fréquentes

### PR rejetée

**Raisons** :
- Ne suit pas les directives
- Tests insuffisants
- Changements cassants non notifiés
- Mauvaise qualité du code

**Solutions** :
- Résoudre les retours
- Mettre à jour la PR
- Soumettre à nouveau

### Conflits de fusion

**Solutions** :
```bash
# Mettre à jour votre branche
git fetch origin
git rebase origin/master

# Résoudre les conflits
# Éditer les fichiers en conflit
git add .
git rebase --continue

# Pousser avec force
git push --force-with-lease
```

---

## Obtenir de l'aide

### Ressources

- **Documentation** : [docs/](../)
- **Issues** : GitHub Issues
- **Discussions** : GitHub Discussions
- **Code de conduite** : CODE_OF_CONDUCT.md

### Contact

- Créer une Issue pour les bugs
- Lancer une Discussion pour les questions
- Mentionner les mainteneurs pour les questions urgentes

---

## Remerciements

Les contributeurs seront reconnus dans :
- La section contributeurs du README.md
- Les notes de version
- La documentation du projet

---

## Licence

En contribuant, vous acceptez que vos contributions soient sous licence Apache 2.0.

---

## Prochaines étapes

- 📚 Lire la [documentation](../)
- 🐛 Voir les [issues ouvertes](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Commencer une [discussion](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Forker et commencer à contribuer !

Merci de contribuer à SiliconLifeCollective ! 🎉
