# Sistema de calendário

> **Versão: v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [Français](../fr-FR/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Italiano](../it-IT/calendar-system.md) | [Polski](../pl-PL/calendar-system.md) | **Português**

## Visão geral

O SiliconLifeCollective suporta 32 diferentes sistemas de calendário para conversão de datas e precisão histórica.

## Calendários suportados (32)

### Calendários principais (6)

1. **Calendário gregoriano (Gregorian)** - Calendário internacional padrão, ID: `gregorian`
2. **Calendário lunar chinês (Chinese Lunar)** - Calendário chinês tradicional com cálculo de meses intercalares, ID: `lunar`
3. **Calendário islâmico (Islamic)** - Calendário da Hégira islâmica, ID: `islamic`
4. **Calendário hebraico (Hebrew)** - Calendário hebraico, ID: `hebrew`
5. **Calendário persa (Persian)** - Calendário solar iraniano, ID: `persian`
6. **Calendário indiano (Indian)** - Calendário nacional indiano, ID: `indian`

### Calendários históricos chineses (2)

7. **Calendário histórico chinês (Chinese Historical)** - Suporta o ciclo Ganzhi e as eras imperiais, ID: `chinese_historical`
   - **Ciclo Ganzhi**: Ciclo de 60 anos (Troncos Celestiais + Ramos Terrestres)
   - **Eras imperiais**: Suporta as eras de todas as dinastias chinesas (Kangxi, Qianlong, Zhenguan, etc.)
   - **Base de dados dinâmica**: Base de dados completa integrada de dinastias e eras chinesas
8. **Calendário sexagenário (Sexagenary)** - Ciclo Ganzhi de 60 anos, ID: `sexagenary`

### Calendários da Ásia Oriental (6)

9. **Calendário japonês (Japanese)** - Calendário das eras Nengo japonesas, ID: `japanese`
10. **Calendário vietnamita (Vietnamese)** - Calendário lunar vietnamita (variante zodiacal do Gato), ID: `vietnamese`
11. **Calendário tibetano (Tibetan)** - Sistema de calendário tibetano, ID: `tibetan`
12. **Calendário mongol (Mongolian)** - Calendário mongol, ID: `mongolian`
13. **Calendário Dai (Dai)** - Calendário Dai com cálculo lunar completo, ID: `dai`
14. **Calendário Dai Dehong (Dehong Dai)** - Variante Dai Dehong, ID: `dehong_dai`

### Calendários históricos (6)

15. **Calendário maia (Mayan)** - Calendário longo maia, ID: `mayan`
16. **Calendário romano (Roman)** - Calendário romano antigo, ID: `roman`
17. **Calendário juliano (Julian)** - Calendário juliano, ID: `julian`
18. **Calendário republicano francês (French Republican)** - Calendário da Revolução Francesa, ID: `french_republican`
19. **Calendário copta (Coptic)** - Calendário copta ortodoxo, ID: `coptic`
20. **Calendário etíope (Ethiopian)** - Calendário etíope, ID: `ethiopian`

### Calendários regionais (6)

21. **Calendário budista (Buddhist)** - Era budista (BE), ano + 543, ID: `buddhist`
22. **Calendário Saka (Saka)** - Calendário nacional indiano, ID: `saka`
23. **Calendário Vikram Samvat (Vikram Samvat)** - Calendário hindu, ID: `vikram_samvat`
24. **Calendário javanês (Javanese)** - Calendário javanês, ID: `javanese`
25. **Calendário inuíte (Inuit)** - Calendário inuíte, ID: `inuit`
26. **Calendário Xishuangbanna Dai (Xishuangbanna Dai)** - Variante Dai de Xishuangbanna, ID: `xishuangbanna_dai`

### Calendários especializados (6)

27. **Calendário lunar chinês (Chinese Lunar)** - Cálculo lunar com meses intercalares
28. **Calendário Yi (Yi)** - Calendário da etnia Yi, ID: `yi`
29. **Calendário de Bahá'í (Bahá'í)** - Calendário Badí`, ID: `bahaai`
30. **Calendário zoroastriano (Zoroastrian)** - Calendário zoroastriano, ID: `zoroastrian`
31. **Calendário nepalês (Nepali)** - Calendário Bikram Sambat nepalês, ID: `nepali`
32. **Calendário celta (Celtic)** - Calendário celta, ID: `celtic`

## Conversão entre calendários

O sistema suporta a conversão entre quaisquer dois calendários suportados:

```csharp
// Converter data gregoriana para calendário lunar chinês
var lunarDate = CalendarConverter.Convert(gregorianDate, "lunar");

// Converter data gregoriana para calendário islâmico
var islamicDate = CalendarConverter.Convert(gregorianDate, "islamic");
```

## API de calendário

### Obter a data atual

```
GET /api/calendar/now?calendar={type}
```

### Converter data

```
GET /api/calendar/convert?from={type1}&to={type2}&date={date}
```

### Listar calendários

```
GET /api/calendar/list
```

---

## Próximos passos

- 📚 Ler a [documentação de arquitetura](architecture.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
- 🌐 Ler o [guia da interface Web](web-ui-guide.md)
