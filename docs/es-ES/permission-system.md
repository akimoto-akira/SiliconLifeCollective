# Sistema de Permisos

> **Versión: v0.1.0-alpha**

[English](../en/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | **Español** | [Deutsch](../de-DE/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md)

## Resumen

El sistema de permisos asegura que todas las operaciones iniciadas por IA sean apropiadamente verificadas y auditadas.

## Cadena de Permisos de 5 Niveles

```
┌─────────────────────────────────────────────┐
│          Verificación de Permisos            │
├─────────────────────────────────────────────┤
│  Nivel 1: IsCurator                          │
│  ↓ Omitir si es verdadero                   │
│  Nivel 2: UserFrequencyCache                 │
│  ↓ Límite de velocidad                       │
│  Nivel 3: GlobalACL                          │
│  ↓ Lista de control de acceso                │
│  Nivel 4: IPermissionCallback                │
│  ↓ Lógica personalizada                      │
│  Nivel 5: IPermissionAskHandler              │
│  ↓ Preguntar al usuario                      │
│  Resultado: Permitido o Denegado             │
└─────────────────────────────────────────────┘
```

## Nivel 1: IsCurator

Los administradores/curadores omiten todas las verificaciones de permisos.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## Nivel 2: UserFrequencyCache

Límites de velocidad por usuario para prevenir abuso.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## Nivel 3: GlobalACL

La lista de control de acceso global define reglas explícitas.

### Estructura de ACL

```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### Formato de Recurso

```
{tipo}:{acción}

Ejemplos:
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## Nivel 4: IPermissionCallback

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
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // Lógica personalizada
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## Nivel 5: IPermissionAskHandler

Preguntar al usuario por permisos cuando todos los otros niveles son indecisos.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // Enviar mensaje al usuario a través de mensajería instantánea
        await SendMessageAsync($"Allow {request.Resource}?");
        
        // Esperar respuesta del usuario
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

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
1. **Caché de frecuencia** - Verificar decisiones de usuario en caché
2. **IPermissionCallback** - Evaluación de callback personalizado
3. **Estado de curador** - Si es curador, devolver `AskUser` (requiere confirmación)
4. **ACL global** - Verificar reglas de control de acceso
5. **Predeterminado** - Denegar cuando no hay reglas coincidentes

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
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // Permisos basados en tiempo
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // Permisos basados en recurso
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }
    
    return PermissionResult.Allowed();
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
