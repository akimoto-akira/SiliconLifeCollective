# Sistema de Calendário

> **Versão: v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Русский](../ru-RU/calendar-system.md)

## Visão Geral

O SiliconLifeCollective suporta 32 sistemas de calendário diferentes, para conversão de datas e precisão histórica.

## Calendários Suportados (32)

### Calendários Principais (6)

1. **Gregoriano (Gregorian)** - Calendário internacional padrão, ID: `gregorian`
2. **Lunar Chinês (Chinese Lunar)** - Calendário tradicional chinês, com cálculo de meses intercalares, ID: `lunar`
3. **Islâmico (Islamic)** - Calendário Hijri islâmico, ID: `islamic`
4. **Hebraico (Hebrew)** - Calendário judaico, ID: `hebrew`
5. **Persa (Persian)** - Calendário solar iraniano, ID: `persian`
6. **Indiano (Indian)** - Calendário nacional indiano, ID: `indian`

### Calendários Históricos Chineses (2)

7. **Histórico Chinês (Chinese Historical)** - Suporta era Ganzhi e era imperial, ID: `chinese_historical`
   - **Era Ganzhi**: Ciclo de 60 anos (Troncos Celestiais + Ramos Terrestres)
   - **Era Imperial**: Suporta as eras imperiais de várias dinastias da história chinesa (Kangxi, Qianlong, Zhenguan, etc.)
   - **Base de dados dinâmica**: Base de dados completa de dinastias e eras imperiais da história chinesa incorporada
8. **Sexagenário (Sexagenary)** - Ciclo sexagenário de 60 anos, ID: `sexagenary`

### Calendários da Ásia Oriental (6)

9. **Japonês (Japanese)** - Calendário de era japonesa (Nengo), ID: `japanese`
10. **Vietnamita (Vietnamese)** - Calendário lunar vietnamita (variante do zodíaco do gato), ID: `vietnamese`
11. **Tibetano (Tibetan)** - Sistema de calendário tibetano, ID: `tibetan`
12. **Mongol (Mongolian)** - Calendário mongol, ID: `mongolian`
13. **Dai (Dai)** - Calendário Dai, com cálculo lunar completo, ID: `dai`
14. **Dai de Dehong (Dehong Dai)** - Variante do calendário Dai de Dehong, ID: `dehong_dai`

### Calendários Históricos (6)

15. **Maia (Mayan)** - Contagem Longa Maia, ID: `mayan`
16. **Romano (Roman)** - Calendário romano antigo, ID: `roman`
17. **Juliano (Julian)** - Calendário juliano, ID: `julian`
18. **Republicano Francês (French Republican)** - Calendário da Revolução Francesa, ID: `french_republican`
19. **Copta (Coptic)** - Calendário ortodoxo copta, ID: `coptic`
20. **Etíope (Ethiopian)** - Calendário etíope, ID: `ethiopian`

### Calendários Regionais (6)

21. **Budista (Buddhist)** - Era Budista (BE), ano + 543, ID: `buddhist`
22. **Saka (Saka)** - Era Saka (Indonésia), ID: `saka`
23. **Vikram Samvat (Vikram Samvat)** - Calendário hindu, ID: `vikram_samvat`
24. **Javanês (Javanese)** - Calendário islâmico javanês, ID: `javanese`
25. **Chula Sakarat (Chula Sakarat)** - Calendário budista do Sudeste Asiático, ano - 638, ID: `chula_sakarat`
26. **Khmer (Khmer)** - Calendário khmer, ID: `khmer`

### Calendários Modernos (3)

27. **República da China (ROC)** - Calendário da República da China, ano - 1911, ID: `roc`
28. **Juche (Juche)** - Calendário norte-coreano, ano - 1911, ID: `juche`
29. **Zoroastriano (Zoroastrian)** - Calendário zoroastriano, ID: `zoroastrian`

### Calendários Étnicos (3)

30. **Yi (Yi)** - Sistema de calendário Yi, ID: `yi`
31. **Cherokee (Cherokee)** - Calendário Cherokee, ID: `cherokee`
32. **Inuit (Inuit)** - Calendário Inuit, ID: `inuit`

---

## Usar a Ferramenta de Calendário

### Conversão Básica

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Resposta**:
```json
{
  "result": "Lunar 3 do 4º mês do ano Bingwu",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Consulta Multi-Calendário

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Resposta**: Retorna a data em todos os 32 sistemas de calendário.

---

## API de Calendário

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

### Exemplo: Calendário Personalizado

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Lógica de conversão
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversão inversa
        return new GregorianDate(year, month, day);
    }
}
```

---

## Funcionalidades Especiais

### Detalhes do Calendário Histórico Chinês (Novo)

O calendário histórico chinês é um dos destaques deste sistema, suportando duas funcionalidades principais:

#### 1. Sistema de Era Ganzhi

Usa um ciclo de 60 anos, formado pela combinação de Troncos Celestiais e Ramos Terrestres:

```
Troncos Celestiais (10): Jia, Yi, Bing, Ding, Wu, Ji, Geng, Xin, Ren, Gui
Ramos Terrestres (12): Zi, Chou, Yin, Mao, Chen, Si, Wu, Wei, Shen, You, Xu, Hai
```

**Exemplos**:
- 2026 = Ano Bingwu
- 2025 = Ano Yisi (Ano da Cobra)
- 2024 = Ano Jiachen (Ano do Dragão)

**Exemplo de uso**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Resposta**:
```json
{
  "result": "Ano Bingwu 3º mês 9º dia",
  "ganzhi_year": "Bingwu",
  "zodiac": "Cavalo"
}
```

#### 2. Sistema de Era Imperial

Base de dados completa de dinastias e eras imperiais da história chinesa incorporada:

**Dinastias suportadas** (parcial):
- Dinastia Qing: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Dinastia Ming: Hongwu, Yongle, Jiajing, Wanli, Chongzhen, etc.
- Dinastia Tang: Zhenguan, Kaiyuan, Tianbao, etc.
- Dinastia Han: Jianyuan, Yuanguang, Yuanshuo, etc.
- Outras dinastias...

**Exemplo de uso**:
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Resposta**:
```json
{
  "result": "Kangxi 60º ano 3º mês 15º dia",
  "era": "Kangxi",
  "era_year": 60,
  "dynasty": "Qing"
}
```

### Tratamento de Meses Intercalares

Calendários com meses intercalares:
- Lunar Chinês
- Hebraico
- Budista
- Vietnamita

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "4º mês intercalar"
}
```

---

## Precisão do Calendário

### Cálculos Astronómicos

- Baseados em dados astronómicos reais
- Suporte a datas históricas
- Tratamento de reformas de calendário

### Limitações Conhecidas

- Algumas datas antigas são aproximadas
- As reformas de calendário variam por região
- Não inclui tratamento de segundos intercalares

---

## Casos de Uso

### Pesquisa Histórica

Converter datas históricas para calendários modernos:

```
Pergunta: "Quando ocorreu a Revolução Francesa?"
Resposta: "14 de Julho de 1789 (Gregoriano)"
         "26 de Termidor do Ano I (Republicano Francês)"
```

### Aplicações Culturais

Suportar festivais tradicionais:

```
Ano Novo Chinês de 2026:
- Gregoriano: 17 de Fevereiro de 2026
- Lunar Chinês: 1º dia do 1º mês
```

### Agendamento Multicultural

Agendar eventos respeitando múltiplos calendários:

```
Reunião: 2026-04-20
- Evitar a oração de sexta-feira islâmica
- Respeitar o Shabbat judaico
- Considerar os feriados chineses
```

---

## Melhores Práticas

### 1. Especificar Sempre o Calendário

Nunca assumir o sistema de calendário:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Especificar explicitamente!
}
```

### 2. Tratar Datas Inválidas

Algumas datas não existem em certos calendários:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Considerar Fuso Horário

A conversão de datas pode variar conforme o fuso horário:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Próximos Passos

- 📚 Leia o [guia de arquitectura](architecture.md)
- 🛠️ Consulte o [guia de desenvolvimento](development-guide.md)
- 🔧 Consulte a [referência de ferramentas](tools-reference.md)
- 🚀 Comece com o [guia de início rápido](getting-started.md)
