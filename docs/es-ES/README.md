![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Versión: v0.2.0-alpha** | **Silicon Life Collective** — Una plataforma de colaboración multiagente basada en .NET 9, donde los agentes de IA se denominan **Seres de Silicio**, que logran la autoevolución mediante compilación dinámica con Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | **Español** | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 Características Principales

### Sistema de Agentes
- **Orquestación multiagente** — Gestionada de forma unificada por el *Curador de Silicio*, con un mecanismo de programación justa por intervalos de tiempo impulsado por reloj
- **Impulsado por Archivo de Alma** — Cada Ser de Silicio está impulsado por un archivo de indicaciones principal (`soul.md`) que define su personalidad única y patrones de comportamiento
- **Arquitectura Cuerpo-Cerebro** — El *Cuerpo* (SiliconBeing) mantiene los signos vitales y detecta escenas de activación; el *Cerebro* (ContextManager) se encarga de cargar el historial, invocar la IA, ejecutar herramientas y persistir las respuestas
- **Capacidad de autoevolución** — Mediante la tecnología de compilación dinámica Roslyn, los Seres de Silicio pueden reescribir su propio código para evolucionar
- **Gestión de estados de actividad** — Soporta nueve estados de actividad: Idle (inactivo), SingleChat (chat 1:1), GroupChat (chat grupal), Task (tarea), Timer (temporizador), Broadcast (difusión), Project (proyecto), MemoryCompression (compresión de memoria), Stopped (detenido); tras 10 errores consecutivos, entra automáticamente en estado Stopped

### Sistema de Plugins
- **Arquitectura de extensión por plugins** — Extensión de funcionalidad mediante la interfaz IPlugin, con soporte para carga dinámica de DLLs de plugins desde directorios
- **Declaración de capacidades de plugins** — Los plugins declaran las capacidades necesarias (Network, FileIO, Process, AI) mediante el atributo `[PluginCapability]`, y el cargador flexibiliza las reglas de escaneo de seguridad en consecuencia; las capacidades no declarables (P/Invoke, Unsafe, Reflection Emit, etc.) siempre están bloqueadas
- **Carga aislada** — Utiliza AssemblyLoadContext personalizado para carga aislada, evitando que los plugins afecten la estabilidad del programa principal
- **Integración de herramientas** — Los plugins pueden registrar herramientas personalizadas mediante la interfaz ITool, integrándose automáticamente en el ciclo de llamadas a herramientas

### Herramientas y Ejecución
- **24 herramientas integradas** — Cubren calendario, chat, configuración, disco, red, memoria, tareas, temporizador, base de conocimiento, notas de trabajo, espacio de trabajo de proyectos, WebView, etc.
- **Aislamiento de escenarios de herramientas** — Cada herramienta declara los escenarios disponibles mediante el atributo `ToolScenario` (Chat, Task, Timer, MemoryCompression, Project); el atributo `ChatOnly` restringe las herramientas solo al escenario de chat
- **Interfaz de capacidades IAIClient** — Los clientes de IA declaran las capacidades de modo streaming, llamadas a herramientas, ventana de contexto, visión y audio, y el ContextManager adapta su comportamiento en consecuencia
- **Ciclo de llamadas a herramientas** — La IA devuelve una llamada a herramienta → se ejecuta la herramienta → el resultado se retroalimenta a la IA → el ciclo continúa hasta que se devuelve una respuesta de texto puro
- **Seguridad de Ejecutor-Permiso** — Todas las operaciones de E/S pasan por verificación estricta de permisos a través de ejecutores
  - Cadena de verificación de permisos de 3 niveles: UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → denegación por defecto)
  - Registro de auditoría completo de todas las decisiones de permisos

### IA y Conocimiento
- **Soporte para múltiples backends de IA**
  - **Ollama** — Despliegue de modelos locales, utilizando la API HTTP nativa
  - **Alibaba Cloud Bailian (DashScope)** — Servicio de IA en la nube, compatible con la API de OpenAI, soporta más de 13 modelos, despliegue en múltiples regiones
  - **Volcengine Ark (VolcengineArk)** — Servicio de IA en la nube de ByteDance, soporta modos de streaming y no streaming, con control de velocidad integrado
  - **Herdsman** — Motor de inferencia sin autenticación, compatible con el formato API OpenAI
  - **Meituan LongCat** — Gran modelo de desarrollo propio de Meituan, compatible con el formato API OpenAI, autenticación por clave API
  - **Qiniu Cloud AI** — Servicio de IA en la nube de Qiniu, autenticación por clave API
- **32 sistemas de calendario** — Cobertura completa de los principales calendarios del mundo, incluyendo gregoriano, lunar chino, islámico, hebreo, japonés, persa, maya, calendarios históricos chinos, etc.
- **Sistema de Red de Conocimiento** — Grafo de conocimiento basado en tripletas (sujeto-relación-objeto), con soporte para almacenamiento, consulta y descubrimiento de rutas
- **Espacio de trabajo de proyectos** — Gestión de espacios de proyectos, soporta creación/archivado/destrucción de proyectos, asignación de roles, notas de trabajo, seguimiento de tareas y aislamiento de permisos de herramientas
- **Motor de flujos de trabajo** — Motor de máquina de estados basado en plantillas, soporta plantillas de flujo de trabajo personalizadas, transiciones de estado, ejecución impulsada por Tick y gestión del ciclo de vida de instancias
- **Mecanismo de desvanecimiento de memoria** — Servicio de decaimiento temporal (MemoryFadeService), que aplica automáticamente decaimiento de importancia y archivado automático a las memorias de todos los Seres de Silicio cada hora

### Interfaz Web
- **Interfaz Web moderna** — Servidor HTTP integrado, con soporte para actualizaciones en tiempo real mediante SSE
- **7 temas de piel** — Administración, Chat, Creativo, Desarrollo, Alto Contraste, Claro, Minimalista, con soporte para descubrimiento y cambio automático
- **24 controladores** — Funcionalidad completa de gestión del sistema, chat, configuración y monitoreo
- **Cero dependencias de frameworks frontend** — Generación de HTML/CSS/JS en el servidor mediante `H`, `CssBuilder` y `JsBuilder`

### Internacionalización y Localización
- **34 variantes de idioma** con soporte completo, cubriendo 2 sistemas de escritura y múltiples variantes regionales
  - **Chino simplificado**: zh-CN (China continental), zh-SG (Singapur), zh-MY (Malasia) (3 variantes)
  - **Chino tradicional**: zh-HK (Hong Kong), zh-TW (Taiwán), zh-MO (Macao) (3 variantes)
  - **Inglés**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - **Español**: es-ES, es-MX (2 variantes)
  - **Alemán**: de-DE, de-AT, de-CH, de-LU, de-LI (5 variantes)
  - **Francés**: fr-FR, fr-CA, fr-CH (3 variantes)
  - **Japonés**: ja-JP | **Coreano**: ko-KR | **Checo**: cs-CZ (3 variantes)
  - **Italiano**: it-IT | **Polaco**: pl-PL | **Portugués**: pt-PT, pt-BR (4 variantes)

### Datos y Almacenamiento
- **Almacenamiento de alto rendimiento SpeedyPack** — La versión Fast utiliza el motor de almacenamiento .spk propio, con mapeo de directorios en memoria + caché de entradas + cola de escritura asíncrona
- **Almacenamiento en sistema de archivos** — La versión Default utiliza almacenamiento JSON puro en sistema de archivos
- **Consultas con índice temporal** — Soporte para consultas eficientes por rango de tiempo a través de la interfaz `ITimeStorage`
- **Compactación automática** — SpeedyPack soporta compactación automática periódica, recuperando espacio libre
- **Dependencias mínimas** — La biblioteca principal solo depende de Microsoft.CodeAnalysis.CSharp para compilación dinámica

## 🔄 Arquitectura de Doble Versión

Este proyecto proporciona dos versiones de implementación para satisfacer diferentes escenarios:

### SiliconLife.Default (Versión por defecto)
- **Posicionamiento**: Implementación por defecto, principalmente para verificar la viabilidad de la arquitectura
- **Modo de ejecución**: Aplicación de consola
- **Método de almacenamiento**: Almacenamiento JSON puro en sistema de archivos
- **Escenarios aplicables**: Alta seguridad de datos, recursos de memoria limitados, volúmenes de datos pequeños
- **Características**: Simple y fiable, persistencia inmediata de datos, sin riesgo de pérdida en memoria
- **Descripción del rol**: Como implementación de referencia para verificación de arquitectura, adecuada para primeros contactos, depuración de desarrollo o escenarios donde la seguridad de datos es prioritaria
- **Comando de inicio**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Versión de alto rendimiento)
- **Posicionamiento**: Versión de producción recomendada
- **Modo de ejecución**: Aplicación de escritorio (bandeja del sistema en Windows/macOS / ventana de estado en Linux)
- **Método de almacenamiento**: Almacenamiento en memoria SpeedyPack + persistencia por lotes asíncrona (formato de archivo .spk)
- **Escenarios aplicables**: Alta concurrencia, baja latencia, grandes volúmenes de datos
- **Soporte de plataforma**: Windows/macOS (funcionalidad completa, incluyendo bandeja del sistema), Linux (ventana de estado, sin icono en bandeja)
- **Características**:
  - Optimización de rendimiento extrema
  - Ejecución en segundo plano en la bandeja del sistema de Windows/macOS, con ventana de estado de la bandeja para monitoreo en tiempo real; ventana de estado directa en Linux
  - Motor SpeedyPack + compactación automática para garantizar la seguridad de los datos
  - Arquitectura Component UI, 27 componentes declarativos
  - 7 temas de piel, con soporte para descubrimiento y cambio automático
  - Herramienta de recarga en caliente para actualización y reinicio en línea → Linux abre automáticamente el navegador para acceder a la Web UI, soporta el parámetro `--no-tray`
- **Mejora de rendimiento**: Latencia de lectura de almacenamiento reducida 1000 veces, latencia de escritura reducida 15000 veces, capacidad de procesamiento concurrente aumentada 50 veces
- **Descripción del rol**: Implementación de grado de producción profundamente optimizada, es la opción preferida para ejecución a largo plazo y entornos de producción reales
- **Comando de inicio**: `dotnet run --project src/SiliconLife.Fast`

### Comparación de versiones

| Característica | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **Modo de ejecución** | Aplicación de consola | Aplicación de escritorio (bandeja del sistema en Windows/macOS / ventana de estado en Linux) |
| **Interfaz de usuario** | Web UI (acceso por navegador) | Windows/macOS: icono de bandeja + ventana de bandeja + Web UI; Linux: ventana de estado + Web UI |
| **Bandeja del sistema** | ❌ No | ✅ Windows/macOS soportan minimizar a la bandeja; Linux sin icono de bandeja |
| **Ejecución en segundo plano** | ❌ Cerrar la consola cierra la aplicación | ✅ Windows/macOS ejecución continua en bandeja; Linux ejecución en ventana de estado |
| **Método de almacenamiento** | Almacenamiento JSON en sistema de archivos | Almacenamiento en memoria SpeedyPack + persistencia asíncrona |
| **Motor de almacenamiento** | E/S de sistema de archivos | SiliconLife.Speedy (formato .spk) |
| **Latencia de lectura** | ~10ms (E/S de disco) | ~0.01ms (operación en memoria) |
| **Latencia de escritura** | ~15ms (escritura síncrona) | ~0.001ms (escritura asíncrona) |
| **Capacidad concurrente** | ~100 req/s | ~5000 req/s |
| **Consumo de memoria** | ~200MB | ~500MB |
| **Seguridad de datos** | Muy alta (persistencia inmediata) | Alta (persistencia asíncrona + compactación automática) |
| **Escenarios aplicables** | Seguridad de datos prioritaria, volúmenes pequeños | Rendimiento prioritario, grandes volúmenes, alta concurrencia |

## 🛠️ Stack Tecnológico

| Componente | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Lenguaje de programación | C# | C# |
| Tipo de aplicación | Aplicación de consola | Aplicación de escritorio (bandeja del sistema en Windows/macOS / ventana de estado en Linux) |
| Integración de IA | Ollama (local), Alibaba Cloud Bailian (nube), Volcengine Ark (nube), Herdsman, Meituan LongCat, Qiniu Cloud AI | Ollama (local), Alibaba Cloud Bailian (nube), Volcengine Ark (nube), Herdsman, Meituan LongCat, Qiniu Cloud AI |
| Almacenamiento de datos | Sistema de archivos (JSON + directorio de índice temporal) | SpeedyPack (formato .spk, mapeo en memoria + persistencia asíncrona) |
| Servidor Web | HttpListener (integrado en .NET) | HttpListener (integrado en .NET) |
| Compilación dinámica | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatización de navegador | Playwright (WebView) | Playwright (WebView) |
| Sistema de plugins | ✅ Soportado (IPlugin + PluginLoader) | ✅ Soportado (IPlugin + PluginLoader) |
| Bandeja del sistema | ❌ No soportado | ✅ Windows/macOS soportados (NotifyIcon); Linux sin icono de bandeja |
| Licencia | Apache-2.0 | Apache-2.0 |

## 📁 Estructura del Proyecto

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Biblioteca principal (interfaces, clases abstractas)
│   │   ├── AI/                            # Interfaces de cliente de IA, gestor de contexto, modelos de mensajes
│   │   ├── Audit/                         # Sistema de auditoría de uso de tokens
│   │   ├── Chat/                          # Sistema de chat, gestión de sesiones, canal de difusión
│   │   ├── Compilation/                   # Compilación dinámica, escaneo de seguridad, cifrado de código
│   │   ├── Config/                        # Sistema de gestión de configuración
│   │   ├── Executors/                     # Ejecutores (disco, red, línea de comandos)
│   │   ├── IM/                            # Interfaz de proveedor de mensajería instantánea
│   │   ├── Knowledge/                     # Sistema de red de conocimiento
│   │   ├── Localization/                  # Sistema de localización
│   │   ├── Logging/                       # Sistema de registro
│   │   ├── Plugins/                       # Sistema de plugins (interfaz IPlugin, cargador PluginLoader)
│   │   ├── Project/                       # Sistema de gestión de proyectos
│   │   ├── Runtime/                       # Bucle principal, objetos de reloj, núcleo principal
│   │   ├── Security/                      # Sistema de gestión de permisos
│   │   ├── SiliconBeing/                  # Clase base de Ser de Silicio, gestor, fábrica
│   │   ├── Storage/                       # Interfaces de almacenamiento
│   │   ├── Time/                          # Fecha incompleta (consultas por rango temporal)
│   │   ├── Tools/                         # Interfaces de herramientas y gestor de herramientas
│   │   ├── WebView/                       # Interfaz de WebView para navegador
│   │   ├── Workflow/                      # Motor de flujos de trabajo (plantillas, instancias, transiciones de estado)
│   │   └── ServiceLocator.cs              # Localizador de servicios global
│   │
│   ├── SiliconLife.Common/                # Implementaciones compartidas (usadas por ambas versiones)
│   │   ├── AI/                            # Clientes y fábricas de IA (Ollama, DashScope, VolcengineArk, Herdsman, LongCat, QiniuAI)
│   │   ├── Calendar/                      # 32 implementaciones de calendario
│   │   ├── Localization/                  # Clase base de localización y 34 variantes de idioma/región
│   │   ├── Resources/                     # Archivos de recursos compartidos
│   │   ├── Security/                      # Gestor de permisos
│   │   ├── SiliconBeing/                  # Implementación predeterminada de Ser de Silicio
│   │   ├── Tools/                         # 23 implementaciones de herramientas genéricas
│   │   ├── Web/                           # Infraestructura Web
│   │   └── WebView/                       # Implementación Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Capa de aplicación (Web UI + documentación de ayuda, compartida por Default y Fast)
│   │   ├── Config/                        # Configuración de la aplicación
│   │   ├── Data/                          # Directorio de datos
│   │   ├── Help/                          # Localización de documentación de ayuda (multilingüe)
│   │   ├── Tools/                         # HelpTool (herramienta de consulta de documentación de ayuda)
│   │   └── Web/                           # Implementación de Web UI
│   │       ├── Component/                 # Biblioteca de componentes UI (27 componentes)
│   │       ├── Controllers/               # 24 controladores
│   │       ├── Models/                    # Modelos de vista
│   │       ├── Views/                     # Vistas HTML
│   │       └── Skins/                     # 7 temas de piel
│   │
│   ├── SiliconLife.Default/               # Implementación por defecto + punto de entrada de la aplicación (versión de consola)
│   │   ├── Program.cs                     # Punto de entrada (ensambla todos los componentes)
│   │   ├── Config/                        # Datos de configuración por defecto
│   │   ├── Knowledge/                     # Implementación de red de conocimiento
│   │   ├── Logging/                       # Implementación de proveedor de registros (consola + sistema de archivos)
│   │   ├── Project/                       # Implementación del sistema de proyectos
│   │   └── Storage/                       # Implementación de almacenamiento en sistema de archivos
│   │
│   ├── SiliconLife.Fast/                  # Implementación de alto rendimiento + punto de entrada de la aplicación (versión de escritorio)
│   │   ├── Program.cs                     # Punto de entrada (aplicación de escritorio)
│   │   ├── App.axaml / App.cs             # Definición de aplicación Avalonia
│   │   ├── Config/                        # Datos de configuración (compartidos con Default)
│   │   ├── Knowledge/                     # Implementación de red de conocimiento (optimizada en memoria)
│   │   ├── Logging/                       # Proveedor de registros de alto rendimiento
│   │   ├── Project/                       # Implementación del sistema de proyectos
│   │   ├── Storage/                       # Adaptadores de almacenamiento SpeedyPack
│   │   └── Tray/                          # Bandeja del sistema (localización para 34 variantes de idioma)
│   │
│   ├── SiliconLife.Speedy/                # Motor de almacenamiento de alto rendimiento SpeedyPack
│   │   ├── SpeedyPack.cs                  # Clase principal (mapeo de directorios en memoria + caché + escritura asíncrona)
│   │   ├── SpeedyPackOptions.cs           # Opciones de configuración (TTL de caché, máximo de entradas, etc.)
│   │   ├── IPackTransaction.cs            # Interfaz de transacciones
│   │   ├── SpkFileInfo.cs                 # Información de archivo
│   │   └── Internal/                      # Implementación interna
│   │       ├── DirectoryMap.cs            # Mapeo de directorios en memoria
│   │       ├── EntryCache.cs              # Caché de entradas
│   │       ├── FreeList.cs                # Gestión de espacio libre
│   │       ├── PackFileReader.cs          # Lector de archivos de paquete
│   │       ├── PackFileWriter.cs          # Escritor de archivos de paquete
│   │       ├── WriteQueue.cs              # Cola de escritura asíncrona
│   │       ├── WriteOperation.cs          # Operación de escritura
│   │       ├── SpeedyTransaction.cs       # Implementación de transacciones
│   │       ├── SpkHeader.cs               # Encabezado de archivo de paquete
│   │       └── PathNormalizer.cs          # Normalización de rutas
│   │
│   └── SiliconLife.Speedy.Manager/        # Herramienta de gestión SpeedyPack (Avalonia UI)
│       ├── MainForm.cs                    # Ventana principal
│       ├── Program.cs                     # Punto de entrada
│       └── slc.ico                        # Icono de la aplicación
│
├── docs/                                  # Documentación multilingüe
│   ├── zh-CN/                             # Documentación en chino simplificado
│   ├── en/                                # Documentación en inglés
│   └── ...                                # Documentación en otros idiomas
│
└── 总文档/                                 # Documentos de requisitos y arquitectura
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Resumen de Arquitectura

### Arquitectura de Programación
```
Bucle Principal (hilo dedicado, Perro Guardián + Interruptor de Circuito)
  └── Objetos de Reloj (ordenados por prioridad)
       └── Gestor de Seres de Silicio
            └── Ejecutor de Ser de Silicio (hilo temporal, tiempo de espera + Interruptor de Circuito)
                 └── SerDeSilicio.Tick()
                      └── GestorDeContexto.Pensar()
                           └── ClienteAI.Chatear()
                                └── Ciclo de llamadas a herramientas → Persistir en sistema de chat
```

### Arquitectura de Seguridad
Todas las operaciones de E/S iniciadas por la IA deben pasar por una cadena de seguridad estricta:

```
Llamada a herramienta → Ejecutor → Gestor de Permisos → [Caché de frecuencia → Retrollamada → (IsCurator: preguntar al usuario | Non-curator: ACL Global)]
```

## 🚀 Inicio Rápido

### Requisitos Previos

- **.NET 9 SDK** — [Enlace de descarga](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend de IA** (elegir uno):
  - **Ollama**: [Instalar Ollama](https://ollama.com) y extraer un modelo (por ejemplo, `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: Obtener una clave API desde la [consola de Bailian](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: Obtener una clave API desde la [consola de Volcengine](https://console.volcengine.com/ark)
  - **Herdsman**: Sin autenticación, compatible con el formato API OpenAI
  - **Meituan LongCat**: Obtener una clave API desde la plataforma Meituan
  - **Qiniu Cloud AI**: Obtener una clave API desde la [consola de Qiniu](https://portal.qiniu.com/)

### Construir el Proyecto

```bash
dotnet restore
dotnet build
```

### Ejecutar el Sistema

#### Método 1: Ejecutar la versión Default (aplicación de consola)

```bash
dotnet run --project src/SiliconLife.Default
```

La aplicación iniciará el servidor Web y abrirá automáticamente la Web UI en el navegador.

**Escenarios aplicables**:
- ✅ Requisitos de seguridad de datos muy altos
- ✅ Recursos de memoria limitados (RAM < 2GB)
- ✅ Volúmenes de datos pequeños, uso a corto plazo
- ✅ Fase de depuración de desarrollo

#### Método 2: Ejecutar la versión Fast (aplicación de escritorio)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**: La aplicación se iniciará en modo ventana, se minimizará a la bandeja del sistema y se ejecutará continuamente en segundo plano.

**Linux**: La aplicación mostrará una ventana de estado (sin icono en la bandeja del sistema) y abrirá automáticamente el navegador para acceder a la Web UI. También se puede usar el parámetro `--no-tray` para omitir la apertura automática del navegador:

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Escenarios aplicables**:
- ✅ Escenarios de alta concurrencia (> 5 usuarios)
- ✅ Grandes volúmenes de datos (uso superior a 3 meses)
- ✅ Necesidad de respuestas de baja latencia
- ✅ Necesidad de ejecución en segundo plano en la bandeja del sistema

### Publicar como Archivo Único

```bash
# Windows - Versión Default
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Versión Fast
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Versión Default
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Versión Fast
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Versión Default
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Versión Fast
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Hoja de Ruta de Desarrollo

### ✅ Completado
- [x] Fase 1: Chat de IA por consola
- [x] Fase 2: Esqueleto del framework (Bucle Principal + Objetos de Reloj + Perro Guardián + Interruptor de Circuito)
- [x] Fase 3: Primer Ser de Silicio con Archivo de Alma (arquitectura Cuerpo-Cerebro)
- [x] Fase 4: Memoria persistente (Sistema de Chat + Interfaz de Almacenamiento de Tiempo)
- [x] Fase 5: Sistema de herramientas + Ejecutores
- [x] Fase 6: Sistema de permisos (cadena de 5 niveles, auditor de registros, Lista de Control de Acceso Global)
- [x] Fase 7: Compilación dinámica + Autoevolución (Roslyn)
- [x] Fase 8: Memoria a largo plazo + Tareas + Temporizadores
- [x] Fase 9: Núcleo Principal + Colaboración multiagente
- [x] Fase 10: Web UI (HTTP + SSE, 24 controladores, 7 pieles)
- [x] Fase 10.5: Mejoras incrementales (Canal de Difusión, Auditoría de Tokens, 32 calendarios, mejoras de herramientas, localización para 34 variantes de idioma)
- [x] Fase 10.6: Perfeccionamiento y optimización (WebView, sistema de ayuda, espacio de trabajo de proyectos, red de conocimiento, motor de flujos de trabajo)
- [x] Fase 11: Motor de almacenamiento SpeedyPack (reemplazo de LiteDB, mapeo en memoria, cola de escritura asíncrona, compactación automática)
- [x] Fase 12: Sistema de plugins (interfaz IPlugin, declaración de capacidades PluginLoader, carga aislada, integración de herramientas)

### 🚧 Planificado
- [ ] Fase 13: Integración de mensajería instantánea externa (Feishu / WhatsApp / Telegram)
- [ ] Fase 14: Ecosistema de habilidades (mercado de plugins, distribución de paquetes de habilidades)

## 📚 Documentación

- [Diseño de Arquitectura](architecture.md) — Diseño del sistema, mecanismo de programación, arquitectura de componentes
- [Modelo de Seguridad](security.md) — Modelo de permisos, ejecutores, seguridad de compilación dinámica
- [Guía de Desarrollo](development-guide.md) — Desarrollo de herramientas, guía de extensión
- [Referencia de API](api-reference.md) — Documentación de endpoints de la API Web
- [Referencia de Herramientas](tools-reference.md) — Descripción detallada de las herramientas integradas
- [Guía de Web UI](web-ui-guide.md) — Guía de uso de la interfaz Web
- [Guía de Seres de Silicio](silicon-being-guide.md) — Guía de desarrollo de agentes
- [Sistema de Permisos](permission-system.md) — Explicación detallada de la gestión de permisos
- [Sistema de Calendario](calendar-system.md) — Descripción de los 32 sistemas de calendario
- [Inicio Rápido](getting-started.md) — Guía de introducción detallada
- [Solución de Problemas](troubleshooting.md) — Preguntas frecuentes
- [Hoja de Ruta](roadmap.md) — Plan de desarrollo completo
- [Registro de Cambios](changelog.md) — Historial de actualizaciones de versiones
- [Guía de Contribución](contributing.md) — Cómo participar en el proyecto

## 🤝 Contribuir

¡Agradecemos todas las formas de contribución! Consulte la [Guía de Contribución](contributing.md) para más detalles.

### Flujo de Trabajo de Desarrollo
1. Hacer fork de este repositorio
2. Crear una rama de funcionalidad (`git checkout -b feature/AmazingFeature`)
3. Confirmar los cambios (`git commit -m 'feat: add some AmazingFeature'`)
4. Empujar a la rama (`git push origin feature/AmazingFeature`)
5. Enviar un Pull Request

## 💡 Guía de Selección de Versión

### ¿Qué versión debería usar?

**SiliconLife.Default (Implementación por defecto — verificación de viabilidad de arquitectura):**
- 📌 Es su primer contacto con este proyecto y desea comprender rápidamente la arquitectura del sistema
- 📌 Está realizando depuración de desarrollo y necesita una forma de ejecución simple y directa
- 📌 La seguridad de datos es su principal consideración
- 📌 Su sistema tiene menos de 4GB de memoria
- 📌 Solo necesita uso individual o volúmenes de datos pequeños

**SiliconLife.Fast (Versión de producción recomendada):**
- ⚡ Necesita un entorno de producción con ejecución estable a largo plazo
- ⚡ Ya está familiarizado con la arquitectura del sistema y está listo para el despliegue formal
- ⚡ Necesita soportar acceso concurrente de múltiples usuarios
- ⚡ Necesita ejecución en segundo plano en la bandeja del sistema
- ⚡ Busca la experiencia de rendimiento más extrema

> **Recomendación general**: SiliconLife.Default es adecuado como verificación de arquitectura y experiencia de introducción; para entornos de producción reales, se recomienda encarecidamente usar SiliconLife.Fast.

### ¿Se puede migrar de Default a Fast?

¡**Por supuesto!** Ambas versiones comparten el mismo:
- ✅ Formato de archivo de configuración (config.json)
- ✅ Interfaz de herramientas
- ✅ Configuración de Being
- ✅ Interfaz Web UI

**Pasos de migración:**
1. Haga una copia de seguridad de su directorio de datos de Default
2. Inicie la versión Fast usando el mismo directorio de datos
3. Fast importará automáticamente los datos existentes al motor de almacenamiento SpeedyPack
4. Una vez verificada la funcionalidad correcta, puede usar la versión Fast para el uso diario

### ¿Pueden coexistir ambas versiones?

¡**Sí!** Se recomienda la siguiente estrategia de despliegue:

**Estrategia 1: Default para verificación, Fast para producción**
```
Entorno de desarrollo/verificación: SiliconLife.Default (verificar arquitectura, depurar funcionalidad)
Entorno de producción: SiliconLife.Fast (alto rendimiento, ejecución en segundo plano, procesamiento de solicitudes en tiempo real)
```

**Estrategia 2: Fast como ejecución principal, Default para copias de seguridad periódicas**
```
SiliconLife.Fast (uso diario, procesamiento de solicitudes en tiempo real)
    ↓ Copia de seguridad periódica
SiliconLife.Default (archivado de datos fríos, respaldo de seguridad de datos)
```

## 📄 Licencia

Este proyecto está licenciado bajo la Apache License 2.0 — consulte el archivo [LICENSE](../../LICENSE) para más detalles.

## 👨‍💻 Autor

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Agradecimientos

Gracias a todos los desarrolladores y proveedores de plataformas de IA que han contribuido a este proyecto.

---

**Silicon Life Collective** — Haciendo que los agentes de IA realmente "cobren vida"
