# Web UI Průvodce

[English](../en/web-ui-guide.md) | [中文文档](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md)

## Přehled

Web UI je vestavěný HTTP server s kompletním rozhraním pro správu křemíkových bytostí.

## Přístup

```
http://localhost:8080
```

## Systém Skinů

### Dostupné Skiny

| Skin | Popis |
|------|-------|
| Admin | Profesionální, datově orientované rozhraní |
| Chat | Konverzační, message-centric design |
| Creative | Umělecké, vizuálně bohaté rozložení |
| Dev | Developer-centric, code-centric rozhraní |

### Přepínání Skinů

1. Přejděte na inicializační stránku
2. Vyberte skin z palety
3. Klikněte na "Použít"

## Controllery

### About

Informace o projektu.

### Audit

Dashboard auditu použití tokenů:
- Grafy trendů
- Export dat
- Filtrování podle bytosti

### Being

Správa křemíkových bytostí:
- Seznam bytostí
- Detaily bytosti
- Vytvoření nové bytosti
- Zobrazení historie chatu

### Chat

Rozhraní chatu v reálném čase:
- SSE aktualizace
- Více relací
- Nahrávání souborů
- Indikátor načítání

### CodeBrowser

Prohlížení a úprava kódu:
- Strom souborů
- Editor kódu
- Uložení změn
- Code hover hints

### Config

Správa konfigurace:
- AI klienti
- Jazyk
- Port
- Další nastavení

### Dashboard

Přehled systému:
- Počet bytostí
- Stav systému
- Metriky výkonu

### Executor

Stav executorů:
- Seznam executorů
- Stav fronty
- Statistiky

### Init

Průvodce inicializací pro první spuštění.

### Knowledge

Vizualizace knowledge graph (placeholder).

### Log

Prohlížeč systémových logů:
- Filtrování podle bytosti
- Úrovně logů
- Časové rozsahy

### Memory

Prohlížeč dlouhodobé paměti:
- Pokročilé filtrování
- Statistiky
- Detaily pamětí

### Permission

Správa oprávnění:
- GlobalACL
- Pravidla
- Audit log

### PermissionRequest

Fronta požadavků na oprávnění:
- Čekající požadavky
- Schvalování/Zamítání

### Project

Správa projektů (placeholder).

### Task

Systém úkolů:
- Seznam úkolů
- Vytvoření úkolu
- Stav úkolu
- Status badges

### Timer

Správa časovačů:
- Seznam časovačů
- Nastavení alarmů
- Periodické časovače

## SSE (Server-Sent Events)

Automatické aktualizace v reálném čase pro:
- Nové chatové zprávy
- Změny stavu bytostí
- Systémové události

## Nahrávání Souborů

### Dialog Zdroje Souboru

1. Klikněte na "Připojit soubor"
2. Vyberte zdroj:
   - Nahrát soubor
   - URL soubor
3. Potvrďte výběr

### Podporované Typy

- Textové soubory
- Obrázky
- Dokumenty
- Kódové soubory

## Lokalizace

### Podporované Jazyky

21 jazykových variant:
- Čínština: 6 variant
- Angličtina: 10 variant
- Španělština: 2 varianty
- Japonština
- Korejština
- Čeština

### Přepínání Jazyka

1. Přejděte na Config
2. Změňte Language
3. Restartujte aplikaci

## Best Practices

1. **Používejte SSE** pro aktualizace v reálném čase
2. **Vyberte appropriate skin** pro váš use case
3. **Monitorujte audit log** pravidelně
4. **Zálohujte konfiguraci** před změnami
5. **Testujte v různých prohlížečích**

## Řešení Problémů

### SSE Connection Lost

- Zkontrolujte síťové připojení
- Počkejte na automatický reconnect
- Zkontrolujte browser konzoli

### Page Not Loading

- Ověřte, že server běží
- Zkontrolujte port v konfiguraci
- Podívejte se na logy pro chyby

### Skin Not Applying

- Vyčistěte cache prohlížeče
- Zkontrolujte, že skin existuje
- Restartujte aplikaci

## Další Informace

- [Architektura](architecture.md)
- [API Reference](api-reference.md)
- [Vývojový Průvodce](development-guide.md)
