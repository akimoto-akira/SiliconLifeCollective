# Sistema de Permisos

## Resumen

El sistema de permisos de Silicon Life Collective proporciona control granular sobre las operaciones que los Seres de Silicio pueden realizar. Todas las operaciones de I/O iniciadas por IA deben pasar por una cadena de verificación de permisos de 5 niveles.

---

## Cadena de Verificación de Permisos

```
Tool Call → Ejecutor → PermissionManager → [IsCurator → FrequencyCache → GlobalACL → Callback → AskUser]
```

### Nivel 1: IsCurator

Si el solicitante es el Curador de Silicio, permitir automáticamente (a menos que se anule explícitamente).

```csharp
if (request.Being.IsCurator)
    return PermissionResult.Allowed;
```

### Nivel 2: UserFrequencyCache

Verificar límites de frecuencia para prevenir abuso:

- Máximo de solicitudes por minuto
- Máximo de solicitudes por hora
- Bloqueo temporal después de demasiados rechazos

### Nivel 3: GlobalACL

Verificar la Lista de Control de Acceso Global:

- Listas blancas de recursos permitidos
- Listas negras de recursos bloqueados
- Reglas por tipo de permiso

### Nivel 4: IPermissionCallback

Callback personalizable que los usuarios pueden implementar para lógica de permisos personalizada:

#### Implementación predeterminada de DefaultPermissionCallback

`DefaultPermissionCallback` proporciona reglas de permisos predeterminadas integrales, que incluyen:

##### Reglas de acceso a la red
- **Direcciones de bucle invertido**: Permitir localhost, 127.0.0.1, ::1
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
  - Sitios web gubernamentales: .gov, .go.jp, .go.kr
- **Lista negra de dominios**:
  - Sitios de suplantación de IA: chatgpt, openai, deepseek, etc. (dominios falsos)
  - Herramientas de IA maliciosas: wormgpt, darkgpt, fraudgpt, etc.
  - Dominios relacionados con granjas de contenido de IA y mercado negro

```csharp
public interface IPermissionCallback
{
    PermissionResult CheckPermission(PermissionRequest request);
}
```

### Nivel 5: IPermissionAskHandler

Si todos los niveles anteriores no pueden tomar una decisión, preguntar al usuario:

- Interfaz web muestra diálogo de solicitud de permiso
- Usuario puede permitir/denegar temporal o permanentemente
- La decisión se almacena en caché para solicitudes futuras

---

## Tipos de Permisos

| Tipo | Descripción | Ejemplo |
|------|-------------|---------|
| `NetworkAccess` | Solicitudes de red | HTTP GET/POST |
| `CommandLine` | Ejecución de comandos | `git status` |
| `FileAccess` | Operaciones de archivos | Leer/escribir archivos |
| `Function` | Llamadas a funciones | Funciones sensibles |
| `DataAccess` | Acceso a datos | Consultar base de datos |

---

## Resultados de Permisos

### Allowed

La operación procede inmediatamente.

```csharp
return new PermissionResult(PermissionStatus.Allowed);
```

### Denied

La operación se bloquea y se registra en auditoría.

```csharp
return new PermissionResult(PermissionStatus.Denied, "Acceso denegado: recurso restringido");
```

### AskUser

La operación se pausa hasta que el usuario responda.

```csharp
return new PermissionResult(PermissionStatus.AskUser, "¿Permitir acceso a este archivo?");
```

---

## Gestor de Permisos Privado

Cada Ser de Silicio tiene su propio `PermissionManager`:

- Estados de permisos aislados entre seres
- Cachés independientes
- Reglas de permisos específicas por ser
- Auditoría separada

---

## Ejecutores y Permisos

Todos los ejecutores verifican permisos antes de ejecutar:

### DiskExecutor

```csharp
public ExecutorResult Execute(ExecutorRequest request)
{
    var permission = PermissionManager.CheckPermission(
        PermissionType.FileAccess,
        request.Resource
    );
    
    if (!permission.IsAllowed)
        return ExecutorResult.Denied(permission.Reason);
    
    // Ejecutar operación de archivo
}
```

### NetworkExecutor

```csharp
public ExecutorResult Execute(ExecutorRequest request)
{
    var permission = PermissionManager.CheckPermission(
        PermissionType.NetworkAccess,
        request.Url
    );
    
    if (!permission.IsAllowed)
        return ExecutorResult.Denied(permission.Reason);
    
    // Ejecutar solicitud HTTP
}
```

---

## Registro de Auditoría

Todas las decisiones de permisos se registran:

```csharp
public class PermissionAuditLog
{
    public DateTime Timestamp { get; set; }
    public Guid BeingId { get; set; }
    public PermissionType Type { get; set; }
    public string Resource { get; set; }
    public PermissionResult Result { get; set; }
    public string Reason { get; set; }
}
```

Los registros son accesibles a través de:
- Interfaz web (Controlador de Auditoría)
- API (`/api/audit/permissions`)
- Archivos de registro

---

## Configuración

### config.json

```json
{
  "PermissionSystem": {
    "EnableAuditing": true,
    "MaxRequestsPerMinute": 60,
    "MaxRequestsPerHour": 1000,
    "CooldownMinutes": 1,
    "MaxConsecutiveDenials": 3
  }
}
```

---

## Mejores Prácticas

1. **Principio de Menor Privilegio**: Solo otorgar permisos necesarios
2. **Monitorear Auditoría**: Revisar regularmente registros de permisos
3. **Actualizar ACL**: Mantener listas de control de acceso actualizadas
4. **Configurar Límites**: Establecer límites de frecuencia apropiados
5. **Probar Permisos**: Verificar configuraciones de permisos en entorno de prueba

---

## Solución de Problemas

### Permisos Denegados Inesperadamente

1. Verificar logs de auditoría
2. Revisar configuración de GlobalACL
3. Verificar cachés de frecuencia
4. Comprobar reglas de callback

### Solicitudes de Permiso Frecuentes

1. Agregar recursos a lista blanca
2. Ajustar límites de frecuencia
3. Implementar callback personalizado

### Curador Sin Acceso

1. Verificar flag `IsCurator`
2. Comprobar anulaciones explícitas
3. Revisar logs de auditoría
