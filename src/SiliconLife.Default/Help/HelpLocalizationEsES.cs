// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Collective;

namespace SiliconLife.Default.Help;

/// <summary>
/// Spanish help documentation implementation
/// Implementación de documentación de ayuda en español
/// </summary>
public class HelpLocalizationEsES : HelpLocalizationBase
{
    #region Help Documents
    
    public override string GettingStarted_Title => "Inicio Rápido";
    public override string BeingManagement_Title => "Gestión de Seres de Silicio";
    public override string ChatSystem_Title => "Función de Chat";
    public override string TaskTimer_Title => "Tareas y Temporizadores";
    public override string Permission_Title => "Gestión de Permisos";
    public override string Config_Title => "Gestión de Configuración";
    public override string FAQ_Title => "Preguntas Frecuentes";
    public override string Memory_Title => "Sistema de Memoria";
    public override string OllamaSetup_Title => "Instalación de Ollama y Descarga de Modelos";
    public override string BailianDashScope_Title => "Guía de Uso de la Plataforma Alibaba Cloud Bailian";

    public override string[] GettingStarted_Tags => new[] { "instalación", "inicio", "introducción", "inicio rápido", "empezar", "inicialización", "ejecutar", "configurar entorno" };
    public override string[] BeingManagement_Tags => new[] { "ser de silicio", "crear", "configurar", "gestión de seres de silicio", "entidad", "perfil", "ajustes", "gestión" };
    public override string[] ChatSystem_Tags => new[] { "chat", "conversación", "mensaje", "función de chat", "comunicación", "intercambio", "diálogo", "discusión" };
    public override string[] TaskTimer_Tags => new[] { "tarea", "temporizador", "programación", "tareas y temporizadores", "automatización", "programado", "recordatorio", "período" };
    public override string[] Permission_Tags => new[] { "permiso", "seguridad", "control de acceso", "gestión de permisos", "autorización", "privacidad", "protección", "autenticación" };
    public override string[] Config_Tags => new[] { "configuración", "ajustes", "opciones", "gestión de configuración", "preferencias", "personalización", "sistema", "parámetros" };
    public override string[] FAQ_Tags => new[] { "preguntas frecuentes", "ayuda", "problemas", "soporte", "solución de problemas", "guía", "asistencia", "respuestas" };
    public override string[] Memory_Tags => new[] { "memoria", "historial", "registro", "sistema de memoria", "actividad", "rastreo", "búsqueda", "registro de eventos" };
    public override string[] OllamaSetup_Tags => new[] { "Ollama", "instalación", "modelo", "descarga", "IA local", "configuración", "ajustes", "ejecución" };
    public override string[] BailianDashScope_Tags => new[] { "Bailian", "DashScope", "Alibaba Cloud", "IA en la nube", "API", "configuración", "modelo", "pago" };
    
    public override string GettingStarted => @"
# Inicio Rápido

## Iniciar el Sistema

### Inicio con Doble Clic (Recomendado)

Encuentre el archivo del programa y haga doble clic para iniciar:
- **Windows**: `SiliconLife.Default.exe`
- El sistema se iniciará automáticamente y **abrirá el navegador automáticamente**

¡Así de simple! No se requiere ninguna configuración.

## Primer Uso

Al iniciar por primera vez, el sistema **completará automáticamente toda la inicialización**:
- ✅ Crea automáticamente el Ser de Silicio Principal
- ✅ Utiliza el archivo de alma integrado (prompt)
- ✅ Guarda la configuración automáticamente
- ✅ Todos los servicios se preparan automáticamente

¡Solo necesita esperar a que se abra el navegador y podrá comenzar a usar!

## Descripción General de la Interfaz

La interfaz del sistema se divide en dos partes principales:

### Barra de Navegación Izquierda

Contiene los siguientes módulos funcionales:

- **💬 Chat** - Conversar con seres de silicio
- **📊 Panel de Control** - Ver el estado del sistema
- **🧠 Seres de Silicio** - Ver y gestionar seres de silicio
- **🔍 Auditoría** - Ver registros de operaciones
- **📚 Base de Conocimientos** - Gestionar grafos de conocimiento
- **📁 Proyectos** - Gestionar proyectos de código
- **📝 Registros** - Ver registros del sistema
- **⚙ Configuración** - Configuración del sistema
- **❓ Ayuda** - Este documento
- **ℹ Acerca de** - Información del sistema

### Área de Contenido Principal

Muestra el contenido de la página actual, que varía según el módulo funcional que seleccione.

## Comenzar Rápidamente

### 1. Conversar con Seres de Silicio

Esta es la función más utilizada:

1. Haga clic en el icono **💬 Chat** en la izquierda
2. Seleccione un ser de silicio en la lista izquierda (por defecto hay un Ser de Silicio Principal)
3. Ingrese un mensaje en el cuadro de entrada en la parte inferior
4. Presione `Enter` para enviar
5. La IA le responderá en tiempo real

**Consejos:** 
- Presione `Shift + Enter` para crear una nueva línea
- Haga clic en el botón ⏹ para detener la respuesta de la IA

### 2. Ver Información de Seres de Silicio

Para ver información detallada de los seres de silicio:

1. Haga clic en el icono **🧠 Seres de Silicio** en la izquierda
2. Haga clic en cualquier tarjeta de ser de silicio
3. Se mostrará información detallada en el lado derecho:
   - Estado (Inactivo/En ejecución)
   - Cantidad de temporizadores y tareas
   - Enlaces a memoria, permisos, historial de chat, etc.

### 3. Modificar Configuración del Sistema

Para ajustar la configuración del sistema:

1. Haga clic en el icono **⚙ Configuración** en la izquierda
2. Encuentre el elemento de configuración que desea modificar
3. Haga clic en el botón ""Editar""
4. Ingrese el nuevo valor y guarde

**Configuraciones comunes:**
- Modificar el idioma de la interfaz
- Cambiar el tema de la interfaz
- Ajustar el modelo de IA
- Modificar el puerto de acceso

## Carga de Archivos

Permita que la IA analice el contenido de archivos:

1. Haga clic en el botón **📁** en la interfaz de chat
2. Ingrese la ruta completa del archivo
   - Por ejemplo: `C:\Users\SuNombreDeUsuario\Documents\informe.pdf`
3. Haga clic en ""Confirmar carga""
4. La IA leerá y analizará el archivo

**Tipos de archivos soportados:**
- Archivos de texto: .txt, .md, .json
- Archivos de código: .cs, .js, .py
- Archivos de configuración: .yml, .yaml
- Otros: .csv, .log, etc.

## Ver Historial de Conversaciones

Para revisar conversaciones anteriores:

1. Vaya a la página **🧠 Seres de Silicio**
2. Haga clic en el ser de silicio que desea ver
3. Haga clic en el enlace ""Historial de chat""
4. Navegue por todas las sesiones históricas

## Obtener Ayuda

Cuando encuentre problemas:

- **Ver ayuda**: Haga clic en el icono **❓ Ayuda** en la izquierda
- **Ver registros**: Haga clic en el icono **📝 Registros** en la izquierda
- **Reiniciar el sistema**: Muchos problemas se pueden resolver reiniciando

## Próximos Pasos

Ahora que ha comprendido las operaciones básicas, puede:

- 📖 Leer otros documentos de ayuda para conocer funciones detalladas
- 💬 Conversar con el Ser de Silicio Principal y dejar que le ayude a completar tareas
- ⚙ Explorar opciones de configuración para personalizar su sistema

¡Le deseamos un uso agradable!
";

    public override string BeingManagement => @"
# Gestión de Seres de Silicio

## ¿Qué es un Ser de Silicio?

Un Ser de Silicio (Silicon Being) es la entidad central del sistema. Cada ser de silicio es un agente de IA independiente que posee:
- **Archivo de alma**: El prompt central que define patrones de comportamiento, personalidad y capacidades
- **Sistema de memoria**: Almacena historial de conversaciones e información importante
- **Sistema de tareas**: Ejecuta tareas programadas y operaciones automatizadas
- **Conjunto de herramientas**: Varias herramientas funcionales que pueden ser invocadas

## Ver Seres de Silicio

### Lista de Seres de Silicio

Al entrar en la página de ""Seres de Silicio"" puede ver todos los seres de silicio, mostrados en formato de tarjetas:
- **Nombre**: El nombre visible del ser de silicio
- **Estado**: Inactivo (verde) o En ejecución (azul)
- **Tipo**: Si hay código compilado personalizado, se mostrará una etiqueta de tipo

### Ver Detalles de Seres de Silicio

Al hacer clic en cualquier tarjeta de ser de silicio, se mostrará información detallada en el lado derecho:
- **ID**: Identificador único del ser de silicio
- **Estado**: Estado de ejecución actual
- **Compilación personalizada**: Si se ha cargado código personalizado
- **Cantidad de temporizadores**: Haga clic para ver la gestión de temporizadores
- **Cantidad de tareas**: Haga clic para ver la lista de tareas
- **Memoria**: Haga clic para ver el sistema de memoria
- **Permisos**: Haga clic para ver la configuración de permisos
- **Historial de chat**: Ver registros de conversaciones históricas
- **Notas de trabajo**: Ver notas de trabajo
- **Cliente de IA**: Haga clic para ver y modificar la configuración de IA
- **Archivo de alma**: Haga clic para ver y editar el prompt

## Editar Seres de Silicio

### Editar Archivo de Alma

El archivo de alma determina los patrones de comportamiento y el alcance de capacidades del ser de silicio.

1. Haga clic en el enlace ""Archivo de alma"" en la página de detalles del ser de silicio
2. Entrará en el editor de archivo de alma (soporta formato Markdown)
3. Modifique el contenido del prompt
4. Guarde los cambios

### Editar Configuración de IA

Puede configurar servicios de IA independientes para cada ser de silicio:

1. Haga clic en el enlace ""Cliente de IA"" en la página de detalles del ser de silicio
2. Seleccione el tipo de cliente de IA (como Ollama, OpenAI, etc.)
3. Configure parámetros como punto de acceso API, modelo, clave, etc.
4. Entra en vigor inmediatamente después de guardar

## Guía de Escritura de Archivos de Alma

### Estructura Básica

```markdown
# Configuración de Rol

Eres un [descripción del rol], experto en:
- Habilidad 1
- Habilidad 2
- Habilidad 3

# Directrices de Comportamiento

1. Directriz 1
2. Directriz 2
3. Directriz 3

# Flujo de Trabajo

Al recibir una tarea:
1. Comprender los requisitos
2. Analizar el plan
3. Ejecutar operaciones
4. Informar resultados
```

### Consejos de Escritura

1. **Definir claramente el rol**: Defina claramente las responsabilidades y el área de especialización del ser de silicio
2. **Establecer límites de comportamiento**: Explique qué se puede hacer y qué no se debe hacer
3. **Proporcionar flujo de trabajo**: Guíe al ser de silicio sobre cómo manejar tareas
4. **Usar formato Markdown**: Soporta títulos, listas, bloques de código, etc.

### Ejemplo: Asistente de Programación

```markdown
# Configuración de Rol

Eres un asistente de desarrollo full-stack profesional, experto en:
- Desarrollo C# / .NET
- Diseño de arquitectura y revisión de código
- Diseño y optimización de bases de datos
- Desarrollo web frontend

# Directrices de Comportamiento

1. Proporcione siempre ejemplos de código ejecutables
2. Explique la lógica clave del código y el pensamiento de diseño
3. Proporcione recomendaciones de mejores prácticas
4. Cuando no esté seguro de un problema, infórmelo claramente al usuario

# Normas de Código

- Seguir los principios SOLID
- Usar nombres claros
- Agregar comentarios necesarios
- Considerar el manejo de errores y casos límite
```

## Estado de Seres de Silicio

### Estado de Ejecución

- **Inactivo**: Esperando tareas o conversaciones (indicador verde)
- **En ejecución**: Ejecutando tareas o en conversación (indicador azul)

### Monitorear Seres de Silicio

A través del panel de control puede ver:
- Número total actual de seres de silicio
- Estado de ejecución de tareas de cada ser de silicio
- Estadísticas de uso de recursos

## Mejores Prácticas

1. **Separación de responsabilidades**: Diferentes seres de silicio se encargan de diferentes áreas (como asistente de programación, asistente de atención al cliente, análisis de datos, etc.)
2. **Optimización continua**: Optimice continuamente el archivo de alma según los comentarios de uso real para mejorar el rendimiento del ser de silicio
3. **Respaldar configuración**: Se recomienda hacer una copia de seguridad del archivo de alma de los seres de silicio importantes

## Solución de Problemas

### P: ¿El ser de silicio no responde?

Verifique:
1. ¿El servicio de IA se está ejecutando normalmente?
2. ¿La conexión de red es normal?
3. ¿El archivo de alma está configurado correctamente?
4. Consulte los registros del sistema para obtener información detallada de errores

### P: ¿Cómo modificar el modelo de IA de un ser de silicio?

Haga clic en el enlace ""Cliente de IA"" en la página de detalles del ser de silicio, seleccione un nuevo modelo de IA y configúrelo. Entra en vigor inmediatamente después de guardar. Las nuevas conversaciones usarán el nuevo modelo.

### P: ¿El comportamiento del ser de silicio no cumple con las expectativas?

1. Verifique si el archivo de alma es claro y preciso
2. Agregue más directrices de comportamiento y restricciones
3. Proporcione orientación específica del flujo de trabajo
4. Pruebe y optimice continuamente
";

    public override string ChatSystem => @"
# Función de Chat

## Iniciar Conversación

1. Haga clic en el icono **💬 Chat** en la barra de navegación izquierda
2. Seleccione el ser de silicio con el que desea conversar en la lista izquierda
3. Ingrese el mensaje en el cuadro de entrada en la parte inferior
4. Presione la tecla `Enter` o haga clic en el botón ""Enviar""
5. La IA responderá en tiempo real (el texto se muestra carácter por carácter)

## Descripción de la Interfaz

### Diseño de la Interfaz

- **Lista izquierda**: Muestra todos los seres de silicio, haga clic para cambiar el objeto de conversación
- **Área central**: Muestra los mensajes de conversación
  - Sus mensajes se muestran a la derecha
  - Las respuestas de la IA se muestran a la izquierda
- **Área de entrada inferior**: Ingrese mensajes y botón de envío

### Descripción de Botones

- **Botón Enviar**: Envía el mensaje que ha ingresado
- **Botón Detener ⏹**: Aparece cuando la IA está respondiendo, haga clic para interrumpir la respuesta de la IA
- **Botón Archivo 📁**: Carga archivos para que la IA los analice

## Operaciones Básicas

### Enviar Mensajes

- Presione `Enter` para enviar después de ingresar el mensaje
- Presione `Shift + Enter` para crear una nueva línea

### Detener Respuesta

Si la IA está respondiendo, puede:
- Hacer clic en el botón ""⏹ Detener""
- O enviar directamente un nuevo mensaje (interrumpirá automáticamente la respuesta actual)

### Cargar Archivos

Permita que la IA analice el contenido de archivos:

1. Haga clic en el botón **📁** junto al cuadro de entrada
2. Ingrese la ruta del archivo en el panel emergente
   - Por ejemplo: `C:\Users\SuNombreDeUsuario\Documents\informe.pdf`
3. Haga clic en ""Confirmar carga""
4. La IA leerá y analizará el archivo

**Tipos de archivos soportados**:
- Archivos de texto: .txt, .md, .json, .xml
- Archivos de código: .cs, .js, .py, .java, etc.
- Archivos de configuración: .yml, .yaml, .ini, .conf
- Otros archivos: .csv, .log, etc.

## Características de Conversación

### Visualización en Tiempo Real con Streaming

Las respuestas de la IA se mostrarán carácter por carácter, no necesita esperar la respuesta completa para ver el contenido.

### Conversación Multi-turno

- El sistema guarda automáticamente el historial de conversaciones
- La IA recordará lo que se dijo anteriormente
- Puede hacer referencia directamente a conversaciones anteriores

### Invocación de Herramientas

La IA puede invocar automáticamente herramientas durante la conversación para:
- Consultar calendarios
- Gestionar configuración del sistema
- Ejecutar código
- Leer archivos
- Buscar ayuda
- Crear notas
- Consultar memoria

Cuando la IA invoca una herramienta, verá el nombre de la herramienta y el resultado de ejecución.

### Conversación Multilingüe

Puede conversar con la IA en cualquier idioma, y la IA responderá automáticamente en el mismo idioma.

## Ver Historial de Conversaciones

Si desea ver registros de conversaciones pasadas:

1. Haga clic en el icono **🧠 Seres de Silicio** en la barra de navegación izquierda
2. Haga clic en la tarjeta del ser de silicio que desea ver
3. Encuentre el enlace ""Historial de chat"" en los detalles del lado derecho
4. Haga clic para entrar y ver todas las sesiones históricas

## Preguntas Frecuentes

### P: ¿Qué hacer si la respuesta de la IA es muy lenta?

**Posibles razones**:
- El modelo utilizado es grande y requiere más tiempo de cálculo
- Retraso de red al usar modelos en la nube
- El historial de conversaciones es muy largo

**Soluciones**:
- Intente usar modelos locales (como Ollama)
- Seleccione modelos más ligeros

### P: ¿La IA no invoca herramientas?

**Verifique los siguientes puntos**:
1. Confirme si la herramienta está habilitada
2. Verifique si hay restricciones de permisos
3. Confirme si el modelo de IA soporta invocación de herramientas

### P: ¿Cómo cargar archivos?

Haga clic en el botón ""📁"" junto al cuadro de entrada, ingrese la ruta completa del archivo (como `C:\documentos\archivo.pdf`), y luego haga clic en ""Confirmar carga"".

### P: ¿Cómo ver conversaciones anteriores?

En la página ""Seres de Silicio"", haga clic en el enlace ""Historial de chat"" del ser de silicio correspondiente para ver todas las sesiones históricas.

## Consejos de Uso

1. **Ser claro**: Describa sus necesidades con un lenguaje preciso
2. **Hacer preguntas por pasos**: Divida problemas complejos en múltiples preguntas pequeñas
3. **Proporcionar contexto**: Explique información de contexto relevante cuando sea necesario
4. **Usar carga de archivos**: Cuando necesite que la IA analice archivos, proporcione directamente la ruta del archivo
5. **Prestar atención a la invocación de herramientas**: Observe las herramientas que invoca la IA para asegurar que las operaciones cumplan con sus expectativas
";

    public override string TaskTimer => @"
# Tareas y Temporizadores

## Descripción General

El sistema de tareas y temporizadores registra la ejecución automatizada de los seres de silicio. Puede ver la lista de tareas y el estado de los temporizadores para entender qué está haciendo el ser de silicio y cuándo se ejecuta.

## Sistema de Tareas

### ¿Qué es una Tarea?

Las tareas son elementos de trabajo que el ser de silicio está ejecutando o ha completado, como:
- Tareas de procesamiento creadas automáticamente por la IA
- Elementos de trabajo generados por el sistema
- Tareas de ejecución activadas por temporizadores

### Ver Lista de Tareas

**Método 1: Ver todas las tareas**

1. Haga clic en el icono ""Tareas"" en la barra de navegación izquierda (si existe)
2. La página mostrará la lista de tareas de todos los seres de silicio

**Método 2: Ver tareas de un ser de silicio específico**

1. Vaya a la página **🧠 Seres de Silicio**
2. Haga clic en el ser de silicio que desea ver
3. Encuentre el enlace ""Tareas"" en los detalles
4. Haga clic para entrar en la página de tareas

### Información de Tareas

Cada tarea muestra la siguiente información:

- **Nombre de la tarea**: El título de la tarea
- **Estado**:
  - Esperando (amarillo)
  - En ejecución (azul)
  - Completado (verde)
  - Fallido (rojo)
  - Cancelado (gris)
- **Prioridad**: El nivel de prioridad de la tarea
- **Asignado a**: El ser de silicio que ejecuta la tarea
- **Hora de creación**: Cuándo se creó la tarea
- **Descripción**: Detalles de la tarea

### Descripción de Estados de Tareas

- **Esperando**: La tarea ha sido creada, esperando ejecución
- **En ejecución**: La tarea se está ejecutando
- **Completado**: La tarea se completó exitosamente
- **Fallido**: La ejecución de la tarea falló, puede ver la información de error
- **Cancelado**: La tarea fue cancelada

## Sistema de Temporizadores

### ¿Qué es un Temporizador?

Los temporizadores son mecanismos de activación automática que permiten a los seres de silicio ejecutar operaciones en momentos específicos. El sistema utiliza un sistema de calendario para definir las condiciones de activación.

### Ver Lista de Temporizadores

**Método 1: Ver todos los temporizadores**

1. Haga clic en el icono ""Temporizadores"" en la barra de navegación izquierda (si existe)
2. La página mostrará la lista de temporizadores de todos los seres de silicio

**Método 2: Ver temporizadores de un ser de silicio específico**

1. Vaya a la página **🧠 Seres de Silicio**
2. Haga clic en el ser de silicio que desea ver
3. Encuentre el enlace ""Temporizadores"" en los detalles
4. Haga clic para entrar en la página de temporizadores

### Información de Temporizadores

Cada temporizador muestra la siguiente información:

- **Nombre del temporizador**: Identificador del temporizador
- **Estado**: En ejecución o detenido
- **Tipo**: Tipo de activación del temporizador
- **Hora de activación**: Próxima hora de activación
- **Sistema de calendario**: Calendario utilizado (como calendario gregoriano, lunar, etc.)
- **Cantidad de activaciones**: Número total de veces que se ha activado
- **Hora de creación**: Cuándo se creó el temporizador
- **Última activación**: Cuándo fue la última activación

### Tipos de Temporizadores

El sistema soporta múltiples formas de activación:

- **Activación por intervalo**: Se activa cada cierto tiempo fijo
  - Por ejemplo: cada 2 horas, cada 30 minutos
  
- **Activación por calendario**: Se activa según condiciones del calendario
  - Por ejemplo: todos los días a las 9:00, todos los lunes, el día 1 de cada mes
  - Soporta múltiples sistemas de calendario como gregoriano, lunar, etc.

## Ver Historial de Ejecución

### Historial de Ejecución de Temporizadores

Entienda el estado de ejecución de los temporizadores:

1. Vaya a la página de temporizadores
2. Encuentre el temporizador que desea ver
3. Haga clic en el enlace ""Historial de ejecución""
4. Ver todos los registros de activación

### Detalles de Ejecución

Información detallada de cada ejecución:

1. Encuentre una ejecución específica en el historial de ejecución
2. Haga clic para ver detalles
3. Puede ver:
   - Hora de ejecución
   - Resultado de ejecución
   - Mensajes de conversación relacionados
   - Información de errores (si falló)

### Mensajes de Ejecución

Ver la conversación completa durante una ejecución:

1. Encuentre el enlace ""Mensajes"" en la página de detalles de ejecución
2. Ver la conversación completa entre la IA y el usuario
3. Entender cómo la IA manejó esta activación

## Preguntas Frecuentes

### P: ¿Cómo crear una nueva tarea?

**R:** Las tareas son generadas automáticamente por el sistema, no se soporta la creación manual. Cuando el ser de silicio necesita ejecutar algún trabajo, creará automáticamente una tarea.

### P: ¿Cómo crear un nuevo temporizador?

**R:** Los temporizadores son gestionados automáticamente por los seres de silicio, no se soporta la creación manual. Los seres de silicio configurarán temporizadores según sea necesario para ejecutar tareas periódicas.

### P: ¿Se pueden eliminar tareas o temporizadores?

**R:** El sistema no proporciona funcionalidad de eliminación manual. Las tareas y temporizadores son gestionados automáticamente por los seres de silicio.

### P: ¿Qué hacer si una tarea muestra ""Fallido""?

**Recomendaciones:**
1. Ver la información de error de la tarea
2. Entender la causa del fallo
3. Si es un problema temporal, la tarea puede reintentar
4. Si falla continuamente, puede conversar con el ser de silicio para entender la situación

### P: ¿El temporizador no se activa?

**Verifique:**
1. Si el temporizador está en estado de ejecución
2. Si se cumplen las condiciones de activación
3. Si el ser de silicio se está ejecutando normalmente
4. Ver el historial de ejecución para entender la situación

### P: ¿Cómo entender qué está haciendo el ser de silicio?

**Métodos:**
1. Ver la lista de tareas para entender las tareas actualmente en ejecución
2. Ver la lista de temporizadores para entender las operaciones que se activarán próximamente
3. Ver el historial de ejecución para entender actividades pasadas
4. Conversar directamente con el ser de silicio para preguntar

### P: ¿Qué significa la prioridad de la tarea?

**R:** La prioridad indica la importancia de la tarea. Cuanto menor sea el número, mayor será la prioridad. Las tareas de alta prioridad se procesarán primero.

## Consejos de Uso

1. **Verificar regularmente**: Entender el estado de ejecución automatizada del ser de silicio
2. **Prestar atención a tareas fallidas**: Manejar situaciones anormales de manera oportuna
3. **Ver historial de ejecución**: Entender el patrón de trabajo de la IA
4. **Combinar con conversación**: Discutir el estado de tareas y temporizadores con el ser de silicio

## Notas Técnicas

### Almacenamiento de Datos

Los datos de tareas y temporizadores se almacenan en el directorio de datos del sistema, asociados con el ser de silicio:
```
data/
  beings/
    {ID del ser de silicio}/
      tasks/      (datos de tareas)
      timers/     (datos de temporizadores)
```

### Gestión Automática

El sistema automáticamente:
- Crea y gestiona tareas
- Activa temporizadores
- Registra historial de ejecución
- Limpia datos expirados

No necesita gestionar manualmente, el sistema se encargará de todo.
";

    public override string Permission => @"
# Gestión de Permisos

## ¿Qué es el Sistema de Permisos?

El sistema de permisos protege la seguridad de su sistema y evita que la IA ejecute operaciones no autorizadas. Cuando la IA intenta ejecutar ciertas operaciones (como acceder a archivos, ejecutar comandos, etc.), el sistema verifica si está permitido.

## ¿Cómo Funcionan los Permisos?

### Ventana Emergente Automática de Permisos

Cuando la IA intenta ejecutar una operación que requiere permisos, el sistema mostrará una ventana emergente para preguntarle:

**El contenido de la ventana incluye:**
- Tipo de permiso (como acceso a archivos, ejecución de comandos, etc.)
- Recurso solicitado (como ruta de archivo)
- Información detallada

**Puede elegir:**
- **Permitir**: Ejecutar esta operación
- **Rechazar**: Bloquear esta operación

### Orden de Verificación de Permisos

El sistema verifica los permisos en el siguiente orden:

1. **Ser de Silicio Principal**: Si es una operación del administrador principal, se permite automáticamente
2. **Límite de frecuencia**: Evita una gran cantidad de solicitudes en poco tiempo
3. **Reglas globales**: Reglas de permitir/rechazar preestablecidas
4. **Reglas personalizadas**: Reglas de permisos que usted escribe (si existen)
5. **Preguntar al usuario**: Si nada de lo anterior puede decidir, se le preguntará en una ventana emergente

## Reglas de Permisos Integradas

El sistema ya tiene algunas reglas de permisos seguras preestablecidas:

### Reglas de Acceso a Archivos

**Acceso permitido:**
- Directorio temporal propio del ser de silicio
- Carpetas comunes del usuario (escritorio, descargas, documentos, imágenes, música, videos)
- Carpetas de usuario público

**Acceso prohibido:**
- Directorios clave del sistema (directorios del sistema Windows, /etc /boot de Linux, etc.)
- Directorios de datos de otros seres de silicio

**Rutas no coincidentes:**
- Se le preguntará en una ventana emergente si permite

## Reglas de Permisos Personalizadas (Función Avanzada)

Si necesita un control de permisos más detallado, puede escribir reglas de permisos personalizadas.

### Acceder a la Página de Edición de Permisos

1. Vaya a la página **🧠 Seres de Silicio**
2. Haga clic en el ser de silicio que desea configurar
3. Encuentre el enlace ""Permisos"" en los detalles
4. Entre en el editor de código de permisos

### Editor de Código de Permisos

El editor de permisos es una interfaz de edición de código que soporta:
- Resaltado de sintaxis de código C#
- Autocompletado de código
- Guardado automático
- Escaneo de seguridad (para evitar código malicioso)

**Método de guardado:**
- Haga clic en el botón ""Guardar"" en el editor
- El sistema primero compilará y verificará
- Solo entrará en vigor después de pasar el escaneo de seguridad

### Plantilla Predeterminada

Si aún no hay código de permisos personalizados, el sistema proporcionará una plantilla predeterminada. Puede modificarla basándose en la plantilla.

## Ver Reglas de Permisos

### Ver Lista Actual de Reglas

1. Entre en la página de edición de permisos
2. La página mostrará todas las reglas de permisos de este ser de silicio
3. Cada regla incluye:
   - Tipo de permiso
   - Ruta del recurso
   - Permitir/Rechazar
   - Descripción

## Historial de Solicitudes de Permisos

Todas las solicitudes de permisos se registrarán en el registro de auditoría:

1. Haga clic en el icono **🔍 Auditoría** en la izquierda
2. Filtre registros relacionados con permisos
3. Ver solicitudes históricas y sus decisiones

## Preguntas Frecuentes

### P: ¿Por qué fue rechazada la operación de la IA?

**Posibles razones:**
- La operación está en las reglas de rechazo
- Se activó el límite de frecuencia
- Usted eligió rechazar anteriormente

**Solución:**
1. Ver el registro de auditoría para entender la causa específica
2. Si es necesario, modificar las reglas de permisos
3. Reejecutar la operación

### P: ¿Qué hacer si hay demasiadas ventanas emergentes de permisos?

**Recomendaciones:**
- Para operaciones seguras de uso común, puede considerar escribir reglas personalizadas para permitir automáticamente
- Ver si puede reducir las ventanas emergentes modificando las reglas

### P: ¿Es peligroso el código de permisos personalizado?

**Garantía de seguridad:**
- El código pasará por un escaneo de seguridad
- El código malicioso será rechazado
- Si la compilación falla, no entrará en vigor

**Recomendaciones:**
- Si no está familiarizado con la programación, se recomienda usar las reglas predeterminadas
- Haga una copia de seguridad del código original antes de modificar
- Pruebe antes de aplicar al entorno de producción

### P: ¿Error de configuración de permisos que impide el uso?

**Solución:**
1. Operar como Ser de Silicio Principal (el administrador principal tiene los permisos más altos)
2. Eliminar el código de permisos personalizado (borrar el código y guardar)
3. El sistema restaurará las reglas predeterminadas

### P: ¿Se pueden configurar diferentes permisos para diferentes seres de silicio?

**R:** Sí. Cada ser de silicio tiene una configuración de permisos independiente y no se afectan entre sí.

## Recomendaciones de Seguridad

1. **Permitir con precaución operaciones sensibles**: Como eliminar archivos, ejecutar comandos, etc.
2. **Verificar regularmente el registro de auditoría**: Entender el historial de operaciones de la IA
3. **No modificar reglas de permisos arbitrariamente**: A menos que entienda su impacto
4. **Mantener el sistema actualizado**: Obtener la última protección de seguridad

## Descripción de Tipos de Permisos

El sistema soporta los siguientes tipos de permisos:

- **Acceso a red**: La IA intenta acceder a recursos de red
- **Ejecución de comandos**: La IA intenta ejecutar programas de línea de comandos
- **Acceso a archivos**: La IA intenta leer o escribir archivos
- **Llamada de funciones**: La IA intenta invocar funciones específicas
- **Acceso a datos**: La IA intenta acceder a datos del sistema

Cada tipo tiene diferentes niveles de seguridad y métodos de procesamiento.
";

    public override string Config => @"
# Gestión de Configuración

## ¿Qué es la Gestión de Configuración?

La página de gestión de configuración le permite ajustar varias configuraciones del sistema, incluyendo servicios de IA, red, idioma, tema de interfaz, etc.

## ¿Cómo Usar la Página de Configuración?

1. Haga clic en el icono **⚙ Configuración** en la barra de navegación izquierda
2. La página mostrará múltiples grupos de configuración, cada grupo contiene varios elementos de configuración
3. Encuentre el elemento de configuración que desea modificar y haga clic en el botón ""Editar"" en el lado derecho
4. Ingrese el nuevo valor en el cuadro de edición emergente
5. Haga clic en el botón ""Guardar""

## Descripción de Grupos de Configuración

### Configuración Básica

Contiene la configuración básica del sistema:

- **Directorio de datos**: Ubicación de la carpeta que almacena todos los datos del sistema
  - Valor predeterminado: `./data`
  - Recomendación: Mantener el valor predeterminado a menos que haya necesidades especiales

- **Idioma**: Idioma de visualización de la interfaz del sistema
  - Soportado: Chino simplificado, chino tradicional, inglés, japonés, coreano, alemán, español, etc.
  - Después de modificar: La página se actualizará automáticamente para aplicar el nuevo idioma

### Configuración de IA

Configurar la conexión y el modelo del servicio de IA:

- **Tipo de cliente de IA**: Seleccionar el servicio de IA a usar
  - Ollama (ejecución local, recomendado)
  - OpenAI (servicio en la nube)
  - Otros servicios compatibles con la API de OpenAI

- **Configuración de IA**: Configuración detallada del servicio de IA
  - `endpoint`: Dirección API (como `http://localhost:11434`)
  - `model`: Nombre del modelo utilizado (como `qwen3.5:cloud`)
  - `temperature`: Grado de creatividad de respuesta (0-1, predeterminado 0.7)
  - `maxTokens`: Longitud máxima de respuesta (predeterminado 4096)

**Editar configuración de IA**:
1. Haga clic en el botón de edición de ""Configuración de IA""
2. Se abrirá un editor de diccionario
3. Puede agregar, modificar, eliminar elementos de configuración
4. Haga clic en ""Guardar"" para aplicar

### Configuración de Ejecución

Controlar el comportamiento del sistema durante la ejecución:

- **Tiempo de espera de ejecución**: Tiempo máximo de ejecución de una sola tarea
  - Predeterminado: 10 minutos
  - Recomendación: Mantener el valor predeterminado a menos que las tareas sean particularmente complejas

- **Cantidad máxima de tiempos de espera**: Cuántas veces consecutivas de tiempo de espera activan el mecanismo de protección
  - Predeterminado: 3 veces
  - Función: Evitar reintentos infinitos del sistema

- **Tiempo de espera del vigilante**: Cuánto tiempo sin respuesta antes de reiniciar el sistema
  - Predeterminado: 10 minutos
  - Función: Recuperar automáticamente el sistema bloqueado

- **Nivel mínimo de registro**: Qué niveles de registro registrar
  - Trace: Más detallado (incluye toda la información de depuración)
  - Debug: Información de depuración
  - Info: Información general (recomendado)
  - Warning: Solo advertencias
  - Error: Solo errores

### Configuración Web

Configurar los parámetros del servidor web:

- **Puerto web**: Puerto de acceso del sistema
  - Predeterminado: 8080
  - Dirección de acceso: `http://localhost:8080`
  - Después de modificar: Necesita reiniciar el sistema para que surta efecto

- **Permitir acceso de red interna**: Si permitir que otros dispositivos en la red de área local accedan
  - Cerrado (predeterminado): Solo accesible desde esta máquina
  - Abierto: Otros dispositivos en la misma red también pueden acceder
  - Nota: Después de abrir, se requieren permisos de administrador

- **Tema web**: Tema de la interfaz
  - Puede seleccionar diferentes temas para cambiar la apariencia de la interfaz
  - Entra en vigor inmediatamente después de modificar

### Configuración de Usuario

- **Nombre de usuario**: Su nombre que se muestra en el sistema
  - Predeterminado: User
  - Puede cambiarlo a cualquier nombre que le guste

## Editar Elementos de Configuración

### Métodos de Edición para Diferentes Tipos

El sistema mostrará diferentes interfaces de edición según el tipo de elemento de configuración:

**Tipo de texto**:
- Muestra un cuadro de entrada de texto
- Ingrese directamente el nuevo valor

**Tipo numérico**:
- Muestra un cuadro de entrada numérico
- Puede ingresar enteros o decimales

**Tipo booleano (sí/no)**:
- Muestra una casilla de verificación
- Marcado significa ""sí"", desmarcado significa ""no""

**Tipo de enumeración (lista desplegable)**:
- Muestra una lista desplegable
- Seleccione uno de las opciones preestablecidas

**Intervalo de tiempo**:
- Muestra cuatro cuadros de entrada: días, horas, minutos, segundos
- Complete los valores correspondientes respectivamente

**Ruta de directorio**:
- Muestra un cuadro de entrada de ruta y botón ""Examinar""
- Haga clic en ""Examinar"" para seleccionar una carpeta
- También puede ingresar la ruta directamente

**Tipo de diccionario (pares clave-valor)**:
- Muestra un editor de pares clave-valor
- Puede agregar múltiples líneas de pares clave-valor
- Haga clic en el botón ""Agregar"" para agregar una nueva línea
- Haga clic en el botón ""Eliminar"" para eliminar una línea

### Guardar Configuración

- Haga clic en ""Guardar"" después de cada modificación de configuración
- La mayoría de las configuraciones entran en vigor inmediatamente
- Algunas configuraciones (como el puerto) requieren reiniciar el sistema

## Preguntas Frecuentes

### P: ¿No puedo acceder al sistema después de modificar el puerto?

**Solución**:
1. Verificar si el puerto está siendo ocupado por otro programa
2. Confirmar si el firewall permite ese puerto
3. Usar el nuevo puerto para acceder: `http://localhost:nuevo_puerto`

### P: ¿Cómo restaurar la configuración predeterminada?

**Método 1**: Modificación manual
1. Entrar en la página de configuración
2. Cambiar cada elemento de configuración de vuelta a su valor predeterminado

**Método 2**: Eliminar archivo de configuración
1. Cerrar el sistema
2. Eliminar el archivo `config.json`
3. Reiniciar el sistema (creará automáticamente la configuración predeterminada)

### P: ¿Qué hacer si la conexión de IA falla?

**Verifique los siguientes puntos**:
1. ¿El servicio de IA se está ejecutando normalmente?
2. ¿La dirección del punto de acceso es correcta?
3. Si es un servicio en la nube, ¿la clave API es correcta?
4. ¿La conexión de red es normal?

**Solución**:
1. Entrar en el grupo ""Configuración de IA""
2. Haga clic en el botón de edición de ""Configuración de IA""
3. Verificar si `endpoint` y `model` son correctos
4. Guardar después de modificar

### P: ¿Cuándo surte efecto después de modificar la configuración?

- **Efecto inmediato**: Idioma, tema, configuración de IA, nombre de usuario, etc.
- **Requiere reinicio**: Puerto web, configuración de acceso de red interna

### P: ¿Dónde está el archivo de configuración?

El archivo de configuración se encuentra en el archivo `config.json` en el directorio de ejecución del sistema.

## Consejos de Uso

1. **Modificar con precaución**: Mantener los valores predeterminados para elementos de configuración inciertos
2. **Registrar cambios**: Registrar el contenido y la razón de las modificaciones después de cambiar la configuración
3. **Respaldar configuración**: Puede copiar el archivo `config.json` como respaldo antes de modificaciones importantes
4. **Entorno de prueba**: Si es posible, verificar la configuración en un entorno de prueba primero
5. **Seguridad primero**: Asegurar la seguridad de la red antes de abrir el acceso de red interna
";

    public override string FAQ => @"
# Preguntas Frecuentes

## Comenzar a Usar

### P: ¿Cómo iniciar el sistema?

**R:** Haga doble clic en el archivo del programa para iniciar. El sistema abrirá automáticamente el navegador para entrar en la interfaz.

### P: ¿Qué necesito hacer al iniciar por primera vez?

**R:** ¡No necesita hacer nada! El sistema completará automáticamente la inicialización, incluyendo la creación del Ser de Silicio Principal. Solo necesita esperar a que se abra el navegador para comenzar a usar.

### P: ¿El navegador no se abrió después de iniciar el sistema?

**R:** Acceda manualmente a `http://localhost:8080`.

## Conversación con IA

### P: ¿Qué hacer si la respuesta de la IA es muy lenta?

**Posibles razones:**
- El modelo utilizado es grande
- Retraso de red (al usar IA en la nube)
- El historial de conversaciones es muy largo

**Soluciones:**
- Usar servicios de IA locales (como Ollama)
- Seleccionar modelos más ligeros

### P: ¿La respuesta de la IA no cumple con las expectativas?

**Recomendaciones:**
1. Verificar si el archivo de alma es claro y preciso
2. Proporcionar más información de contexto en la conversación
3. Intentar describir sus necesidades de manera más específica

### P: ¿La IA no invoca herramientas?

**Verifique:**
1. Confirmar que la herramienta está habilitada
2. Ver si hay restricciones de permisos
3. Confirmar que el modelo de IA soporta invocación de herramientas

### P: ¿Cómo hacer que la IA analice archivos?

**Método:**
1. Haga clic en el botón ""📁 Archivo"" en la interfaz de chat
2. Ingrese la ruta completa del archivo (como `C:\documentos\informe.pdf`)
3. Haga clic en ""Confirmar carga""
4. La IA leerá y analizará el archivo

## Seres de Silicio

### P: ¿Cómo crear un nuevo ser de silicio?

**R:** Actualmente el sistema no soporta la creación directa de seres de silicio. El Ser de Silicio Principal puede crear y gestionar otros seres de silicio, puede conversar con el administrador principal y dejar que le ayude a crear.

### P: ¿Cómo modificar el comportamiento de un ser de silicio?

**Método:**
1. Entrar en la página ""Seres de Silicio""
2. Haga clic en el ser de silicio que desea modificar
3. Haga clic en el enlace ""Archivo de alma""
4. Modificar el contenido del prompt
5. Guardar

### P: ¿Cómo configurar una IA diferente para un ser de silicio?

**Método:**
1. Entrar en la página ""Seres de Silicio""
2. Haga clic en el ser de silicio objetivo
3. Haga clic en el enlace ""Cliente de IA""
4. Seleccionar servicio de IA y configurar
5. Guardar

### P: ¿El ser de silicio no responde?

**Verifique:**
1. ¿El servicio de IA se está ejecutando normalmente?
2. ¿La conexión de red es normal?
3. Ver los registros del sistema para obtener errores detallados

## Configuración del Sistema

### P: ¿Cómo modificar el idioma del sistema?

**Método:**
1. Haga clic en el icono ""⚙ Configuración"" en la izquierda
2. Encontrar el elemento de configuración ""Idioma""
3. Haga clic en ""Editar""
4. Seleccionar idioma de la lista desplegable
5. Guardar (la página se actualizará automáticamente)

### P: ¿Cómo cambiar el tema de la interfaz?

**Método:**
1. Entrar en la página ""Configuración""
2. Encontrar el elemento de configuración ""Tema web""
3. Haga clic en ""Editar""
4. Seleccionar el tema que le gusta
5. Guardar

### P: ¿Cómo modificar el puerto de acceso?

**Método:**
1. Entrar en la página ""Configuración""
2. Encontrar el elemento de configuración ""Puerto web""
3. Haga clic en ""Editar""
4. Ingresar nuevo número de puerto (como 9000)
5. Guardar y reiniciar el sistema

**Nota:** Después de modificar el puerto, necesita usar el nuevo puerto para acceder, como `http://localhost:9000`

### P: ¿Cómo permitir que otros dispositivos en la red de área local accedan?

**Método:**
1. Entrar en la página ""Configuración""
2. Encontrar el elemento de configuración ""Permitir acceso de red interna""
3. Haga clic en ""Editar""
4. Marcar ""Sí""
5. Guardar

**Nota:** Se requieren permisos de administrador, después de modificar otros dispositivos pueden acceder a través de `http://su_IP:8080`

## Historial de Chat

### P: ¿Cómo ver conversaciones pasadas?

**Método:**
1. Entrar en la página ""Seres de Silicio""
2. Haga clic en el ser de silicio que desea ver
3. Encontrar el enlace ""Historial de chat"" en los detalles
4. Haga clic para entrar y navegar por todas las sesiones históricas

### P: ¿Cómo eliminar el historial de conversaciones?

**R:** El sistema actualmente no proporciona la función de eliminar historial de conversaciones. El historial de conversaciones se guarda automáticamente para que el ser de silicio pueda recordar conversaciones anteriores.

## Datos y Almacenamiento

### P: ¿Dónde se almacenan los datos?

**R:** Por defecto se almacenan en la carpeta `data` en el directorio de ejecución del programa.

### P: ¿Cómo respaldar datos?

**Método:** Copiar toda la carpeta `data` a una ubicación segura.

### P: ¿Cómo migrar a una nueva computadora?

**Pasos:**
1. Cerrar el sistema
2. Copiar toda la carpeta `data`
3. Instalar el sistema en la nueva computadora
4. Colocar la carpeta `data` en el directorio del programa de la nueva computadora
5. Iniciar el sistema

## Archivo de Configuración

### P: ¿Dónde está el archivo de configuración?

**R:** En el archivo `config.json` en el directorio de ejecución del programa.

### P: ¿Puedo editar el archivo de configuración directamente?

**R:** Sí, pero no se recomienda. Se recomienda modificar a través de la página de configuración de la interfaz web, que es más segura y menos propensa a errores.

### P: ¿Qué hacer si configuré mal?

**Solución:**
1. Cerrar el sistema
2. Eliminar el archivo `config.json`
3. Reiniciar el sistema (creará automáticamente la configuración predeterminada)

**O:** Si tiene un respaldo, puede restaurar el archivo de configuración respaldado.

## Problemas de Rendimiento

### P: ¿El sistema funciona lentamente?

**Recomendaciones:**
- Usar servicios de IA locales (como Ollama)
- Seleccionar modelos de IA más ligeros
- Reducir la cantidad de tareas ejecutándose simultáneamente

### P: ¿El uso de memoria es alto?

**Recomendaciones:**
- Usar modelos de IA más ligeros
- Limpiar regularmente datos innecesarios

## Obtener Ayuda

### P: ¿Qué hacer si encuentro un problema?

**Pasos recomendados:**
1. **Ver documentos de ayuda**: Haga clic en el icono ""❓ Ayuda"" en la izquierda
2. **Ver registros**: Ver los registros de ejecución del sistema en la página ""📝 Registros""
3. **Reiniciar el sistema**: Muchos problemas se pueden resolver reiniciando

### P: ¿Cómo ver los registros del sistema?

**Método:**
1. Haga clic en el icono ""📝 Registros"" en la izquierda
2. Navegar por la lista de registros
3. Puede filtrar por nivel (errores, advertencias, etc.)

## Otros Problemas

### P: ¿Qué idiomas soporta el sistema?

**R:** Soporta chino simplificado, chino tradicional, inglés, japonés, coreano, alemán, español y otros idiomas.

### P: ¿Necesito conexión a internet para usar?

**R:** Depende del servicio de IA que use:
- **IA local (como Ollama)**: No necesita conexión a internet
- **IA en la nube (como OpenAI)**: Necesita conexión a internet

### P: ¿Es seguro el sistema?

**R:** Sí. El sistema tiene un mecanismo de gestión de permisos integrado, todas las operaciones de la IA pasarán por verificación de permisos, y las operaciones sensibles solicitarán su confirmación.

### P: ¿Se pueden personalizar las funciones?

**R:** El sistema soporta la extensión de funciones escribiendo código, pero esto requiere ciertos conocimientos de programación. Se recomienda que los usuarios comunes usen las funciones proporcionadas por el sistema.
";

    public override string Memory => @"
# Sistema de Memoria

## ¿Qué es el Sistema de Memoria?

El sistema de memoria registra todo el historial de actividades de los seres de silicio, incluyendo conversaciones, invocación de herramientas, eventos del sistema, etc. A través del sistema de memoria, puede entender qué ha hecho el ser de silicio, cuándo lo hizo y cuáles fueron los resultados.

## ¿Cómo Acceder al Sistema de Memoria?

Acceder a través de la página de seres de silicio:

1. Haga clic en el icono **🧠 Seres de Silicio** en la izquierda
2. Haga clic en la tarjeta del ser de silicio que desea ver
3. Encuentre el enlace ""Memoria"" en los detalles del lado derecho
4. Haga clic para entrar en la página de memoria

## Descripción de la Página de Memoria

### Diseño de la Página

- **Parte superior**: Selector de seres de silicio e información estadística
- **Área de filtros**: Condiciones de filtro como tipo, tiempo, palabras clave, etc.
- **Área de lista**: Muestra la lista de entradas de memoria
- **Área de detalles**: Muestra contenido detallado después de hacer clic en una memoria

### Tipos de Memoria

El sistema registrará los siguientes tipos de memoria:

- **Conversación**: Contenido de conversaciones entre usuario e IA
- **Invocación de herramientas**: Registros de ejecución de herramientas invocadas por la IA
- **Eventos del sistema**: Eventos importantes de ejecución del sistema
- **Resumen**: Resúmenes comprimidos de conversaciones o eventos

## Ver Memoria

### Navegar por la Lista de Memoria

1. Seleccionar el ser de silicio que desea ver
2. La página mostrará la lista de memoria de ese ser de silicio
3. Cada memoria muestra:
   - Icono de tipo
   - Resumen de contenido
   - Tiempo
   - Estado (éxito/fallo)

### Ver Detalles de Memoria

Al hacer clic en cualquier entrada de memoria, se mostrará:
- Contenido completo
- Marca de tiempo
- Parámetros relacionados
- Resultado de ejecución (si es invocación de herramienta)

### Rastrear Contexto Original

Para algunas entradas de memoria, el sistema proporciona la función ""Rastrear"":
1. Haga clic en el botón ""Rastrear"" en los detalles de la memoria
2. El sistema mostrará el contexto completo cuando ocurrió esa memoria
3. Le ayuda a entender por qué la IA hizo eso en ese momento

## Filtrar Memoria

### Filtrar por Tipo

Haga clic en el filtro de tipo, seleccione el tipo de memoria que desea ver:
- Solo ver conversaciones
- Solo ver invocación de herramientas
- Solo ver eventos del sistema
- Solo ver resúmenes

### Filtrar por Tiempo

Puede seleccionar un rango de tiempo:
- Ingresar fecha de inicio
- Ingresar fecha de fin
- Solo mostrar memorias dentro de ese período

### Búsqueda por Palabras Clave

Ingrese palabras clave en el cuadro de búsqueda:
- Soporta chino e inglés
- Buscará todo el contenido de la memoria
- Mostrará automáticamente los resultados coincidentes después de ingresar

**Consejos de búsqueda:**
- Usar palabras clave específicas facilita encontrar resultados
- Puede combinar filtros de tipo y tiempo
- Si hay demasiados resultados, intente con palabras clave más específicas

### Mostrar Resúmenes o Registros Originales

- **Mostrar todo**: Mostrar todas las memorias
- **Solo resúmenes**: Mostrar solo registros de resúmenes comprimidos
- **Solo originales**: Mostrar solo registros detallados originales

## Estadísticas de Memoria

La parte superior de la página mostrará información estadística:
- Número total de memorias
- Cantidad de memorias de cada tipo
- Uso de almacenamiento

A través de estas estadísticas, puede entender:
- Nivel de actividad del ser de silicio
- Tipos principales de actividades realizadas
- Si es necesario limpiar memorias antiguas

## Navegación por Páginas

Si hay muchas memorias, el sistema las mostrará por páginas:
- Cada página muestra 20 registros por defecto
- Usar botones de número de página para navegar
- Puede ajustar la cantidad mostrada por página

## Preguntas Frecuentes

### P: ¿Cómo encontrar una conversación específica?

**Método:**
1. Ingresar palabras clave de la conversación en el cuadro de búsqueda
2. Seleccionar ""Conversación"" en el filtro de tipo
3. Si conoce el tiempo aproximado, puede establecer un rango de tiempo
4. Navegar por los resultados de búsqueda

### P: ¿Qué hacer si la memoria ocupa demasiado espacio?

**Recomendaciones:**
- La memoria se gestiona automáticamente, generalmente no requiere intervención manual
- El sistema creará resúmenes para comprimir el historial
- Si realmente es necesario, puede contactar al administrador del sistema

### P: ¿Se pueden eliminar memorias?

**R:** El sistema no proporciona la función de eliminar memorias. La memoria es un historial importante del ser de silicio, conservarla ayuda a la IA a entender y responder mejor a las preguntas.

### P: ¿Se pueden exportar memorias?

**R:** La versión actual no soporta la función de exportación. Los datos de memoria se almacenan en el directorio de datos del sistema.

### P: ¿Por qué algunas memorias son ""Resúmenes""?

**R:** El sistema comprime automáticamente conversaciones o eventos largos en resúmenes para ahorrar espacio de almacenamiento y mejorar la eficiencia de las consultas. Los resúmenes conservan información clave pero omiten detalles.

### P: ¿Cómo ver información detallada de herramientas invocadas por la IA?

**Método:**
1. Seleccionar ""Invocación de herramientas"" en el filtro de tipo
2. Encontrar el registro de invocación de herramienta correspondiente
3. Haga clic para ver información detallada
4. Puede ver el nombre de la herramienta, parámetros, resultado de ejecución, etc.

### P: ¿No se encuentran resultados en la búsqueda de memoria?

**Recomendaciones:**
1. Verificar si las palabras clave son correctas
2. Intentar usar diferentes palabras clave
3. Verificar si el rango de tiempo está configurado correctamente
4. Confirmar si el ser de silicio seleccionado es correcto
5. Intentar no configurar condiciones de filtro, ver todas las memorias

## Consejos de Uso

1. **Verificar regularmente**: Entender la situación de actividades del ser de silicio
2. **Usar filtros**: Ubicar rápidamente la información necesaria
3. **Usar rastreo**: Entender el proceso de decisión de la IA
4. **Prestar atención a estadísticas**: Entender el estado de ejecución del sistema

## Notas Técnicas

### Almacenamiento de Datos

Los datos de memoria se almacenan en el directorio de datos del sistema:
```
data/
  beings/
    {ID del ser de silicio}/
      memory/
        (archivos de memoria)
```

### Gestión Automática

El sistema automáticamente:
- Registra actividades importantes
- Crea resúmenes de conversaciones
- Mantiene índices de tiempo
- Optimiza el rendimiento de consultas

No necesita gestionar manualmente la memoria, el sistema se encargará de todo.
";

    public override string OllamaSetup => @"
# Instalación de Ollama y Descarga de Modelos

## ¿Qué es Ollama?

Ollama es una herramienta de ejecución de modelos de IA local de código abierto que le permite ejecutar modelos de lenguaje grande en su propia computadora sin necesidad de conexión a internet (después de descargar el modelo).

**Ventajas:**
- Ejecución completamente local, protege la privacidad
- Soporta múltiples modelos de IA
- Instalación simple, fácil de usar
- Gratuito y de código abierto

## Descargar e Instalar Ollama

### Sistema Windows

**Paso 1: Descargar el paquete de instalación**

Visite la página de descarga del sitio web oficial de Ollama:
- URL: https://ollama.com/download
- Descargue automáticamente el paquete de instalación de Windows (ollama-setup.exe)

**Paso 2: Ejecutar el instalador**

1. Haga doble clic en el archivo `ollama-setup.exe` descargado
2. Siga las indicaciones del asistente de instalación para completar la instalación
3. Después de completar la instalación, Ollama se iniciará automáticamente

**Paso 3: Verificar la instalación**

1. Abra el símbolo del sistema (presione `Win + R`, ingrese `cmd`, presione Enter)
2. Ingrese el siguiente comando:
   ```
   ollama --version
   ```
3. Si muestra el número de versión, significa que la instalación fue exitosa

### Sistema Mac

**Método 1: Descargar paquete de instalación**

1. Visite https://ollama.com/download
2. Descargue el paquete de instalación de Mac
3. Haga doble clic en el paquete de instalación, arrástrelo a la carpeta de aplicaciones

**Método 2: Instalar usando terminal**

Abra la terminal (Terminal), ingrese:
```bash
brew install ollama
```

**Verificar instalación:**
```bash
ollama --version
```

### Sistema Linux

**Comando de instalación en un paso:**

Abra la terminal, ejecute:
```bash
curl -fsSL https://ollama.com/install.sh | sh
```

**Verificar instalación:**
```bash
ollama --version
```

## Iniciar Ollama

### Windows

- Ollama se iniciará automáticamente después de la instalación
- Puede ver el icono de Ollama en la bandeja del sistema (esquina inferior derecha)
- Haga clic derecho en el icono para gestionar

### Mac / Linux

Ejecute en la terminal:
```bash
ollama serve
```

O ejecute directamente:
```bash
ollama
```

Esto abrirá un menú interactivo.

## Descargar y Ejecutar Modelos

### ¿Qué es un Modelo?

El modelo es el ""cerebro"" de la IA, determina las capacidades de la IA. Diferentes modelos tienen diferentes características:
- **Diferentes tamaños**: Cuanto mayor sea el modelo, más fuerte será la capacidad, pero requiere más memoria
- **Diferentes áreas de especialización**: Algunos son buenos en conversación, otros en programación

### Inteligencia del Modelo (Unidad B)

La ""inteligencia"" de los modelos de IA generalmente se representa con **B (Billion, mil millones de parámetros)**:
- **7B-8B**: Nivel básico, puede completar tareas simples, pero el rendimiento puede no ser bueno en escenarios complejos
- **13B-14B**: Nivel medio, buen rendimiento en la mayoría de las tareas diarias
- **32B y superior**: Nivel alto, mejor razonamiento complejo y comprensión de texto largo

**Se recomienda usar modelos de 8B o superior en este sistema** para obtener una buena experiencia de uso.

### Modelos Locales vs Modelos en la Nube

Ollama soporta dos formas de ejecución de modelos:

**Modelos locales:**
- Los archivos del modelo se descargan a su computadora
- Ejecución completamente local, no necesita conexión a internet (después de descargar)
- Limitado por la configuración de su hardware (memoria, tarjeta gráfica)
- Generalmente parámetros 4B-70B
- Uso gratuito, sin límites

**Modelos en la nube:**
- Los modelos se ejecutan en el servidor en la nube de Ollama
- Solo necesita descargar el identificador del modelo (muy pequeño)
- Puede ejecutar modelos súper grandes que las computadoras domésticas no pueden soportar (generalmente más de 200B)
- Necesita conexión a internet para usar
- Tiene límites de cuota de uso (**se actualiza semanalmente**)
- Puede usar habilitando la función en la nube del cliente Ollama

### Recomendaciones de Configuración de Hardware

### Modelos Recomendados

A continuación se muestran los modelos gratuitos de uso común:

| Nombre del Modelo | Inteligencia | Tamaño | Características | Escenarios Adecuados |
|---------|------|------|------|---------|
| **qwen3.5:8b** | 8B | Aproximadamente 4-5GB | Fuerte capacidad en chino, buen rendimiento general | Conversación diaria, escritura, traducción |
| **qwen3.5:14b** | 14B | Aproximadamente 8-9GB | Mayor capacidad en chino, mejora en razonamiento | Tareas complejas, procesamiento de texto largo |
| **qwen3.5:32b** | 32B | Aproximadamente 18-20GB | Alta inteligencia, excelente razonamiento complejo | Tareas profesionales, análisis profundo |
| **llama3:8b** | 8B | Aproximadamente 4-5GB | Fuerte capacidad en inglés, buena versatilidad | Conversación en inglés, tareas generales |
| **llama3:70b** | 70B | Aproximadamente 40GB | Inteligencia ultra alta, mejor en inglés | Tareas difíciles en inglés |
| **gemma3:4b** | 4B | Aproximadamente 2-3GB | Ligero, rápido | Respuesta rápida, computadoras de baja configuración |
| **gemma3:12b** | 12B | Aproximadamente 7-8GB | Equilibrio entre rendimiento y recursos | Uso diario |
| **mistral:7b** | 7B | Aproximadamente 4GB | Equilibrio entre rendimiento y velocidad | Escenarios generales |
| **codellama:7b** | 7B | Aproximadamente 4GB | Especializado en programación | Generación y depuración de código |
| **codellama:13b** | 13B | Aproximadamente 7-8GB | Mayor capacidad de programación | Tareas de código complejas |

**Recomendado para usuarios chinos: qwen3.5:8b o qwen3.5:14b**

### Descargar Modelos

**Método 1: Descarga por línea de comandos**

Abra la terminal (o símbolo del sistema), ingrese:

```bash
ollama pull qwen3.5
```

El sistema descargará automáticamente el modelo, lo cual tomará algún tiempo (dependiendo de la velocidad de la red y el tamaño del modelo).

**Método 2: Ejecutar y descargar automáticamente**

```bash
ollama run qwen3.5
```

Si el modelo no se ha descargado, comenzará a descargar automáticamente.

### Ejecutar Modelos

Después de completar la descarga, ejecute el modelo:

```bash
ollama run qwen3.5
```

Esto abrirá una interfaz de conversación interactiva, puede conversar directamente con la IA.

**Ejemplo de conversación:**
```
>>> ¡Hola!
¡Hola! Soy Qwen, ¿en qué puedo ayudarte?

>>> Por favor, escribe un poema sobre la primavera
La brisa primaveral acaricia el rostro y las flores se abren,
La verde vitalidad llena el jardín de fragancia.
Las golondrinas regresan buscando sus viejos nidos,
En abril, qué buenos momentos en el mundo humano.
```

Presione `Ctrl + D` o ingrese `/bye` para salir de la conversación.

### Ver Modelos Descargados

```bash
ollama list
```

Mostrará la lista de todos los modelos descargados.

### Eliminar Modelos No Necesarios

```bash
ollama rm qwen3.5
```

## Usar Ollama en Silicon Life

### Configurar Conexión

1. Asegúrese de que Ollama esté iniciado y funcionando
2. Abra el sistema Silicon Life
3. Vaya a la página **⚙ Configuración**
4. Encuentre ""Tipo de cliente de IA"", seleccione `OllamaClient`
5. En ""Configuración de IA"" configure:
   - **endpoint**: `http://localhost:11434` (predeterminado)
   - **model**: `qwen3.5` (u otro modelo que haya descargado)
6. Guardar configuración

### Probar Conexión

1. Vaya a la página **💬 Chat**
2. Seleccione un ser de silicio
3. Envíe un mensaje
4. Si recibe una respuesta, significa que la conexión fue exitosa

## Preguntas Frecuentes

### P: ¿Qué hacer si la descarga de Ollama es muy lenta?

**Solución:**
- Los archivos de modelo generalmente son grandes (2-8GB), la descarga toma tiempo
- Asegúrese de que la conexión de red sea estable
- Puede descargar durante la noche o cuando la red esté libre

### P: ¿Qué hacer si la descarga se interrumpe?

**Solución:**
Vuelva a ejecutar el comando de descarga, continuará descargando:
```bash
ollama pull qwen3.5
```

### P: ¿Cómo saber qué tamaño de modelo puede ejecutar mi computadora?

**Recomendaciones de coincidencia entre memoria y tamaño de modelo:**
- **4GB de memoria**: Se recomienda modelo de menos de 2GB (aproximadamente 2B-3B)
- **8GB de memoria**: Puede ejecutar modelo de 4GB (aproximadamente 7B-8B)
- **16GB de memoria**: Puede ejecutar modelo de 8GB (aproximadamente 13B-14B)
- **32GB de memoria**: Puede ejecutar modelo de 16GB, pero se ralentizará notablemente y se calentará más (aproximadamente 32B)
- **64GB o más**: Puede ejecutar modelos más grandes sin problemas

**Recordatorio importante:**
- Cuando una laptop con 32GB de memoria ejecuta un modelo de alrededor de 16B, experimentará **ralentizaciones notables** y **mayor calentamiento**
- Esto no es un fallo, sino un comportamiento normal por insuficiencia de recursos de hardware
- **Recomendación**: En este caso, seleccione un modelo más pequeño (8B-14B), o actualice a hardware de mayor configuración

**Comience a probar con modelos ligeros**, si funciona sin problemas, luego intente con modelos más grandes.

### P: ¿Qué hacer si Ollama no se inicia?

**Verifique:**
1. Si el puerto 11434 está siendo ocupado por otro programa
2. Reinstalar Ollama
3. Ver los registros de Ollama para obtener información de errores

### P: ¿Qué hacer si el modelo se ejecuta muy lentamente?

**Recomendaciones:**
- Usar un modelo más pequeño (como gemma3 en lugar de qwen3.5)
- Cerrar otros programas que consumen memoria
- Verificar si la configuración de la computadora cumple con los requisitos

### P: ¿Se pueden usar múltiples modelos simultáneamente en Silicon Life?

**R:** Sí. Descargue múltiples modelos en Ollama, luego seleccione diferentes modelos para diferentes seres de silicio en la configuración de IA de Silicon Life.

### P: ¿Ollama necesita conexión a internet?

**R:** 
- **Al descargar modelos**: Necesita conexión a internet
- **Al ejecutar modelos**: No necesita conexión a internet (el modelo ya está descargado localmente)

### P: ¿Cuánto espacio en disco ocupan los modelos?

**R:** 
- Modelos pequeños: Aproximadamente 2-4GB
- Modelos medianos: Aproximadamente 4-8GB
- Modelos grandes: Más de 8GB

Se recomienda mantener suficiente espacio en disco.

## Obtener Más Ayuda

- **Sitio web oficial de Ollama**: https://ollama.com
- **Documentación de Ollama**: https://docs.ollama.com
- **Biblioteca de modelos**: https://ollama.com/library (ver todos los modelos disponibles)

## Próximos Pasos

Después de instalar Ollama y descargar modelos, puede:
- Configurar y usar IA local en Silicon Life
- Disfrutar de servicios de IA completamente locales
- Proteger su privacidad y seguridad de datos

¡Le deseamos un uso agradable!
";

    public override string BailianDashScope => @"
# Guía de Uso de la Plataforma Alibaba Cloud Bailian

## ¿Qué es Alibaba Cloud Bailian?

Alibaba Cloud Bailian (DashScope) es una plataforma de servicios de modelos grandes proporcionada por Alibaba Cloud, que ofrece múltiples modelos de IA de alta calidad, incluyendo Tongyi Qianwen, DeepSeek, GLM, Kimi, etc.

**Ventajas:**
- Alta inteligencia de modelos (puede alcanzar cientos de B)
- No requiere hardware local, ejecución en la nube
- Soporta múltiples modelos de IA de primer nivel
- Pago por uso, costo controlable
- Compatible con formato de API de OpenAI

## Registro y Activación del Servicio

### Paso 1: Registrar Cuenta de Alibaba Cloud

1. Visite el sitio web oficial de Alibaba Cloud: https://www.aliyun.com
2. Haga clic en ""Registro Gratuito""
3. Complete el registro según las indicaciones (soporta registro por teléfono móvil, correo electrónico)
4. Complete la verificación de identidad real (necesita usar Alipay o tarjeta bancaria)

### Paso 2: Activar Servicio Bailian

1. Inicie sesión en la consola de Alibaba Cloud
2. Busque ""Bailian"" o ""DashScope""
3. Haga clic para entrar en la página del producto Bailian
4. Haga clic en ""Activar Ahora""
5. Lea y acepte el acuerdo de servicio
6. Complete la activación

### Paso 3: Obtener API Key

1. Entre en la consola de Bailian
2. Encuentre ""Gestión de API Key"" o ""Gestión de Claves"" en el menú izquierdo
3. Haga clic en ""Crear API Key""
4. Asigne un nombre a la clave (como ""SiliconLife"")
5. Copie y guarde la API Key (**se muestra solo una vez, guárdela adecuadamente**)

## Configurar Bailian en Silicon Life

### Pasos de Configuración

1. Abra el sistema Silicon Life
2. Vaya a la página **⚙ Configuración**
3. Encuentre ""Tipo de cliente de IA"", seleccione `DashScopeClient`
4. En ""Configuración de IA"" complete:
   - **API Key**: Pegue la API Key que copió
   - **Region (Región)**: Seleccione la región del servidor (como beijing)
   - **Model (Modelo)**: Seleccione un modelo (**después de completar la API Key y seleccionar la región, el sistema obtendrá automáticamente todos los modelos disponibles en esa región**)
5. Guardar configuración

**Consejo:**
- Debe completar la API Key y seleccionar la región primero para que se cargue la lista desplegable de modelos
- Si la carga de la lista de modelos falla, se mostrará una lista de modelos recomendados

### Selección de Región

El sistema soporta las siguientes regiones:

| Región | Ubicación | Descripción |
|------|------|------|
| **beijing** | Beijing, China | Recomendado por defecto, velocidad de acceso rápida en China |
| **virginia** | Virginia, EE.UU. | Adecuado para usuarios en el extranjero |
| **singapore** | Singapur | Región Asia-Pacífico |
| **hongkong** | Hong Kong, China | Región Asia-Pacífico |
| **frankfurt** | Frankfurt, Alemania | Región europea |

**Recomendaciones de selección de región:**
- **Usuarios en China continental**: Seleccione beijing (Beijing), la velocidad de acceso es más rápida, **pero el soporte para traducción es más débil**
- **Necesita traducción de alta calidad**: Seleccione singapore (Singapur) o hongkong (Hong Kong), mejor efecto de traducción, los usuarios en China continental también pueden acceder
- **Usuarios en el extranjero**: Seleccione la región más cercana a usted

### Selección de Modelo

Al configurar el modelo, el sistema obtendrá automáticamente la lista de modelos disponibles de la plataforma Bailian. Si la obtención falla, se mostrarán modelos recomendados:

**Modelos recomendados:**

| Nombre del Modelo | Inteligencia | Características | Escenarios Adecuados |
|---------|------|------|---------|
| **qwen3-max** | Ultra grande | Versión más potente de Tongyi Qianwen | Razonamiento complejo, tareas profesionales |
| **qwen3.6-plus** | Grande | Equilibrio entre rendimiento y costo | Uso diario (recomendado) |
| **qwen3.6-flash** | Mediano | Rápido, bajo costo | Respuesta rápida |
| **qwen-max** | Ultra grande | Modelo insignia de la generación anterior | Tareas de alta dificultad |
| **qwen-plus** | Grande | Equilibrio de rendimiento | Escenarios generales |
| **qwen-turbo** | Mediano | Más rápido | Tareas simples |
| **qwen3-coder-plus** | Grande | Especializado en programación | Generación y depuración de código |
| **qwq-plus** | Grande | Fuerte capacidad de razonamiento | Matemáticas, razonamiento lógico |
| **deepseek-v3.2** | Ultra grande | Última versión de DeepSeek | Fuerte capacidad integral |
| **deepseek-r1** | Ultra grande | Especializado en razonamiento | Razonamiento complejo |
| **glm-5.1** | Grande | Modelo de Zhipu AI | Escenarios en chino |
| **kimi-k2.5** | Grande | Modelo de Moonshot | Procesamiento de texto largo |
| **llama-4-maverick** | Grande | Modelo de código abierto de Meta | Escenarios en inglés |

**Recomendado para usuarios chinos: qwen3.6-plus o qwen3-max**

## Descripción de Costos

### Método de Facturación

La plataforma Bailian utiliza un modelo de **pago por uso**:
- Facturación por cantidad de tokens ingresados
- Diferentes modelos tienen diferentes precios
- Cuanto mayor es la inteligencia del modelo, más caro es el precio

### Cuota Gratuita

- Los nuevos usuarios generalmente tienen una cuota de prueba gratuita
- Algunos modelos tienen cuota de llamada gratuita
- La cuota específica está sujeta a los anuncios de la plataforma Bailian

### Ver Uso

1. Inicie sesión en la consola de Bailian
2. Vaya a ""Consulta de Uso"" o ""Gestión de Facturas""
3. Ver cantidad de llamadas y costos

### Recomendaciones para Ahorrar Costos

- Seleccionar el modelo adecuado (no necesariamente el más caro)
- Evitar enviar textos demasiado largos
- Verificar regularmente el uso, controlar costos

### Descripción de Estadísticas de Uso de Tokens

**Importante:** Dado que la API de la plataforma Alibaba Cloud Bailian (DashScope) **no devuelve de manera estable el campo de uso de Tokens** en la respuesta, este sistema **no puede estadísticas** el uso de Tokens de conversación al usar modelos Bailian.

**Esto significa:**
- El sistema no mostrará cuántos Tokens se usaron
- No se puede ver el historial de consumo de Tokens en el sistema
- No se puede realizar análisis de costos basado en el uso de Tokens

**Cómo ver el uso:**
- Inicie sesión en la consola de Alibaba Cloud Bailian para ver el uso y costos reales
- La consola de Bailian proporcionará estadísticas de llamadas detalladas e información de facturas

## Preguntas Frecuentes

### P: ¿Dónde obtener la API Key?

**R:** 
1. Inicie sesión en la consola de Alibaba Cloud Bailian
2. Encuentre ""Gestión de API Key""
3. Crear nueva API Key
4. Copiar y guardar adecuadamente

### P: ¿Qué hacer si la API Key se filtra?

**R:**
1. Inicie sesión inmediatamente en la consola de Bailian
2. Eliminar la API Key filtrada
3. Crear nueva API Key
4. Actualizar la configuración en Silicon Life

### P: ¿Cuál es la mejor región para elegir?

**R:**
- **Usuarios nacionales**: Seleccione beijing (Beijing), la velocidad es más rápida
- **Usuarios en el extranjero**: Seleccione la región más cercana
- La región no afecta la calidad del modelo, solo afecta la velocidad de acceso

### P: ¿Por qué falla la carga de la lista de modelos?

**Posibles razones:**
1. La API Key es incorrecta o ha expirado
2. Problemas de conexión de red
3. Servicio de Bailian anormal

**Solución:**
1. Verificar si la API Key es correcta
2. Verificar la conexión de red
3. Intentar nuevamente más tarde

### P: ¿Se pueden usar múltiples modelos?

**R:** Sí. En Silicon Life, puede configurar diferentes modelos Bailian para diferentes seres de silicio.

### P: ¿Cuál es la diferencia entre Bailian y Ollama?

| Característica | Bailian (DashScope) | Ollama |
|------|------------------|--------|
| Ubicación de ejecución | Nube | Computadora local |
| Requisitos de hardware | Ninguno | Requiere configuración alta |
| Tamaño del modelo | Puede alcanzar cientos de B | Generalmente 4B-70B |
| Costo | Pago por uso | Gratuito |
| Conexión a internet | Debe estar conectado | No necesita conexión después de descargar |
| Privacidad | Datos enviados a la nube | Completamente local |

**Recomendaciones de selección:**
- Configuración de hardware baja → Seleccione Bailian
- Necesita modelos de inteligencia ultra alta → Seleccione Bailian
- Prioriza la privacidad, no quiere gastar dinero → Seleccione Ollama
- Uso en entorno sin internet → Seleccione Ollama

### P: ¿Qué hacer si la llamada falla?

**Verifique:**
1. Si la API Key es correcta
2. Si el saldo de la cuenta es suficiente
3. Si excedió la cuota gratuita
4. Ver la información de errores en la consola de Bailian

### P: ¿Cómo controlar los costos?

**Recomendaciones:**
1. Configurar alertas de presupuesto (consola de Bailian)
2. Verificar regularmente el uso
3. Seleccionar modelos con buena relación calidad-precio
4. Evitar llamar frecuentemente a modelos grandes

## Mejores Prácticas

### 1. Seleccionar el Modelo Adecuado

- **Conversación diaria**: qwen3.6-plus (equilibrio)
- **Razonamiento complejo**: qwen3-max o deepseek-r1
- **Tareas de programación**: qwen3-coder-plus
- **Respuesta rápida**: qwen3.6-flash o qwen-turbo

### 2. Gestionar API Key

- Guardar adecuadamente, no compartir públicamente
- Rotar regularmente (crear nuevas, eliminar antiguas)
- Crear diferentes Key para diferentes propósitos

### 3. Monitorear Uso

- Verificar el uso una vez por semana
- Configurar alertas de presupuesto
- Investigar oportunamente uso anormal

### 4. Optimizar Uso

- Simplificar el contenido de entrada, reducir texto innecesario
- Configurar razonablemente la longitud del historial de conversaciones
- Usar el modelo adecuado, no perseguir ciegamente modelos grandes

## Obtener Más Ayuda

- **Sitio web oficial de Bailian**: https://bailian.console.aliyun.com
- **Documentación de API**: https://help.aliyun.com/zh/model-studio
- **Lista de modelos**: https://help.aliyun.com/zh/model-studio/models
- **Descripción de precios**: https://www.aliyun.com/price/product#/dashscope
- **Soporte técnico**: Enviar ticket o contactar al servicio de atención al cliente de Alibaba Cloud

## Próximos Pasos

Después de configurar Bailian, puede:
- Usar modelos de IA en la nube de alta calidad en Silicon Life
- Experimentar servicios de IA de inteligencia ultra alta
- No preocuparse por la configuración de hardware local

¡Le deseamos un uso agradable!
";
    
    #endregion
}
