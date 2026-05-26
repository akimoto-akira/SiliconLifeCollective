# Système de Calendrier

> **Version : v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Русский](../ru-RU/calendar-system.md)

## Aperçu

SiliconLifeCollective prend en charge 32 systèmes calendaires différents pour la conversion de dates et la précision historique.

## Calendriers pris en charge (32)

### Calendriers principaux (6)

1. **Grégorien (Gregorian)** - Calendrier international standard, ID : `gregorian`
2. **Lunaire chinois (Chinese Lunar)** - Calendrier traditionnel chinois, avec calcul des mois intercalaires, ID : `lunar`
3. **Islamique (Islamic)** - Calendrier hégirien islamique, ID : `islamic`
4. **Hébraïque (Hebrew)** - Calendrier juif, ID : `hebrew`
5. **Persan (Persian)** - Calendrier solaire iranien, ID : `persian`
6. **Indien (Indian)** - Calendrier national indien, ID : `indian`

### Calendriers historiques chinois (2)

7. **Calendrier historique chinois (Chinese Historical)** - Prend en charge la numérotation par cycle sexagésimal et les ères impériales, ID : `chinese_historical`
   - **Cycle sexagésimal** : Cycle de 60 ans (Tiges célestes + Branches terrestres)
   - **Ères impériales** : Prend en charge les ères de toutes les dynasties chinoises (Kangxi, Qianlong, Zhenguan, etc.)
   - **Base de données dynamique** : Base de données intégrée complète des dynasties et ères de l'histoire chinoise
8. **Calendrier sexagésimal (Sexagenary)** - Cycle sexagésimal de 60 ans, ID : `sexagenary`

### Calendriers est-asiatiques (6)

9. **Japonais (Japanese)** - Calendrier des ères japonaises (Nengo), ID : `japanese`
10. **Vietnamien (Vietnamese)** - Calendrier lunaire vietnamien (variante du zodiac avec le Chat), ID : `vietnamese`
11. **Tibétain (Tibetan)** - Système calendrier tibétain, ID : `tibetan`
12. **Mongol (Mongolian)** - Calendrier mongol, ID : `mongolian`
13. **Dai (Dai)** - Calendrier Dai, avec calcul lunaire complet, ID : `dai`
14. **Dai Dehong (Dehong Dai)** - Variante du calendrier Dai de Dehong, ID : `dehong_dai`

### Calendriers historiques (6)

15. **Maya (Mayan)** - Compte long maya, ID : `mayan`
16. **Romain (Roman)** - Calendrier romain antique, ID : `roman`
17. **Julien (Julian)** - Calendrier julien, ID : `julian`
18. **Républicain français (French Republican)** - Calendrier révolutionnaire français, ID : `french_republican`
19. **Copte (Coptic)** - Calendrier copte orthodoxe, ID : `coptic`
20. **Éthiopien (Ethiopian)** - Calendrier éthiopien, ID : `ethiopian`

### Calendriers régionaux (6)

21. **Bouddhiste (Buddhist)** - Ère bouddhiste (BE), année + 543, ID : `buddhist`
22. **Saka (Saka)** - Ère Saka (Indonésie), ID : `saka`
23. **Vikram Samvat (Vikram Samvat)** - Calendrier hindou, ID : `vikram_samvat`
24. **Javanais (Javanese)** - Calendrier islamique javanais, ID : `javanese`
25. **Chula Sakarat (Chula Sakarat)** - Calendrier bouddhiste d'Asie du Sud-Est, année - 638, ID : `chula_sakarat`
26. **Khmer (Khmer)** - Calendrier khmer, ID : `khmer`

### Calendriers modernes (3)

27. **Républicain de Chine (ROC)** - Calendrier de la République de Chine, année - 1911, ID : `roc`
28. **Juche (Juche)** - Calendrier nord-coréen, année - 1911, ID : `juche`
29. **Zoroastrien (Zoroastrian)** - Calendrier zoroastrien, ID : `zoroastrian`

### Calendriers ethniques (3)

30. **Yi (Yi)** - Système calendrier Yi, ID : `yi`
31. **Cherokee (Cherokee)** - Calendrier cherokee, ID : `cherokee`
32. **Inuit (Inuit)** - Calendrier inuit, ID : `inuit`

---

## Utilisation de l'Outil Calendrier

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

**Réponse** : Retourne la date dans les 32 systèmes calendaires.

---

## API Calendrier

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

### Détails du calendrier historique chinois (nouveau)

Le calendrier historique chinois est l'une des fonctionnalités majeures de ce système, prenant en charge deux fonctions principales :

#### 1. Système de numérotation par cycle sexagésimal

Utilise un cycle de 60 ans, composé de la combinaison des Tiges célestes et des Branches terrestres :

```
Tiges célestes (10) : 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
Branches terrestres (12) : 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**Exemples** :
- 2026 = 丙午年
- 2025 = 乙巳年 (année du Serpent)
- 2024 = 甲辰年 (année du Dragon)

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

#### 2. Système des ères impériales

Base de données intégrée complète des dynasties et ères impériales de l'histoire chinoise :

**Dynasties prises en charge** (partiel) :
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
- Lunaire chinois
- Hébraïque
- Bouddhiste
- Vietnamien

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
- Prise en charge des dates historiques
- Traitement des réformes calendaires

### Limites connues

- Certaines dates anciennes sont approximatives
- Les réformes calendaires varient selon les régions
- Ne prend pas en compte les secondes intercalaires

---

## Cas d'utilisation

### Recherche historique

Convertir des dates historiques en calendrier moderne :

```
Question : "Quand a été la Révolution française ?"
Réponse : "14 juillet 1789 (grégorien)"
         "26 thermidor an I (républicain français)"
```

### Applications culturelles

Prise en charge des fêtes traditionnelles :

```
Nouvel An chinois 2026 :
- Grégorien : 17 février 2026
- Lunaire : Premier jour du premier mois
```

### Planification multiculturelle

Planifier des événements respectant plusieurs calendriers :

```
Réunion : 2026-04-20
- Éviter la prière du vendredi islamique
- Respecter le shabbat juif
- Prendre en compte les jours fériés chinois
```

---

## Bonnes pratiques

### 1. Toujours spécifier le calendrier

Ne jamais supposer le système calendaire :

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Spécifiez explicitement !
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

Les conversions de dates peuvent varier selon les fuseaux horaires :

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 🔧 Consulter la [référence des outils](tools-reference.md)
- 🚀 Commencer avec le [guide de démarrage rapide](getting-started.md)
