# 캘린더 시스템

[English](calendar-system.md) | [简体中文](docs/zh-CN/calendar-system.md) | [繁體中文](docs/zh-HK/calendar-system.md) | [Español](docs/es-ES/calendar-system.md) | [日本語](docs/ja-JP/calendar-system.md) | [한국어](docs/ko-KR/calendar-system.md) | [Čeština](docs/cs-CZ/calendar-system.md)

## 개요

SiliconLifeCollective는 날짜 변환 및 역사적 정확성을 위해 32개의 다른 캘린더 시스템을 지원합니다.

## 지원되는 캘린더

### 주요 캘린더

1. **그레고리력** - 국제 표준 캘린더
2. **중국 음력** - 전통 중국 캘린더
3. **이슬람력** - 이슬람 히지라 캘린더
4. **히브리력** - 유대 캘린더
5. **페르시아력** - 이란 캘린더
6. **인도력** - 인도 국가 캘린더

### 동아시아 캘린더

7. **중국 역사력** - 역사 중국 연호
8. **일본력** - 일본 황실 연호
9. **한국력** - 한국 캘린더 시스템
10. **베트남력** - 베트남 캘린더
11. **티베트력** - 티베트 캘린더
12. **몽골력** - 몽골 캘린더

### 역사적 캘린더

13. **마야력** - 마야 장기曆
14. **로마력** - 고대 로마 캘린더
15. **율리우스력** - 율리우스 캘린더
16. **프랑스 공화국력** - 프랑스 혁명 캘린더
17. **콥트력** - 콥트 정교회 캘린더
18. **에티오피아력** - 에티오피아 캘린더

### 지역 캘린더

19. **불교력** - 불교 시대
20. **사카력** - 사카 시대 (인도)
21. **비크람 삼바트** - 힌두 캘린더
22. **자와력** - 자와 캘린더
23. **발리력** - 발리 캘린더

### 현대 캘린더

24. **중화민국(대만)** - 민국력
25. **주체력** - 북한 캘린더
26. **출라삭krat** - 동남아시아 불교력

### 민족 캘린더 (중국)

27. **다이력** - 다이 캘린더
28. **더홍 다이력** - 더홍 다이 캘린더
29. **이족력** - 이족 캘린더
30. **체로키력** - 체로키 캘린더
31. **이누이트력** - 이누이트 캘린더
32. **육십갑자** - 60년 주기

---

## 캘린더 도구 사용법

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

### 다중 캘린더 조회

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**응답**: 32개 캘린더 시스템 모두의 날짜를 반환합니다.

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

### 예시: 사용자 정의 캘린더

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // 변환 로직
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 역변환
        return new GregorianDate(year, month, day);
    }
}
```

---

## 특수 기능

### 중국 역사 연호

동적 연호 데이터베이스 지원:

```json
{
  "era": "康熙",
  "year": 60,
  "month": 3,
  "day": 15
}
```

변환 결과: `1721-04-12` (그레고리력)

### 육십갑자

천간과 지지의 60년 주기:

```
天干: 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支: 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

예시: 2026 = 丙午年

### 윤달 처리

윤달이 있는 캘린더:
- 중국 음력
- 히브리력
- 불교력
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

### 천문학적 계산

- 실제 천문학 데이터 기반
- 역사적 날짜 지원
- 캘린더 개혁 처리

### 알려진 제한사항

- 일부 고대 날짜는 근사값
- 캘린더 개혁은 지역별로 다름
- 윤초 처리 미포함

---

## 사용 사례

### 역사 연구

역사적 날짜를 현대 캘린더로 변환:

```
Q: "프랑스 혁명은 언제였나요?"
A: "1789년 7월 14일 (그레고리력)"
   "공화국 1년 Messidor 26일 (프랑스 공화국력)"
```

### 문화 응용

전통 축제 지원:

```
2026년 중국 설:
- 그레고리력: 2026년 2월 17일
- 중국 음력: 正月初一
```

### 다문화 스케줄링

여러 캘린더를 고려한 이벤트 스케줄:

```
회의: 2026-04-20
- 이슬람 금요일 기도 피함
- 유대 안식일 존중
- 중국 공휴일 고려
```

---

## 모범 사례

### 1. 항상 캘린더 명시

캘린더 시스템을 가정하지 마십시오:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // 명시적!
}
```

### 2. 잘못된 날짜 처리

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

## 문제 해결

### 변환된 날짜가 잘못됨

**확인**:
- 올바른 캘린더 지정
- 해당 캘린더의 유효한 날짜
- 역사적 정확도 고려

### 윤달 문제

**확인**:
- 캘린더가 윤달 지원
- 해당 연도에 실제로 윤달 존재
- 윤달 계산 정확성

### 역사 연호를 찾을 수 없음

**확인**:
- 연호 이름 철자
- 연호 날짜 범위
- 데이터베이스에 해당 연호 포함 여부

---

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 확인
- 🔧 [도구 레퍼런스](tools-reference.md) 참조
- 🚀 [빠른 시작 가이드](getting-started.md)로 시작
