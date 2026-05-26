# Sistema de Calendario

> **Versión: v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | **Español** | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Русский](../ru-RU/calendar-system.md)

## Resumen

SiliconLifeCollective soporta 32 sistemas de calendario diferentes para conversión de fechas y precisión histórica.

## Calendarios Soportados (32)

### Calendarios Principales (6)

1. **Gregoriano (Gregorian)** - Calendario estándar internacional, ID: `gregorian`
2. **Lunar chino (Chinese Lunar)** - Calendario tradicional chino, con cálculo de meses intercalares, ID: `lunar`
3. **Islámico (Islamic)** - Calendario Hijri islámico, ID: `islamic`
4. **Hebreo (Hebrew)** - Calendario judío, ID: `hebrew`
5. **Persa (Persian)** - Calendario solar iraní, ID: `persian`
6. **Indio (Indian)** - Calendario nacional indio, ID: `indian`

### Calendarios Históricos Chinos (2)

7. **Calendario histórico chino (Chinese Historical)** - Soporta ciclos sexagenarios y eras imperiales, ID: `chinese_historical`
   - **Ciclo sexagenario**: Ciclo de 60 años (Tallos Celestiales + Ramas Terrenales)
   - **Eras imperiales**: Soporta las eras de todas las dinastías de la historia china (Kangxi, Qianlong, Zhenguan, etc.)
   - **Base de datos dinámica**: Base de datos integrada completa de dinastías y eras de la historia china
8. **Calendario sexagenario (Sexagenary)** - Ciclo sexagenario de 60 años, ID: `sexagenary`

### Calendarios de Asia Oriental (6)

9. **Japonés (Japanese)** - Calendario de eras japonés (Nengo), ID: `japanese`
10. **Vietnamita (Vietnamese)** - Calendario lunar vietnamita (variante del gato zodiacal), ID: `vietnamese`
11. **Tibetano (Tibetan)** - Sistema de calendario tibetano, ID: `tibetan`
12. **Mongol (Mongolian)** - Calendario mongol, ID: `mongolian`
13. **Dai (Dai)** - Calendario Dai, con cálculo lunar completo, ID: `dai`
14. **Dai de Dehong (Dehong Dai)** - Variante del calendario Dai de Dehong, ID: `dehong_dai`

### Calendarios Históricos (6)

15. **Maya (Mayan)** - Cuenta larga maya, ID: `mayan`
16. **Romano (Roman)** - Calendario romano antiguo, ID: `roman`
17. **Juliano (Julian)** - Calendario juliano, ID: `julian`
18. **Republicano francés (French Republican)** - Calendario revolucionario francés, ID: `french_republican`
19. **Copto (Coptic)** - Calendario ortodoxo copto, ID: `coptic`
20. **Etíope (Ethiopian)** - Calendario etíope, ID: `ethiopian`

### Calendarios Regionales (6)

21. **Budista (Buddhist)** - Era budista (BE), año + 543, ID: `buddhist`
22. **Saka (Saka)** - Era Saka (Indonesia), ID: `saka`
23. **Vikram Samvat (Vikram Samvat)** - Calendario hindú, ID: `vikram_samvat`
24. **Javanés (Javanese)** - Calendario islámico javanés, ID: `javanese`
25. **Chula Sakarat (Chula Sakarat)** - Calendario budista del sudeste asiático, año - 638, ID: `chula_sakarat`
26. **Jemer (Khmer)** - Calendario jemer, ID: `khmer`

### Calendarios Modernos (3)

27. **República de China (ROC)** - Calendario de la República de China, año - 1911, ID: `roc`
28. **Juche (Juche)** - Calendario de Corea del Norte, año - 1911, ID: `juche`
29. **Zoroástrico (Zoroastrian)** - Calendario zoroástrico, ID: `zoroastrian`

### Calendarios Étnicos (3)

30. **Yi (Yi)** - Sistema de calendario Yi, ID: `yi`
31. **Cherokee (Cherokee)** - Calendario cherokee, ID: `cherokee`
32. **Inuit (Inuit)** - Calendario inuit, ID: `inuit`

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
  "result": "Cuarto día del tercer mes del calendario lunar del año Bingwu",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Consulta Multi-Calendario

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Respuesta**: Retorna la fecha en los 32 sistemas de calendario.

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

## Funciones Especiales

### Detalle del Calendario Histórico Chino (Nuevo)

El calendario histórico chino es una de las características destacadas de este sistema, soportando dos funciones principales:

#### 1. Sistema de Ciclo Sexagenario

Adopta un ciclo de 60 años, formado por la combinación de Tallos Celestiales y Ramas Terrenales:

```
Tallos Celestiales (10): Jia, Yi, Bing, Ding, Wu, Ji, Geng, Xin, Ren, Gui
Ramas Terrenales (12): Zi, Chou, Yin, Mao, Chen, Si, Wu, Wei, Shen, You, Xu, Hai
```

**Ejemplos**:
- 2026 = Año Bingwu
- 2025 = Año Yisi (Año de la Serpiente)
- 2024 = Año Jiachen (Año del Dragón)

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
  "result": "Año Bingwu, tercer mes, noveno día",
  "ganzhi_year": "Bingwu",
  "zodiac": "Caballo"
}
```

#### 2. Sistema de Eras Imperiales

Base de datos integrada completa de dinastías y eras imperiales de la historia china:

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
  "result": "Kangxi año 60, tercer mes, día quince",
  "era": "Kangxi",
  "era_year": 60,
  "dynasty": "Qing"
}
```

### Manejo de Meses Intercalares

Calendarios con meses intercalares:
- Lunar chino
- Hebreo
- Budista
- Vietnamita

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "Cuarto mes intercalar"
}
```

---

## Precisión del Calendario

### Cálculos Astronómicos

- Basados en datos astronómicos reales
- Soporte para fechas históricas
- Manejo de reformas de calendario

### Limitaciones Conocidas

- Algunas fechas antiguas son aproximadas
- Las reformas de calendario varían por región
- No incluye manejo de segundos intercalares

---

## Casos de Uso

### Investigación Histórica

Convertir fechas históricas a calendarios modernos:

```
Pregunta: "¿Cuándo fue la Revolución Francesa?"
Respuesta: "14 de julio de 1789 (gregoriano)"
          "26 de Termidor del Año I (calendario republicano francés)"
```

### Aplicaciones Culturales

Soporte para festividades tradicionales:

```
Año Nuevo Chino de 2026:
- Gregoriano: 17 de febrero de 2026
- Lunar chino: Primer día del primer mes
```

### Programación Multicultural

Programar eventos respetando múltiples calendarios:

```
Reunión: 2026-04-20
- Evitar la oración del viernes islámico
- Respetar el Shabat judío
- Considerar los feriados chinos
```

---

## Mejores Prácticas

### 1. Siempre Especificar el Calendario

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

### 3. Considerar las Zonas Horarias

Las conversiones de fechas pueden variar según la zona horaria:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Siguientes Pasos

- 📚 Leer la [guía de arquitectura](architecture.md)
- 🛠️ Consultar la [guía de desarrollo](development-guide.md)
- 🔧 Ver la [referencia de herramientas](tools-reference.md)
- 🚀 Comenzar con la [guía de inicio rápido](getting-started.md)
