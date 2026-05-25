# Система разрешений

> **Версия: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [Français](../fr-FR/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | **Русский** | [Čeština](../cs-CZ/permission-system.md)

## Обзор

Система разрешений гарантирует, что все операции, инициированные ИИ, должным образом проверены и аудированы.

## Цепочка разрешений из 5 уровней

```
┌─────────────────────────────────────────────┐
│          Проверка разрешений                 │
├─────────────────────────────────────────────┤
│  Уровень 1: UserFrequencyCache              │
│  ↓ Кэшированные пользовательские решения (HighDeny/HighAllow) │
│  Уровень 2: IPermissionCallback             │
│  ↓ Пользовательская логика (Allowed/Denied/AskUser) │
│  Уровень 3: IsCurator ?                     │
│  ↓ Да → IPermissionAskHandler (спросить пользователя) │
│  ↓ Нет → GlobalACL → Отказ по умолчанию     │
│  Результат: Разрешено или Отказано           │
└─────────────────────────────────────────────┘
```

> **Примечание**: Фактический приоритет опроса в `PermissionManager.CheckPermission()`:
> 1. **UserFrequencyCache** — Сначала проверить кэшированные высокочастотные пользовательские решения
> 2. **IPermissionCallback** — Оценить пользовательские правила обратного вызова
