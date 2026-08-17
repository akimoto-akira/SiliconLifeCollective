# Sistema de Permisos

> **Versión: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | **Español** | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Resumen

El sistema de permisos asegura que todas las operaciones iniciadas por IA sean verificadas y auditadas apropiadamente.

## Cadena de Verificación de Permisos

```
┌─────────────────────────────────────────────┐
│          Verificación de Permisos            │
├─────────────────────────────────────────────┤
│  Nivel 1: UserFrequencyCache                 │
│  ↓ Caché de decisiones frecuentes del usuario│
│    (HighDeny/HighAllow)                      │
│  Nivel 2: IPermissionCallback                │
│  ↓ Lógica personalizada (Allowed/Denied/     │
│    AskUser)                                  │
│  Nivel 3: IsCurator?                         │
│  ↓ Sí → IPermissionAskHandler (preguntar     │
│    al usuario)                               │
│  ↓ No → GlobalACL → denegación por defecto   │
│  Resultado: Permitir o Denegar               │
└─────────────────────────────────────────────┘
```

> **Nota**: La prioridad de consulta real de `PermissionManager.CheckPermission()` es:
> 1. **UserFrequencyCache** — Verifica primero la caché de decisiones frecuentes del usuario
> 2. **IPermissionCallback** — Evalúa las reglas de retrollamada personalizadas
> 3. **Ramificación del Curador** — Cuando la retrollamada devuelve AskUser o no hay retrollamada:
>    - **Curador** → `IPermissionAskHandler` (pregunta al usuario a través de IM)
>    - **No-curador** → `GlobalACL` → denegación por defecto

## Nivel 1: UserFrequencyCache

Caché de decisiones frecuentes del usuario (HighDeny/HighAllow) por cada ser, solo en memoria.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny tiene prioridad sobre HighAllow**
- **Solo en memoria**: La caché no se persiste, se pierde al reiniciar
- **Expiración configurable**: El usuario puede establecer el período de validez de las entradas de la caché

## Nivel 2: IPermissionCallback

Retrollamada personalizada para lógica de permisos dinámica.

### Implementación por defecto DefaultPermissionCallback

`DefaultPermissionCallback` proporciona reglas de permisos por defecto completas, incluyendo:

#### Reglas de acceso a red
- **Direcciones de bucle invertido**: Permite localhost, 127.0.0.1, ::1
- **Direcciones IP privadas**:
  - 192.168.x.x (Clase C) - Permitido
  - 10.x.x.x (Clase A) - Permitido
  - 172.16-31.x.x (Clase B) - Preguntar al usuario
- **Lista blanca de dominios**:
  - Motores de búsqueda: Google, Bing, DuckDuckGo, Yandex, Sogou, etc.
  - Servicios de IA: OpenAI, Anthropic, HuggingFace, Ollama, etc.
  - Servicios para desarrolladores: GitHub, StackOverflow, npm, NuGet, etc.
  - Redes sociales: Weibo, Zhihu, Reddit, Discord, etc.
  - Plataformas de video: YouTube, Bilibili, Douyin, TikTok, etc.
  - **Información meteorológica**: wttr.in
  - Sitios gubernamentales: .gov, .go.jp, .go.kr
- **Lista negra de dominios**:
  - Sitios de suplantación de IA: chatgpt, openai, deepseek y otros dominios de imitación
  - Herramientas de IA maliciosas: wormgpt, darkgpt, fraudgpt, etc.
  - Granjas de contenido de IA y dominios relacionados con el mercado negro

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Nivel 3: Ramificación (IsCurator / GlobalACL)

Cuando la retrollamada devuelve `AskUser` o no hay retrollamada configurada, el sistema se ramifica según la identidad del Curador:

### Rama del Curador (IsCurator = true)

Para el Curador de Silicio, el sistema solicita la decisión del usuario a través de mensajería instantánea:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // El usuario confirma o deniega en la Web UI
    }
}
```

### Rama de no-curador (IsCurator = false)

Para seres que no son el Curador, el sistema verifica la lista de control de acceso global. Si no hay reglas coincidentes, la solicitud se deniega por defecto.

### Estructura de GlobalACL

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

Las reglas se evalúan en orden; la primera coincidencia gana. Solo el Curador de Silicio puede modificar la ACL Global.

### Formato de Recursos

```
{tipo}:{ruta}

Ejemplos:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Cuando una operación del Curador requiere confirmación del usuario, se solicita el permiso a través de `IPermissionAskHandler`.

### Implementación IMPermissionAskHandler

`IMPermissionAskHandler` envía solicitudes de permiso al usuario a través de la Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Envía un mensaje al usuario a través de mensajería instantánea
        SendMessageAsync($"¿Permitir {resource}?");

        // Espera la respuesta del usuario
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Cola de Solicitudes de Permiso PermissionRequestQueue

`PermissionRequestQueue` gestiona las solicitudes de permisos pendientes, soportando espera asíncrona de la respuesta del usuario:

- **Encolado de solicitudes** — Cuando la cadena de permisos llega al nivel 5, se crea un `TaskCompletionSource<AskPermissionResult>` y se encola
- **Visualización en Web UI** — Las solicitudes de permisos pendientes se muestran en la Web UI a través de `PermissionRequestController`
- **Respuesta del usuario** — El usuario aprueba o deniega en la Web UI, con opción de almacenar en caché la decisión y establecer la duración de la caché
- **Opciones de caché** — El usuario puede almacenar en caché la decisión de permisos por 1 hora, 24 horas, 7 días o 30 días
- **Mecanismo de tiempo de espera** — Sin respuesta en 60 segundos, la página de solicitud se cierra automáticamente

## Sistema de Auditoría

Todas las decisiones de permisos se registran:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## Evaluación Programática de Permisos

### API EvaluatePermission

El método `PermissionManager.EvaluatePermission()` proporciona una pre-evaluación de permisos de solo lectura que no activa solicitudes al usuario. `PermissionTool` usa este método para que la IA verifique el estado de los permisos antes de intentar una operación.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Valor de retorno**: `PermissionResult` de tres estados:
- `Allowed` - La operación está permitida
- `Denied` - La operación está denegada
- `AskUser` - Se requiere confirmación del usuario en el momento de la ejecución

**Orden de evaluación**:
1. **Caché de frecuencia** - Verifica las decisiones del usuario en caché
2. **IPermissionCallback** - Evaluación de la retrollamada personalizada
3. **Estado del Curador** - Si es el Curador, devuelve `AskUser` (requiere confirmación)
4. **ACL Global** - Verifica las reglas de control de acceso
5. **Por defecto** - Deniega cuando no hay reglas coincidentes

> **Nota**: A diferencia de la cadena de permisos completa, `EvaluatePermission` **no** llama a `IPermissionAskHandler`. Solo informa cuál *será* el resultado en el momento de la ejecución.

## Gestión de Permisos

### Otorgar Permisos

**A través de la Web UI**:
1. Navegar a **Gestión de Permisos**
2. Hacer clic en **Agregar Regla**
3. Configurar:
   - Usuario
   - Recurso
   - Permitir/Denegar
   - Duración

**A través de la API**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### Revocar Permisos

Operar a través de la página de gestión de permisos en la Web UI.

### Ver Permisos

```bash
curl http://localhost:8080/api/permissions/list
```

## Sistema de Permisos de Herramientas

Además de la cadena de verificación de permisos a nivel de operaciones, el sistema proporciona un mecanismo de gestión de **permisos de herramientas** para controlar qué herramientas pueden usar los Seres de Silicio.

### Permisos de Herramientas de Dos Niveles

Los permisos de herramientas se dividen en dos niveles:

1. **Nivel del Ser de Silicio** — Controla qué operaciones de herramientas puede usar un Ser de Silicio individual
2. **Nivel de proyecto** — Controla las operaciones de herramientas disponibles dentro del espacio del proyecto, independiente de los permisos a nivel del Ser de Silicio

### Configuración de Permisos de Herramientas

Cada operación de cada herramienta puede configurarse independientemente como permitida o denegada:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### Plantillas de Permisos

El sistema proporciona plantillas de permisos de herramientas predefinidas que se pueden aplicar rápidamente a los Seres de Silicio:

- **readonly** — Permisos de solo lectura (permite operaciones de lectura, deniega operaciones de escritura)
- **full** — Permisos completos (permite todas las operaciones)
- **restricted** — Permisos restringidos (solo permite operaciones básicas)

### Gestión en Web UI

Gestionar permisos de herramientas a través de la Web UI:

- **Página de permisos de herramientas del Ser de Silicio** — `/beings/tool-permissions`
- **Página de permisos de herramientas del proyecto** — `/project/{id}/tool-permissions`

### Endpoints de API

| Endpoint | Método | Descripción |
|------|------|------|
| `/api/beings/tool-permissions` | GET | Obtener permisos de herramientas del Ser de Silicio |
| `/api/beings/tool-permissions` | PUT | Actualizar permisos de herramientas del Ser de Silicio |
| `/api/beings/tool-permissions/templates` | GET | Obtener lista de plantillas de permisos |
| `/api/beings/tool-permissions/apply-template` | POST | Aplicar plantilla de permisos |
| `/api/projects/{id}/tool-permissions` | GET | Obtener permisos de herramientas del proyecto |
| `/api/projects/{id}/tool-permissions` | PUT | Actualizar permisos de herramientas del proyecto |

### Permisos de acción de las habilidades

Las habilidades reutilizan el mecanismo de permisos de acción de herramientas: el id de la habilidad sirve como nombre de herramienta, con la acción `execute`.

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "daily_news_digest:execute": "denied",
    "code_review:execute": "allowed"
  }
}
```

- Las habilidades deshabilitadas no aparecen en las definiciones de herramientas visibles para la IA (la IA no las "ve")
- La ejecución de la habilidad también tiene una verificación en tiempo de ejecución, incluso un esquema obsoleto no puede eludirla
- Los permisos de las herramientas internas de la habilidad = permisos del ser ∪ restricciones de la propia habilidad (unión del lado restrictivo, solo puede restringir, no ampliar)

### Permisos de acción de las herramientas wrapper MCP

Cada herramienta wrapper inyectada por un servidor MCP (`mcp_{serverId}_{toolName}`) declara automáticamente una única acción `execute`:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "mcp_filesystem_read_file:execute": "denied",
    "mcp_github_create_issue:execute": "allowed"
  }
}
```

- La disponibilidad de herramientas externas se puede controlar con precisión por ser o proyecto
- Cuando todas las acciones `execute` de un servidor están deshabilitadas, la herramienta se elimina por completo del esquema visible para la IA
- La deshabilitación/eliminación de un servidor (vía Web UI) revoca inmediatamente todas sus herramientas

---

## Mejores Prácticas

### 1. Principio de Mínimo Privilegio

Otorgar solo los permisos mínimos necesarios:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Usar Permisos con Límite de Tiempo

Nunca otorgar permisos permanentes a menos que sea absolutamente necesario.

### 3. Monitorear los Registros de Permisos

Revisar periódicamente los registros de auditoría para entender:
- Intentos de acceso denegados
- Patrones inusuales
- Escalada de permisos

### 4. Implementar Retrollamadas Personalizadas

Para lógica compleja, usar `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Permisos basados en tiempo
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Permisos basados en recursos
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Escenarios Comunes

### Escenario 1: La IA quiere leer un archivo

```
IA: "Necesito leer config.json"
↓
Cadena de permisos:
1. UserFrequencyCache? Sin decisión en caché
2. IPermissionCallback? Devuelve AskUser (no explícitamente permitido)
3. IsCurator? No → Verificar GlobalACL
4. GlobalACL? Regla encontrada: file:... = Allowed
5. Resultado: Permitido
```

### Escenario 2: La IA quiere ejecutar código

```
IA: "Quiero compilar y ejecutar código"
↓
Cadena de permisos:
1. UserFrequencyCache? Sin decisión en caché
2. IPermissionCallback? Devuelve AskUser
3. IsCurator? Sí → IPermissionAskHandler
4. El usuario aprueba
5. Resultado: Permitido
```

### Escenario 3: Denegación en caché

```
IA: "Necesito acceder a C:\Windows"
↓
Cadena de permisos:
1. UserFrequencyCache? Encontrado en caché HighDeny
2. Resultado: Denegado (sin más verificaciones)
```

## Solución de Problemas

### Permisos denegados inesperadamente

**Verificar**:
1. Estado IsCurator del usuario
2. Entradas HighDeny en la caché de frecuencia
3. Reglas de GlobalACL
4. Lógica de retrollamada
5. Tiempo de espera agotado de la respuesta del usuario

### Los permisos no expiran

**Verificar**:
- El campo `expiresAt` está configurado correctamente
- La zona horaria es correcta
- El reloj está sincronizado

### Los registros de auditoría no se registran

**Verificar**:
- El registrador de auditoría está registrado
- El backend de almacenamiento es accesible
- Hay espacio suficiente en disco

## Siguientes Pasos

- 📚 Leer la [guía de arquitectura](architecture.md)
- 🛠️ Consultar la [guía de desarrollo](development-guide.md)
- 🔒 Consultar la [documentación de seguridad](security.md)
- 🚀 Ver la [guía de inicio rápido](getting-started.md)
