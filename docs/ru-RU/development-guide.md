# Руководство по разработке

> **Версия: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [Français](../fr-FR/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | **Русский** | [Čeština](../cs-CZ/development-guide.md)

## Обзор архитектуры

SiliconLifeCollective следует **архитектуре тело-мозг**, со строгим разделением между основными интерфейсами и реализациями по умолчанию.

### Структура проекта

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Интерфейсы, абстрактные классы, общая инфраструктура
│   ├── SiliconLife.Common/          # Общие реализации (используются обеими версиями)
│   ├── SiliconLife.Default/         # Стандартная реализация, точка входа (проверка осуществимости)
│   ├── SiliconLife.Fast/            # Высокопроизводительная реализация, точка входа (производственная версия)
│   ├── SiliconLife.Speedy/          # Высокопроизводительный движок хранения SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Инструмент управления SpeedyPack (Windows Forms)
└── docs/                            # Многоязычная документация
```

**Направление зависимостей**:
- `SiliconLife.Default` → `SiliconLife.Core` (однонаправленное)
- `SiliconLife.Fast` → `SiliconLife.Core` (однонаправленное)
- `SiliconLife.Common` → `SiliconLife.Core` (однонаправленное)

**Описание ролей версий**:
