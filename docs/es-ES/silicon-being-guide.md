# Guía de Ser Silicona

> **Versión: v0.1.0-alpha**

[English](../en/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | **Español** | [Deutsch](../de-DE/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md)

## Resumen

Los Seres Silicona son agentes impulsados por IA que pueden pensar, actuar y evolucionar de forma autónoma.

## Arquitectura

### Separación Cuerpo-Cerebro

```
┌─────────────────────────────────────┐
│         Ser Silicona                 │
├──────────────────┬──────────────────┤
│   Cuerpo          │   Cerebro         │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Gestión de      │ • Cargar          │
│   estado         │   historial       │
│ • Detección de    │ • Invocar IA      │
│   activación     │ • Ejecutar        │
│ • Ciclo de vida  │   herramientas    │
│                  │ • Persistir       │
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

## Crear Ser

### A través de Web UI

1. Navegar a **Gestión de Seres**
2. Hacer clic en **Crear Nuevo Ser**
3. Completar:
   - Nombre
   - Contenido del alma
   - Opciones de configuración
4. Hacer clic en **Crear**

### A través de API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Ciclo de Vida del Ser

### Estados

```
Created → Starting → Running → Stopping → Stopped
                    ↓
                  Error
```

### Operaciones

- **Iniciar**: Inicializar y comenzar procesamiento
- **Detener**: Cierre elegante
- **Pausar**: Suspender temporalmente (mantener estado)
- **Reanudar**: Continuar desde estado pausado

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
- `Completed` - Completado exitosamente
- `Failed` - Fallo en ejecución
- `Cancelled` - Cancelado manualmente

## Sistema de Temporizadores

### Tipos de Temporizadores

1. **Una vez**: Ejecutar una vez después de un retraso
2. **Intervalo**: Ejecutar repetidamente a intervalos fijos
3. **Cron**: Ejecutar basado en expresión cron

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
- **Largo plazo**: Conocimiento y experiencia persistidos
- **Episódica**: Eventos e interacciones indexados por tiempo

### Estructura de Almacenamiento

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

## Sistema de Notas de Trabajo

### Resumen

Las notas de trabajo son un sistema de diario personal para Seres Silicona, con diseño de páginas, usado para registrar progreso de trabajo, aprendizajes, notas de proyecto, etc.

### Características

- **Gestión por páginas**: Cada nota es una página independiente, accesible por número de página
- **Soporte Markdown**: Contenido soporta formato Markdown (texto, listas, tablas, bloques de código)
- **Indexación por palabras clave**: Soporte para añadir palabras clave a notas, facilitando búsqueda
- **Función de resumen**: Cada nota tiene un resumen breve para navegación rápida
- **Generación de índice**: Puede generar índice de todas las notas para comprensión del contexto general
- **Marcas de tiempo**: Registro automático de creación y actualización
- **Privado por defecto**: Solo accesible por el propio ser (el curador puede gestionar)

### Escenarios de Uso

- Registrar progreso y reflexiones de trabajo diario
- Tomar notas de aprendizaje y descubrimientos técnicos
- Documentar decisiones y razonamiento de proyectos
- Mantener conocimiento personal y experiencia

### Operaciones

#### Crear Nota

```csharp
var note = new WorkNote
{
    BeingId = being.Id,
    Title = "Nueva característica implementada",
    Content = "# Progreso\n\nImplementé...",
    Keywords = new[] { "feature", "implementation" }
};

await workNoteSystem.CreateAsync(note);
```

#### Buscar Notas

```csharp
var results = await workNoteSystem.SearchAsync(being.Id, "implementación");
```

#### Generar Índice

```csharp
var index = await workNoteSystem.GenerateIndexAsync(being.Id);
```

## Sistema de Red de Conocimiento

### Tripletas de Conocimiento

Los seres pueden almacenar y consultar conocimiento usando estructura de tripla:

```
(Sujeto, Relación, Objeto)

Ejemplos:
- (Python, es_un, lenguaje_de_programación)
- (Roslyn, es_usado_para, compilación_dinámica)
- (.NET 9, soporta, C# 13)
```

### Operaciones

```csharp
// Añadir conocimiento
await knowledgeSystem.AddAsync(new KnowledgeTriple
{
    Subject = "SiliconBeing",
    Relation = "tiene",
    Object = "archivo_de_alma"
});

// Consultar conocimiento
var triples = await knowledgeSystem.QueryAsync("SiliconBeing");

// Encontrar ruta entre conceptos
var path = await knowledgeSystem.GetPathAsync("Python", "compilación");
```

## Auto-Evolución

### Reemplazo de Clase

Los seres pueden reescribir su propio código C#:

1. La IA genera nuevo código de clase
2. El sistema compila el código usando Roslyn
3. Si la compilación es exitosa, reemplaza la instancia actual
4. El estado se migra a la nueva instancia
5. El código cifrado se persiste en disco

### Restricciones de Seguridad

- El código debe heredar de `SiliconBeingBase`
- Solo se permiten ensamblados específicos
- Análisis estático para operaciones peligrosas
- Almacenamiento cifrado con AES-256

## Mejores Prácticas

### 1. Escribir Archivos de Alma Claros

El archivo de alma define el comportamiento del ser. Sé específico sobre:
- Personalidad y tono
- Capacidades y limitaciones
- Guías de comportamiento
- Dominio de conocimiento

### 2. Usar Tareas para Trabajo Complejo

Descomponer trabajo complejo en tareas manejables:
- Establecer prioridades claras
- Definir fechas de vencimiento
- Monitorear progreso

### 3. Mantener Notas de Trabajo

Las notas de trabajo ayudan a los seres a:
- Rastrear progreso a lo largo del tiempo
- Documentar decisiones
- Construir conocimiento incrementalmente

### 4. Monitorear Estados del Ser

Verificar regularmente estados del ser:
- Detectar errores temprano
- Optimizar rendimiento
- Ajustar configuración según sea necesario

## Solución de Problemas

### Ser No Se Inicia

**Verificar**:
1. El archivo de alma existe y es válido
2. La configuración es correcta
3. Los registros no muestran errores

### Ser Se Comporta Inesperadamente

**Verificar**:
1. El archivo de alma define comportamiento claramente
2. Las herramientas están disponibles
3. Los permisos están configurados correctamente

### Memoria No Persiste

**Verificar**:
1. El sistema de almacenamiento está configurado
2. Hay espacio en disco suficiente
3. Los permisos de archivo son correctos

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🔧 Ver la [Referencia de Herramientas](tools-reference.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
