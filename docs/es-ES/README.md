![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Versión: v0.1.0-alpha** | **Silicon Life Collective** — Una plataforma de colaboración multiagente basada en .NET 9, donde los agentes de IA se denominan **Ser Silicona**, capaces de auto-evolucionarse mediante compilación dinámica Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | **Español** | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md)

## 🌟 Características Principales

### Sistema de Agentes
- **Orquestación Multiagente** — Gestión unificada por el *Curador Silicona*, con mecanismo de programación justa por intervalo de tiempo impulsado por reloj
- **Impulsado por Archivo de Alma** — Cada Ser Silicona es impulsado por un archivo de indicación central (`soul.md`), definiendo personalidad y patrones de comportamiento únicos
- **Arquitectura Cuerpo-Cerebro** — El *Cuerpo* (SiliconBeing) mantiene signos vitales y detecta escenarios de activación; el *Cerebro* (ContextManager) carga historial, invoca IA, ejecuta herramientas y persiste respuestas
- **Capacidad de Auto-Evolución** — Mediante tecnología de compilación dinámica Roslyn, los Seres Silicona pueden reescribir su propio código para evolucionar
- **Gestión de Estados de Actividad** — Soporta cuatro estados de actividad: Idle (inactivo), Working (trabajando), Error (error), Stopped (detenido). Entrada automática al estado Stopped tras 10 errores consecutivos

### Sistema de Plugins
- **Arquitectura de Extensión por Plugins** — Extensión de funcionalidad mediante la interfaz IPlugin, soportando carga dinámica de DLLs de plugins desde directorios
- **Sandbox de Seguridad** — El cargador de plugins ejecuta un escaneo de seguridad estricto, prohibiendo el acceso a espacios de nombres como System.IO, System.Net, etc.
- **Carga Aislada** — Usa AssemblyLoadContext personalizado para carga aislada, evitando que los plugins afecten la estabilidad del programa principal
- **Integración de Herramientas** — Los plugins pueden registrar herramientas personalizadas mediante la interfaz ITool, integrándose automáticamente al ciclo de invocación de herramientas

### Herramientas y Ejecución
- **24 Herramientas Integradas** — Cubren calendario, chat, configuración, disco, red, memoria, tareas, temporizadores, base de conocimientos, notas de trabajo, navegador WebView, recarga en caliente, etc.
- **Herramienta de Recarga en Caliente** — Soporta compilación automática, actualización de archivos y reinicio de SiliconLife.Fast durante la ejecución, sin intervención manual
- **Ciclo de Invocación de Herramientas** — IA devuelve invocación de herramienta → Ejecutar herramienta → Retroalimentar resultados a IA → Ciclo continuo hasta devolver respuesta de texto puro
- **Seguridad Ejecutor-Permiso** — Todas las operaciones de E/S pasan por verificación estricta de permisos a través de ejecutores
  - Cadena de permisos de 5 niveles: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Registro de auditoría completo para todas las decisiones de permisos

### IA y Conocimiento
- **Soporte para Múltiples Backends de IA**
  - **Ollama** — Despliegue local de modelos, usando API HTTP nativa
  - **Alibaba Cloud Bailian (DashScope)** — Servicio de IA en la nube, compatible con API OpenAI, soporte para 13+ modelos, despliegue multi-región
  - **Volcengine Ark (VolcengineArk)** — Servicio de IA en la nube de ByteDance, soporta modos streaming y no-streaming, control de velocidad integrado
- **32 Sistemas de Calendario** — Cobertura completa de los principales calendarios globales, incluyendo Gregoriano, Lunar Chino, Islámico, Hebreo, Japonés, Persa, Maya, Calendario Histórico Chino, etc.
- **Sistema de Red de Conocimiento** — Gráfico de conocimiento basado en tripletas (sujeto-relación-objeto), soportando almacenamiento, consulta y descubrimiento de rutas

### Interfaz Web
- **Web UI Moderna** — Servidor HTTP integrado, soporte para actualizaciones en tiempo real SSE
- **7 Temas de Piel** — Versión de gestión, versión de chat, versión de creación, versión de desarrollo, alto contraste, claro, minimalista, soporte para descubrimiento y cambio automáticos
- **20+ Controladores** — Funcionalidad completa de gestión del sistema, chat, configuración y monitoreo
- **Sin Dependencias de Framework Frontend** — Generación de HTML/CSS/JS en el servidor mediante `H`, `CssBuilder` y `JsBuilder`

### Internacionalización y Localización
- Soporte completo para **29 implementaciones de idioma**, cubriendo 2 sistemas de escritura y múltiples variantes regionales
  - **Chino simplificado**: zh-CN (China continental), zh-SG (Singapur), zh-MY (Malasia) (3 variantes)
  - **Chino tradicional**: zh-HK (Hong Kong), zh-TW (Taiwán), zh-MO (Macao) (3 variantes)
  - **Inglés**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - **Español**: es-ES, es-MX (2 variantes)
  - **Alemán**: de-DE, de-AT, de-CH, de-LU, de-LI (5 variantes)
  - **Francés**: fr-FR, fr-CA, fr-CH (3 variantes)
  - **Japonés**: ja-JP | **Coreano**: ko-KR | **Checo**: cs-CZ (3 variantes)

### Datos y Almacenamiento
- **Almacenamiento de Alto Rendimiento SpeedyPack** — La versión Fast usa el motor de almacenamiento .spk propio, mapeo de directorios en memoria + caché de entradas + cola de escritura asíncrona
- **Almacenamiento en Sistema de Archivos** — La versión Default usa almacenamiento puro en sistema de archivos JSON
- **Consulta Indexada por Tiempo** — Soporte para consultas eficientes por rango de tiempo a través de la interfaz `ITimeStorage`
- **Compresión Automática** — SpeedyPack soporta compresión automática programada, recuperando espacio libre
- **Dependencias Mínimas** — La biblioteca central solo depende de Microsoft.CodeAnalysis.CSharp para compilación dinámica

## 🔄 Arquitectura de Versión Dual

Este proyecto proporciona dos versiones de implementación para satisfacer diferentes necesidades de escenarios:

### SiliconLife.Default (Versión Predeterminada)
- **Posicionamiento**: Implementación predeterminada, utilizada principalmente para verificación de viabilidad de arquitectura
- **Modo de Ejecución**: Aplicación de consola
- **Método de Almacenamiento**: Almacenamiento JSON puro en sistema de archivos
- **Escenarios Aplicables**: Requisitos altos de seguridad de datos, recursos de memoria limitados, pequeño volumen de datos
- **Características**: Simple y confiable, persistencia de datos inmediata, sin riesgo de pérdida de memoria
- **Descripción de Rol**: Implementación de referencia para verificación de arquitectura, adecuada para primer contacto, depuración de desarrollo o escenarios donde la seguridad de datos es prioritaria
- **Comando de Inicio**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Versión de Alto Rendimiento)
- **Posicionamiento**: Versión de producción principal
- **Modo de Ejecución**: Aplicación de formularios Windows (soporta bandeja del sistema)
- **Método de Almacenamiento**: Almacenamiento en memoria SpeedyPack + persistencia asíncrona por lotes (formato .spk)
- **Escenarios Aplicables**: Alta concurrencia, baja latencia, escenarios de gran volumen de datos
- **Características**:
  - Optimización extrema de rendimiento
  - Ejecución en segundo plano de la bandeja, monitoreo en tiempo real mediante ventana de estado de la bandeja
  - Motor SpeedyPack + compresión automática garantizan seguridad de datos
  - Arquitectura Component UI, 30+ componentes declarativos
  - 7 Temas de Piel, soporte para descubrimiento y cambio automáticos
  - Herramienta de recarga en caliente para actualizaciones y reinicios en línea
- **Mejora de Rendimiento**: Latencia de lectura de almacenamiento reducida 1000x, latencia de escritura reducida 15000x, capacidad de procesamiento concurrente aumentada 50x
- **Descripción de Rol**: Implementación de nivel de producción con optimización profunda, la mejor opción para operaciones a largo plazo y entornos de producción reales
- **Comando de Inicio**: `dotnet run --project src/SiliconLife.Fast`

### Comparación de Versiones

| Característica | SiliconLife.Default | SiliconLife.Fast |
|----------------|---------------------|------------------|
| **Modo de Ejecución** | Aplicación de consola | Aplicación de formularios (bandeja del sistema) |
| **Interfaz de Usuario** | Web UI (acceso por navegador) | Icono de bandeja + ventana de bandeja + Web UI |
| **Bandeja del Sistema** | ❌ Ninguna | ✅ Soporta minimizar a la bandeja |
| **Ejecución en Segundo Plano** | ❌ Sale cuando se cierra la consola | ✅ Ejecución continua en segundo plano de la bandeja |
| **Método de Almacenamiento** | Almacenamiento JSON en sistema de archivos | Almacenamiento en memoria SpeedyPack + persistencia asíncrona |
| **Motor de Almacenamiento** | I/O de sistema de archivos | SiliconLife.Speedy (formato .spk) |
| **Latencia de Lectura** | ~10ms (I/O de disco) | ~0.01ms (operación en memoria) |
| **Latencia de Escritura** | ~15ms (escritura síncrona) | ~0.001ms (escritura asíncrona) |
| **Concurrencia** | ~100 req/s | ~5000 req/s |
| **Uso de Memoria** | ~200MB | ~500MB |
| **Seguridad de Datos** | Extremadamente alta (persistencia inmediata) | Alta (persistencia asíncrona + compresión automática) |
| **Escenarios Aplicables** | Seguridad de datos primero, datos pequeños | Rendimiento primero, datos grandes, alta concurrencia |

## 🛠️ Stack Tecnológico

| Componente | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 Windows |
| Lenguaje de Programación | C# | C# |
| Tipo de Aplicación | Aplicación de consola | Aplicación de formularios Windows |
| Integración IA | Ollama (local), Alibaba Cloud Bailian (nube) | Ollama (local), Alibaba Cloud Bailian (nube), Volcengine Ark (nube) |
| Almacenamiento de Datos | Sistema de archivos (JSON + directorios indexados por tiempo) | SpeedyPack (formato .spk, mapeo en memoria + persistencia asíncrona) |
| Servidor Web | HttpListener (integrado en .NET) | HttpListener (integrado en .NET) |
| Compilación Dinámica | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatización de Navegador | Playwright (WebView) | Playwright (WebView) |
| Sistema de Plugins | ✅ Soportado (IPlugin + PluginLoader) | ✅ Soportado (IPlugin + PluginLoader) |
| Bandeja del Sistema | ❌ No soportado | ✅ Soportado (NotifyIcon) |
| Licencia | Apache-2.0 | Apache-2.0 |

## 📁 Estructura del Proyecto

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Biblioteca central (interfaces, clases abstractas)
│   │   ├── AI/                            # Interfaces de cliente IA, gestor de contexto, modelos de mensajes
│   │   ├── Audit/                         # Sistema de auditoría de uso de tokens
│   │   ├── Chat/                          # Sistema de chat, gestión de sesiones, canales de broadcast
│   │   ├── Compilation/                   # Compilación dinámica, escaneo de seguridad, cifrado de código
│   │   ├── Config/                        # Sistema de gestión de configuración
│   │   ├── Executors/                     # Ejecutores (disco, red, línea de comandos)
│   │   ├── IM/                            # Interfaces de proveedor de mensajería instantánea
│   │   ├── Knowledge/                     # Sistema de red de conocimiento
│   │   ├── Localization/                  # Sistema de localización
│   │   ├── Logging/                       # Sistema de registro
│   │   ├── Plugins/                       # Sistema de plugins (interfaz IPlugin, cargador PluginLoader)
│   │   ├── Project/                       # Sistema de gestión de proyectos
│   │   ├── Runtime/                       # Bucle principal, objetos de reloj, host central
│   │   ├── Security/                      # Sistema de gestión de permisos
│   │   ├── SiliconBeing/                  # Clase base de Ser Silicona, gestor, fábrica
│   │   ├── Storage/                       # Interfaces de almacenamiento
│   │   ├── Time/                          # Fecha incompleta (consulta de rango de tiempo)
│   │   ├── Tools/                         # Interfaces de herramientas y gestor de herramientas
│   │   ├── WebView/                       # Interfaz de navegador WebView
│   │   └── ServiceLocator.cs              # Localizador de servicios global
│   │
│   ├── SiliconLife.Common/                # Implementación compartida (común a ambas versiones)
│   │   ├── AI/                            # Clientes IA y fábricas (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 implementaciones de calendario
│   │   ├── Localization/                  # Clase base de localización y 29 variantes lingüísticas/regionales
│   │   ├── Resources/                     # Archivos de recursos compartidos
│   │   ├── Security/                      # Gestor de permisos
│   │   ├── SiliconBeing/                  # Implementación predeterminada de Ser Silicona
│   │   ├── Tools/                         # 23 herramientas comunes (incluye herramienta de recarga en caliente)
│   │   ├── Web/                           # Infraestructura Web
│   │   └── WebView/                       # Implementación Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Capa de aplicación (Web UI + docs de ayuda, compartida entre Default y Fast)
│   │   ├── Config/                        # Configuración de la aplicación
│   │   ├── Data/                          # Directorio de datos
│   │   ├── Help/                          # Documentación de ayuda localizada (multilingüe)
│   │   └── Web/                           # Implementación Web UI
│   │       ├── Component/                 # Biblioteca de componentes UI (30+ componentes)
│   │       ├── Controllers/               # 22 controladores
│   │       ├── Models/                    # Modelos de vista
│   │       ├── Views/                     # Vistas HTML
│   │       └── Skins/                     # 7 temas de piel
│   │
│   ├── SiliconLife.Default/               # Implementación predeterminada + punto de entrada (versión consola)
│   │   ├── Program.cs                     # Punto de entrada (ensambla todos los componentes)
│   │   ├── Config/                        # Datos de configuración predeterminados
│   │   ├── IM/                            # Proveedor WebUI
│   │   ├── Knowledge/                     # Implementación de red de conocimiento
│   │   ├── Logging/                       # Implementaciones de proveedores de registro
│   │   ├── Project/                       # Implementación del sistema de proyectos
│   │   ├── Security/                      # Callbacks de permisos predeterminados
│   │   ├── Storage/                       # Implementación de almacenamiento en sistema de archivos
│   │   └── Tools/                         # Herramientas específicas de versión (HelpTool)
│   │
│   └── SiliconLife.Fast/                  # Implementación de alto rendimiento + punto de entrada (versión Forms)
│       ├── Program.cs                     # Punto de entrada (aplicación de formularios)
│       ├── Config/                        # Datos de configuración (compartidos con Default)
│       ├── IM/                            # Proveedor WebUI
│       ├── Knowledge/                     # Implementación de red de conocimiento (optimizada en memoria)
│       ├── Logging/                       # Proveedor de registro de alto rendimiento
│       ├── Project/                       # Implementación del sistema de proyectos
│       ├── Security/                      # Callbacks de permisos optimizados
│       ├── Storage/                       # Adaptador de almacenamiento SpeedyPack
│       ├── Tools/                         # Herramientas específicas de versión (HelpTool)
│       └── Tray/                          # Bandeja del sistema (localización en 29 variantes)
│
│   ├── SiliconLife.Speedy/                # Motor de almacenamiento de alto rendimiento SpeedyPack
│   │   ├── SpeedyPack.cs                  # Clase central (mapeo de directorios en memoria + caché + escritura asíncrona)
│   │   ├── SpeedyPackOptions.cs           # Opciones de configuración (TTL de caché, máximo de entradas, etc.)
│   │   ├── IPackTransaction.cs            # Interfaz de transacciones
│   │   ├── SpkFileInfo.cs                 # Información de archivo
│   │   └── Internal/                      # Implementación interna
│       │   ├── DirectoryMap.cs            # Mapeo de directorios en memoria
│       │   ├── EntryCache.cs              # Caché de entradas
│       │   ├── FreeList.cs                # Gestión de espacio libre
│       │   ├── PackFileReader.cs          # Lector de archivos de paquete
│       │   ├── PackFileWriter.cs          # Escritor de archivos de paquete
│       │   ├── WriteQueue.cs              # Cola de escritura asíncrona
│       │   ├── WriteOperation.cs          # Operación de escritura
│       │   ├── SpeedyTransaction.cs       # Implementación de transacciones
│       │   ├── SpkHeader.cs              # Cabecera de archivo de paquete
│       │   └── PathNormalizer.cs          # Normalización de rutas
│   │
│   └── SiliconLife.Speedy.Manager/        # Herramienta de gestión SpeedyPack (Windows Forms)
│       ├── MainForm.cs                    # Ventana principal
│       ├── Program.cs                     # Punto de entrada
│       └── slc.ico                        # Icono de la aplicación
│
├── docs/                                  # Documentación multilingüe
│   ├── zh-CN/                             # Documentación en chino simplificado
│   ├── en/                                # Documentación en inglés
│   └── ...                                # Documentación en otros idiomas
```

## 🏗️ Resumen de Arquitectura

### Arquitectura de Programación
```
Bucle principal (hilo dedicado, watchdog + cortacircuitos)
  └── Objetos de reloj (ordenados por prioridad)
       └── Gestor de Seres Silicona
            └── Ejecutor de Ser Silicona (hilo temporal, timeout + cortacircuitos)
                 └── SiliconBeing.Tick()
                      └── ContextManager.Think()
                           └── IAIClient.Chat()
                                └── Ciclo de invocación de herramientas → Persistir en sistema de chat
```

### Arquitectura de Seguridad
Todas las operaciones de E/S iniciadas por IA deben pasar por una cadena de seguridad estricta:

```
Invocación de herramienta → Ejecutor → Gestor de permisos → [IsCurator → caché de frecuencia → ACL global → callback → preguntar al usuario]
```

## 🚀 Inicio Rápido

### Prerrequisitos

- **.NET 9 SDK** — [Enlace de descarga](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend de IA** (elegir uno):
  - **Ollama**: [Instalar Ollama](https://ollama.com) y obtener modelo (ej. `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: Obtener clave API desde [Consola Bailian](https://bailian.console.aliyun.com/)

### Construir el Proyecto

```bash
dotnet restore
dotnet build
```

### Ejecutar el Sistema

#### Método 1: Ejecutar la versión Default (Aplicación de consola)

```bash
dotnet run --project src/SiliconLife.Default
```

La aplicación iniciará el servidor web y abrirá automáticamente la Web UI en el navegador.

**Escenarios aplicables**:
- ✅ Requisitos extremadamente altos de seguridad de datos
- ✅ Recursos de memoria limitados (RAM < 2GB)
- ✅ Pequeño volumen de datos, uso a corto plazo
- ✅ Fase de depuración de desarrollo

#### Método 2: Ejecutar la versión Fast (Aplicación de formularios Windows)

```bash
dotnet run --project src/SiliconLife.Fast
```

La aplicación se iniciará en modo formulario, se minimizará a la bandeja del sistema y se ejecutará continuamente en segundo plano.

**Escenarios aplicables**:
- ✅ Escenarios de alta concurrencia (> 5 usuarios)
- ✅ Gran volumen de datos (uso de más de 3 meses)
- ✅ Necesidad de respuesta de baja latencia
- ✅ Necesidad de ejecución en segundo plano de la bandeja

### Publicar como Archivo Único

```bash
# Windows - Versión Default
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Versión Fast
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Solo versión Default
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Solo versión Default
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Hoja de Ruta de Desarrollo

### ✅ Completado
- [x] Fase 1: Chat de IA en consola
- [x] Fase 2: Esqueleto del framework (bucle principal + objetos de reloj + watchdog + cortacircuitos)
- [x] Fase 3: Primer Ser Silicona con archivo de alma (arquitectura cuerpo-cerebro)
- [x] Fase 4: Memoria persistente (sistema de chat + interfaz de almacenamiento por tiempo)
- [x] Fase 5: Sistema de herramientas + ejecutores
- [x] Fase 6: Sistema de permisos (cadena de 5 niveles, registrador de auditoría, ACL global)
- [x] Fase 7: Compilación dinámica + auto-evolución (Roslyn)
- [x] Fase 8: Memoria a largo plazo + tareas + temporizadores
- [x] Fase 9: Host central + colaboración multiagente
- [x] Fase 10: Web UI (HTTP + SSE, 20+ controladores, 7 pieles)
- [x] Fase 10.5: Mejoras incrementales (canales de broadcast, auditoría de tokens, 32 calendarios, mejoras de herramientas, localización en 29 idiomas)
- [x] Fase 10.6: Perfeccionamiento y optimización (WebView, sistema de ayuda, espacio de trabajo de proyectos, red de conocimiento)
- [x] Fase 11: Motor de almacenamiento SpeedyPack (reemplazo de LiteDB, mapeo en memoria, cola de escritura asíncrona, compresión automática)
- [x] Fase 12: Sistema de plugins (interfaz IPlugin, sandbox de seguridad PluginLoader, carga aislada, integración de herramientas)

### 🚧 Planificado
- [ ] Fase 13: Integración de mensajería instantánea externa (Feishu / WhatsApp / Telegram)
- [ ] Fase 14: Ecosistema de habilidades (mercado de plugins, distribución de paquetes de habilidades)

## 📚 Documentación

- [Diseño de Arquitectura](architecture.md) — Diseño del sistema, mecanismo de programación, arquitectura de componentes
- [Modelo de Seguridad](security.md) — Modelo de permisos, ejecutores, seguridad de compilación dinámica
- [Guía de Desarrollo](development-guide.md) — Desarrollo de herramientas, guía de extensión
- [Referencia de API](api-reference.md) — Documentación de endpoints Web API
- [Referencia de Herramientas](tools-reference.md) — Detalles de herramientas integradas
- [Guía de Web UI](web-ui-guide.md) — Guía de uso de la interfaz web
- [Guía de Ser Silicona](silicon-being-guide.md) — Guía de desarrollo de agentes
- [Sistema de Permisos](permission-system.md) — Gestión detallada de permisos
- [Sistema de Calendario](calendar-system.md) — Descripción de 32 sistemas de calendario
- [Inicio Rápido](getting-started.md) — Guía detallada de introducción
- [Solución de Problemas](troubleshooting.md) — Preguntas frecuentes
- [Hoja de Ruta](roadmap.md) — Plan de desarrollo completo
- [Registro de Cambios](changelog.md) — Historial de actualizaciones de versiones
- [Guía de Contribución](contributing.md) — Cómo participar en el proyecto

## 🤝 Contribuir

¡Aceptamos todas las formas de contribución! Para más detalles, consulta la [Guía de Contribución](contributing.md).

### Flujo de Trabajo de Desarrollo
1. Hacer fork del repositorio
2. Crear una rama de característica (`git checkout -b feature/AmazingFeature`)
3. Confirmar cambios (`git commit -m 'feat: add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Enviar un Pull Request

## � Guía de Selección de Versión

### ¿Qué versión debería usar?

**SiliconLife.Default (Implementación predeterminada — verificación de viabilidad de arquitectura):**
- 📌 Es su primer contacto con este proyecto, desea comprender rápidamente la arquitectura del sistema
- 📌 Está realizando depuración de desarrollo, necesita una forma de ejecución simple y directa
- 📌 La seguridad de datos es su principal consideración
- 📌 Su sistema tiene menos de 4GB de memoria
- 📌 Solo necesita uso individual o un volumen de datos pequeño

**SiliconLife.Fast (Versión de producción principal):**
- ⚡ Necesita un entorno de producción estable a largo plazo
- ⚡ Ya está familiarizado con la arquitectura del sistema, listo para despliegue oficial
- ⚡ Necesita soportar acceso concurrente de múltiples usuarios
- ⚡ Necesita ejecución en segundo plano de la bandeja del sistema
- ⚡ Busca la experiencia de rendimiento extrema

> **Recomendación general**: SiliconLife.Default es adecuado como verificación de arquitectura y experiencia de introducción; para entornos de producción reales, se recomienda encarecidamente usar SiliconLife.Fast.

### ¿Se puede migrar de Default a Fast?

**¡Por supuesto!** Ambas versiones comparten lo mismo:
- ✅ Formato de archivo de configuración (config.json)
- ✅ Interfaz de herramientas
- ✅ Configuración de Being
- ✅ Interfaz Web UI

**Pasos de migración:**
1. Haga una copia de seguridad de su directorio de datos Default
2. Inicie la versión Fast con el mismo directorio de datos
3. Fast importará automáticamente los datos existentes al motor de almacenamiento SpeedyPack
4. Después de verificar que las funciones son normales, puede usar la versión Fast diariamente

### ¿Pueden coexistir ambas versiones?

**¡Sí!** Se recomienda la siguiente estrategia de despliegue:

**Estrategia 1: Default para verificación, Fast para producción**
```
Entorno de desarrollo/verificación: SiliconLife.Default (verificar arquitectura, depurar funciones)
Entorno de producción: SiliconLife.Fast (alto rendimiento, ejecución en segundo plano, procesamiento de solicitudes en tiempo real)
```

**Estrategia 2: Fast como ejecución principal, Default para copias de seguridad periódicas**
```
SiliconLife.Fast (uso diario, procesamiento de solicitudes en tiempo real)
    ↓ Copias de seguridad periódicas
SiliconLife.Default (archivado de datos fríos, respaldo de seguridad de datos)
```

## 📄 Licencia

Este proyecto está bajo la Licencia Apache 2.0 — ver el archivo [LICENSE](../../LICENSE) para más detalles.

## 👨‍💻 Autor

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Agradecimientos

Gracias a todos los desarrolladores y proveedores de plataformas de IA que han contribuido a este proyecto.

---

**Silicon Life Collective** — Hacer que los agentes de IA realmente "cobren vida"
