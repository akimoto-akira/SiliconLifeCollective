# カレンダーシステム

[English](calendar-system.md) | [简体中文](docs/zh-CN/calendar-system.md) | [繁體中文](docs/zh-HK/calendar-system.md) | [Español](docs/es-ES/calendar-system.md) | [日本語](docs/ja-JP/calendar-system.md) | [한국어](docs/ko-KR/calendar-system.md) | [Čeština](docs/cs-CZ/calendar-system.md)

## 概要

SiliconLifeCollective は日付変換と歴史的精度のために 32 種類のカレンダーシステムをサポートしています。

## サポートされているカレンダー

### 主要カレンダー

1. **グレゴリオ暦** - 国際標準カレンダー
2. **中国旧暦** - 伝統的中国カレンダー
3. **イスラム暦** - イスラム Hijri 暦
4. **ヘブライ暦** - ユダヤ暦
5. **ペルシャ暦** - イラン暦
6. **インド暦** - インド国立カレンダー

### 東アジアカレンダー

7. **中国歴史暦** - 歴史的中國元号
8. **日本暦** - 日本元号
9. **韓国暦** - 韓国カレンダーシステム
10. **ベトナム暦** - ベトナムカレンダー
11. **チベット暦** - チベットカレンダー
12. **モンゴル暦** - モンゴルカレンダー

### 歴史カレンダー

13. **マヤ暦** - マヤ長期暦
14. **ローマ暦** - 古代ローマカレンダー
15. **ユリウス暦** - ユリウス暦
16. **フランス革命暦** - フランス革命カレンダー
17. **コプト暦** - コプト正教会カレンダー
18. **エチオピア暦** - エチオピアカレンダー

### 地域カレンダー

19. **仏暦** - 仏教紀元
20. **サカ暦** - サカ紀元（インド）
21. **ヴィクラム・サムヴァト暦** - ヒンドゥー暦
22. **ジャワ暦** - ジャワカレンダー
23. **バリ暦** - バリカレンダー

### 現代カレンダー

24. **中華民國（台湾）暦** - 民国暦
25. **主体暦** - 北朝鮮カレンダー
26. **チュラサッカ暦** - 東南アジア仏教

### 民族カレンダー（中国）

27. **タイ暦** - タイカレンダー
28. **徳宏タイ暦** - 徳宏タイカレンダー
29. **イ暦** - イ族カレンダー
30. **チェロキー暦** - チェロキーカレンダー
31. **イヌイット暦** - イヌイットカレンダー
32. **干支暦** - 60年周期

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
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### 複数カレンダークエリ

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**レスポンス**：32 種類のカレンダーシステムでの日付を返します。

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

### 中国歴史元号

動的元号データベースをサポート：

```json
{
  "era": "康熙",
  "year": 60,
  "month": 3,
  "day": 15
}
```

変換先：`1721-04-12`（グレゴリオ暦）

### 干支

天干地支付き 60 年周期：

```
天干：甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支：子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

例：2026 = 丙午年

### 閏月処理

閏月付きカレンダー：
- 中国旧暦
- ヘブライ暦
- 仏暦
- ベトナム暦

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## カレンダー精度

### 天文計算

- 実際の天文データに基づく
- 歴史的日付をサポート
- カレンダー改革を処理

### 既知の制限

- 一部の古代日付は概算
- カレンダー改革は地域によって異なる
- うるう秒処理は含まれていない

---

## ユースケース

### 歴史研究

歴史的日付を現代カレンダーに変換：

```
Q: "フランス革命はいつ？"
A: "1789年7月14日（グレゴリオ暦）"
   "元年メッシドール26日（フランス革命暦）"
```

### 文化アプリケーション

伝統的な祭日をサポート：

```
2026年春節：
- グレゴリオ暦：2026年2月17日
- 中国旧暦：正月初一
```

### マルチカルチャースケジューリング

複数のカレンダーを尊重してイベントをスケジュール：

```
会議：2026-04-20
- イスラム教の金曜礼拝を回避
- ユダヤ教の安息日を尊重
- 中国の祝日を考慮
```

---

## ベストプラクティス

### 1. 常にカレンダーを指定

カレンダーシステムを推定しないでください：

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // 明示的！
}
```

### 2. 無効な日付を処理

一部の日付は特定のカレンダーに存在しません：

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. タイムゾーンを考慮

日付変換はタイムゾーンによって異なる場合があります：

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## トラブルシューティング

### 変換が間違った日付を返す

**確認**：
- 正しいカレンダーが指定されている
- 日付がそのカレンダーで有効
- 歴史的精度に注意

### 閏月の問題

**確認**：
- カレンダーが閏月をサポート
- 年に実際に閏月がある
- 閏月計算が正しい

### 歴史元号が見つからない

**確認**：
- 元号名のスペル
- 元号の日付範囲
- データベースにその元号が含まれている

---

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認
- 🔧 [ツールリファレンス](tools-reference.md)を参照
- 🚀 [クイックスタートガイド](getting-started.md)で開始
