# Руководство по Silicon Beings

> **Версия: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [Français](../fr-FR/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | **Русский** | [Čeština](../cs-CZ/silicon-being-guide.md)

## Обзор

Silicon Beings — это ИИ-управляемые агенты, способные автономно мыслить, действовать и развиваться.

## Архитектура

### Разделение Body-Brain

```
┌─────────────────────────────────────┐
│         Silicon Being                │
├──────────────────┬──────────────────┤
│   Body           │   Brain          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Управление     │ • Загрузка       │
│   состоянием     │   истории        │
│ • Обнаружение    │ • Вызов ИИ       │
│   триггеров      │ • Выполнение     │
│ • Жизненный цикл │   инструментов   │
│                  │ • Сохранение     │
│                  │   ответа         │
└──────────────────┴──────────────────┘
```

## Файл души
