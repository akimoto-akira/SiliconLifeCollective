# Přehled API

[English](../en/api-reference.md) | [中文文档](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

## Core Rozhraní

### IAIClient

Základní rozhraní pro AI klienty.

```csharp
public interface IAIClient
{
    string Name { get; }
    Task<AIResponse> ChatAsync(AIRequest request);
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### ITool

Rozhraní pro všechny nástroje.

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### IStorage

Rozhraní pro úložiště dat.

```csharp
public interface IStorage
{
    Task<string> ReadAsync(string key);
    Task WriteAsync(string key, string value);
    Task<bool> ExistsAsync(string key);
    Task DeleteAsync(string key);
}
```

### ITimeStorage

Rozhraní pro úložiště s časovým indexem.

```csharp
public interface ITimeStorage : IStorage
{
    Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end);
}
```

## Systém Nástrojů

### ToolManager

Automaticky objevuje a registruje nástroje prostřednictvím reflexe.

**Klíčové metody**:
- `DiscoverTools()` - Skenování assembly pro implementace ITool
- `ExecuteTool(string toolName, ToolCall call)` - Provedení nástroje
- `GetToolDefinitions()` - Získání definic všech nástrojů pro AI

### Built-in Nástroje

| Nástroj | Popis |
|---------|-------|
| CalendarTool | Operace kalendáře (now, format, convert, atd.) |
| ChatTool | Komunikace mezi bytostmi |
| ConfigTool | Správa konfigurace (pouze Kurátor) |
| CuratorTool | Nástroje správy Kurátora (pouze Kurátor) |
| DiskTool | Operace se soubory |
| MemoryTool | Správa dlouhodobé paměti |
| NetworkTool | Síťové požadavky |
| SystemTool | Systémové informace a operace |
| TaskTool | Správa úkolů |
| TimerTool | Správa časovačů |
| TokenAuditTool | Audit použití tokenů (pouze Kurátor) |

## Systém Oprávnění

### PermissionManager

Soukromá instance pro každou bytost.

**Priorita ověřování**:
1. IsCurator (zkratka na Povoleno)
2. User High Deny Cache
3. User High Allow Cache
4. Permission Callback

**Klíčové metody**:
- `CheckPermissionAsync(PermissionType type, string resource)` - Kontrola oprávnění
- `UpdateHighAllow(string prefix)` - Aktualizace High Allow cache
- `UpdateHighDeny(string prefix)` - Aktualizace High Deny cache

## Web API

### RESTful Endpointy

#### Chat API

- `GET /api/chat/sessions` - Získání seznamu relací
- `GET /api/chat/messages/{sessionId}` - Získání zpráv relace
- `POST /api/chat/send` - Odeslání zprávy

#### Being API

- `GET /api/beings` - Získání seznamu bytostí
- `GET /api/beings/{id}` - Získání detailů bytosti
- `POST /api/beings/create` - Vytvoření nové bytosti

#### Memory API

- `GET /api/memory` - Získání pamětí s filtrováním
- `GET /api/memory/stats` - Získání statistik pamětí
- `POST /api/memory/compress` - Komprese pamětí

### SSE (Server-Sent Events)

Endpoint: `/api/sse`

**Typy událostí**:
- `chat_message` - Nová chatová zpráva
- `being_status` - Změna stavu bytosti
- `system_event` - Systémová událost

## Konfigurace

### Struktura Konfigurace

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    },
    "DashScope": {
      "ApiKey": "your-api-key",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  },
  "Language": "cs-CZ",
  "Port": 8080
}
```

### Dynamické Možnosti

AI klienti poskytují dynamické možnosti konfigurace:

- **Modely**: Získány prostřednictvím API nebo fallback seznam
- **Oblasti**: Beijing, Virginia, Singapore, Hongkong, Frankfurt
- **Zobrazené názvy**: Lokalizované podle nastaveného jazyka

## Návratové Kódy

### HTTP Status Kódy

| Kód | Popis |
|-----|-------|
| 200 | Úspěch |
| 400 | Špatný požadavek |
| 401 | Neoprávněný |
| 403 | Zakázáno |
| 404 | Nenalezeno |
| 500 | Interní chyba serveru |

### PermissionResult

| Výsledek | Popis |
|----------|-------|
| Allowed | Operace povolena |
| Denied | Operace zamítnuta |
| AskUser | Vyžadováno potvrzení uživatele |

## Příklady Použití

### Volání AI Klienta

```csharp
var client = ServiceLocator.Instance.Get<IAIClient>();
var request = new AIRequest
{
    Messages = new List<Message>
    {
        new Message { Role = "user", Content = "Ahoj!" }
    }
};

var response = await client.ChatAsync(request);
Console.WriteLine(response.Content);
```

### Provedení Nástroje

```csharp
var toolManager = being.ToolManager;
var result = await toolManager.ExecuteTool("calendar", new ToolCall
{
    Name = "calendar",
    Parameters = new Dictionary<string, object>
    {
        ["action"] = "now",
        ["calendar"] = "gregorian"
    }
});
```

### Kontrola Oprávnění

```csharp
var permission = await being.PermissionManager.CheckPermissionAsync(
    PermissionType.FileAccess, 
    "file:C:\\data\\test.txt"
);

if (permission == PermissionResult.Allowed)
{
    // Provést operaci
}
```

---

## API Pracovních Poznámek

### Získat Seznam Pracovních Poznámek

**GET** `/api/beings/{id}/work-notes`

**Odpověď**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "Modul ověřování uživatele dokončen",
      "keywords": ["authentication", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Získat Detaily Jedné Poznámky

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Odpověď**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "Modul ověřování uživatele dokončen",
  "content": "## Detaily implementace\n\n- Použití JWT tokenu\n- Podpora OAuth2",
  "keywords": ["authentication", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Vytvořit Novou Poznámku

**POST** `/api/beings/{id}/work-notes`

**Požadavek**:
```json
{
  "summary": "Modul ověřování uživatele dokončen",
  "content": "## Detaily implementace\n\n- Použití JWT tokenu",
  "keywords": "authentication,JWT,OAuth2"
}
```

**Odpověď**: `201 Created`

### Aktualizovat Poznámku

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Požadavek**:
```json
{
  "summary": "Modul ověřování uživatele a testy dokončeny",
  "content": "## Aktualizovaný obsah\n\nPřidány unit testy",
  "keywords": "authentication,JWT,OAuth2,tests"
}
```

### Smazat Poznámku

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Vyhledat Poznámky

**GET** `/api/beings/{id}/work-notes/search?keyword=authentication&maxResults=10`

### Získat Adresář Poznámek

**GET** `/api/beings/{id}/work-notes/directory`

---

## API Sítě Znalostí

### Získat Statistiky Znalostí

**GET** `/api/knowledge/stats`

**Odpověď**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Přidat Trojici Znalostí

**POST** `/api/knowledge/triples`

**Požadavek**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Odpověď**: `201 Created`

### Dotaz na Znalost

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Odpověď**:
```json
{
  "triples": [
    {
      "subject": "Python",
      "predicate": "is_a",
      "object": "programming_language",
      "confidence": 0.95,
      "tags": ["programming", "language"]
    }
  ]
}
```

### Vyhledat Znalost

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Získat Cestu Znalostí

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Odpověď**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Validovat Znalost

**POST** `/api/knowledge/validate`

**Požadavek**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Smazat Znalost

**DELETE** `/api/knowledge/triples/{id}`

---

## API Správy Projektů

### Získat Seznam Projektů

**GET** `/api/projects`

**Odpověď**:
```json
{
  "projects": [
    {
      "id": "project-uuid",
      "name": "My Project",
      "description": "Project description",
      "createdAt": "2026-04-25T10:00:00Z"
    }
  ]
}
```

### Vytvořit Projekt

**POST** `/api/projects`

**Požadavek**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Získat Detaily Projektu

**GET** `/api/projects/{id}`

### Aktualizovat Projekt

**PUT** `/api/projects/{id}`

### Smazat Projekt

**DELETE** `/api/projects/{id}`
