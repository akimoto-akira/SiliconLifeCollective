# API reference

> **Verze: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | **Čeština** | [Русский](../ru-RU/api-reference.md)

## Web API koncové body

Základní URL: `http://localhost:8080`

### Autentizace

Většina koncových bodů vyžaduje autentizaci pomocí session cookie spravované přes Web UI. Před inicializací systému budou všechny požadavky kromě stránky nápovědy přesměrovány na inicializační stránku.

---

## Řídicí panel

### Získání statistik řídicího panelu

**GET** `/api/dashboard/stats`

Vrací přehledná data systému (počet bytostí, stav běhu atd.).

### Získání metrik výkonu

**GET** `/api/dashboard/metrics`

Vrací data metrik výkonu v reálném čase.

---

## Chatovací systém

### Chatovací stránka

**GET** `/chat`

Vrací stránku chatovacího rozhraní.

### Streamový chat (SSE)

**GET** `/api/chat/stream`

Streamový chat prostřednictvím Server-Sent Events (SSE).

**Odpověď**: Server-Sent Events stream

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Získání seznamu relací

**GET** `/api/chat/conversations`

Vrací seznam všech aktivních chatovacích relací.

**Příklad odpovědi**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat s Xiaoyou",
      "lastMessage": "Obsah poslední zprávy",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Získání historie zpráv

**GET** `/api/chat/messages`

Parametr dotazu: `channelId` — ID kanálu/relace

Vrací historii zpráv zadané relace.

### Získání historie chatu

**GET** `/api/chat/history`

Vrací globální historii chatu.

### Odeslání zprávy

**POST** `/api/chat/send`

**Tělo požadavku**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Obsah testovací zprávy"
}
```

**Odpověď**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Zastavení AI přemýšlení

**POST** `/api/chat/stop`

Zastaví aktuálně probíhající generování AI odpovědi.

**Tělo požadavku**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Nahrání souboru

**POST** `/api/chat/upload`

Nahraje soubor do chatovací relace (podpora multipart/form-data).

---

## Správa Křemíkových Bytostí

### Stránka správy bytostí

**GET** `/beings`

Vrací stránku rozhraní pro správu Křemíkových Bytostí.

### Získání seznamu bytostí

**GET** `/api/beings` nebo **GET** `/api/beings/list`

Vrací seznam všech registrovaných Křemíkových Bytostí.

**Příklad odpovědi**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soulPath": "path/to/soul.md"
    }
  ]
}
```

**Hodnoty stavu**: `idle` | `running` | `waiting_permission` | `stopped`

### Získání detailů bytosti

**GET** `/api/beings/detail`

Parametr dotazu: `beingId` — ID bytosti

Vrací detailní informace o zadané bytosti.

### Získání aktivních stavů bytostí

**GET** `/api/beings/activity`

Vrací informace o aktivních stavech jednotlivých bytostí.

### Stránka editoru Souboru Duše

**GET** `/beings/soul`

Vrací rozhraní editoru Souboru Duše.

### Uložení Souboru Duše

**POST** `/api/beings/soul/save`

**Tělo požadavku**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### Stránka editoru AI konfigurace

**GET** `/beings/ai-config`

Vrací rozhraní editoru AI konfigurace.

### Uložení AI konfigurace

**POST** `/api/beings/ai-config/save`

**Tělo požadavku**:
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### Získání seznamu dostupných AI modelů

**GET** `/api/beings/ai-config/models`

Parametry dotazu: `clientType`, `apiKey`, `region`

Vrací seznam dostupných modelů pro zadaného AI klienta.

---

## Zobrazení historie chatu

### Stránka historie chatu

**GET** `/chat-history`

Vrací hlavní stránku historie chatu.

### Stránka detailů historie chatu

**GET** `/chat-history-detail`

Vrací stránku s detaily historie chatu pro zadanou relaci.

### Stránka detailů historie skupinového chatu

**GET** `/group-chat-history-detail`

Vrací stránku s detaily historie skupinového chatu.

### Stránka detailů historie vysílání

**GET** `/broadcast-history-detail`

Vrací stránku s detaily historie vysílacího kanálu.

### Získání seznamu historických relací

**GET** `/api/chat-history/conversations`

Vrací seznam všech historických relací.

### Získání historických zpráv

**GET** `/api/chat-history/messages`

Parametr dotazu: `sessionId` — ID relace

Vrací záznamy zpráv zadané historické relace.

---

## Správa časovačů

### Stránka časovačů

**GET** `/timers`

Vrací stránku rozhraní pro správu časovačů.

### Získání seznamu časovačů

**GET** `/api/timers/list`

Vrací seznam všech časovačů.

### Stránka detailů cyklů časovače

**GET** `/timer-cycles/{timerId}`

Vrací stránku s detaily prováděcích cyklů zadaného časovače.

### Získání seznamu cyklů časovače

**GET** `/api/timer-cycles/list`

Parametr dotazu: `timerId` — ID časovače

Vrací seznam všech prováděcích cyklů zadaného časovače.

### Stránka detailů jednoho cyklu

**GET** `/timer-cycle/{cycleIndex}`

Vrací stránku s detaily jednoho provedení.

### Získání zpráv cyklu

**GET** `/api/timer-cycle/messages`

Parametr dotazu: `cycleIndex` — index cyklu

Vrací zprávy související se zadaným prováděcím cyklem.

---

## Správa úkolů

### Stránka úkolů

**GET** `/tasks`

Vrací stránku rozhraní pro správu úkolů.

### Získání seznamu úkolů

**GET** `/api/tasks/list`

Vrací seznam všech úkolů.

### Stránka detailů cyklů úkolu

**GET** `/task-cycles/{taskId}`

Vrací stránku s detaily prováděcích cyklů zadaného úkolu.

### Získání seznamu cyklů úkolu

**GET** `/api/task-cycles/list`

Parametr dotazu: `taskId` — ID úkolu

Vrací seznam všech prováděcích cyklů zadaného úkolu.

### Stránka detailů jednoho cyklu úkolu

**GET** `/task-cycle/{cycleIndex}`

Vrací stránku s detaily jednoho provedení úkolu.

### Získání zpráv cyklu úkolu

**GET** `/api/task-cycle/messages`

Parametr dotazu: `cycleIndex` — index cyklu

Vrací zprávy související se zadaným prováděcím cyklem úkolu.

---

## Systém oprávnění

### Stránka správy oprávnění

**GET** `/permissions`

Vrací stránku rozhraní pro správu oprávnění.

### Získání seznamu pravidel oprávnění

**GET** `/api/permissions/list`

Vrací všechna aktuálně nakonfigurovaná pravidla oprávnění.

**Příklad odpovědi**:
```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed",
      "description": "Allow GitHub API access"
    }
  ]
}
```

### Uložení pravidla oprávnění

**POST** `/api/permissions/save`

**Tělo požadavku**:
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### Stránka žádosti o oprávnění

**GET** `/permission/request`

Zobrazuje stránku žádosti o oprávnění, umožňuje uživateli schválit nebo zamítnout žádost Křemíkové Bytosti o oprávnění.

**Parametry dotazu**:

| Parametr | Typ | Popis |
|------|------|------|
| `userId` | `Guid` | ID Křemíkové Bytosti žádající o oprávnění |
| `type` | `string` | Typ oprávnění |
| `resource` | `string` | Cesta k požadovanému prostředku |
| `allowCode` | `string` | Kódový identifikátor pro povolení operace |
| `denyCode` | `string` | Kódový identifikátor pro zamítnutí operace |

### Kontrola nevyřízených žádostí o oprávnění

**GET** `/permission/check`

Parametr dotazu: `userId` — ID Křemíkové Bytosti

**Odpověď**:
```json
{
  "pending": true
}
```

### Odpověď na žádost o oprávnění

**GET** `/permission/respond`

**Parametry dotazu**:

| Parametr | Typ | Popis |
|------|------|------|
| `userId` | `Guid` | ID Křemíkové Bytosti |
| `allowed` | `bool` | Zda povolit |
| `addToCache` | `bool` | Zda uložit rozhodnutí do mezipaměti |
| `cacheDuration` | `double` | Doba trvání mezipaměti (hodiny) |

**Odpověď**:
```json
{
  "success": true
}
```

---

## Protokolový systém

### Stránka protokolů

**GET** `/logs`

Vrací stránku rozhraní pro prohlížení protokolů.

### Získání seznamu protokolů

**GET** `/api/logs/list`

Parametry dotazu podporují filtrování podle úrovně a časového rozsahu.

**Příklad odpovědi**:
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Failed to connect to AI service",
      "source": "OllamaClient"
    }
  ]
}
```

### Získání protokolů seskupených podle bytostí

**GET** `/api/logs/beings`

Statistiky protokolů seskupené podle Křemíkových Bytostí.

### Získání dostupných úrovní protokolů

**GET** `/api/logs/levels`

Vrací seznam úrovní protokolů dostupných v systému.

---

## Statistiky využití

### Stránka statistik využití

**GET** `/usage`

Vrací stránku rozhraní pro statistiky využití.

### Získání shrnutí využití

**GET** `/api/usage/summary`

Vrací shrnutí využití Tokenů a nákladů.

### Získání dat trendu

**GET** `/api/usage/trend`

Parametry dotazu: `startDate`, `endDate`

Vrací data trendu využití za zadané časové období.

### Export dat využití

**GET** `/api/usage/export`

Exportuje data využití ve formátu ke stažení.

---

## Auditní stopa

### Stránka auditu

**GET** `/audit`

Vrací stránku rozhraní auditní stopy.

### Získání seznamu auditů

**GET** `/api/audit/list`

Vrací seznam položek auditního protokolu.

### Získání shrnutí auditu

**GET** `/api/audit/summary`

Vrací souhrnné statistiky auditních dat.

### Získání auditů seskupených podle bytostí

**GET** `/api/audit/beings`

Statistiky auditů seskupené podle Křemíkových Bytostí.

---

## Správa konfigurace

### Stránka konfigurace

**GET** `/config`

Vrací stránku rozhraní pro konfiguraci systému.

### Uložení konfigurace

**POST** `/config/save`

**Tělo požadavku**:
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    }
  }
}
```

### Získání možností AI konfigurace

**GET** `/config/aioptions`

Vrací dostupné typy AI klientů a jejich dynamické možnosti (dostupné modely, regiony atd.).

---

## Paměťový systém

### Stránka paměti

**GET** `/memory`

Vrací stránku rozhraní pro správu paměti.

### Získání seznamu pamětí

**GET** `/api/memory/list`

Vrací seznam položek paměti Křemíkových Bytostí.

### Získání detailů paměti

**GET** `/api/memory/detail/{id}`

Parametr cesty: `id` — ID položky paměti

Vrací kompletní obsah zadané položky paměti.

### Získání statistik paměti

**GET** `/api/memory/stats`

Vrací statistické informace paměťového systému.

### Vyhledávání v paměti

**GET** `/api/memory/search`

Parametr dotazu: `keyword` — klíčové slovo pro vyhledávání

Vyhledává odpovídající položky paměti.

### Získání pamětí seskupených podle bytostí

**GET** `/api/memory/beings`

Statistiky pamětí seskupené podle Křemíkových Bytostí.

### Získání sledování paměti

**GET** `/api/memory/trace/{id}`

Parametr cesty: `id` — ID položky paměti

Vrací řetězec sledování původu zadané položky paměti.

### Získání HTML časové osy paměti

**GET** `/api/memory/timeline-html`

Vrací HTML zobrazení časové osy paměti.

---

## Pracovní poznámky

### Stránka pracovních poznámek

**GET** `/work-notes`

Vrací stránku rozhraní pracovních poznámek.

### Získání seznamu pracovních poznámek

**GET** `/api/work-notes/list`

Vrací seznam pracovních poznámek.

### Čtení pracovní poznámky

**GET** `/api/work-notes/read`

Parametr dotazu: `noteId` — ID poznámky

Vrací obsah zadané poznámky.

### Získání adresáře poznámek

**GET** `/api/work-notes/directory`

Vrací adresářovou strukturu poznámek.

### Vyhledávání v pracovních poznámkách

**GET** `/api/work-notes/search`

Parametr dotazu: `keyword` — klíčové slovo pro vyhledávání

Vyhledává odpovídající pracovní poznámky.

### Vytvoření pracovní poznámky

**POST** `/api/work-notes/create`

**Tělo požadavku**:
```json
{
  "title": "Název poznámky",
  "content": "Obsah poznámky",
  "keywords": ["klíčové slovo 1", "klíčové slovo 2"]
}
```

### Aktualizace pracovní poznámky

**POST** `/api/work-notes/update`

**Tělo požadavku**:
```json
{
  "noteId": "note-uuid",
  "title": "Aktualizovaný název",
  "content": "Aktualizovaný obsah"
}
```

### Smazání pracovní poznámky

**POST** `/api/work-notes/delete`

**Tělo požadavku**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Znalostní síť

### Stránka znalostní sítě

**GET** `/knowledge`

Vrací stránku rozhraní pro správu znalostní sítě.

### Získání znalostního grafu

**GET** `/api/knowledge/graph`

Vrací data grafu znalostních trojic (subjekt-relace-objekt).

---

## Správa projektů

### Stránka projektů

**GET** `/project`

Vrací stránku rozhraní pro správu projektů.

### Stránka projektových pracovních poznámek

**GET** `/project/{id}/work-notes`

Parametr cesty: `id` — ID projektu

Vrací stránku pracovních poznámek zadaného projektu.

### Stránka projektových úkolů

**GET** `/project/{id}/tasks`

Parametr cesty: `id` — ID projektu

Vrací stránku správy úkolů zadaného projektu.

### Stránka projektových oprávnění nástrojů

**GET** `/project/{id}/tool-permissions`

Parametr cesty: `id` — ID projektu

Vrací stránku správy oprávnění nástrojů zadaného projektu.

### Stránka projektových pracovních postupů

**GET** `/project/{id}/workflow`

Parametr cesty: `id` — ID projektu

Vrací stránku správy pracovních postupů zadaného projektu.

### Získání detailů projektového pracovního postupu

**GET** `/api/projects/workflow-detail`

Parametr dotazu: `projectId` — ID projektu

Vrací detaily pracovního postupu přidruženého k projektu.

### Přiřazení projektové role

**POST** `/api/projects/assign-role`

**Tělo požadavku**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Odebrání projektové role

**POST** `/api/projects/remove-role`

**Tělo požadavku**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Získání seznamu projektů

**GET** `/api/projects/list`

Vrací seznam všech projektů.

### Získání seznamu šablon projektových pracovních postupů

**GET** `/api/projects/list-workflow-templates`

Vrací seznam dostupných šablon pracovních postupů.

### Vytvoření projektu

**POST** `/api/projects/create`

**Tělo požadavku**:
```json
{
  "name": "Můj projekt",
  "description": "Popis projektu"
}
```

### Archivace projektu

**POST** `/api/projects/{id}/archive`

Parametr cesty: `id` — ID projektu

Archivuje zadaný projekt.

### Obnovení projektu

**POST** `/api/projects/{id}/restore`

Parametr cesty: `id` — ID projektu

Obnovuje archivovaný projekt.

### Zničení projektu

**POST** `/api/projects/{id}/destroy`

Parametr cesty: `id` — ID projektu

Trvale odstraňuje zadaný projekt (není obnovitelné).

### Získání detailů projektu

**GET** `/api/projects/detail`

Parametr dotazu: `projectId` — ID projektu

Vrací detailní informace o projektu.

### Aktualizace projektu

**POST** `/api/projects/update`

**Tělo požadavku**:
```json
{
  "projectId": "project-uuid",
  "name": "Aktualizovaný název",
  "description": "Aktualizovaný popis"
}
```

### Přiřazení člena do projektu

**POST** `/api/projects/assign`

**Tělo požadavku**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Odebrání člena z projektu

**POST** `/api/projects/remove`

**Tělo požadavku**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Získání seznamu projektových pracovních poznámek

**GET** `/api/projects/{id}/work-notes/list`

Parametr cesty: `id` — ID projektu

Vrací seznam pracovních poznámek zadaného projektu.

### Čtení projektové pracovní poznámky

**GET** `/api/projects/{id}/work-notes/read`

Parametr cesty: `id` — ID projektu

Vrací obsah pracovních poznámek zadaného projektu.

### Vytvoření projektové pracovní poznámky

**POST** `/api/projects/{id}/work-notes/create`

Parametr cesty: `id` — ID projektu

Vytváří novou pracovní poznámku v zadaném projektu.

### Aktualizace projektové pracovní poznámky

**POST** `/api/projects/{id}/work-notes/update`

Parametr cesty: `id` — ID projektu

Aktualizuje pracovní poznámku v zadaném projektu.

### Smazání projektové pracovní poznámky

**POST** `/api/projects/{id}/work-notes/delete`

Parametr cesty: `id` — ID projektu

Maže pracovní poznámku v zadaném projektu.

### Získání seznamu projektových úkolů

**GET** `/api/projects/{id}/tasks/list`

Parametr cesty: `id` — ID projektu

Vrací seznam úkolů zadaného projektu.

### Vytvoření projektového úkolu

**POST** `/api/projects/{id}/tasks/create`

Parametr cesty: `id` — ID projektu

Vytváří nový úkol v zadaném projektu.

### Aktualizace projektového úkolu

**POST** `/api/projects/{id}/tasks/update`

Parametr cesty: `id` — ID projektu

Aktualizuje úkol v zadaném projektu.

### Smazání projektového úkolu

**POST** `/api/projects/{id}/tasks/delete`

Parametr cesty: `id` — ID projektu

Maže úkol v zadaném projektu.

### Přiřazení zodpovědné osoby k úkolu

**POST** `/api/projects/{id}/tasks/assign`

Parametr cesty: `id` — ID projektu

Přiřazuje zodpovědnou osobu k projektovému úkolu.

### Odebrání zodpovědné osoby z úkolu

**POST** `/api/projects/{id}/tasks/remove-assignee`

Parametr cesty: `id` — ID projektu

Odebírá zodpovědnou osobu z projektového úkolu.

### Označení úkolu jako dokončeného

**POST** `/api/projects/{id}/tasks/complete`

Parametr cesty: `id` — ID projektu

Označuje projektový úkol jako dokončený.

### Označení úkolu jako selhaného

**POST** `/api/projects/{id}/tasks/fail`

Parametr cesty: `id` — ID projektu

Označuje projektový úkol jako selhaný.

### Zrušení úkolu

**POST** `/api/projects/{id}/tasks/cancel`

Parametr cesty: `id` — ID projektu

Ruší projektový úkol.

---

## Správa oprávnění nástrojů

### Získání oprávnění nástrojů Křemíkové Bytosti

**GET** `/api/beings/tool-permissions`

Parametr dotazu: `beingId` — ID Křemíkové Bytosti

Vrací konfiguraci oprávnění nástrojů zadané Křemíkové Bytosti.

### Aktualizace oprávnění nástrojů Křemíkové Bytosti

**PUT** `/api/beings/tool-permissions`

**Tělo požadavku**:
```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

### Získání šablon oprávnění nástrojů

**GET** `/api/beings/tool-permissions/templates`

Vrací seznam dostupných šablon oprávnění nástrojů.

### Aplikace šablony oprávnění nástrojů

**POST** `/api/beings/tool-permissions/apply-template`

**Tělo požadavku**:
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### Získání projektových oprávnění nástrojů

**GET** `/api/projects/{id}/tool-permissions`

Parametr cesty: `id` — ID projektu

Vrací konfiguraci oprávnění nástrojů zadaného projektu.

### Aktualizace projektových oprávnění nástrojů

**PUT** `/api/projects/{id}/tool-permissions`

Parametr cesty: `id` — ID projektu

**Tělo požadavku**:
```json
{
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

---

## Správa exekutorů

### Stránka exekutorů

**GET** `/executor`

Vrací stránku rozhraní pro správu exekutorů.

### Získání stavu exekutorů

**GET** `/api/executors/status`

Vrací stav běhu jednotlivých exekutorů (disk, síť, příkazový řádek).

---

## Prohlížeč kódu

### Stránka prohlížeče kódu

**GET** `/code`

Vrací stránku rozhraní prohlížeče kódu.

### Získání seznamu typů kódu

**GET** `/api/code/types`

Vrací seznam podporovaných typů/jazyků kódu.

### Získání detailů kódu

**GET** `/api/code/detail`

Parametry dotazu: `filePath`, `lineNumber`

Vrací detaily kódu zadaného souboru.

---

## Plovoucí tipy kódu

### Získání plovoucího tipu

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Získání informací o plovoucím tipu pro umístění v kódu (podobné inteligentním tipům v IDE).

### Registrace umístění v kódu

**POST** `/api/code/register`

Registruje umístění v kódu pro sledování.

### Aktualizace umístění v kódu

**POST** `/api/code/update`

Aktualizuje informace o registrovaném umístění v kódu.

### Zrušení registrace umístění v kódu

**POST** `/api/code/unregister`

Zruší registraci sledování umístění v kódu, které již není potřeba.

---

## Systém dokumentace nápovědy

### Stránka nápovědy

**GET** `/help` nebo **GET** `/help/index`

Vrací hlavní stránku dokumentace nápovědy.

### Stránka tématu nápovědy

**GET** `/help/{topic}`

Parametr cesty: `topic` — identifikátor tématu

Vrací stránku dokumentace nápovědy pro zadané téma.

### Vyhledávání v dokumentaci nápovědy

**GET** `/api/help/search`

Parametr dotazu: `keyword` — klíčové slovo pro vyhledávání

Vyhledává odpovídající témata dokumentace nápovědy.

---

## Inicializace

### Stránka průvodce inicializací

**GET** `/init`

Vrací stránku průvodce inicializací při prvním spuštění.

### Odeslání inicializace

**POST** `/init`

Odesílá konfiguraci inicializace při prvním spuštění.

### Procházení a výběr datového adresáře

**GET** `/init/browse`

Otevírá prohlížeč adresářů pro výběr umístění datového úložiště.

### Získání metadat AI konfigurace

**GET** `/init/ai-config-metadata`

Vrací dostupné typy AI klientů a metadata jejich konfiguračních polí.

---

## Řízení systému

### Elegantní vypnutí

**POST** `/api/system/shutdown`

> **Poznámka**: Povoleno pouze požadavky z localhost

Spouští proces elegantního vypnutí aplikace:

1. Zastavení Hlavní Smyčky (MainLoop)
2. Uložení aktuální konfigurace
3. Zavření HTTP naslouchávače

**Odpověď**:
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## O projektu

### Stránka o projektu

**GET** `/about`

Vrací stránku o projektu, obsahující systémové informace a seznam načtených zásuvných modulů.

**Data seznamu zásuvných modulů**:
```json
{
  "plugins": {
    "plugin-id": {
      "name": "My Plugin",
      "version": "1.0.0",
      "description": "Plugin description",
      "author": "Author Name"
    }
  }
}
```

---

## Chybové odpovědi

Všechny koncové body vrací standardizované chybové odpovědi:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
  }
}
```

### Běžné chybové kódy

| Kód | HTTP stav | Popis |
|------|-------------|------|
| `PERMISSION_DENIED` | 403 | Nedostatečná oprávnění |
| `NOT_FOUND` | 404 | Prostředek nenalezen |
| `VALIDATION_ERROR` | 400 | Neplatné parametry požadavku |
| `INTERNAL_ERROR` | 500 | Interní chyba serveru |
| `SERVICE_UNAVAILABLE` | 503 | AI služba nedostupná |

---

## SSE události

Server-Sent Events se používají pro aktualizace v reálném čase:

### Chatovací události

```javascript
const eventSource = new EventSource('/api/chat/stream');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Tool executing:', data.tool);
      break;
    case 'complete':
      console.log('Chat complete, session:', data.sessionId);
      break;
    case 'error':
      console.error('Error:', data.message);
      break;
  }
};
```

---

## Rozhraní AI klienta

### Rozhraní IAIClient

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### Struktura AIRequest

```csharp
public class AIRequest
{
    public List<Message> Messages { get; set; }
    public List<ToolDefinition> Tools { get; set; }
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2000;
    public string Model { get; set; }
}
```

### Struktura AIResponse

```csharp
public class AIResponse
{
    public string Content { get; set; }
    public List<ToolCall> ToolCalls { get; set; }
    public TokenUsage Usage { get; set; }
    public string Model { get; set; }
}
```

---

## Rozhraní systému nástrojů

### Rozhraní ITool

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### Struktura ToolCall

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### Struktura ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Další kroky

- 🚀 Prohlédněte [příručku rychlého startu](getting-started.md)
- 🛠️ Přečtěte [vývojářskou příručku](development-guide.md)
- 📚 Prohlédněte [dokumentaci architektury](architecture.md)
- 🔒 Přečtěte o [bezpečnostním modelu](security.md)
