# Guía de Web UI

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
- Clientes personalizados

### Ajustes de Almacenamiento

- Ruta base
- Indexación por tiempo
- Estrategia de limpieza

### Localización

Cambiar entre 21 variantes de idioma:
- Chino (6 variantes): Simplificado, Tradicional, Singapur, Macao, Taiwán, Malasia
- Inglés (10 variantes): EE.UU., Reino Unido, Canadá, Australia, India, Singapur, Sudáfrica, Irlanda, Nueva Zelanda, Malasia
- Español (2 variantes): España, México
- Japonés, Coreano, Checo

---

## Sistema de Pieles

### Pieles Disponibles

1. **Admin** - Interfaz profesional de gestión
2. **Chat** - Diseño centrado en conversaciones
3. **Creative** - Estilo creativo y artístico
4. **Dev** - Layout orientado a desarrolladores

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

---

## Sistema de Registros

### Funcionalidades

- Ver registros del sistema en tiempo real
- Filtrar por nivel (Info, Warning, Error)
- Filtrar por Ser Silicona
- Consulta por rango de tiempo

### Niveles de Registro

- **Info** - Información general
- **Warning** - Advertencias
- **Error** - Errores
- **Debug** - Información de depuración

---

## Sistema de Memoria

### Vista de Línea de Tiempo

Visualizar memoria a largo plazo en vista cronológica:
- Eventos y interacciones indexados por tiempo
- Navegación por fecha
- Filtrado avanzado

### Filtrado

- Por fecha
- Por tipo de evento
- Por palabras clave
- Por ser

---

## Base de Conocimiento

### Gestión de Tripletas

Crear y gestionar conocimiento usando estructura de tripla:
- **Sujeto**: El concepto principal
- **Relación**: La conexión entre conceptos
- **Objeto**: El concepto relacionado

### Descubrimiento de Rutas

Encontrar conexiones entre conceptos:
- Visualización gráfica
- Ruta más corta
- Puntuación de confianza

### Búsqueda

- Búsqueda de texto completo
- Coincidencia de palabras clave
- Filtrado por etiquetas

---

## Explorador de Código

### Árbol de Archivos

Navegar la estructura de archivos del proyecto:
- Expandir/colapsar directorios
- Iconos de tipo de archivo
- Búsqueda de archivos

### Resaltado de Sintaxis

Soporte para múltiples lenguajes:
- C#
- JavaScript
- Python
- Markdown
- Y más...

---

## Editor de Código

### Monaco Editor

Editor de código profesional con:
- Resaltado de sintaxis
- Autocompletado
- Indicaciones flotantes
- Plegado de código
- Búsqueda y reemplazo

### Indicaciones Flotantes

Al pasar el cursor sobre código:
- Ver documentación
- Ver definiciones de tipos
- Ver referencias

---

## Gestión de Proyectos

### Espacio de Trabajo

Organizar proyectos:
- Crear proyectos
- Configurar directorios
- Gestionar miembros

### Sistema de Tareas

Gestionar tareas del proyecto:
- Crear tareas
- Asignar prioridades
- Seguimiento de progreso

### Notas de Trabajo

Sistema de notas personales para seres:
- Crear notas en formato Markdown
- Añadir palabras clave
- Generar índice
- Búsqueda de notas

---

## Sistema de Temporizadores

### Tipos de Temporizadores

1. **Una vez**: Ejecutar una vez después de un retraso
2. **Intervalo**: Ejecutar repetidamente a intervalos fijos
3. **Cron**: Ejecutar basado en expresión cron

### Gestión

- Crear temporizadores
- Pausar/reanudar
- Ver historial de ejecución
- Eliminar temporizadores

---

## Sistema de Ayuda

### Documentos Multilingües

Acceder a documentos de ayuda en múltiples idiomas:
- Selección de idioma
- Búsqueda por tema
- Navegación por categorías

### Temas de Ayuda

- Introducción
- Arquitectura
- Guía de desarrollo
- Referencia de herramientas
- Solución de problemas
- Y más...

---

## Mejores Prácticas

### 1. Usar SSE para Actualizaciones en Tiempo Real

Para características en tiempo real, usar eventos SSE en lugar de sondeo:

```javascript
const source = new EventSource('/api/events');
source.onmessage = (event) => {
    // Manejar actualización
};
```

### 2. Gestionar Reconexión

Implementar lógica de reconexión para conexiones SSE:

```javascript
source.onerror = () => {
    setTimeout(() => {
        source.close();
        // Reconectar
    }, 5000);
};
```

### 3. Usar Pieles Apropiadas

Elegir piel basada en tarea:
- **Admin** para gestión del sistema
- **Chat** para interacción con IA
- **Creative** para trabajo creativo
- **Dev** para desarrollo de código

---

## Solución de Problemas

### No Se Puede Acceder a Web UI

**Verificar**:
1. La aplicación se está ejecutando
2. El puerto 8080 no está en uso
3. El firewall permite el puerto 8080

### SSE No Funciona

**Verificar**:
1. El navegador soporta SSE
2. La conexión no está bloqueada
3. Los registros del servidor muestran conexiones SSE

### Piel No Se Carga Correctamente

**Verificar**:
1. La piel está registrada en `SkinManager`
2. No hay errores de CSS
3. El navegador cacheó CSS antiguo (limpiar caché)

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🔧 Ver la [Referencia de Herramientas](tools-reference.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
