# Guía de Web UI

> **Versión: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | **Español** | [Deutsch](../de-DE/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md)

## Resumen

La Web UI proporciona una interfaz completa para gestionar Seres Silicona, monitorear el estado del sistema e interactuar con agentes de IA. El sistema utiliza una arquitectura de renderizado puro del lado del servidor, sin dependencias de frameworks frontend, generando HTML, CSS y JavaScript a través de los constructores `H`, `CssBuilder` y `JsBuilder`.

## Acceso

URL predeterminada: `http://localhost:8080`

## Navegación

### Secciones Principales

1. **Panel de Control** - Resumen del sistema y métricas
2. **Seres** - Gestionar Seres Silicona
3. **Chat** - Interactuar con seres (soporte para subida de archivos, SSE en tiempo real)
4. **Historial de Chat** - Ver historial de chat de Seres Silicona (lista de sesiones, detalles de mensajes)
5. **Tareas** - Gestión de tareas (tareas personales)
6. **Temporizadores** - Configuración de temporizadores (crear, pausar, historial de ejecución)
7. **Configuración** - Ajustes del sistema (clientes IA, localización)
8. **Permisos** - Control de acceso (gestión de ACL, consulta de permisos)
9. **Registros** - Registros del sistema (filtrar por nivel, consulta por rango de tiempo)
10. **Auditoría** - Uso de tokens y seguimiento de auditoría
11. **Memoria** - Memoria de seres (vista de línea de tiempo, filtrado avanzado)
12. **Conocimiento** - Base de conocimiento (gestión de tripletas, descubrimiento de rutas)
13. **Explorador de Código** - Exploración de código (árbol de archivos, resaltado de sintaxis)
14. **Editor de Código** - Edición de código con indicaciones flotantes (Monaco Editor)
15. **Proyecto** - Gestión de proyectos (espacio de trabajo, tareas, notas de trabajo)
16. **Ejecutores** - Gestión de ejecutores (disco, red, línea de comandos)
17. **Ayuda** - Sistema de documentos de ayuda (soporte multilingüe, búsqueda por tema)
18. **Acerca de** - Información del sistema y versión

---

## Panel de Control

### Funcionalidades

- Métricas de rendimiento del sistema (CPU, memoria, tiempo de ejecución)
- Resumen de estado de seres
- Estadísticas de uso de IA
- Acciones rápidas

### Actualizaciones en Tiempo Real

Usar SSE (Eventos Enviados por el Servidor) para obtener datos en tiempo real:

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
- Enlace al archivo de alma
- Acciones rápidas (iniciar/detener/configurar)

### Detalles del Ser

- Configuración completa
- Editor de archivo de alma
- Historial de tareas
- Visor de memoria
- Métricas de rendimiento

### Crear Ser

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
- Visualización de invocación de herramientas

### Usar el Chat

1. Seleccionar un ser
2. Escribir un mensaje
3. Ver respuestas en streaming
4. Ver ejecución de herramientas en tiempo real

### Visualización de Invocación de Herramientas

Cuando la IA invoca una herramienta:
```
🔧 Herramienta: calendar
📥 Entrada: {"date": "2026-04-20"}
📤 Salida: "农历四月初三"
```

---

## Configuración

### Clientes de IA

Configurar backends de IA:
- Ollama (local)
- Bailian (nube)
- Volcengine Ark (nube)
- Clientes personalizados

### Ajustes de Almacenamiento

- Versión Default: ruta base, indexación por tiempo, estrategia de limpieza
- Versión Fast: configuración del motor de almacenamiento SpeedyPack, gestión de archivos .spk, ajustes de auto-compresión

### Localización

Cambiar entre 29 variantes de idioma:
- Chino (6 variantes): Simplificado, Tradicional, Singapur, Macao, Taiwán, Malasia
- Inglés (10 variantes): EE.UU., Reino Unido, Canadá, Australia, India, Singapur, Sudáfrica, Irlanda, Nueva Zelanda, Malasia
- Alemán (5 variantes): Alemania, Austria, Suiza, Luxemburgo, Liechtenstein
- Francés (3 variantes): Francia, Canadá, Suiza
- Japonés, Coreano, Checo

---

## Sistema de Pieles

### Pieles Disponibles

1. **Admin** - Interfaz profesional de gestión
2. **Chat** - Diseño centrado en conversaciones
3. **Creative** - Estilo creativo y artístico
4. **Dev** - Layout orientado a desarrolladores
5. **HighContrast** - Tema de alto contraste (versión Fast)
6. **Minimal** - Estilo minimalista (versión Fast)
7. **Light** - Tema claro (versión Fast)

### Cambiar Piel

1. Hacer clic en **Configuración** (icono de engranaje)
2. Seleccionar **Piel**
3. Elegir la piel deseada
4. La interfaz se actualiza inmediatamente

### Piel Personalizada

Crear piel personalizada implementando `ISkin`:

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

### Añadir Regla de Permiso

1. Hacer clic en **Añadir Regla**
2. Configurar:
   - Usuario
   - Recurso (ej. `disk:read`)
   - Permitir/Denegar
   - Duración
3. Guardar

### Seguimiento de Auditoría

Ver todas las decisiones de permisos:
- Marca de tiempo
- Usuario
- Recurso
- Decisión
- Razón

---

## Gestión de Tareas

### Lista de Tareas

- Todas las tareas y su estado
- Filtrar por ser o estado
- Indicadores de prioridad

### Detalles de Tarea

- Descripción
- Prioridad
- Fecha límite
- Historial de ejecución
- Resultado de salida

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
- Próxima ejecución
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

- Filtrar por nivel (Información/Advertencia/Error)
- Buscar por palabra clave
- Selección de rango de tiempo
- Actualizaciones en tiempo real

### Detalles de Registro

Cada entrada de registro muestra:
- Marca de tiempo
- Nivel
- Origen
- Mensaje
- Traza de pila (para errores)

---

## Informe de Auditoría

### Uso de Tokens

- Total de tokens utilizados
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
- Indicaciones flotantes para identificadores
- Compilación en tiempo real

### Indicaciones Flotantes

Al pasar el cursor sobre cualquier identificador:
- Información de tipo
- Documentación
- Ubicación de definición
- Referencias

---

## Visualización de Historial de Chat

### Funcionalidades

- Navegación del historial de chat de Seres Silicona
- Lista de sesiones
- Detalles de mensajes
- Vista de línea de tiempo

### Usar Historial de Chat

1. Navegar a la página de **Seres**
2. Hacer clic en el enlace **Historial de Chat** del Ser Silicona
3. Ver lista de sesiones:
   - Título de la sesión
   - Fecha de creación
   - Número de mensajes
4. Hacer clic en una sesión para ver detalles:
   - Historial completo de mensajes
   - Marcas de tiempo
   - Información del remitente
   - Registros de invocación de herramientas

### Implementación Técnica

- **Controlador**: `ChatHistoryController`
- **Modelo de vista**: `ChatHistoryViewModel`
- **Vistas**:
  - `ChatHistoryListView` - Lista de sesiones
  - `ChatHistoryDetailView` - Detalles de mensajes
- **Rutas API**:
  - `/api/chat-history/{beingId}/conversations` - Obtener lista de sesiones
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Obtener detalles de mensajes

---

## Subida de Archivos

### Funcionalidades

- Diálogo de fuente de archivos
- Soporte para subida de múltiples archivos
- Gestión de metadatos de archivos
- Visualización del progreso de subida

### Usar Subida de Archivos

1. En la interfaz de chat, hacer clic en el botón **Subir Archivo**
2. Se abre el diálogo de fuente de archivos
3. Seleccionar fuente de archivos:
   - Archivo local
   - Ruta del sistema de archivos
4. Seleccionar archivos (soporte para selección múltiple)
5. Confirmar subida
6. La información del archivo se adjuntará al mensaje

### Tipos de Archivo Soportados

- Archivos de texto (.txt, .md, .json, .xml, etc.)
- Archivos de código (.cs, .js, .py, .java, etc.)
- Archivos de configuración (.yml, .yaml, .ini, .conf, etc.)
- Archivos de documento (.csv, .log, etc.)

---

## Indicador de Carga

### Funcionalidades

- Visualización del estado de carga de la página de chat
- Selección automática de la sesión del principal
- Retroalimentación del progreso de carga de datos

### Comportamiento

- Mostrar animación de carga al cargar la página
- Ocultar automáticamente cuando se completa la carga de datos
- Selección automática de la sesión del principal (si existe)
- Texto de indicación de carga multilingüe

---

## Sistema de Documentación de Ayuda (Nuevo)

### Resumen de Funcionalidades

El sistema de documentación de ayuda proporciona soporte de documentación multilingüe para Seres Silicona y usuarios.

### Usar la Documentación de Ayuda

1. Navegar a la página de **Ayuda**
2. Ver lista de temas de ayuda:
   - Guía de inicio rápido
   - Referencia de uso de herramientas
   - Guía de gestión de permisos
   - Manual de solución de problemas
   - Guía de desarrollo
3. Hacer clic en un tema para ver contenido detallado:
   - Contenido documental estructurado (renderizado Markdown)
   - Soporte multilingüe (sigue la configuración de localización del sistema)
   - Recomendación de temas relacionados
4. Usar la función de búsqueda para localizar rápidamente:
   - Búsqueda por palabra clave (soporta chino, inglés)
   - Resultados de búsqueda ordenados por relevancia

### Acceso de Seres Silicona a la Ayuda

Los Seres Silicona pueden acceder a la documentación de ayuda a través de la herramienta `help`:
```json
{
  "action": "get_topics"
}
```

### Implementación Técnica

- **Controlador**: `HelpController`
- **Herramienta**: `HelpTool`
- **Rutas API**:
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
   - Etiquetas del proyecto (categorización)
   - Estado del proyecto (en progreso, completado, archivado)
2. **Ver detalles del proyecto**:
   - Información básica del proyecto
   - Lista de tareas asociadas
   - Lista de notas de trabajo
   - Estadísticas de progreso del proyecto
3. **Archivar proyecto**: Conservar datos históricos pero ya no activo

### Notas de Trabajo (Privadas)

Notas de trabajo personales de los Seres Silicona, similares a un diario:

1. **Crear nota**:
   - Resumen (descripción breve)
   - Contenido (soporta formato Markdown)
   - Palabras clave (para búsqueda)
   - Registro automático de marca de tiempo
2. **Gestionar notas**:
   - Navegar por línea de tiempo (diseño paginado)
   - Buscar notas (por palabra clave, resumen, contenido)
   - Generar directorio (navegación rápida de la estructura de notas)
   - Actualizar y eliminar notas
3. **Control de permisos**:
   - Privadas por defecto, solo el ser mismo puede acceder
   - El Principal Silicona puede gestionar todas las notas

### Implementación Técnica

- **Controlador**: `WorkNoteController`
- **Herramientas**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Rutas API**:
  - `/api/worknotes` - Obtener lista de notas de trabajo
  - `/api/worknotes/{id}` - Obtener detalles de la nota
  - `/api/worknotes/search?q=keyword` - Buscar notas
  - `/api/worknotes/directory` - Generar directorio de notas
  - `/api/projects` - API de gestión de proyectos

---

## Diseño Responsivo

La Web UI se adapta a diferentes tamaños de pantalla:
- Escritorio: Layout completo
- Tablet: Barra lateral comprimida
- Móvil: Menú colapsable

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

### No Se Puede Conectar

**Verificar**:
- El servidor está ejecutándose
- El puerto 8080 no está bloqueado
- Configuración de firewall

### SSE No Funciona

**Verificar**:
- El navegador soporta SSE
- Sin buffer de proxy para SSE
- Estabilidad de red

### Rendimiento Lento

**Optimizar**:
- Reducir nivel de detalle de registros
- Limpiar datos de auditoría antiguos
- Verificar recursos del sistema

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Ver la [Guía de Desarrollo](development-guide.md)
- � Explorar la [Referencia de API](api-reference.md)
- 🚀 Ver la [Guía de Inicio Rápido](getting-started.md)
