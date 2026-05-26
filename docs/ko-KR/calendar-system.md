# 캘린더 시스템

> **버전: v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | **한국어** | [Čeština](../cs-CZ/calendar-system.md) | [Русский](../ru-RU/calendar-system.md)

## 개요

SiliconLifeCollective는 날짜 변환 및 역사적 정확성을 위해 32종의 캘린더 시스템을 지원합니다.

## 지원 캘린더 (32종)

### 주요 캘린더 (6종)

1. **그레고리력 (Gregorian)** - 국제 표준 캘린더, ID: `gregorian`
2. **음력 (Chinese Lunar)** - 중국 전통 캘린더, 윤달 계산 포함, ID: `lunar`
3. **이슬람력 (Islamic)** - 이슬람 히즈라력, ID: `islamic`
4. **히브리력 (Hebrew)** - 유대 캘린더, ID: `hebrew`
5. **페르시아력 (Persian)** - 이란 태양력, ID: `persian`
6. **인도력 (Indian)** - 인도 국가력, ID: `indian`

### 중국 역사력 (2종)

7. **중국 역사력 (Chinese Historical)** - 간지기년 및 제왕 연호 지원, ID: `chinese_historical`
   - **간지기년**: 60년 주기 (천간 + 지지)
   - **제왕 연호**: 중국 역사 각 왕조의 연호 지원 (강희, 건륭, 정관 등)
   - **동적 데이터베이스**: 내장된 완전한 중국 역사 왕조 및 연호 데이터베이스
8. **간지력 (Sexagenary)** - 60년 간지 주기, ID: `sexagenary`

### 동아시아 캘린더 (6종)

9. **일본력 (Japanese)** - 일본 연호(원호)력, ID: `japanese`
10. **베트남력 (Vietnamese)** - 베트남 음력 (고양이띠 변형), ID: `vietnamese`
11. **티베트력 (Tibetan)** - 티베트력 시스템, ID: `tibetan`
12. **몽골력 (Mongolian)** - 몽골력, ID: `mongolian`
13. **타이족력 (Dai)** - 타이족력, 완전한 음력 계산 포함, ID: `dai`
14. **더홍 타이족력 (Dehong Dai)** - 더홍 타이족력 변형, ID: `dehong_dai`

### 역사 캘린더 (6종)

15. **마야력 (Mayan)** - 마야 장기력, ID: `mayan`
16. **로마력 (Roman)** - 고대 로마력, ID: `roman`
17. **율리우스력 (Julian)** - 율리우스력, ID: `julian`
18. **프랑스 공화력 (French Republican)** - 프랑스 혁명력, ID: `french_republican`
19. **콥트력 (Coptic)** - 콥트 정교회력, ID: `coptic`
20. **에티오피아력 (Ethiopian)** - 에티오피아력, ID: `ethiopian`

### 지역 캘린더 (6종)

21. **불력 (Buddhist)** - 불기(BE), 연도 + 543, ID: `buddhist`
22. **사카력 (Saka)** - 사카 기원(인도네시아), ID: `saka`
23. **비크람 삼바트력 (Vikram Samvat)** - 힌두력, ID: `vikram_samvat`
24. **자바력 (Javanese)** - 자바 이슬람력, ID: `javanese`
25. **출라사카랏력 (Chula Sakarat)** - 동남아시아 불력, 연도 - 638, ID: `chula_sakarat`
26. **크메르력 (Khmer)** - 크메르력, ID: `khmer`

### 현대 캘린더 (3종)

27. **중화민국력 (ROC)** - 민국력, 연도 - 1911, ID: `roc`
28. **주체력 (Juche)** - 조선력, 연도 - 1911, ID: `juche`
29. **조로아스터력 (Zoroastrian)** - 조로아스터교력, ID: `zoroastrian`

### 민족 캘린더 (3종)

30. **이족력 (Yi)** - 이족력 시스템, ID: `yi`
31. **체로키력 (Cherokee)** - 체로키력, ID: `cherokee`
32. **이누이트력 (Inuit)** - 이누이트력, ID: `inuit`

---

## 캘린더 툴 사용

### 기본 변환

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**응답**:
```json
{
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### 다중 캘린더 쿼리

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**응답**: 32종 캘린더 시스템 전체의 날짜를 반환합니다.

---

## 캘린더 API

### CalendarBase 인터페이스

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

### 예시: 커스텀 캘린더

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";

    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // 转换逻辑
        return new CalendarDate(year, month, day);
    }

    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 反向转换
        return new GregorianDate(year, month, day);
    }
}
```

---

## 특수 기능

### 중국 역사력 상세 (신규)

중국 역사력은 본 시스템의 주요 하이라이트로, 두 가지 핵심 기능을 지원합니다:

#### 1. 간지기년 시스템

천간과 지지의 조합으로 이루어진 60년 주기를 채택합니다:

```
天干（10）：甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支（12）：子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**예시**:
- 2026년 = 병오년
- 2025년 = 을사년 (뱀의 해)
- 2024년 = 갑진년 (용의 해)

**사용 예시**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**응답**:
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "马"
}
```

#### 2. 제왕 연호 시스템

내장된 완전한 중국 역사 왕조 및 제왕 연호 데이터베이스:

**지원 왕조** (일부):
- 청나라: 강희, 옹정, 건륭, 가경, 도광, 함풍, 동치, 광서, 선통
- 명나라: 홍무, 영락, 가정, 만력, 숭정 등
- 당나라: 정관, 개원, 천보 등
- 한나라: 건원, 원광, 원삭 등
- 기타 왕조...

**사용 예시**:
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**응답**:
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### 윤달 처리

윤달이 있는 캘린더:
- 음력
- 히브리력
- 불력
- 베트남력

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## 캘린더 정확도

### 천문 계산

- 실제 천문 데이터 기반
- 역사적 날짜 지원
- 역법 개혁 처리

### 알려진 제한 사항

- 일부 고대 날짜는 근사치입니다
- 역법 개혁은 지역에 따라 다릅니다
- 윤초 처리는 포함하지 않습니다

---

## 사용 사례

### 역사 연구

역사적 날짜를 현대 캘린더로 변환:

```
질문: "프랑스 대혁명은 언제였나요?"
답변: "1789년 7월 14일 (그레고리력)"
       "원년 테르미도르 26일 (프랑스 공화력)"
```

### 문화 응용

전통 명절 지원:

```
2026년 춘절:
- 그레고리력: 2026년 2월 17일
- 음력: 정월 초하루
```

### 다문화 일정 조율

여러 캘린더를 존중하는 이벤트 일정 수립:

```
회의: 2026-04-20
- 이슬람 금요일 기도 시간 피하기
- 유대교 안식일 존중
- 중국 휴일 고려
```

---

## 모범 사례

### 1. 항상 캘린더를 명시할 것

캘린더 시스템을 추정하지 마세요:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // 명시적으로 지정!
}
```

### 2. 유효하지 않은 날짜 처리

일부 날짜는 특정 캘린더에 존재하지 않습니다:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. 시간대 고려

날짜 변환은 시간대에 따라 달라질 수 있습니다:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 확인
- 🔧 [툴 참조](tools-reference.md) 확인
- 🚀 [시작 가이드](getting-started.md)로 시작
