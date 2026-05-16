# Système de calendrier

> **Version : v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | **Français** | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md)

## Aperçu

SiliconLifeCollective prend en charge 32 systèmes de calendrier différents pour la conversion de dates et la précision historique.

## Calendriers pris en charge (32)

### Calendriers principaux (6)

1. **Calendrier grégorien (Gregorian)** - Calendrier standard international, ID : `gregorian`
2. **Calendrier lunaire chinois (Chinese Lunar)** - Calendrier chinois traditionnel avec calcul des mois intercalaires, ID : `lunar`
3. **Calendrier islamique (Islamic)** - Calendrier de l'Hégire islamique, ID : `islamic`
4. **Calendrier hébraïque (Hebrew)** - Calendrier juif, ID : `hebrew`
5. **Calendrier persan (Persian)** - Calendrier solaire iranien, ID : `persian`
6. **Calendrier indien (Indian)** - Calendrier national indien, ID : `indian`

### Calendriers historiques chinois (2)

7. **Calendrier historique chinois (Chinese Historical)** - Prend en charge le cycle Ganzhi et les ères impériales, ID : `chinese_historical`
   - **Cycle Ganzhi** : Cycle de 60 ans (Tiges célestes + Branches terrestres)
   - **Ères impériales** : Prend en charge les ères de toutes les dynasties chinoises (Kangxi, Qianlong, Zhenguan, etc.)
   - **Base de données dynamique** : Base de données complète intégrée des dynasties et ères chinoises
8. **Calendrier sexagésimal (Sexagenary)** - Cycle Ganzhi de 60 ans, ID : `sexagenary`

### Calendriers est-asiatiques (6)

9. **Calendrier japonais (Japanese)** - Calendrier des ères Nengo japonaises, ID : `japanese`
10. **Calendrier vietnamien (Vietnamese)** - Calendrier lunaire vietnamien (variante zodiaque du Chat), ID : `vietnamese`
11. **Calendrier tibétain (Tibetan)** - Système de calendrier tibétain, ID : `tibetan`
12. **Calendrier mongol (Mongolian)** - Calendrier mongol, ID : `mongolian`
13. **Calendrier Dai (Dai)** - Calendrier Dai avec calcul lunaire complet, ID : `dai`
14. **Calendrier Dai Dehong (Dehong Dai)** - Variante Dai Dehong, ID : `dehong_dai`

### Calendriers historiques (6)

15. **Calendrier maya (Mayan)** - Calendrier long maya, ID : `mayan`
16. **Calendrier romain (Roman)** - Calendrier romain ancien, ID : `roman`
17. **Calendrier julien (Julian)** - Calendrier julien, ID : `julian`
18. **Calendrier républicain français (French Republican)** - Calendrier de la Révolution française, ID : `french_republican`
19. **Calendrier copte (Coptic)** - Calendrier copte orthodoxe, ID : `coptic`
20. **Calendrier éthiopien (Ethiopian)** - Calendrier éthiopien, ID : `ethiopian`

### Calendriers régionaux (6)

21. **Calendrier bouddhiste (Buddhist)** - Ère bouddhiste (BE), année + 543, ID : `buddhist`
22. **Calendrier Saka (Saka)** - Ère Saka (Indonésie), ID : `saka`
23. **Calendrier Vikram Samvat (Vikram Samvat)** - Calendrier hindou, ID : `vikram_samvat`
24. **Calendrier javanais (Javanese)** - Calendrier islamique javanais, ID : `javanese`
25. **Calendrier Chula Sakarat (Chula Sakarat)** - Calendrier bouddhiste d'Asie du Sud-Est, année - 638, ID : `chula_sakarat`
26. **Calendrier khmer (Khmer)** - Calendrier khmer, ID : `khmer`

### Calendriers modernes (3)

27. **Calendrier ROC (ROC)** - Calendrier Minguo, année - 1911, ID : `roc`
28. **Calendrier Juche (Juche)** - Calendrier nord-coréen, année - 1911, ID : `juche`
29. **Calendrier zoroastrien (Zoroastrian)** - Calendrier zoroastrien, ID : `zoroastrian`

### Calendriers ethniques (3)

30. **Calendrier Yi (Yi)** - Système de calendrier Yi, ID : `yi`
31. **Calendrier cherokee (Cherokee)** - Calendrier cherokee, ID : `cherokee`
32. **Calendrier inuit (Inuit)** - Calendrier inuit, ID : `inuit`

---

## Utiliser l'outil de calendrier

### Conversion de base

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Réponse** :
```json
{
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Requête multi-calendrier

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Réponse** : Retourne la date dans les 32 systèmes de calendrier.

---

## API de calendrier

### Interface CalendarBase

```csharp
public abstract class CalendarBase
{
    public abstract string Name { get; }
    
    public abstract CalendarDate ConvertFromGregorian(GregorianDate date);
    
    public abstract GregorianDate ConvertToGregorian(CalendarDate date);
    
    public virtual bool IsLeapYear(int year) => false;
    
    public virtual int GetDaysInMonth(int year, int month) => 30;
}
```

### Exemple : Calendrier personnalisé

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Logique de conversion
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversion inverse
        return new GregorianDate(year, month, day);
    }
}
```

---

## Fonctionnalités spéciales

### Calendrier historique chinois en détail (Nouveau)

Le calendrier historique chinois est une fonctionnalité phare du système avec deux fonctions principales :

#### 1. Système de cycle annuel Ganzhi

Utilise un cycle de 60 ans, combinant les Tiges célestes et les Branches terrestres :

```
Tiges célestes (10) : 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
Branches terrestres (12) : 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**Exemples** :
- 2026 = 丙午年
- 2025 = 乙巳年 (Année du Serpent)
- 2024 = 甲辰年 (Année du Dragon)

**Exemple d'utilisation** :
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Réponse** :
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "马"
}
```

#### 2. Système d'ères impériales

Base de données complète intégrée des dynasties et ères impériales chinoises :

**Dynasties prises en charge** (extrait) :
- Dynastie Qing : Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Dynastie Ming : Hongwu, Yongle, Jiajing, Wanli, Chongzhen, etc.
- Dynastie Tang : Zhenguan, Kaiyuan, Tianbao, etc.
- Dynastie Han : Jianyuan, Yuanguang, Yuanshuo, etc.
- Autres dynasties...

**Exemple d'utilisation** :
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Réponse** :
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### Traitement des mois intercalaires

Calendriers avec mois intercalaires :
- Calendrier lunaire chinois
- Calendrier hébraïque
- Calendrier bouddhiste
- Calendrier vietnamien

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## Précision des calendriers

### Calculs astronomiques

- Basés sur des données astronomiques réelles
- Prend en charge les dates historiques
- Traite les réformes calendaires

### Limitations connues

- Certaines dates antiques sont approximatives
- Les réformes calendaires varient selon les régions
- N'inclut pas le traitement des secondes intercalaires

---

## Cas d'utilisation

### Recherche historique

Convertir des dates historiques en calendriers modernes :

```
Question : "Quand a eu lieu la Révolution française ?"
Réponse : "14 juillet 1789 (Grégorien)"
         "26 Thermidor I (Républicain français)"
```

### Applications culturelles

Prise en charge des fêtes traditionnelles :

```
Nouvel An chinois 2026 :
- Grégorien : 17 février 2026
- Calendrier lunaire : 1er jour du 1er mois
```

### Planification multiculturelle

Planifier des événements en tenant compte de plusieurs calendriers :

```
Réunion : 2026-04-20
- Éviter la prière du vendredi islamique
- Respecter le Shabbat juif
- Prendre en compte les jours fériés chinois
```

---

## Bonnes pratiques

### 1. Toujours spécifier le calendrier

Ne jamais supposer le système de calendrier :

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Spécifier explicitement !
}
```

### 2. Gérer les dates invalides

Certaines dates n'existent pas dans certains calendriers :

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Prendre en compte les fuseaux horaires

La conversion de dates peut varier selon le fuseau horaire :

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 🔧 Voir la [référence des outils](tools-reference.md)
- 🚀 Commencer le [guide de démarrage rapide](getting-started.md)
