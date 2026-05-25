# Sistema de Permisos

> **Versión: v0.2.0-alpha**

[English](../en/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | **Español** | [Deutsch](../de-DE/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Resumen

El sistema de permisos asegura que todas las operaciones iniciadas por IA sean apropiadamente verificadas y auditadas.

## Cadena de Permisos de 3 Niveles

```
┌─────────────────────────────────────────────┐
│          Verificación de Permisos            │
├─────────────────────────────────────────────┤
│  Nivel 1: UserFrequencyCache                 │
│  ↓ Decisiones de usuario en caché (HighDeny/HighAllow)│
│  Nivel 2: IPermissionCallback                │
│  ↓ Lógica personalizada (Permitido/Denegado/PreguntarUsuario)│
│  Nivel 3: IsCurator?                         │
│  ↓ Sí → IPermissionAskHandler (preguntar al usuario)│
│  ↓ No → GlobalACL → Denegación predeterminada │
│  Resultado: Permitido o Denegado             │
└─────────────────────────────────────────────┘
```

> **Nota**: La prioridad de consulta real en `PermissionManager.CheckPermission()` es:
> 1. **UserFrequencyCache** — Verificar primero las decisiones de usuario de alta frecuencia en caché
> 2. **IPermissionCallback** — Evaluar reglas de callback personalizadas
> 3. **Bifurcación de curador** — Si el callback devuelve AskUser o no hay callback configurado:
>    - **Curador** → `IPermissionAskHandler` (preguntar al usuario vía IM)
>    - **No-curador** → `GlobalACL` → denegación predeterminada

## Nivel 1: UserFrequencyCache

Caché por ser, solo en memoria, de decisiones de usuario de alta frecuencia (HighDeny/HighAllow).

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny** tiene prioridad sobre **HighAllow**
- **Solo memoria**: Los cachés no se persisten, se pierden al reiniciar
- **Expiración configurable**: Los usuarios pueden establecer período de validez para entradas de caché

## Nivel 2: IPermissionCallback

Callbacks personalizados para lógica de permisos dinámica.

### Implementación Predeterminada DefaultPermissionCallback

`DefaultPermissionCallback` proporciona reglas de permisos predeterminadas integrales, incluyendo:

#### Reglas de Acceso a Red
- **Direcciones de bucle invertido**: Permitir localhost, 127.0.0.1, ::1
- **Direcciones IP privadas**:
  - 192.168.x.x (Clase C) - Permitir
  - 10.x.x.x (Clase A) - Permitir
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
  - Sitios de suplantación de IA: dominios falsos de chatgpt, openai, deepseek, etc.
  - Herramientas de IA maliciosas: wormgpt, darkgpt, fraudgpt, etc.
  - Dominios relacionados con granjas de contenido de IA y mercados negros

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

## Nivel 3: Bifurcación de Curador (IsCurator → AskHandler / GlobalACL)

Cuando el callback devuelve `AskUser` o no hay callback configurado, el sistema se bifurca según el estado de curador:

### Ruta de Curador → IPermissionAskHandler

Si el Being es un curador (`IsCurator = true`), se invoca `IPermissionAskHandler` para preguntar interactivamente al usuario.

```csharp
if (being.IsCurator)
{
    return askHandler.AskUser(callerId, permissionType, resource);
}
```

### Ruta de No-Curador → GlobalACL → Denegación predeterminada

Los Beings normales consultan GlobalACL sincrónicamente sin bloqueo. Si no hay regla ACL coincidente, se deniega por defecto.

```csharp
var aclResult = globalACL.Check(request);
return aclResult ?? PermissionResult.Denied("No matching ACL rule");
```

## Global ACL (Lista de Control de Acceso)

Tabla de reglas compartida persistente en Storage, gestionada solo por el Silicon Curador.

### Estructura de ACL

```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

### Formato de Recurso

```
{tipo}:{ruta}

Ejemplos:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler (Ruta de Curador)

Invocado cuando el Being es un curador y IPermissionCallback devuelve AskUser.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"¿Permitir {resource}?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### Cola de Solicitudes de Permiso PermissionRequestQueue

`PermissionRequestQueue` gestiona las solicitudes de permiso pendientes, soportando espera asíncrona de respuesta del usuario:

- **Encolar solicitud** — Cuando se alcanza la ruta de curador, se crea un `TaskCompletionSource<AskPermissionResult>` y se encola
- **Visualización en Web UI** — Las solicitudes de permiso pendientes se muestran en la Web UI a través de `PermissionRequestController`
- **Respuesta del usuario** — El usuario aprueba o deniega en la Web UI, con opción de almacenar en caché la decisión y establecer la duración de la caché
- **Opciones de caché** — El usuario puede almacenar en caché la decisión de permisos por 1 hora, 24 horas, 7 días o 30 días
- **Mecanismo de timeout** — La página de solicitud se cierra automáticamente después de 30 minutos sin respuesta

## Sistema de Auditoría

Todas las decisiones de permisos se registran:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
}
```

## Evaluación Programática de Permisos

### API EvaluatePermission

El método `PermissionManager.EvaluatePermission()` proporciona pre-evaluación de permisos de solo lectura, sin activar prompts de usuario. `PermissionTool` usa este método para permitir que la IA verifique el estado de permisos antes de intentar una operación.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Valor de retorno**: `PermissionResult` de tres estados:
- `Allowed` - Operación permitida
- `Denied` - Operación denegada
- `AskUser` - Requiere confirmación de usuario al ejecutar

**Orden de evaluación**:
1. **Caché de frecuencia** — Verificar decisiones de usuario en caché
2. **IPermissionCallback** — Evaluación de callback personalizado
3. **Bifurcación de curador** — Si es curador, devuelve `AskUser` (requiere confirmación); si no es curador, verifica **GlobalACL**, luego denegación predeterminada

> **Nota**: A diferencia de la cadena completa de permisos, `EvaluatePermission` **no** llama a `IPermissionAskHandler`. Solo informa cuál *sería* el resultado al ejecutar.

## Gestionar Permisos

### Otorgar Permisos

**A través de Web UI**:
1. Navegar a **Gestión de Permisos**
2. Hacer clic en **Añadir Regla**
3. Configurar:
   - Usuario
   - Recurso
   - Permitir/Denegar
   - Duración

**A través de API**:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### Revocar Permisos

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### Ver Permisos

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## Mejores Prácticas

### 1. Principio de Mínimo Privilegio

Otorgar solo los permisos mínimos necesarios:

```json
{
  "resource": "disk:read",  // No disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // Siempre establecer expiración
}
```

### 2. Usar Permisos con Límite de Tiempo

Nunca otorgar permisos permanentes a menos que sea absolutamente necesario.

### 3. Monitorear Registros de Permisos

Revisar regularmente registros de auditoría para:
- Intentos de acceso denegados
- Patrones anómalos
- Elevación de permisos

### 4. Implementar Callbacks Personalizados

Para lógica compleja, usar `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Escenarios Comunes

### Escenario 1: IA Quiere Leer un Archivo

```
IA: "Necesito leer config.json"
↓
Cadena de permisos:
1. ¿IsCurator? No
2. ¿Límite de velocidad? Normal
3. ¿GlobalACL? Regla encontrada: disk:read = Permitido
4. Resultado: Permitido
```

### Escenario 2: IA Quiere Ejecutar Código

```
IA: "Quiero compilar y ejecutar código"
↓
Cadena de permisos:
1. ¿IsCurator? No
2. ¿Límite de velocidad? Normal
3. ¿GlobalACL? No se encontró regla
4. ¿Callback? Devuelve indeciso
5. ¿Preguntar al usuario? Usuario aprueba
6. Resultado: Permitido
```

### Escenario 3: Límite de Velocidad Excedido

```
IA: "Necesito hacer 100 solicitudes HTTP"
↓
Cadena de permisos:
1. ¿IsCurator? No
2. ¿Límite de velocidad? Ya excedido
3. Resultado: Denegado
```

## Solución de Problemas

### Permisos Denegados Inesperadamente

**Verificar**:
1. Estado de IsCurator del usuario
2. Configuración de límites de velocidad
3. Reglas de GlobalACL
4. Lógica de callback
5. Timeout de respuesta de usuario

### Permisos No Expiran

**Verificar**:
- Campo `expiresAt` configurado correctamente
- Zona horaria correcta
- Sincronización de reloj

### Registros de Auditoría No se Registran

**Verificar**:
- Registrador de auditoría registrado
- Backend de almacenamiento accesible
- Espacio en disco suficiente

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🔒 Ver la [Documentación de Seguridad](security.md)
- 🚀 Consultar la [Guía de Inicio Rápido](getting-started.md)
