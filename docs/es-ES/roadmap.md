# Hoja de Ruta

> **Versión: v0.1.0-alpha**

[English](../en/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | **Español** | [Deutsch](../de-DE/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md)

## Principios Rectores

Cada fase termina con un sistema **funcional y observable**. Ninguna fase produce "un montón de infraestructura sin nada que mostrar".

---

## ~~Fase 1: Puede Chatear~~ ✅ Completado

**Objetivo**: Entrada de consola → Llamada de IA → Salida de consola. Unidad mínima verificable.

| # | Módulo | Descripción |
|---|--------|-------------|
| 1.1 | Estructura de solución y proyectos | Crear `SiliconLifeCollective.sln`, con `src/SiliconLife.Core/` (biblioteca central) y `src/SiliconLife.Default/` (implementación predeterminada + punto de entrada) |
| 1.2 | Configuración (mínima) | Singleton + deserialización JSON. Leer `config.json`. Generar automáticamente valores predeterminados si falta |
| 1.3 | Localización (mínima) | Clase abstracta `LocalizationBase`, implementación `ZhCN`. Añadir `Language` en configuración |
| 1.4 | OllamaClient (mínimo) | Interfaz `IAIClient`, llamada HTTP a Ollama local `/api/chat`. Sin streaming, sin invocación de herramientas |
| 1.5 | E/S de consola | `while(true) + Console.ReadLine()`, leer entrada → llamar IA → imprimir respuesta |
| 1.6 | Encabezado de copyright | Añadir encabezado Apache 2.0 a todos los archivos fuente C# |

**Entregable**: Programa de chat de consola con modelo Ollama local.

**Verificación**: Ejecutar programa, escribir "hello", ver respuesta de IA.

---

## ~~Fase 2: Tiene Esqueleto~~ ✅ Completado

**Objetivo**: Reemplazar "bucle desnudo" con estructura de framework. Comportamiento sin cambios.

| # | Módulo | Descripción |
|---|--------|-------------|
| 2.1 | Almacenamiento (mínimo) | Interfaz `IStorage` (Read/Write/Exists/Delete, pares clave-valor). Implementación `FileSystemStorage`. Clase de instancia (no estática). Acceso directo al sistema de archivos — **IA no puede controlar IStorage** | [Deutsch](../de-DE/roadmap.md) |
| 2.2 | Bucle principal + objetos de reloj | Bucle infinito, intervalos de reloj precisos (`Stopwatch` + `Thread.Sleep`). Programación por prioridad |
| 2.3 | Estandarización de IAIClient | Interfaz `IAIClientFactory`. OllamaClient refactorizado para implementar interfaz estándar |
| 2.4 | Migración de consola | Migrar `while(true)` a objeto de reloj impulsado por bucle principal. Mismo comportamiento que Fase 1 |

**Entregable**: Bucle principal ejecutando reloj, chat de consola todavía funciona.

**Verificación**: Registrar objeto de reloj de prueba, imprimir conteo de reloj cada segundo; chat de consola todavía funciona.

---

## ~~Fase 3: Tiene Alma~~ ✅ Completado

**Objetivo**: Primer Ser Silicona vivo en el framework.

| # | Módulo | Descripción |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Clase base abstracta con Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. `Tick()` y `ExecuteOneRound()` abstractos |
| 3.2 | Carga de archivo de alma | `SoulFileManager`: leer `soul.md` desde directorio de datos del ser |
| 3.3 | ContextManager (mínimo) | Conectar archivo de alma + mensajes recientes → llamar IA → obtener respuesta. Sin invocación de herramientas, sin persistencia |
| 3.4 | ISiliconBeingFactory | Interfaz de fábrica para crear instancias de seres |
| 3.5 | SiliconBeingManager (mínimo) | Hereda objeto de reloj (prioridad=0). Iterar todos los seres, llamar sus Tick secuencialmente |
| 3.6 | DefaultSiliconBeing | Implementación de comportamiento estándar. Verificar mensajes no leídos → crear ContextManager → ExecuteOneRound → salida |
| 3.7 | Estructura de directorio de seres | `DataDirectory/SiliconManager/{GUID}/`, con `soul.md` y `state.json` |

**Entregable**: Ser Silicona impulsado por bucle principal, recibe entrada de consola, carga archivo de alma, llama IA.

**Verificación**: Entrada de consola → activación de reloj de bucle principal → procesamiento de ser (con comportamiento guiado por archivo de alma) → respuesta de IA. Estilo de respuesta debe ser diferente de Fase 1.

---

## ~~Fase 4: Tiene Memoria~~ ✅ Completado

**Objetivo**: Conversaciones persisten después de reinicio.

| # | Módulo | Descripción |
|---|--------|-------------|
| 4.1 | ChatSystem | Concepto de canal (dos GUID = un canal). Modelo de mensaje con persistencia. Sin chat grupal aún |
| 4.2 | IIMProvider + IMManager | Interfaz `IIMProvider`. `ConsoleProvider` como canal de mensajería instantánea formal. `IMManager` enruta mensajes |
| 4.3 | ContextManager mejorado | Extraer historial de sistema de chat. Persistir respuestas de IA. Soporte para continuación de invocación de herramientas de múltiples rondas |
| 4.4 | Modelo IMessage | Modelo de mensaje unificado compartido por sistema de chat y gestor de mensajería instantánea |

**Entregable**: Sistema de chat con memoria persistente.

**Verificación**: Chatear varias rondas → salir → reiniciar → preguntar "¿de qué hablamos?" → el ser puede responder.

---

## ~~Fase 5: Puede Actuar (Sistema de Herramientas)~~ ✅ Completado

**Objetivo**: Seres Silicona pueden ejecutar acciones, no solo chatear.

| # | Módulo | Descripción |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interfaz `ITool` con Name, Description, Execute. `ToolResult` con Success, Message, Data |
| 5.2 | ToolManager | Instancia por ser. Descubrimiento de herramientas basado en reflexión. Soporte de atributo `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Soporte de invocación de herramientas | Analizar tool_calls de IA. Bucle: ejecutar herramienta → enviar resultado de vuelta → IA continúa → hasta texto puro |
| 5.4 | Clase base de ejecutor | Clase base abstracta con hilo de programación independiente, cola de solicitudes, control de timeout |
| 5.5 | NetworkExecutor | Solicitudes HTTP a través de ejecutor. Timeout, cola |
| 5.6 | CommandLineExecutor | Ejecución de shell a través de ejecutor. Detección de separador multiplataforma |
| 5.7 | DiskExecutor | Operaciones de archivo a través de ejecutor. Sin verificación de permisos aún (Fase 6) |
| 5.8–5.12 | Herramientas integradas | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Entregable**: Seres Silicona pueden invocar herramientas para ejecutar acciones.

---

## ~~Fase 6: Seguridad (Sistema de Permisos)~~ ✅ Completado

**Objetivo**: Todas las operaciones de E/S pasan por verificación de permisos.

| # | Módulo | Descripción |
|---|--------|-------------|
| 6.1 | PermissionManager | Gestor de permisos privado por ser. Cadena de 5 niveles |
| 6.2 | GlobalACL | Lista de control de acceso global. Persistida al almacenamiento |
| 6.3 | UserFrequencyCache | Caché de frecuencia de usuario (solo memoria) para reducir prompts repetitivos |
| 6.4 | IPermissionCallback | Interfaz de callback para lógica de permisos personalizada |
| 6.5 | IPermissionAskHandler | Interfaz para preguntar al usuario por permisos |
| 6.6 | AuditLogger | Registro de auditoría para todas las decisiones de permisos |
| 6.7 | Integración de ejecutor | Ejecutores consultan PermissionManager antes de ejecutar |

**Entregable**: Sistema de permisos de 5 niveles con registro de auditoría.

---

## ~~Fase 7: Auto-Evolución (Compilación Dinámica)~~ ✅ Completado

**Objetivo**: Seres pueden reescribir su propio código.

| # | Módulo | Descripción |
|---|--------|-------------|
| 7.1 | DynamicBeingLoader | Cargador de compilación dinámica usando Roslyn |
| 7.2 | SecurityScanner | Análisis estático de código para operaciones peligrosas |
| 7.3 | Control de referencias en compilación | Compilador solo obtiene lista permitida de ensamblados |
| 7.4 | Almacenamiento cifrado | Código cifrado con AES-256 en disco |
| 7.5 | Reemplazo atómico | Compilar en memoria → crear instancia → migrar estado → intercambiar → persistir |
| 7.6 | ReplacePermissionCallback | Permitir a seres compilar e inyectar callbacks de permisos personalizados |

**Entregable**: Seres pueden auto-evolucionarse reescribiendo código.

---

## ~~Fase 8: Memoria a Largo Plazo y Tareas~~ ✅ Completado

**Objetivo**: Seres pueden recordar y planificar.

| # | Módulo | Descripción |
|---|--------|-------------|
| 8.1 | Sistema de memoria a largo plazo | Persistir y consultar experiencias pasadas |
| 8.2 | Sistema de tareas | Crear, gestionar y completar tareas |
| 8.3 | Sistema de temporizadores | Temporizadores de una vez, intervalo y cron |
| 8.4 | Sistema de notas de trabajo | Notas personales para seres con Markdown y palabras clave |

**Entregable**: Seres pueden recordar pasado y planificar futuro.

---

## ~~Fase 9: Host Central~~ ✅ Completado

**Objetivo**: Unificar todos los componentes en host central.

| # | Módulo | Descripción |
|---|--------|-------------|
| 9.1 | CoreHost | Host unificado que ensambla todos los componentes |
| 9.2 | ServiceLocator | Registro y recuperación global de servicios |
| 9.3 | Gestión multiagente | Múltiples seres coexisten y colaboran |
| 9.4 | Programación prioritaria del curador | Curador responde inmediatamente a mensajes de usuario |

**Entregable**: Sistema multiagente completamente funcional.

---

## ~~Fase 10: Web UI~~ ✅ Completado

**Objetivo**: Interfaz web moderna para gestión e interacción.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.1 | Servidor HTTP | HttpListener integrado, enrutamiento básico |
| 10.2 | SSE | Eventos enviados por servidor para actualizaciones en tiempo real |
| 10.3 | 20+ controladores | Being, Chat, Config, Permission, Log, Audit, etc. |
| 10.4 | Sistema de pieles | 4 pieles: Admin, Chat, Creative, Dev |
| 10.5 | Constructores HTML/CSS/JS | `H`, `CssBuilder`, `JsBuilder` para generación de marcado |

**Entregable**: Web UI completa con gestión de seres, chat y configuración.

---

## ~~Fase 10.5: Mejoras Incrementales~~ ✅ Completado

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.5.1 | Canales de broadcast | Anuncios a nivel del sistema |
| 10.5.2 | Auditoría de tokens | Seguimiento de uso de tokens de IA |
| 10.5.3 | 32 calendarios | Implementación completa de todos los sistemas de calendario |
| 10.5.4 | Mejoras de herramientas | Herramientas adicionales y mejoras |
| 10.5.5 | Localización en 21 idiomas | Soporte multilingüe integral |

---

## ~~Fase 10.6: Perfeccionamiento~~ ✅ Completado

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.6.1 | WebView | Automatización de navegador basada en Playwright |
| 10.6.2 | Sistema de ayuda | Documentos de ayuda multilingües |
| 10.6.3 | Proyectos | Espacios de trabajo de proyectos con notas de trabajo |
| 10.6.4 | Red de conocimiento | Sistema de gráfico de conocimiento basado en tripletas |

---

## Fase 11: Integración de Mensajería Instantánea Externa 🚧 Planificado

**Objetivo**: Conectar con plataformas de mensajería instantánea populares.

- [ ] Soporte para Feishu
- [ ] Integración con WhatsApp
- [ ] Conector de Telegram
- [ ] Gestión de múltiples plataformas

---

## Fase 12: Sistema de Plugins y Ecosistema de Habilidades 🚧 Planificado

**Objetivo**: Permitir extensiones de terceros.

- [ ] Sistema de plugins
- [ ] Marketplace de habilidades
- [ ] API de extensión
- [ ] Documentación para desarrolladores de plugins

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
