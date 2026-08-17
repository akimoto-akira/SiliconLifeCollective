# Guía de Web UI

> **Versión: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | **Español** | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Русский](../ru-RU/web-ui-guide.md)

## Resumen

La Web UI proporciona una interfaz completa para gestionar Seres de Silicio, monitorear el estado del sistema e interactuar con agentes de IA. El sistema adopta una arquitectura de renderizado puro del lado del servidor, con cero dependencias de frameworks frontend, generando HTML, CSS y JavaScript a través de los constructores `H`, `CssBuilder` y `JsBuilder`.

## Acceso

URL predeterminada: `http://localhost:8080`

## Navegación

### Secciones Principales

1. **Panel** - Resumen del sistema y métricas
2. **Seres** - Gestión de Seres de Silicio
3. **Chat** - Interacción con los seres (soporta carga de archivos, SSE en tiempo real)
4. **Historial de Chat** - Ver historial de chat de los Seres de Silicio (lista de sesiones, detalles de mensajes)
5. **Tareas** - Gestión de tareas (tareas personales)
6. **Temporizadores** - Configuración de temporizadores (creación, pausa, historial de ejecución)
7. **Configuración** - Ajustes del sistema (cliente de IA, localización)
8. **Permisos** - Control de acceso (gestión de ACL, consulta de permisos)
9. **Registros** - Registros del sistema (filtro por nivel, consulta por rango de tiempo)
10. **Auditoría** - Uso de tokens y pista de auditoría
11. **Memoria** - Memoria de los seres (vista de línea temporal, filtrado avanzado)
12. **Conocimiento** - Base de conocimiento (gestión de tripletas, descubrimiento de rutas)
13. **Navegador de Código** - Exploración de código (árbol de archivos, resaltado de sintaxis)
14. **Editor de Código** - Edición de código con sugerencias flotantes (Monaco Editor)
15. **Proyectos** - Gestión de proyectos (espacio de trabajo, tareas, notas de trabajo)
16. **Ejecutores** - Gestión de ejecutores (disco, red, línea de comandos)
17. **Ayuda** - Sistema de documentación de ayuda (soporte multilingüe, búsqueda por tema)
18. **Acerca de** - Información del sistema y versión

---

## Panel

### Funcionalidades

- Métricas de rendimiento del sistema (CPU, memoria, tiempo de ejecución)
- Resumen del estado de los seres
- Estadísticas de uso de IA
- Acciones rápidas

### Actualizaciones en Tiempo Real

Usar SSE (Server-Sent Events) para datos en tiempo real:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Gestión de Seres

### Lista de Seres

Muestra todos los seres, incluyendo:
- Nombre e ID
- Estado actual (ejecutándose/detenido/error)
- Enlace al Archivo de Alma
- Acciones rápidas (iniciar/detener/configurar)

### Detalles del Ser

- Configuración completa
- Editor del Archivo de Alma
- Historial de tareas
- Visor de memoria
- Métricas de rendimiento

### Crear un Ser

1. Hacer clic en **Crear Nuevo Ser**
2. Completar:
   - Nombre
   - Contenido del alma (editor Markdown)
   - Configuración inicial
3. Hacer clic en **Crear**

---

## Interfaz de Chat

### Funcionalidades

- Flujo de mensajes en tiempo real
- Historial de mensajes
- Soporte para múltiples sesiones
- Visualización de llamadas a herramientas

### Usar el Chat

1. Seleccionar un ser
2. Ingresar un mensaje
3. Ver la respuesta en flujo
4. Ver la ejecución de herramientas en tiempo real

### Visualización de Llamadas a Herramientas

Cuando la IA llama a una herramienta:
```
🔧 Herramienta: calendar
📥 Entrada: {"date": "2026-04-20"}
📤 Salida: "Cuarto día del tercer mes del calendario lunar"
```

---

## Configuración

### Cliente de IA

Configurar el backend de IA:
- Ollama (local)
- Bailian (nube)
- Volcengine Ark (nube)
- Herdsman (local/nube, sin autenticación)
- Meituan LongCat (nube)
- Qiniu Cloud AI (nube)
- DeepSeek (nube, modo thinking, 1M contexto)
- Zhipu AI / GLM (nube, thinking + visión, modelo gratuito)
- Baidu Qianfan / Wenxin (nube, modelos gratuitos)
- Tencent Hunyuan (nube, doble endpoint, 262K contexto)
- MiniMax (nube, thinking, multimodal)
- Moonshot / Kimi (nube, thinking, multimodal)
- SiliconFlow (nube, 100+ modelos agregados)
- Cliente personalizado

### Plataformas IM (Multi-instancia)

La configuración de plataformas IM soporta una arquitectura de multi-instancia, permitiendo habilitar múltiples plataformas simultáneamente:

1. Hacer clic en **Agregar Plataforma** y seleccionar el tipo de plataforma:
   - **Web UI** (integrado, habilitado por defecto)
   - **Feishu** (soporta configuración manual y autorización OAuth con un clic)
   - **WeChat Enterprise** (configuración manual)
   - **DingTalk** (configuración manual, soporta modos de evento Stream / HTTP)
2. Completar los campos de la plataforma según el formulario dinámico (los campos obligatorios están marcados con asterisco, los campos de clave son cuadros de contraseña)
3. Cada instancia se puede habilitar/deshabilitar y eliminar de forma independiente

**Variables de entorno para claves**: Los valores de configuración soportan marcadores de posición `${ENV_VAR}` (por ejemplo, `"${FEISHU_APP_SECRET}"`), que se resuelven desde variables de entorno en tiempo de ejecución. Las claves en texto plano no se escriben en config.json.

**Asistente de autorización OAuth** (Feishu): Tras guardar appId/appSecret, aparece una zona de autorización en línea en la página de configuración. Al hacer clic en el botón **Autorizar**, se abre el navegador del sistema y redirige a la página de autorización de Feishu. Tras completar la autorización, los tokens se escriben automáticamente en la configuración, y la página muestra el estado de autorización en tiempo real a través de SSE (éxito/fallo/tiempo de espera agotado), sin necesidad de copiar y pegar manualmente.

### Servidores MCP

La lista de servidores MCP se gestiona centralmente en la página de configuración (editor de arreglos): cada fila es un servidor (ID, nombre, método de transporte stdio/http, comando o endpoint, estado de habilitación), con soporte para **Agregar** y **Eliminar** en línea. Tras guardar, el servidor se conecta inmediatamente y sus herramientas se inyectan automáticamente en los Seres de Silicio. Consulte la sección [Gestión MCP](#gestión-mcp) a continuación.

### Ajustes de Almacenamiento

- Versión Default: ruta base, índice temporal, estrategia de limpieza
- Versión Fast: configuración del motor de almacenamiento SpeedyPack, gestión de archivos .spk, ajustes de compactación automática

### Localización

Cambiar entre 34 variantes de idioma:
- Chino (6 variantes): simplificado, tradicional, Singapur, Macao, Taiwán, Malasia
- Inglés (10 variantes): estadounidense, británico, canadiense, australiano, indio, singapurense, sudafricano, irlandés, neozelandés, malayo
- Español (2 variantes): España, México
- Alemán (5 variantes): Alemania, Austria, Suiza, Luxemburgo, Liechtenstein
- Francés (3 variantes): Francia, Canadá, Suiza
- Japonés, coreano, checo
- Ruso, portugués (2 variantes), italiano, neerlandés, polaco, sueco

---

## Sistema de Pieles

### Pieles Disponibles

1. **Admin** - Interfaz de administración profesional
2. **Chat** - Diseño centrado en la conversación
3. **Creative** - Estilo creativo y artístico
4. **Dev** - Diseño orientado al desarrollador
5. **HighContrast** - Tema de alto contraste (versión Fast)
6. **Minimal** - Estilo minimalista (versión Fast)
7. **Light** - Tema claro (versión Fast)

### Cambiar Piel

1. Hacer clic en **Configuración** (icono de engranaje)
2. Seleccionar **Piel**
3. Elegir la piel deseada
4. La interfaz se actualiza inmediatamente

### Piel Personalizada

Crear una piel personalizada implementando `ISkin`:

```csharp
public class MySkin : ISkin
{
    public string Name => "MySkin";
    
    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
}
```

---

## Gestión de Permisos

### Ver Permisos

- Listar todas las reglas de permisos
- Filtrar por usuario o recurso
- Ver fechas de expiración

### Agregar Regla de Permiso

1. Hacer clic en **Agregar Regla**
2. Configurar:
   - Tipo de permiso (por ejemplo, `FileAccess`, `NetworkAccess`)
   - Prefijo de recurso (por ejemplo, `C:\Projects`, `api.github.com`)
   - Permitir/Denegar
   - Descripción
3. Guardar

### Pista de Auditoría

Ver todas las decisiones de permisos:
- Marca de tiempo
- Usuario
- Recurso
- Decisión
- Razón

### Gestión de Permisos de Herramientas

Gestionar los permisos de operaciones de herramientas de los Seres de Silicio y los proyectos:

1. **Permisos de herramientas del Ser de Silicio**:
   - Navegar a **Seres** → Seleccionar ser → **Permisos de Herramientas**
   - Ver la configuración de permisos actual
   - Establecer permitir/denegar por operación
   - Aplicar plantilla de permisos (readonly/restricted/full)

2. **Permisos de herramientas del proyecto**:
   - Navegar a **Proyectos** → Seleccionar proyecto → **Permisos de Herramientas**
   - Los permisos de herramientas a nivel de proyecto son independientes del nivel del Ser de Silicio
   - Logran aislamiento de permisos entre proyectos

---

## Gestión de Tareas

### Lista de Tareas

- Todas las tareas y su estado
- Filtrar por ser o estado
- Indicadores de prioridad

### Detalles de la Tarea

- Descripción
- Prioridad
- Fecha límite
- Historial de ejecución
- Resultado

### Crear Tarea

1. Hacer clic en **Crear Tarea**
2. Completar:
   - Asignación de ser
   - Descripción
   - Prioridad (1-10)
   - Fecha límite
3. Crear

---

## Gestión de Temporizadores

### Temporizadores Activos

- Lista de temporizadores en ejecución
- Próxima hora de ejecución
- Estado de repetición

### Crear Temporizador

1. Hacer clic en **Crear Temporizador**
2. Configurar:
   - Asignación de ser
   - Intervalo o expresión cron
   - Acción a ejecutar
   - Configuración de repetición
3. Iniciar

---

## Visor de Registros

### Funcionalidades

- Filtrar por nivel (información/advertencia/error)
- Buscar por palabra clave
- Selección de rango de tiempo
- Actualizaciones en tiempo real

### Detalles del Registro

Cada entrada de registro muestra:
- Marca de tiempo
- Nivel
- Origen
- Mensaje
- Seguimiento de pila (para errores)

---

## Informes de Auditoría

### Uso de Tokens

- Tokens totales usados
- Desglose por modelo
- Cálculo de costos
- Gráficos basados en tiempo

### Exportar Informe

Descargar datos de auditoría:
- Formato CSV
- Selección de rango de fechas
- Filtrar por ser o modelo

---

## Editor de Código

### Funcionalidades

- Resaltado de sintaxis (Monaco Editor)
- Autocompletado de código
- Sugerencias flotantes para identificadores
- Compilación en tiempo real

### Sugerencias Flotantes

Pasar el cursor sobre cualquier identificador para ver:
- Información de tipo
- Documentación
- Ubicación de la definición
- Referencias

---

## Visualización del Historial de Chat

### Funcionalidades

- Navegación del historial de chat de los Seres de Silicio
- Visualización de la lista de sesiones
- Visualización de los detalles de los mensajes
- Vista de línea temporal

### Usar el Historial de Chat

1. Navegar a la página **Seres**
2. Hacer clic en el enlace **Historial de Chat** del Ser de Silicio
3. Ver la lista de sesiones:
   - Título de la sesión
   - Hora de creación
   - Cantidad de mensajes
4. Hacer clic en una sesión para ver los detalles:
   - Historial completo de mensajes
   - Marcas de tiempo
   - Información del remitente
   - Registros de llamadas a herramientas

### Implementación Técnica

- **Controlador**: `ChatHistoryController`
- **Modelo de vista**: `ChatHistoryViewModel`
- **Vistas**:
  - `ChatHistoryListView` - Lista de sesiones
  - `ChatHistoryDetailView` - Detalles de mensajes
- **Rutas de API**:
  - `/api/chat-history/{beingId}/conversations` - Obtener lista de sesiones
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Obtener detalles de mensajes

---

## Carga de Archivos

### Funcionalidades

- Diálogo de origen de archivos
- Soporte para carga de múltiples archivos
- Gestión de metadatos de archivos
- Visualización del progreso de carga

### Usar la Carga de Archivos

1. Hacer clic en el botón **Cargar Archivo** en la interfaz de chat
2. Se abre el diálogo de origen de archivos
3. Seleccionar el origen del archivo:
   - Archivo local
   - Ruta del sistema de archivos
4. Seleccionar archivos (soporta selección múltiple)
5. Confirmar la carga
6. La información del archivo se adjuntará al mensaje

### Tipos de Archivo Soportados

- Archivos de texto (.txt, .md, .json, .xml, etc.)
- Archivos de código (.cs, .js, .py, .java, etc.)
- Archivos de configuración (.yml, .yaml, .ini, .conf, etc.)
- Archivos de documento (.csv, .log, etc.)

---

## Indicador de Carga

### Funcionalidades

- Visualización del estado de carga en la página de chat
- Selección automática de la sesión del Curador
- Retroalimentación del progreso de carga de datos

### Comportamiento

- Mostrar animación de carga al cargar la página
- Ocultar automáticamente cuando se completa la carga de datos
- La sesión del Curador se selecciona automáticamente (si existe)
- Texto de indicación de carga multilingüe

---

## Sistema de Documentación de Ayuda (Nuevo)

### Resumen de Funcionalidades

El sistema de documentación de ayuda proporciona soporte de documentación multilingüe para los Seres de Silicio y los usuarios.

### Usar la Documentación de Ayuda

1. Navegar a la página **Ayuda**
2. Ver la lista de temas de ayuda:
   - Guía de inicio rápido
   - Referencia de uso de herramientas
   - Guía de gestión de permisos
   - Manual de solución de problemas
   - Guía de desarrollo
3. Hacer clic en un tema para ver el contenido detallado:
   - Contenido documental estructurado (renderizado Markdown)
   - Soporte multilingüe (sigue la configuración de localización del sistema)
   - Recomendación de temas relacionados
4. Usar la función de búsqueda para localizar rápidamente:
   - Búsqueda por palabra clave (soporta chino e inglés)
   - Resultados de búsqueda ordenados por relevancia

### Acceso de los Seres de Silicio a la Ayuda

Los Seres de Silicio pueden acceder a la documentación de ayuda a través de la herramienta `help`:
```json
{
  "action": "get_topics"
}
```

### Implementación Técnica

- **Controlador**: `HelpController`
- **Herramienta**: `HelpTool`
- **Rutas de API**:
  - `/api/help` - Obtener lista de temas de ayuda
  - `/api/help/{topicId}` - Obtener detalles del tema
  - `/api/help/search?q=keyword` - Buscar documentación de ayuda

---

## Espacio de Trabajo de Proyectos (Nuevo)

### Resumen de Funcionalidades

El espacio de trabajo de proyectos proporciona un entorno de trabajo estructurado que soporta gestión de proyectos, seguimiento de tareas y notas de trabajo.

### Gestión de Proyectos

1. **Crear proyecto**:
   - Nombre y descripción del proyecto
   - Etiquetas del proyecto (clasificación)
   - Estado del proyecto (en progreso, completado, archivado)
2. **Ver detalles del proyecto**:
   - Información básica del proyecto
   - Lista de tareas asociadas
   - Lista de notas de trabajo
   - Estadísticas de progreso del proyecto
3. **Archivar proyecto**: Conserva datos históricos pero ya no está activo
4. **Gestión de roles del proyecto**:
   - Asignar roles de proyecto a los Seres de Silicio (por ejemplo, developer, reviewer, manager)
   - Eliminar asignaciones de roles
   - Ver lista de miembros y roles del proyecto
5. **Flujos de trabajo del proyecto**:
   - Ver lista de plantillas de flujo de trabajo
   - Vincular plantilla de flujo de trabajo al proyecto
   - Ver estado de la instancia del flujo de trabajo
   - Ver registros de ejecución del flujo de trabajo

### Notas de Trabajo (Privadas)

Notas de trabajo personales de los Seres de Silicio, similares a un diario:

1. **Crear nota**:
   - Resumen (descripción breve)
   - Contenido (soporta formato Markdown)
   - Palabras clave (para búsqueda)
   - Registro automático de marca de tiempo
2. **Gestionar notas**:
   - Navegar por línea temporal (diseño por páginas)
   - Buscar notas (por palabra clave, resumen, contenido)
   - Generar directorio (navegación rápida de la estructura de notas)
   - Actualizar y eliminar notas
3. **Control de permisos**:
   - Privadas por defecto, solo el ser mismo puede acceder
   - El Curador de Silicio puede gestionar todas las notas

### Implementación Técnica

- **Controlador**: `WorkNoteController`
- **Herramientas**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Rutas de API**:
  - `/api/worknotes` - Obtener lista de notas de trabajo
  - `/api/worknotes/{id}` - Obtener detalles de la nota
  - `/api/worknotes/search?q=keyword` - Buscar notas
  - `/api/worknotes/directory` - Generar directorio de notas
  - `/api/projects` - API de gestión de proyectos

---

## Gestión de Habilidades

### Resumen de Funcionalidades

Las Habilidades (Skill) son unidades de capacidad reutilizables que combinan "orquestación de herramientas + plantilla de prompts". La página de gestión de habilidades (`/skill?beingId={id}`) proporciona gestión visual de habilidades para cada Ser de Silicio.

### Diseño de Página

- **Lista de habilidades a la izquierda**: visualización en tarjetas (título, insignia `versión · origen · modo de disparador`, descripción)
- **Editor a la derecha**: editor Markdown (metadatos frontales YAML + cuerpo del prompt)
- **Estadísticas superiores**: total de habilidades / habilidades personalizadas / límite de cuota (por ejemplo, `5 / 2 / 50`)

### Operaciones de Barra de Herramientas

- **Nuevo**: carga una plantilla de habilidad Markdown
- **Importar .md / Importar .json**: importa habilidades desde archivos locales
- **Actualizar**: recarga la lista de habilidades

### Operaciones de Tarjeta de Habilidad

Cada tarjeta de habilidad proporciona 5 operaciones:

| Operación | Descripción |
|------|------|
| Editar | Abre el Markdown en el editor derecho, se puede guardar (upsert) |
| Probar | Ingresa parámetros JSON, ejecuta la habilidad inmediatamente y muestra el resultado |
| Exportar JSON | Descarga `{id}.json` |
| Exportar Markdown | Descarga `{id}.md` |
| Eliminar | Elimina la habilidad (incluyendo archivos persistentes) |

### Escribir Habilidades

Las habilidades se escriben en Markdown. Los metadatos frontales YAML declaran id, descripción, esquema de parámetros, lista blanca de herramientas, modo de disparador, etc.; el cuerpo es la plantilla de prompt (soporta marcadores de posición `{param}`). También es posible escribir solo el cuerpo (omitiendo YAML) — al guardar, la IA completará automáticamente los metadatos faltantes, y los campos proporcionados por el usuario nunca serán sobrescritos.

```markdown
---
id: daily_news_digest
description: Buscar noticias tecnológicas de hoy y generar un resumen
tool_whitelist: [network, work_note]
trigger_mode: Auto
metadata:
  schedule: "0 9 * * *"
---

Por favor, usa la herramienta network para buscar las últimas noticias sobre {topic}, genera un resumen de 500 caracteres y guárdalo en las notas de trabajo.
```

### Implementación Técnica

- **Controlador**: `SkillController` (página + 10 endpoints de API)
- **Núcleo**: `SkillManager` (registro/ejecución/recarga en caliente), `SkillMetadataCompleter` (completado de metadatos por IA)
- **Recarga en caliente**: el ser detecta cambios en el directorio `skills/` cada 30 segundos, no es necesario reiniciar tras guardar en la Web UI
- **Archivado de versiones**: cada actualización se archiva automáticamente en `skills/archive/{id}/{version}.md`

---

## Gestión MCP

### Resumen de Funcionalidades

La página de gestión de MCP (Model Context Protocol, Protocolo de Contexto de Modelo) (`/mcp`) se utiliza para gestionar conexiones a servidores MCP externos. Tras la conexión, las herramientas proporcionadas por el servidor se inyectan automáticamente en todos los Seres de Silicio con el formato `mcp_{serverId}_{toolName}`.

### Lista de Servidores

Muestra para cada servidor: ID, nombre, método de transporte (stdio/http), estado de conexión (connected/disconnected/connecting/error), estado de habilitación, cantidad de herramientas, último error.

### Operaciones de Gestión

| Operación | Descripción |
|------|------|
| Agregar servidor | Completa ID (minúsculas/números/guion bajo), nombre, método de transporte; stdio requiere comando y argumentos, http requiere URL de endpoint |
| Habilitar/Deshabilitar | Interruptor en línea, al deshabilitar las herramientas se eliminan de todos los seres |
| Reconectar | Desconecta y reconecta, actualiza la lista de herramientas |
| Eliminar | Elimina la configuración del servidor y todas sus herramientas |
| Ver herramientas | Expande el servidor, lista los nombres de herramientas (con prefijo), descripciones, esquema de parámetros |
| Probar herramienta | Invoca directamente una herramienta MCP para verificar la conectividad (sin participación de IA) |

### Ejemplo de Adición de Servidor stdio

```json
{
  "id": "filesystem",
  "name": "Filesystem",
  "transport": "stdio",
  "command": "npx",
  "arguments": ["-y", "@modelcontextprotocol/server-filesystem", "/data"],
  "enabled": true
}
```

### Mecanismo de Seguridad

- La adición/eliminación/habilitación/deshabilitación de servidores solo puede ser realizada por el usuario a través de la Web UI; la IA no puede modificar la lista de servidores (la herramienta `mcp` solo proporciona consultas de solo lectura)
- Las herramientas de envoltura MCP se presentan en la matriz de permisos de herramientas con una única acción `execute`, que se puede deshabilitar individualmente por ser/proyecto
- El interruptor global `McpEnabled` puede desactivar con un clic toda la integración MCP

---

## Diseño Responsivo

La Web UI se adapta a diferentes tamaños de pantalla:
- Escritorio: Diseño completo
- Tablet: Barra lateral comprimida
- Móvil: Menú plegable

---

## Atajos de Teclado

| Atajo | Acción |
|----------|--------|
| `Ctrl+K` | Búsqueda rápida |
| `Ctrl+B` | Alternar barra lateral |
| `Ctrl+Enter` | Enviar mensaje |
| `Esc` | Cancelar/Cerrar |

---

## Solución de Problemas

### No se puede conectar

**Verificar**:
- El servidor está ejecutándose
- El puerto 8080 no está bloqueado
- Configuración del firewall

### SSE no funciona

**Verificar**:
- El navegador soporta SSE
- Sin almacenamiento en búfer del proxy para SSE
- Estabilidad de la red

### Rendimiento lento

**Optimizar**:
- Reducir el nivel de detalle de los registros
- Limpiar datos de auditoría antiguos
- Verificar los recursos del sistema

---

## Siguientes Pasos

- 📚 Leer la [guía de arquitectura](architecture.md)
- 🛠️ Consultar la [guía de desarrollo](development-guide.md)
- 📖 Explorar la [referencia de API](api-reference.md)
- 🚀 Ver la [guía de inicio rápido](getting-started.md)
