# Руководство по устранению неполадок

> **Версия: v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [Français](../fr-FR/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | **Русский** | [Čeština](../cs-CZ/troubleshooting.md)

## Частые проблемы

### Сборка и компиляция

#### Проблема: Ошибка сборки, отсутствуют зависимости

**Симптомы**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Решение**:
```bash
dotnet restore
dotnet build
```

#### Проблема: .NET SDK не найден

**Симптомы**:
```
The .NET SDK could not be found
```
