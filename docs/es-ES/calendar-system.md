# Sistema de Calendario

> **Versión: v0.1.0-alpha**

[English](../en/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | **Español** | [Deutsch](../de-DE/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md)

## Resumen

SiliconLifeCollective soporta 32 sistemas de calendario diferentes para conversión de fechas y precisión histórica.

## Calendarios Soportados (32)

### Calendarios Principales (6)

1. **Gregoriano (Gregorian)** - Calendario estándar internacional, ID: `gregorian`
2. **Lunar Chino (Chinese Lunar)** - Calendario tradicional chino, con cálculo de meses bisiestos, ID: `lunar`
3. **Islámico (Islamic)** - Calendario Hijri islámico, ID: `islamic`
4. **Hebreo (Hebrew)** - Calendario judío, ID: `hebrew`
5. **Persa (Persian)** - Calendario solar iraní, ID: `persian`
6. **Indio (Indian)** - Calendario nacional indio, ID: `indian`

### Calendarios Históricos Chinos (2)

7. **Histórico Chino (Chinese Historical)** - Soporta ciclo Ganzhi y años de reinado imperial, ID: `chinese_historical`
   - **Ciclo Ganzhi**: Ciclo de 60 años (troncos celestiales + ramas terrestres)
   - **Años de reinado imperial**: Soporta años de reinado de varias dinastías chinas (Kangxi, Qianlong, Zhenguan, etc.)
   - **Base de datos dinámica**: Base de datos completa incorporada de dinastías y años de reinado de la historia china
8. **Sexagenario (Sexagenary)** - Ciclo Ganzhi de 60 años, ID: `sexagenary`

### Calendarios del Este de Asia (6)

9. **Japonés (Japanese)** - Calendario de eras japonés (Nengo), ID: `japanese`
10. **Vietnamita (Vietnamese)** - Calendario lunar vietnamita (variante del zodiaco del gato), ID: `vietnamese`
11. **Tibetano (Tibetan)** - Sistema de calendario tibetano, ID: `tibetan`
12. **Mongol (Mongolian)** - Calendario mongol, ID: `mongolian`
13. **Dai (Dai)** - Calendario Dai, con cálculo lunar completo, ID: `dai`
14. **Dehong Dai (Dehong Dai)** - Variante del calendario Dai de Dehong, ID: `dehong_dai`

### Calendarios Históricos (6)

15. **Maya (Mayan)** - Cuenta larga del calendario maya, ID: `mayan`
16. **Romano (Roman)** - Calendario de la antigua Roma, ID: `roman`
17. **Juliano (Julian)** - Calendario juliano, ID: `julian`
18. **Republicano Francés (French Republican)** - Calendario revolucionario francés, ID: `french_republican`
19. **Copto (Coptic)** - Calendario copto ortodoxo, ID: `coptic`
20. **Etíope (Ethiopian)** - Calendario etíope, ID: `ethiopian`

### Calendarios Regionales (6)

21. **Budista (Buddhist)** - Era budista (BE), año + 543, ID: `buddhist`
22. **Saka (Saka)** - Era Saka (Indonesia), ID: `saka`
23. **Vikram Samvat (Vikram Samvat)** - Calendario hindú, ID: `vikram_samvat`
24. **Javanés (Javanese)** - Calendario javanés islámico, ID: `javanese`
25. **Chula Sakarat (Chula Sakarat)** - Era budista del sudeste asiático, año - 638, ID: `chula_sakarat`
26. **Jemer (Khmer)** - Calendario jemer, ID: `khmer`

### Calendarios Modernos (3)

27. **ROC (ROC)** - Calendario Minguo, año - 1911, ID: `roc`
28. **Juche (Juche)** - Calendario de Corea del Norte, año - 1911, ID: `juche`
29. **Zoroástrico (Zoroastrian)** - Calendario zoroástrico, ID: `zoroastrian`

### Calendarios Étnicos (3)

30. **Yi (Yi)** - Sistema de calendario Yi, ID: `yi`
31. **Cherokee (Cherokee)** - Calendario Cherokee, ID: `cherokee`
32. **Inuit (Inuit)** - Calendario Inuit, ID: `inuit`

---

## Usar la Herramienta de Calendario

### Conversión Básica

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Respuesta**:
```json
{
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Consulta de Múltiples Calendarios

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Respuesta**: Devuelve la fecha en los 32 sistemas de calendario.

---

## API de Calendario

### Interfaz CalendarBase

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

### Ejemplo: Calendario Personalizado

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Lógica de conversión
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversión inversa
        return new GregorianDate(year, month, day);
    }
}
```

---

## Funcionalidades Especiales

### Detalle del Calendario Histórico Chino (Nuevo)

El calendario histórico chino es un aspecto destacado de este sistema, soportando dos funcionalidades principales:

#### 1. Sistema de Ciclo Ganzhi

Usa un ciclo de 60 años, compuesto por troncos celestiales y ramas terrestres:

```
Troncos Celestiales (10): 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
Ramas Terrestres (12): 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**Ejemplos**:
- 2026 = 丙午年
- 2025 = 乙巳年 (año de la serpiente)
- 2024 = 甲辰年 (año del dragón)

**Ejemplo de uso**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Respuesta**:
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "马"
}
```

#### 2. Sistema de Años de Reinado Imperial

Base de datos completa incorporada de dinastías y años de reinado de la historia china:

**Dinastías soportadas** (parcial):
- Dinastía Qing: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Dinastía Ming: Hongwu, Yongle, Jiajing, Wanli, Chongzhen, etc.
- Dinastía Tang: Zhenguan, Kaiyuan, Tianbao, etc.
- Dinastía Han: Jianyuan, Yuanguang, Yuanshuo, etc.
- Otras dinastías...

**Ejemplo de uso**:
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Respuesta**:
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### Manejo de Meses Bisiestos

Calendarios con meses bisiestos:
- Lunar Chino
- Hebreo
- Budista
- Vietnamita

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## Precisión del Calendario

### Cálculos Astronómicos

- Basado en datos astronómicos reales
- Soporte para fechas históricas
- Manejo de reformas de calendario

### Limitaciones Conocidas

- Algunas fechas antiguas son aproximadas
- Las reformas de calendario varían por región
- No incluye manejo de segundos bisiestos

---

## Casos de Uso

### Investigación Histórica

Convertir fechas históricas al calendario moderno:

```
Pregunta: "¿Cuándo fue la Revolución Francesa?"
Respuesta: "14 de julio de 1789 (Gregoriano)"
           "26 de Termidor del Año I (Republicano Francés)"
```

### Aplicaciones Culturales

Soporte para festivales tradicionales:

```
Año Nuevo Chino 2026:
- Gregoriano: 17 de febrero de 2026
- Lunar Chino: Primer día del primer mes
```

### Programación Multicultural

Programar eventos que respeten múltiples calendarios:

```
Reunión: 2026-04-20
- Evitar oraciones del viernes islámico
- Respetar el Sabbath judío
- Considerar festivos chinos
```

---

## Mejores Prácticas

### 1. Especificar Siempre el Calendario

Nunca asumir el sistema de calendario:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // ¡Especificar explícitamente!
}
```

### 2. Manejar Fechas Inválidas

Algunas fechas no existen en ciertos calendarios:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Considerar Zonas Horarias

Las conversiones de fecha pueden variar por zona horaria:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🔧 Ver la [Referencia de Herramientas](tools-reference.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
