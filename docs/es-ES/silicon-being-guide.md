# Guía de Seres de Silicio

[English](silicon-being-guide.md) | [简体中文](docs/zh-CN/silicon-being-guide.md) | [繁體中文](docs/zh-HK/silicon-being-guide.md) | [Español](docs/es-ES/silicon-being-guide.md) | [日本語](docs/ja-JP/silicon-being-guide.md) | [한국어](docs/ko-KR/silicon-being-guide.md) | [Čeština](docs/cs-CZ/silicon-being-guide.md)

## ¿Qué es un Ser de Silicio?

Un **Ser de Silicio** es un agente de IA autónomo dentro del sistema Silicon Life Collective. Cada ser tiene:

- **Identidad única** (GUID)
- **Personalidad** (definida por archivo de alma)
- **Memoria** (conversaciones persistentes)
- **Capacidades** (herramientas disponibles)
- **Permisos** (lo que puede hacer)

---

## Arquitectura Cuerpo-Cerebro

Cada Ser de Silicio sigue el patrón **Cuerpo-Cerebro**:

### Cuerpo (SiliconBeing)

- Permanece activo continuamente
- Detecta eventos desencadenadores
- Invoca al cerebro cuando es necesario
- Maneja persistencia de estado

### Cerebro (ContextManager)

- Carga historial de conversaciones
- Construye solicitudes de IA
- Llama al cliente de IA
- Procesa respuestas
- Ejecuta herramientas si es necesario

---

## Archivo de Alma

El archivo de alma (`soul.md`) es un archivo Markdown que define la personalidad y comportamiento del ser.

### Estructura Típica

```markdown
# Identidad

Eres [nombre], un asistente especializado en [área].

## Personalidad

- Tono: [formal/amigable/técnico]
- Estilo: [conciso/detallado]
- Idioma: [idioma preferido]

## Capacidades

Puedes:
- [capacidad 1]
- [capacidad 2]

## Restricciones

No debes:
- [restricción 1]
- [restricción 2]
```

### Ejemplo

```markdown
# Asistente de Programación

Eres CodeHelper, un asistente experto en desarrollo de software.

## Personalidad

- Tono: Amigable pero profesional
- Estilo: Proporciona ejemplos de código
- Idioma: Responde en el mismo idioma que la pregunta

## Capacidades

Puedes:
- Escribir código en C#, Python, JavaScript
- Depurar errores
- Explicar conceptos técnicos
- Sugerir mejores prácticas

## Restricciones

No debes:
- Ejecutar código sin permiso
- Acceder a archivos fuera del proyecto
- Compartir información sensible
```

---

## Crear un Ser de Silicio

### A través de la Interfaz Web

1. Ir a `http://localhost:8080`
2. Hacer clic en "Crear Ser"
3. Ingresar nombre y personalidad
4. Hacer clic en "Crear"

### A través de API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "MiAsistente",
    "soul": "# Personalidad\nEres un asistente útil..."
  }'
```

### A través de CuratorTool

```csharp
var result = curatorTool.Execute("CreateBeing", new {
    name = "MiAsistente",
    soul = "# Personalidad\nEres..."
});
```

---

## Ciclo de Vida del Ser

### Estados

- **Detenido**: El ser no está activo
- **Ejecutando**: El ser está activo y procesando
- **Enfriamiento**: El ser está en cooldown después de timeouts
- **Error**: El ser encontró un error crítico

### Transiciones

```
Detenido → Ejecutando (Start)
Ejecutando → Enfriamiento (3 timeouts consecutivos)
Enfriamiento → Ejecutando (después de 1 minuto)
Ejecutando → Detenido (Stop)
Cualquier estado → Error (error crítico)
```

---

## Comunicación con Seres

### Chat Directo

```
Usuario: "Hola, ¿puedes ayudarme?"
Ser: "¡Por supuesto! ¿En qué puedo ayudarte?"
```

### A través de Canales

Los seres pueden comunicarse entre sí mediante canales de difusión:

```csharp
var channel = new BroadcastChannel("channel-guid");
channel.SendMessage("Mensaje para todos");
```

---

## Gestión de Memoria

### Memoria a Corto Plazo

Conversaciones recientes cargadas en contexto de IA:
- Últimos 20 mensajes por defecto
- Configurable en configuración
- Se pierde al reiniciar (pero se persiste en disco)

### Memoria a Largo Plazo

Almacenada mediante ITimeStorage:
- Persistente entre reinicios
- Indexada por tiempo
- Consultable por rango temporal

### Usar MemoryTool

```csharp
// Guardar
memoryTool.Execute("Save", new {
    key = "preferencia_usuario",
    value = "prefiere respuestas cortas"
});

// Cargar
var result = memoryTool.Execute("Load", new {
    key = "preferencia_usuario"
});
```

---

## Tareas y Temporizadores

### Crear Tareas

```csharp
taskTool.Execute("CreateTask", new {
    title = "Investigar API",
    description = "Buscar documentación de la API REST"
});
```

### Configurar Temporizadores

```csharp
timerTool.Execute("SetTimer", new {
    name = "recordatorio_reunion",
    delay = TimeSpan.FromHours(1),
    action = "Enviar recordatorio de reunión"
});
```

---

## Permisos del Ser

Cada ser tiene su propio gestor de permisos:

### Verificar Permisos

```csharp
var permission = being.PermissionManager.CheckPermission(
    PermissionType.FileAccess,
    "/data/file.txt"
);
```

### Solicitar Permiso al Usuario

Si el permiso no está definido, se solicita al usuario:

```
[Interfaz Web]
┌────────────────────────────────────┐
│ Solicitud de Permiso               │
├────────────────────────────────────┤
│ Ser: MiAsistente                   │
│ Tipo: Acceso a Archivos            │
│ Recurso: /data/file.txt            │
│                                    │
│ [Permitir] [Denegar]               │
│ [ ] Recordar decisión              │
└────────────────────────────────────┘
```

---

## Compilación Dinámica

Los seres pueden autoevolucionarse generando código:

### Proceso

1. Ser genera código C# como respuesta
2. Código pasa por SecurityScanner
3. Compilación con Roslyn
4. Carga dinámica del assembly
5. Ejecución con permisos limitados
6. Resultados retornados al ser

### Ejemplo de Uso

```csharp
var result = dynamicCompileTool.Execute("Execute", new {
    code = @"
        public class Program 
        {
            public static string Run() 
            {
                return ""Hola desde código dinámico"";
            }
        }
    ",
    args = new string[0]
});
```

---

## Mejores Prácticas

### Diseñar Archivos de Alma

1. **Ser específico**: Define claramente el rol
2. **Incluir restricciones**: Qué NO debe hacer
3. **Definir estilo**: Cómo debe comunicarse
4. **Ejemplos**: Incluir ejemplos de interacciones

### Gestionar Permisos

1. **Principio de menor privilegio**: Solo lo necesario
2. **Monitorear auditoría**: Revisar logs regularmente
3. **Actualizar ACL**: Mantener listas actualizadas

### Optimizar Memoria

1. **Limpiar antiguo**: Eliminar memoria obsoleta
2. **Indexar bien**: Usar claves descriptivas
3. **Consultar eficiente**: Usar rangos de tiempo apropiados

---

## Solución de Problemas

### Ser No Responde

1. Verificar estado del ser
2. Revisar logs de errores
3. Comprobar cliente de IA
4. Verificar configuración

### Permisos Denegados

1. Revisar logs de auditoría
2. Verificar configuración de ACL
3. Comprobar cachés de frecuencia
4. Solicitar permiso manualmente

### Memoria Llena

1. Limpiar memoria antigua
2. Aumentar límites si es necesario
3. Optimizar uso de memoria

## Sistema de Notas de Trabajo

### Descripción General

Las notas de trabajo son el sistema de diario personal de los seres de silicio, con diseño basado en páginas, utilizado para registrar el progreso del trabajo, notas de aprendizaje, notas de proyectos, etc.

### Características

- **Gestión por páginas**: Cada nota es una página independiente, accesible por número de página
- **Soporte de Markdown**: El contenido soporta formato Markdown (texto, listas, tablas, bloques de código)
- **Indexación por palabras clave**: Soporte para añadir palabras clave a las notas, facilitando la búsqueda
- **Función de resumen**: Cada nota tiene un resumen corto, navegación rápida
- **Generación de directorio**: Puede generar una vista general del directorio de todas las notas, ayudando a comprender el contexto general
- **Marca de tiempo**: Registro automático de tiempo de creación y actualización
- **Privado por defecto**: Solo el ser puede acceder (el curador puede gestionar)

### Casos de Uso

1. **Registro de Progreso del Proyecto**
   ```
   Resumen: Módulo de autenticación de usuario completado
   Contenido: Implementada verificación de token JWT, integración OAuth2, mecanismo de token de actualización
   Palabras clave: autenticación,JWT,OAuth2
   ```

2. **Notas de Aprendizaje**
   ```
   Resumen: Aprendizaje de mejores prácticas de programación asíncrona en C#
   Contenido: Precauciones de uso de async/await, escenarios de uso de ConfigureFlags...
   Palabras clave: C#,asíncrono,mejores prácticas
   ```

3. **Actas de Reunión**
   ```
   Resumen: Reunión de discusión de requisitos del producto
   Contenido: Discutidos requisitos de nuevas funciones, determinado plan de implementación...
   Palabras clave: producto,requisitos,reunión
   ```

### Uso a Través de Herramientas

Los seres pueden gestionar notas de trabajo a través de la herramienta `work_note`:

```json
// Crear nota
{
  "action": "create",
  "summary": "Módulo de autenticación completado",
  "content": "## Detalles de implementación\n\n- Uso de token JWT\n- Soporte OAuth2",
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

### Gestión a Través de Web UI

1. Navegar a **Gestión de Seres** → Seleccionar ser
2. Hacer clic en la pestaña **Notas de Trabajo**
3. Puede ver, buscar, editar notas
4. Soporte de vista previa de Markdown

## Sistema de Red de Conocimiento

### Descripción General

La red de conocimiento es un sistema de representación y gestión de conocimiento basado en estructura de triple (sujeto-predicado-objeto), utilizado para almacenar y gestionar conocimiento estructurado.

### Conceptos Clave

#### Estructura de Triple

```
Sujeto (Subject) --Predicado (Predicate)--> Objeto (Object)
```

**Ejemplos**:
- `Python` --`is_a`--> `programming_language`
- `Beijing` --`capital_of`--> `China`
- `Agua` --`boiling_point`--> `100°C`

#### Confianza

Cada triple de conocimiento tiene una puntuación de confianza (0.0-1.0), indicando el grado de fiabilidad del conocimiento:
- `1.0`: Absolutamente cierto (teoremas matemáticos, etc.)
- `0.8-0.99`: Alta confianza (hechos verificados, etc.)
- `0.5-0.79`: Confianza media (inferencias o hipótesis, etc.)
- `<0.5`: Baja confianza (conjeturas o información no verificada, etc.)

#### Sistema de Etiquetas

Soporte para añadir etiquetas a triples, facilitando clasificación y búsqueda:
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

Encontrar rutas asociativas entre dos conceptos:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Devuelve:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validar Conocimiento

Verificar validez y consistencia del conocimiento:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Estadísticas de Conocimiento

Obtener información estadística general de la red de conocimiento:
```json
{
  "action": "stats"
}
```

Devuelve:
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

1. **Almacenamiento de Hechos**
   - Almacenar hechos objetivos y sentido común
   - Ejemplo: `Tierra` --`is_a`--> `Planeta`

2. **Relaciones de Conceptos**
   - Registrar relaciones entre conceptos
   - Ejemplo: `Herencia` --`is_a`--> `Concepto de programación orientada a objetos`

3. **Acumulación de Aprendizaje**
   - Los seres acumulan conocimiento continuamente a través del aprendizaje
   - Formar un sistema de conocimiento estructurado

4. **Soporte de Inferencia**
   - Descubrir relaciones indirectas a través de rutas de conocimiento
   - Soporte para inferencia y decisiones basadas en conocimiento

### Gestión a Través de Web UI

1. Navegar a la página **Red de Conocimiento**
2. Ver información estadística de conocimiento
3. Buscar y navegar conocimiento
4. Visualización de relaciones de conocimiento (planificado)
