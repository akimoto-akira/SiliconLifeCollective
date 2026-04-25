# Sistema de Calendario

[English](calendar-system.md) | [简体中文](docs/zh-CN/calendar-system.md) | [繁體中文](docs/zh-HK/calendar-system.md) | [Español](docs/es-ES/calendar-system.md) | [日本語](docs/ja-JP/calendar-system.md) | [한국어](docs/ko-KR/calendar-system.md) | [Čeština](docs/cs-CZ/calendar-system.md)

## Resumen

Silicon Life Collective soporta **32 sistemas de calendario** diferentes, proporcionando conversión y consulta de fechas multi-calendario.

---

## Calendarios Soportados

### Calendarios Principales

| Calendario | Código | Descripción |
|-----------|--------|-------------|
| Gregoriano | `Gregorian` | Calendario occidental estándar |
| Lunar Chino | `ChineseLunar` | Calendario lunar tradicional chino |
| Islámico | `Islamic` | Calendario lunar islámico (Hijri) |
| Hebreo | `Hebrew` | Calendario hebreo tradicional |
| Julien | `Julian` | Calendario juliano histórico |
| Persa | `Persian` | Calendario iraní (Solar Hijri) |

### Calendarios Asiáticos

| Calendario | Código | Región |
|-----------|--------|--------|
| Japonés | `Japanese` | Japón |
| Budista | `Buddhist` | Tailandia, Sri Lanka |
| Indiano | `Indian` | India (Saka) |
| Tibetano | `Tibetan` | Tíbet |
| Vietnamita | `Vietnamese` | Vietnam |
| Javanés | `Javanese` | Indonesia (Java) |
| Khmer | `Khmer` | Camboya |
| Mongol | `Mongolian` | Mongolia |

### Calendarios Históricos

| Calendario | Código | Período |
|-----------|--------|---------|
| Maya | `Mayan` | Civilización maya |
| Egipcio Copto | `Coptic` | Egipto copto |
| Republicano Francés | `FrenchRepublican` | Revolución francesa |
| Romano | `Roman` | Antigua Roma |
| Era China Histórica | `ChineseHistorical` | Dinastías chinas |
| Sexagenario | `Sexagenary` | Ciclo chino de 60 años |

### Calendarios Culturales

| Calendario | Código | Cultura |
|-----------|--------|---------|
| Cherokee | `Cherokee` | Nación Cherokee |
| Etíope | `Ethiopian` | Etiopía |
| Inuit | `Inuit` | Pueblos inuit |
| Juche | `Juche` | Corea del Norte |
| República de China | `RepublicOfChina` | Taiwán |
| Zoroástrico | `Zoroastrian` | Persia zoroástrica |
| Vikram Samvat | `VikramSamvat` | India (Hindu) |
| Dai | `Dai` | Pueblo Dai (China) |
| Dehong Dai | `DehongDai` | Dehong, China |
| Chula Sakarat | `ChulaSakarat` | Sudeste asiático |

### Calendarios Especializados

| Calendario | Código | Uso |
|-----------|--------|-----|
| Lunar Chino Histórico | `ChineseHistoricalCalendar` | Eras dinásticas |
| Yi | `Yi` | Pueblo Yi (China) |

---

## Usar CalendarTool

### Obtener Fecha Actual

```csharp
var tool = new CalendarTool();

// Fecha actual en calendario gregoriano
var gregorian = tool.Execute("GetCurrentDate", new { 
    calendar = "Gregorian" 
});

// Fecha actual en calendario lunar chino
var chinese = tool.Execute("GetCurrentDate", new { 
    calendar = "ChineseLunar" 
});
```

### Convertir Fechas

```csharp
// De Gregoriano a Lunar Chino
var result = tool.Execute("ConvertDate", new {
    date = new DateTime(2024, 1, 1),
    targetCalendar = "ChineseLunar"
});

// De Gregoriano a Islámico
var islamic = tool.Execute("ConvertDate", new {
    date = DateTime.Now,
    targetCalendar = "Islamic"
});
```

### Listar Calendarios

```csharp
var calendars = tool.Execute("GetSupportedCalendars", new { });
// Retorna lista de todos los calendarios soportados
```

---

## Implementación de Calendarios

Cada calendario implementa una interfaz común:

```csharp
public interface ICalendar
{
    string Name { get; }
    DateTime ToGregorian(int year, int month, int day);
    (int year, int month, int day) FromGregorian(DateTime date);
    string GetMonthName(int month);
    string GetDayName(int day);
}
```

### Ejemplo: Implementación Gregoriana

```csharp
public class GregorianCalendar : ICalendar
{
    public string Name => "Gregorian";
    
    public DateTime ToGregorian(int year, int month, int day)
    {
        return new DateTime(year, month, day);
    }
    
    public (int year, int month, int day) FromGregorian(DateTime date)
    {
        return (date.Year, date.Month, date.Day);
    }
    
    public string GetMonthName(int month) => month switch
    {
        1 => "Enero",
        2 => "Febrero",
        // ... más meses
        _ => throw new ArgumentException("Mes inválido")
    };
}
```

---

## Localización de Calendarios

Los nombres de meses y días se localizan según el idioma actual del sistema.

### Ejemplo en Español

```csharp
public override string? GetGregorianMonthName(int month) => month switch
{
    1  => "Enero",     2  => "Febrero",
    3  => "Marzo",     4  => "Abril",
    5  => "Mayo",      6  => "Junio",
    7  => "Julio",     8  => "Agosto",
    9  => "Septiembre",10 => "Octubre",
    11 => "Noviembre", 12 => "Diciembre",
    _  => null
};
```

---

## Casos de Uso

### Mostrar Fecha Multi-Calendar

```csharp
var today = DateTime.Now;

var gregorian = calendarTool.Execute("ConvertDate", new {
    date = today,
    targetCalendar = "Gregorian"
});

var chinese = calendarTool.Execute("ConvertDate", new {
    date = today,
    targetCalendar = "ChineseLunar"
});

var islamic = calendarTool.Execute("ConvertDate", new {
    date = today,
    targetCalendar = "Islamic"
});

Console.WriteLine($"Hoy: {gregorian} (Gregoriano)");
Console.WriteLine($"     {chinese} (Lunar Chino)");
Console.WriteLine($"     {islamic} (Islámico)");
```

### Eventos Culturales

Calcular fechas de eventos culturales:

- Año Nuevo Chino (Lunar Chino)
- Ramadán (Islámico)
- Pascua (Hebreo/Gregoriano)
- Songkran (Budista)

---

## Extender con Nuevos Calendarios

Puedes agregar nuevos calendarios implementando `ICalendar`:

```csharp
public class MyCustomCalendar : ICalendar
{
    public string Name => "MiCalendario";
    
    public DateTime ToGregorian(int year, int month, int day)
    {
        // Lógica de conversión
    }
    
    public (int year, int month, int day) FromGregorian(DateTime date)
    {
        // Lógica de conversión inversa
    }
    
    public string GetMonthName(int month)
    {
        // Retornar nombre del mes
    }
}
```

El sistema descubrirá automáticamente el nuevo calendario.

---

## Recursos Adicionales

- [Guía de Arquitectura](architecture.md) - Diseño del sistema
- [Referencia de Herramientas](tools-reference.md) - Documentación de CalendarTool
- [Localización](../es-ES/README.md) - Soporte de idiomas
