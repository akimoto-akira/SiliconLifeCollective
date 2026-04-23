# Diseño de Seguridad

[English](../en/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Español](../es-ES/security.md)

## Resumen

La seguridad en Silicon Life Collective se basa en un modelo de **defensa en capas**. El principio fundamental: **todas las operaciones de E/S deben pasar por ejecutores**, y los ejecutores aplican verificaciones de permisos antes de la ejecución.

```
Tool Call → Ejecutor → PermissionManager → Caché HighDeny → Caché HighAllow → Callback → AskUser
```

---

## Modelo de Permisos

### Tipos de Permisos

| Tipo | Descripción |
|------|-------------|
| `NetworkAccess` | Solicitudes HTTP/HTTPS salientes |
| `CommandLine` | Ejecución de comandos de shell |
| `FileAccess` | Operaciones de archivos y directorios |
| `Function` | Llamadas a funciones sensibles |
| `DataAccess` | Acceso a datos del sistema o usuario |

### Resultados de Permisos

Cada verificación de permisos devuelve uno de tres resultados:

| Resultado | Comportamiento |
|--------|----------|
| **Allowed** | La operación procede inmediatamente |
| **Denied** | La operación se bloquea, se registra en auditoría |
| **AskUser** | La operación se pausa, requiere confirmación del usuario |

### Rol Especial: Curador de Silicio

El Curador de Silicio tiene el nivel de privilegio más alto (`IsCurator = true`). Las verificaciones de permisos para el Curador se cortocircuitan a **Allowed** a menos que el usuario las anule explícitamente.

### Gestores de Permisos Privados

Cada Ser de Silicio tiene su propia instancia de **PermissionManager privado**. Los estados de permisos no se comparten entre seres.

---

## Flujo de Verificación de Permisos

La prioridad de consulta es: **1. User HighDeny → 2. User HighAllow → 3. Función Callback**

```
┌─────────────┐
│ Tool Call   │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Ejecutor   │────▶│ PermissionManager   │
│ (Disco/Red/ │     │ Privado (por ser)   │
│  CLI/...)   │     └────────┬────────────┘
└─────────────┘            │
                           ▼
                  ┌─────────────────┐
                  │ 1. ¿IsCurator?  │──Sí──▶ Allowed
                  └────────┬────────┘
                           │ No
                           ▼
                  ┌─────────────────┐
                  │ 2. User HighDeny│──Coincide──▶ Denied
                  │ (caché memoria) │
                  └────────┬────────┘
                           │ No coincide
                           ▼
                  ┌─────────────────┐
                  │ 3. User HighAllow│──Coincide──▶ Allowed
                  │ (caché memoria) │
                  └────────┬────────┘
                           │ No coincide
                           ▼
                  ┌─────────────────┐
                  │ 4. Función      │
                  │  Callback       │──▶ Allowed / Denied / AskUser
                  └─────────────────┘
```

**Punto clave**: El ejecutor solo ve un booleano (Allowed/Denied). El PermissionManager maneja internamente la decisión de tres vías (Allowed/Denied/AskUser) y resuelve AskUser antes de retornar al ejecutor.

---

## Ejecutores (Frontera de Seguridad)

Los ejecutores son el **único** camino para operaciones de E/S. Aplican:

### Hilo de Programación Independiente

Cada ejecutor posee un **hilo de programación independiente**:

- Los hilos están aislados entre ejecutores — el bloqueo del hilo de un ejecutor no afecta a otros.
- Cada ejecutor puede establecer límites de recursos independientes (CPU, memoria, etc.).
- Gestión de pool de hilos para hilos de ejecutores.

### Timeout y Circuit Breaker

- Timeout por solicitud (predeterminado: 30 segundos)
- Circuit breaker: 3 timeouts consecutivos → enfriamiento de 1 minuto
- Reintentos automáticos después del período de enfriamiento

---

## Seguridad de Compilación Dinámica

Los Seres de Silicio pueden generar y ejecutar código C# dinámicamente mediante Roslyn.

### Escáner de Seguridad

Antes de compilar, el código pasa por `SecurityScanner`:

- Detecta llamadas API peligrosas
- Bloquea acceso a ciertos namespaces
- Verifica patrones de código sospechosos
- Registra todo para auditoría

### Aislamiento

- Código compilado se carga en contexto aislado
- Sin acceso directo a tipos sensibles
- Todas las operaciones de E/S pasan por ejecutores
- Límites de recursos aplicados

---

## Registro de Auditoría

Todas las decisiones de permisos se registran:

- Timestamp
- Ser solicitante
- Tipo de permiso
- Decisión (Allowed/Denied/AskUser)
- Recurso solicitado
- Resultado

Los registros de auditoría son accesibles a través de la interfaz web y APIs.

---

## Mejores Prácticas de Seguridad

1. **Principio de Menor Privilegio**: Otorgar solo permisos necesarios
2. **Verificar Siempre**: Nunca confiar en entrada de IA sin verificación
3. **Registrar Todo**: Mantener registros de auditoría completos
4. **Monitorear Frecuencia**: Usar cachés de frecuencia para prevenir abuso
5. **Actualizar Regularmente**: Mantener dependencias actualizadas
