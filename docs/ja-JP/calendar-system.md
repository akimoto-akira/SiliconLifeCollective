# カレンダーシステム

[English](../en/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | **日本語** | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md)

## 概要

SiliconLifeCollective は、日付変換と歴史的正確性のために 32 種類の異なるカレンダーシステムをサポートしています。

## サポートされるカレンダー（32種類）

### 主要カレンダー（6種類）

1. **グレゴリオ暦 (Gregorian)** - 国際標準カレンダー、ID: `gregorian`
2. **旧暦 (Chinese Lunar)** - 中国伝統カレンダー、閏月計算付き、ID: `lunar`
3. **イスラム暦 (Islamic)** - イスラムヒジュラ暦、ID: `islamic`
4. **ヘブライ暦 (Hebrew)** - ユダヤカレンダー、ID: `hebrew`
5. **ペルシャ暦 (Persian)** - イラン太陽暦、ID: `persian`
6. **インド暦 (Indian)** - インド国家カレンダー、ID: `indian`

### 中国歴史暦（2種類）

7. **中国歴史暦 (Chinese Historical)** - 干支紀年と帝王年号をサポート、ID: `chinese_historical`
   - **干支紀年**：60年周期（天干 + 地支）
   - **帝王年号**：中国歴史の各朝代の年号をサポート（康熙、乾隆、貞観など）
   - **動的データベース**：中国歴史の王朝と年号の完全なデータベースを内蔵
8. **干支暦 (Sexagenary)** - 60年干支周期、ID: `sexagenary`

### 東アジアカレンダー（6種類）

9. **日本暦 (Japanese)** - 日本元号（Nengo）暦、ID: `japanese`
10. **ベトナム暦 (Vietnamese)** - ベトナム旧暦（猫の干支バリアント）、ID: `vietnamese`
11. **チベット暦 (Tibetan)** - チベットカレンダーシステム、ID: `tibetan`
12. **モンゴル暦 (Mongolian)** - モンゴル暦、ID: `mongolian`
13. **タイ暦 (Dai)** - タイ暦、完全な旧暦計算付き、ID: `dai`
14. **徳宏タイ暦 (Dehong Dai)** - 徳宏タイ暦バリアント、ID: `dehong_dai`

### 歴史カレンダー（6種類）

15. **マヤ暦 (Mayan)** - マヤ長期暦、ID: `mayan`
16. **ローマ暦 (Roman)** - 古代ローマカレンダー、ID: `roman`
17. **ユリウス暦 (Julian)** - ユリウス暦、ID: `julian`
18. **フランス共和暦 (French Republican)** - フランス革命カレンダー、ID: `french_republican`
19. **コプト暦 (Coptic)** - コプト正教会カレンダー、ID: `coptic`
20. **エチオピア暦 (Ethiopian)** - エチオピア暦、ID: `ethiopian`

### 地域カレンダー（6種類）

21. **仏暦 (Buddhist)** - 仏暦紀元（BE）、年 + 543、ID: `buddhist`
22. **サカ暦 (Saka)** - サカ紀元（インドネシア）、ID: `saka`
23. **ヴィクラム暦 (Vikram Samvat)** - ヒンドゥーカレンダー、ID: `vikram_samvat`
24. **ジャワ暦 (Javanese)** - ジャワイスラム暦、ID: `javanese`
25. **チュラサカラート暦 (Chula Sakarat)** - 東南アジア仏暦、年 - 638、ID: `chula_sakarat`
26. **クメール暦 (Khmer)** - クメール暦、ID: `khmer`

### 現代カレンダー（3種類）

27. **中華民国暦 (ROC)** - 民国暦、年 - 1911、ID: `roc`
28. **主体暦 (Juche)** - 北朝鮮カレンダー、年 - 1911、ID: `juche`
29. **ゾロアスター暦 (Zoroastrian)** - ゾロアスター教暦、ID: `zoroastrian`

### 民族カレンダー（3種類）

30. **イー族暦 (Yi)** - イー族カレンダーシステム、ID: `yi`
31. **チェロキー暦 (Cherokee)** - チェロキー暦、ID: `cherokee`
32. **イヌイット暦 (Inuit)** - イヌイット暦、ID: `inuit`

---

## カレンダーツールの使用

### 基本変換

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**レスポンス**：
```json
{
  "result": "旧暦丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### マルチカークエリ

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**レスポンス**：32種類すべてのカレンダーシステムでの日付を返します。

---

## カレンダー API

### CalendarBase インターフェース

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

### 例：カスタムカレンダー

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // 変換ロジック
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 逆変換
        return new GregorianDate(year, month, day);
    }
}
```

---

## 特別機能

### 中国歴史暦の詳細（新規）

中国歴史暦は本システムの大きな特徴で、2つのコア機能をサポート：

#### 1. 干支紀年システム

60年周期を採用。天干と地支の組み合わせで構成：

```
天干（10）：甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支（12）：子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**例**：
- 2026年 = 丙午年
- 2025年 = 乙巳年（蛇年）
- 2024年 = 甲辰年（龍年）

**使用例**：
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**レスポンス**：
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "馬"
}
```

#### 2. 帝王年号システム

中国歴史の王朝と帝王年号の完全なデータベースを内蔵：

**サポートされる王朝**（一部）：
- 清：康熙、雍正、乾隆、嘉慶、道光、咸豊、同治、光緒、宣統
- 明：洪武、永楽、嘉靖、万暦、崇禎など
- 唐：貞観、開元、天宝など
- 漢：建元、元光、元朔など
- その他の王朝...

**使用例**：
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**レスポンス**：
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### 閏月処理

閏月のあるカレンダー：
- 旧暦
- ヘブライ暦
- 仏暦
- ベトナム暦

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "閏四月"
}
```

---

## カレンダーの正確性

### 天文計算

- 実際の天文データに基づく
- 歴史日付をサポート
- カレンダー改革を処理

### 既知の制限

- 一部の古代日付は概算
- カレンダー改革は地域によって異なる
- 閏秒処理は含まれない

---

## ユースケース

### 歴史研究

歴史日付を現代カレンダーに変換：

```
質問：「フランス大革命はいつですか？」
回答：「1789年7月14日（グレゴリオ暦）」
       「元年熱月26日（フランス共和暦）」
```

### 文化アプリケーション

伝統祭日をサポート：

```
2026年春节：
- グレゴリオ暦：2026年2月17日
- 旧暦：正月初一
```

### マルチカルチャースケジューリング

複数のカレンダーを尊重したイベントスケジュール：

```
会議：2026-04-20
- イスラム金曜礼拝を回避
- ユダヤ安息日を尊重
- 中国の祝日を考慮
```

---

## ベストプラクティス

### 1. 常にカレンダーを指定

カレンダーシステムを推測しないでください：

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // 明示的に指定！
}
```

### 2. 無効な日付を処理

一部の日付は特定のカレンダーに存在しない：

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. タイムゾーンを考慮

日付変換はタイムゾーンによって異なる：

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)をチェック
- 🔧 [ツールリファレンス](tools-reference.md)を見る
- 🚀 [クイックスタートガイド](getting-started.md)で始める
