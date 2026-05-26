# Guía de Seres de Silicio

> **Versión: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | **Español** | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## Resumen

Los Seres de Silicio son agentes impulsados por IA que pueden pensar, actuar y evolucionar de forma autónoma.

## Arquitectura

### Separación Cuerpo-Cerebro

```
┌─────────────────────────────────────┐
│         Ser de Silicio               │
├──────────────────┬──────────────────┤
│   Cuerpo         │   Cerebro         │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Gestión de     │ • Cargar historial│
│   estado         │ • Invocar IA      │
│ • Detección de   │ • Ejecutar        │
│   activación     │   herramientas   │
│ • Ciclo de vida  │ • Persistir       │
│                  │   respuestas      │
└──────────────────┴──────────────────┘
```

## Archivo de Alma

### Estructura

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### Ejemplo

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## Crear un Ser

### A través de la Web UI

1. Navegar a **Gestión de Seres**
2. Hacer clic en **Crear Nuevo Ser**
3. Completar:
   - Nombre
   - Contenido del alma
   - Opciones de configuración
4. Hacer clic en **Crear**

### A través de la API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Ciclo de Vida del Ser

### Estados de Actividad

Los Seres de Silicio tienen los siguientes estados de actividad:

| Estado | Descripción |
|------|------|
| `Idle` | Estado inactivo, esperando activación del reloj |
| `SingleChat` | En chat uno a uno |
| `GroupChat` | En chat grupal |
| `Task` | Ejecutando tarea |
| `Timer` | Ejecutando temporizador |
| `Stopped` | Detenido, por errores consecutivos o detención manual |

**Mecanismo de estado Stopped**:
- Cuando un Ser de Silicio tiene 10 errores consecutivos, entra automáticamente en estado `Stopped`
- Tras entrar en estado Stopped, el ser ya no ejecutará ninguna tarea
- Cuando llega un nuevo mensaje de chat, el contador de errores se reinicia y el ser reanuda su ejecución
- También se puede reiniciar mediante intervención manual

### Transiciones de Estado

```
Idle → SingleChat → Idle (chat completado)
Idle → GroupChat → Idle (chat grupal completado)
Idle → Task → Idle (tarea completada)
Idle → Timer → Idle (temporizador completado)
Cualquiera → Stopped (10 errores consecutivos)
Stopped → Idle (nuevo mensaje de chat o reinicio manual)
```

### Operaciones

- **Iniciar**: Inicializar y comenzar a procesar
- **Detener**: Apagado elegante
- **Reiniciar**: Recuperar del estado Stopped al estado Idle

## Sistema de Tareas

### Crear Tarea

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Estados de Tarea

- `Pending` - Esperando ejecución
- `Running` - En ejecución
- `SubmittedForReview` - Enviado para revisión
- `UnderReview` - En revisión
- `Rework` - Retrabajo
- `Completed` - Completado exitosamente
- `Failed` - Ejecución fallida
- `Cancelled` - Cancelado manualmente

## Sistema de Temporizadores

### Tipos de Temporizadores

1. **Una sola vez**: Se ejecuta una vez después de un retraso
2. **Intervalo**: Se repite a intervalos fijos
3. **Cron**: Se ejecuta basándose en expresiones cron

### Ejemplo

```csharp
// Ejecutar cada hora
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## Sistema de Memoria

### Tipos de Memoria

- **Corto plazo**: Contexto de conversación actual
- **Largo plazo**: Conocimiento y experiencias persistidas
- **Episódica**: Eventos e interacciones indexados por tiempo

### Estructura de Almacenamiento

Versión Default:
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Versión Fast (almacenamiento SpeedyPack):
```
data/
├── speedy/
│   ├── {being-id}.spk       # Archivo de almacenamiento SpeedyPack
│   └── {being-id}.spk.idx   # Archivo de índice
└── beings/
    └── {being-id}/
        └── soul.md
```

## Sistema de Notas de Trabajo

### Resumen

Las notas de trabajo son el sistema de diario personal de los Seres de Silicio, con diseño por páginas, utilizado para registrar el progreso del trabajo, notas de aprendizaje, notas de proyectos, etc.

### Características

- **Gestión por páginas**: Cada nota es una página independiente, accesible por número de página
- **Soporte Markdown**: El contenido soporta formato Markdown (texto, listas, tablas, bloques de código)
- **Índice de palabras clave**: Soporta agregar palabras clave a las notas para facilitar la búsqueda
- **Función de resumen**: Cada nota tiene un breve resumen para navegación rápida
- **Generación de directorio**: Se puede generar un directorio general de todas las notas para ayudar a comprender el contexto general
- **Marcas de tiempo**: Registra automáticamente las fechas de creación y actualización
- **Privado por defecto**: Solo el ser mismo puede acceder (el Curador puede gestionar)

### Casos de Uso

1. **Registro de progreso del proyecto**
   ```
   Resumen: Completado el módulo de autenticación de usuarios
   Contenido: Implementada verificación de token JWT, integración OAuth2, mecanismo de refresh token
   Palabras clave: autenticación,JWT,OAuth2
   ```

2. **Notas de aprendizaje**
   ```
   Resumen: Aprendizaje de mejores prácticas de programación asíncrona en C#
   Contenido: Precauciones de uso de async/await, escenarios de uso de ConfigureAwait...
   Palabras clave: C#,asíncrono,mejores prácticas
   ```

3. **Actas de reunión**
   ```
   Resumen: Reunión de discusión de requisitos del producto
   Contenido: Se discutieron los requisitos de nuevas funcionalidades, se determinó el plan de implementación...
   Palabras clave: producto,requisitos,reunión
   ```

### Uso a través de Herramientas

Los seres pueden gestionar las notas de trabajo a través de la herramienta `work_note`:

```json
// Crear nota
{
  "action": "create",
  "summary": "Completado el módulo de autenticación de usuarios",
  "content": "## Detalles de implementación\n\n- Uso de token JWT\n- Soporte para OAuth2",
  "keywords": "autenticación,JWT,OAuth2"
}

// Leer nota
{
  "action": "read",
  "page_number": 1
}

// Buscar notas
{
  "action": "search",
  "keyword": "autenticación",
  "max_results": 10
}
```

### Gestión a través de la Web UI

1. Navegar a **Gestión de Seres** → Seleccionar ser
2. Hacer clic en la pestaña **Notas de Trabajo**
3. Se pueden ver, buscar y editar notas
4. Soporta vista previa de Markdown

## Sistema de Red de Conocimiento

### Resumen

La red de conocimiento es un sistema de representación y gestión del conocimiento basado en estructura de tripletas (sujeto-predicado-objeto), utilizado para almacenar y gestionar conocimiento estructurado.

### Conceptos Centrales

#### Estructura Triplete

```
Sujeto (Subject) --Predicado (Predicate)--> Objeto (Object)
```

**Ejemplos**:
- `Python` --`is_a`--> `programming_language`
- `Pekín` --`capital_of`--> `China`
- `Agua` --`boiling_point`--> `100°C`

#### Confianza

Cada tripleta de conocimiento tiene una puntuación de confianza (0.0-1.0), que indica la credibilidad del conocimiento:
- `1.0`: Absolutamente seguro (por ejemplo, teoremas matemáticos)
- `0.8-0.99`: Altamente confiable (por ejemplo, hechos verificados)
- `0.5-0.79`: Confiabilidad media (por ejemplo, inferencias o hipótesis)
- `<0.5`: Baja confiabilidad (por ejemplo, conjeturas o información no verificada)

#### Sistema de Etiquetas

Soporta agregar etiquetas a las tripletas para facilitar la clasificación y búsqueda:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Operaciones de Conocimiento

#### 1. Agregar Conocimiento

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. Consultar Conocimiento

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Buscar Conocimiento

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Descubrir Ruta de Conocimiento

Encontrar la ruta de asociación entre dos conceptos:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Retorna:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validar Conocimiento

Verificar la validez y consistencia del conocimiento:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Estadísticas de Conocimiento

Obtener estadísticas generales de la red de conocimiento:
```json
{
  "action": "stats"
}
```

Retorna:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Casos de Uso

1. **Almacenamiento de hechos**
   - Almacenar hechos objetivos y conocimiento común
   - Ejemplo: `Tierra` --`is_a`--> `planeta`

2. **Relaciones entre conceptos**
   - Registrar relaciones entre conceptos
   - Ejemplo: `Herencia` --`is_a`--> `concepto de programación orientada a objetos`

3. **Acumulación de aprendizaje**
   - Los seres acumulan conocimiento continuamente a través del aprendizaje
   - Forman un sistema de conocimiento estructurado

4. **Soporte de razonamiento**
   - Descubrir relaciones indirectas a través de rutas de conocimiento
   - Soportar razonamiento y toma de decisiones basados en conocimiento

### Gestión a través de la Web UI

1. Navegar a la página **Red de Conocimiento**
2. Ver estadísticas de conocimiento
3. Buscar y navegar el conocimiento
4. Visualización del grafo de relaciones de conocimiento (planificado)

## Operaciones del Navegador WebView (Nuevo)

### Resumen

Los Seres de Silicio pueden navegar páginas web de forma autónoma, obtener información y ejecutar operaciones web a través de la herramienta del navegador WebView. El navegador se ejecuta en modo sin cabeza, completamente invisible para el usuario.

### Características

- **Aislamiento individual**: Cada ser tiene su propia instancia de navegador, cookies y sesión
- **Modo sin cabeza**: Operación autónoma en segundo plano, invisible para el usuario
- **Funcionalidad completa**: Soporta ejecución de JavaScript, renderizado CSS, llenado de formularios, etc.
- **Control de seguridad**: Todas las operaciones deben pasar por la cadena de verificación de permisos

### Operaciones Comunes

#### 1. Abrir el navegador

```json
{
  "action": "open"
}
```

#### 2. Navegar a una página web

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Obtener contenido de la página

```json
{
  "action": "get_page_text"
}
```

Retorna el contenido de texto de la página para que la IA lo analice y comprenda.

#### 4. Hacer clic en un elemento

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Ingresar texto

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "palabra clave de búsqueda"
}
```

#### 6. Ejecutar JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Obtener captura de pantalla

```json
{
  "action": "get_screenshot"
}
```

Retorna una captura de pantalla de la página (codificada en Base64), que puede usarse para análisis visual.

#### 8. Esperar que aparezca un elemento

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Casos de Uso

1. **Obtención de información**
   - Navegar sitios de noticias para obtener las últimas informaciones
   - Consultar documentación y recursos técnicos
   - Monitorear cambios en el contenido de páginas web

2. **Operaciones automatizadas**
   - Llenar y enviar formularios
   - Hacer clic en botones para activar operaciones
   - Extraer datos de páginas web

3. **Análisis web**
   - Analizar la estructura y contenido de páginas
   - Extraer información específica
   - Análisis visual mediante capturas de pantalla

### Precauciones

- Las operaciones del navegador pueden ser lentas, es necesario esperar a que la página se cargue completamente
- Usar `wait_for_element` para asegurar que el elemento aparezca antes de operar
- Respetar los términos de uso de los sitios web y el archivo robots.txt
- Evitar solicitudes frecuentes que puedan resultar en bloqueos

## Mejores Prácticas

### Escritura del Archivo de Alma

1. **Ser específico**: Rasgos de personalidad y límites claros
2. **Definir el alcance**: Lo que el ser debería y no debería hacer
3. **Incluir ejemplos**: Mostrar los patrones de comportamiento esperados
4. **Actualizar regularmente**: Evolucionar el alma según el rendimiento

### Gestión de Tareas

1. **Establecer prioridades**: Usar niveles de prioridad (1-10)
2. **Definir fechas límite**: Siempre establecer fechas límite
3. **Monitorear el progreso**: Verificar regularmente el estado de las tareas
4. **Manejar fallos**: Implementar lógica de reintento

### Optimización de Memoria

1. **Limpiar datos antiguos**: Archivar memorias antiguas periódicamente
2. **Indexar información importante**: Marcar información clave
3. **Usar almacenamiento temporal**: Aprovechar las consultas con índice temporal

### Mecanismo de Desvanecimiento de Memoria

El sistema incluye el servicio de decaimiento temporal `MemoryFadeService`, que simula la característica de olvido de la memoria biológica:

- **Decaimiento automático**: Cada hora aplica un algoritmo de decaimiento de importancia a las entradas de memoria de todos los Seres de Silicio
- **Archivado automático**: Las memorias con importancia por debajo del umbral se archivan automáticamente y ya no participan en la recuperación diaria
- **Seguimiento estadístico**: Registra el número de ciclos de decaimiento y las entradas con cambio de estado

Esto significa que la memoria de los Seres de Silicio se desvanece naturalmente con el tiempo; la información importante debe marcarse activamente como de alta importancia a través de la herramienta de memoria para evitar ser archivada automáticamente.

---

## Espacio de Trabajo de Proyectos

### Resumen

El espacio de trabajo de proyectos es un mecanismo de gestión de espacios que soporta la colaboración de múltiples Seres de Silicio. El Curador de Silicio puede crear espacios de proyecto, asignar Seres de Silicio a los proyectos y asignarles roles.

### Ciclo de Vida del Proyecto

```
Creación → Activo → Archivado → Destrucción
              ↑       |
              └─ Restauración ┘
```

### Roles del Proyecto

Los Seres de Silicio pueden ser asignados a roles específicos dentro de un proyecto:

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### Notas de Trabajo del Proyecto

Las notas de trabajo dentro del espacio del proyecto son públicas y todos los miembros del proyecto pueden acceder:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Completado el módulo de autenticación de usuarios",
  "content": "## Detalles de implementación\n\n- Uso de token JWT",
  "keywords": "autenticación,JWT"
}
```

### Tareas del Proyecto

Las tareas dentro del espacio del proyecto soportan gestión completa del ciclo de vida:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "Implementar autenticación de usuarios",
  "priority": 5
}
```

### Flujos de Trabajo del Proyecto

Los proyectos pueden vincular plantillas de flujos de trabajo para impulsar los flujos de colaboración de los Seres de Silicio:

- Los flujos de trabajo se basan en plantillas de máquina de estados
- Soportan transiciones de estado impulsadas por Tick
- Registran automáticamente logs de transiciones de estado

### Aislamiento de Permisos de Herramientas

Los permisos de herramientas a nivel de proyecto son independientes de los permisos a nivel del Ser de Silicio, logrando aislamiento de permisos entre proyectos. Por ejemplo, un Ser de Silicio puede tener permisos de acceso a red en el proyecto A, pero estar restringido a permisos de solo lectura en el proyecto B.

## Solución de Problemas

### El ser no puede iniciar

**Verificar**:
- El Archivo de Alma existe y es válido
- El cliente de IA está configurado
- Los recursos del sistema son suficientes

### El ser se detiene inesperadamente

**Verificar**:
- Errores en los registros
- Disponibilidad del servicio de IA
- Uso de memoria

### Las tareas no se ejecutan

**Verificar**:
- El sistema de temporizadores está funcionando
- Prioridad y programación de tareas
- Configuración de permisos

## Siguientes Pasos

- 📚 Leer la [guía de arquitectura](architecture.md)
- 🛠️ Consultar la [guía de desarrollo](development-guide.md)
- 🚀 Ver la [guía de inicio rápido](getting-started.md)
