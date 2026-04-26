# Silicon Life Collective

**Silicon Life Collective** — Una plataforma de colaboración multiagente basada en .NET 9, donde los agentes de IA se denominan **Ser Silicona**, capaces de auto-evolucionarse mediante compilación dinámica Roslyn.

[English](../README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | **Español** | [Deutsch](../de-DE/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 Características Principales

### Sistema de Agentes
- **Orquestación Multiagente** — Gestión unificada por el *Curador Silicona*, con mecanismo de programación justa por intervalo de tiempo impulsado por reloj
- **Impulsado por Archivo de Alma** — Cada Ser Silicona es impulsado por un archivo de indicación central (`soul.md`), definiendo personalidad y patrones de comportamiento únicos
- **Arquitectura Cuerpo-Cerebro** — El *Cuerpo* (SiliconBeing) mantiene signos vitales y detecta escenarios de activación; el *Cerebro* (ContextManager) carga historial, invoca IA, ejecuta herramientas y persiste respuestas
- **Capacidad de Auto-Evolución** — Mediante tecnología de compilación dinámica Roslyn, los Seres Silicona pueden reescribir su propio código para evolucionar

### Herramientas y Ejecución
- **23 Herramientas Integradas** — Cubren calendario, chat, configuración, disco, red, memoria, tareas, temporizadores, base de conocimientos, notas de trabajo, navegador WebView, etc.
- **Ciclo de Invocación de Herramientas** — IA devuelve invocación de herramienta → Ejecutar herramienta → Retroalimentar resultados a IA → Ciclo continuo hasta devolver respuesta de texto puro
- **Seguridad Ejecutor-Permiso** — Todas las operaciones de E/S pasan por verificación estricta de permisos a través de ejecutores
  - Cadena de permisos de 5 niveles: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Registro de auditoría completo para todas las decisiones de permisos

### IA y Conocimiento
- **Soporte para Múltiples Backends de IA**
  - **Ollama** — Despliegue local de modelos, usando API HTTP nativa
  - **Alibaba Cloud Bailian (DashScope)** — Servicio de IA en la nube, compatible con API OpenAI, soporte para 13+ modelos, despliegue multi-región
- **32 Sistemas de Calendario** — Cobertura completa de los principales calendarios globales, incluyendo Gregoriano, Lunar Chino, Islámico, Hebreo, Japonés, Persa, Maya, Calendario Histórico Chino, etc.
- **Sistema de Red de Conocimiento** — Gráfico de conocimiento basado en tripletas (sujeto-relación-objeto), soportando almacenamiento, consulta y descubrimiento de rutas

### Interfaz Web
- **Web UI Moderna** — Servidor HTTP integrado, soporte para actualizaciones en tiempo real SSE
- **4 Temas de Piel** — Versión de gestión, versión de chat, versión de creación, versión de desarrollo, soporte para descubrimiento y cambio automáticos
- **20+ Controladores** — Funcionalidad completa de gestión del sistema, chat, configuración y monitoreo
- **Sin Dependencias de Framework Frontend** — Generación de HTML/CSS/JS en el servidor mediante `H`, `CssBuilder` y `JsBuilder`

### Internacionalización y Localización
- Soporte completo para **21 variantes de idioma**
  - Chino: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6 variantes)
  - Inglés: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - Español: es-ES, es-MX (2 variantes)
  - Japonés: ja-JP | Coreano: ko-KR | Checo: cs-CZ

### Datos y Almacenamiento
- **Sin Dependencia de Base de Datos** — Almacenamiento puro en sistema de archivos (formato JSON)
- **Consulta Indexada por Tiempo** — Soporte para consultas eficientes por rango de tiempo a través de la interfaz `ITimeStorage`
- **Dependencias Mínimas** — La biblioteca central solo depende de Microsoft.CodeAnalysis.CSharp para compilación dinámica

## 🛠️ Stack Tecnológico

| Componente | Tecnología |
|------|------|
| Runtime | .NET 9 |
| Lenguaje de Programación | C# |
| Integración IA | Ollama (local), Alibaba Cloud Bailian (nube) |
| Almacenamiento de Datos | Sistema de archivos (JSON + directorios indexados por tiempo) |
| Servidor Web | HttpListener (integrado en .NET) |
| Compilación Dinámica | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatización de Navegador | Playwright (WebView) |
| Licencia | Apache-2.0 |

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
│   └── SiliconLife.Default/               # Implementación predeterminada + punto de entrada de la aplicación
│       ├── Program.cs                     # Punto de entrada (ensambla todos los componentes)
│       ├── AI/                            # Cliente Ollama, cliente Bailian
│       ├── Calendar/                      # 32 implementaciones de calendario
│       ├── Config/                        # Datos de configuración predeterminados
│       ├── Executors/                     # Implementaciones predeterminadas de ejecutores
│       ├── Help/                          # Sistema de documentos de ayuda
│       ├── IM/                            # Proveedor WebUI
│       ├── Knowledge/                     # Implementación de red de conocimiento
│       ├── Localization/                  # Localización en 21 idiomas
│       ├── Logging/                       # Implementaciones de proveedores de registro
│       ├── Project/                       # Implementación del sistema de proyectos
│       ├── Runtime/                       # Objeto de reloj de prueba
│       ├── Security/                      # Callbacks de permisos predeterminados
│       ├── SiliconBeing/                  # Implementación predeterminada de Ser Silicona
│       ├── Storage/                       # Implementación de almacenamiento en sistema de archivos
│       ├── Tools/                         # 23 implementaciones de herramientas integradas
│       ├── WebView/                       # Implementación Playwright WebView
│       └── Web/                           # Implementación Web UI
│           ├── Controllers/               # 20+ controladores
│           ├── Models/                    # Modelos de vista
│           ├── Views/                     # Vistas HTML
│           └── Skins/                     # 4 temas de piel
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

```bash
dotnet run --project src/SiliconLife.Default
```

La aplicación iniciará el servidor web y abrirá automáticamente la Web UI en el navegador.

### Publicar como Archivo Único

```bash
# Windows
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS
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
- [x] Fase 10: Web UI (HTTP + SSE, 20+ controladores, 4 pieles)
- [x] Fase 10.5: Mejoras incrementales (canales de broadcast, auditoría de tokens, 32 calendarios, mejoras de herramientas, localización en 21 idiomas)
- [x] Fase 10.6: Perfeccionamiento y optimización (WebView, sistema de ayuda, espacio de trabajo de proyectos, red de conocimiento)

### 🚧 Planificado
- [ ] Fase 11: Integración de mensajería instantánea externa (Feishu / WhatsApp / Telegram)
- [ ] Fase 12: Sistema de plugins y ecosistema de habilidades

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
