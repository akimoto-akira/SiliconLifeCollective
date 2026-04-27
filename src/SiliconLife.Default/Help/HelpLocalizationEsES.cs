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
    public override string Dashboard_Title => "Panel de Control";
    public override string Task_Title => "Tareas";
    public override string Timer_Title => "Temporizadores";
    public override string Permission_Title => "Gestión de Permisos";
    public override string Config_Title => "Gestión de Configuración";
    public override string FAQ_Title => "Preguntas Frecuentes";
    public override string Memory_Title => "Sistema de Memoria";
    public override string OllamaSetup_Title => "Instalación de Ollama y Descarga de Modelos";
    public override string BailianDashScope_Title => "Guía de Uso de la Plataforma Alibaba Cloud Bailian";
    public override string AIClients_Title => "Configuración de Cliente de IA";
        
        public override string BeingSoul_Title => "Archivo de Alma";
    public override string AuditLog_Title => "Registro de Auditoría";
    public override string KnowledgeGraph_Title => "Grafo de Conocimiento";
    public override string WorkNotes_Title => "Notas de Trabajo";
    public override string Projects_Title => "Gestión de Proyectos";
    public override string Logging_Title => "Sistema de Registro de Eventos";

    public override string[] GettingStarted_Tags => new[] { "instalación", "inicio", "introducción", "inicio rápido", "empezar", "inicialización", "ejecutar", "configurar entorno" };
    public override string[] BeingManagement_Tags => new[] { "ser de silicio", "crear", "configurar", "gestión de seres de silicio", "entidad", "perfil", "ajustes", "gestión" };
    public override string[] ChatSystem_Tags => new[] { "chat", "conversación", "mensaje", "función de chat", "comunicación", "intercambio", "diálogo", "discusión" };
    public override string[] Dashboard_Tags => new[] { "panel de control", "monitor", "estadísticas", "estado", "sistema", "frecuencia de mensajes", "tiempo de actividad", "memoria" };
    public override string[] Task_Tags => new[] { "tarea", "trabajo", "ejecución", "prioridad", "dependencia", "estado", "automatización", "gestión" };
    public override string[] Timer_Tags => new[] { "temporizador", "programación", "disparador", "período", "calendario", "recordatorio", "automático", "programado" };
    public override string[] Permission_Tags => new[] { "permiso", "seguridad", "control de acceso", "gestión de permisos", "autorización", "privacidad", "protección", "autenticación" };
    public override string[] Config_Tags => new[] { "configuración", "ajustes", "opciones", "gestión de configuración", "preferencias", "personalización", "sistema", "parámetros" };
    public override string[] FAQ_Tags => new[] { "preguntas frecuentes", "ayuda", "problemas", "soporte", "solución de problemas", "guía", "asistencia", "respuestas" };
    public override string[] Memory_Tags => new[] { "memoria", "historial", "registro", "sistema de memoria", "actividad", "rastreo", "búsqueda", "registro de eventos" };
    public override string[] OllamaSetup_Tags => new[] { "Ollama", "instalación", "modelo", "descarga", "IA local", "configuración", "ajustes", "ejecución" };
    public override string[] BailianDashScope_Tags => new[] { "Bailian", "DashScope", "Alibaba Cloud", "IA en la nube", "API", "configuración", "modelo", "pago" };
    public override string[] AIClients_Tags => new[] { "cliente de IA", "servicio de IA", "modelo", "configuración", "local", "nube", "Ollama", "DashScope", "ajustes" };
    
    public override string[] BeingSoul_Tags => new[] { "archivo de alma", "personalidad", "prompt", "rol", "comportamiento", "configuración", "carácter", "guía de trabajo", "prompt del sistema" };
    public override string[] AuditLog_Tags => new[] { "registro de auditoría", "Token", "estadísticas de uso", "monitor", "consumo", "análisis", "tendencia", "exportar", "CSV" };
    public override string[] KnowledgeGraph_Tags => new[] { "grafo de conocimiento", "conocimiento", "visualización", "triple", "entidad", "relación", "red", "aprendizaje", "gestión" };
    public override string[] WorkNotes_Tags => new[] { "notas de trabajo", "notas", "registros", "diario", "bitácora", "Markdown", "palabras clave", "versión", "búsqueda" };
    public override string[] Projects_Tags => new[] { "gestión de proyectos", "proyecto", "colaboración", "tarea", "miembro", "archivo", "equipo", "espacio de trabajo", "progreso" };
    public override string[] Logging_Tags => new[] { "sistema de registro", "registro", "eventos", "debug", "error", "advertencia", "monitor", "rastreo", "consola", "archivo" };
    
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

    public override string Dashboard => @"
# Panel de Control

## Descripción General

El Panel de Control es su centro de monitoreo del sistema, permitiéndole comprender el estado de ejecución de los seres de silicio de un vistazo. A través de tarjetas de estadísticas intuitivas y gráficos, puede mantenerse al tanto de la situación general del sistema en cualquier momento.

## Funciones Principales

- **Monitoreo en tiempo real**: Muestra la cantidad de seres de silicio, estado activo y uso de recursos del sistema
- **Estadísticas de mensajes**: Muestra la frecuencia de mensajes de chat recientes en forma de gráfico
- **Actualización automática**: Los datos se actualizan automáticamente, sin necesidad de operación manual

## Descripción de la Interfaz

### Tarjetas de Estadísticas

En la parte superior de la página se muestran 4 tarjetas de estadísticas, presentando información clave del sistema:

| Tarjeta | Descripción |
|------|------|
| 🧠 Total de Seres de Silicio | La cantidad total de todos los seres de silicio que ha creado |
| ⚡ Seres de Silicio Activos | La cantidad de seres de silicio que están trabajando o procesando tareas actualmente |
| ⏱️ Tiempo de Actividad del Sistema | El tiempo que ha estado ejecutándose el programa desde que se inició |
| 💾 Uso de Memoria | El tamaño de memoria de la computadora que está utilizando el programa actualmente |

### Gráfico de Frecuencia de Mensajes

Debajo de las tarjetas de estadísticas se muestra un gráfico de barras, presentando la cantidad de mensajes en cada punto de tiempo en los últimos 20 minutos:
- Cuanto más alta es la barra, más frecuente es la interacción de mensajes en ese período
- Si la barra está vacía, significa que no hubo interacción de mensajes en ese período

## Cómo Acceder

1. Inicie el programa Silicon Life Collective

2. Abra el navegador y acceda a la interfaz web del programa

3. Haga clic en el elemento de menú **📊 Panel de Control** en la barra de navegación izquierda

## Cómo Entender los Datos

### Actividad de Seres de Silicio

- Si la cantidad activa está cerca del total: indica que la mayoría de los seres están ocupados trabajando
- Si la cantidad activa es 0: indica que todos los seres están descansando, esperando nuevas tareas

### Uso de Memoria
- Rango general: 50-300 MB (depende de la cantidad de seres creados y la longitud del historial de conversaciones)
- Si nota que la memoria crece continuamente y supera los 500MB, se recomienda reiniciar el programa

### Tendencia de Frecuencia de Mensajes
- Observe los cambios de altura de las barras para entender en qué momentos usa el sistema con más frecuencia

## Preguntas Frecuentes

### P: ¿Por qué no se actualizan las estadísticas?

**R**: Verifique los siguientes puntos:
1. Si hay errores de JavaScript en la consola del navegador
2. Si las solicitudes de red son normales (F12 → pestaña Network)
3. Si el servicio backend se está ejecutando normalmente
4. Intente actualizar la página (F5)

### P: ¿El gráfico se muestra vacío o sin datos?

**R**: Posibles razones:
1. El sistema acaba de iniciarse, aún no hay registros de interacción de mensajes
2. No se han generado mensajes en los últimos 20 minutos
3. El servicio ChatSystem no se inicializó correctamente

### P: ¿El tiempo de actividad se muestra incorrectamente?

**R**: El tiempo de actividad se calcula desde que se inicia la aplicación, si se muestra anormalmente:
1. Verifique si la hora del sistema es correcta
2. Reinicie la aplicación para reiniciar el cronómetro

### P: ¿Cómo actualizar manualmente los datos?

**R**: La versión actual se actualiza automáticamente, si necesita actualizar manualmente:
- Presione F5 para actualizar toda la página
- O presione Ctrl+F5 para forzar la actualización (limpiar caché)

## Sugerencias de Uso

### Monitoreo Diario

1. **Revisar regularmente**: Se recomienda abrirlo todos los días para entender el estado de ejecución del sistema
2. **Observar tendencias**: A través del gráfico de frecuencia de mensajes, entienda en qué momentos usa el sistema con más frecuencia
3. **Prestar atención a la memoria**: Si el uso de memoria supera los 500MB, puede considerar reiniciar el programa
4. **Entender la actividad**: A través de la cantidad de seres activos, juzgue si el sistema está funcionando normalmente

### Sugerencias de Optimización

1. **Controlar la cantidad de seres**: Crear demasiados seres ocupará más memoria
2. **Limpiar conversaciones antiguas**: Limpie regularmente el historial de conversaciones que no necesita para liberar espacio de almacenamiento
3. **Combinar con herramientas del sistema**: Puede monitorear los recursos del sistema junto con el administrador de tareas de la computadora

### ¿Qué hacer si encuentra problemas?

Si el Panel de Control no se muestra correctamente, puede intentar los siguientes pasos:

1. **Verificar el navegador**: Abra la consola del navegador (presione F12), verifique si hay mensajes de error
2. **Verificar la conexión de red**: Confirme que el programa se está ejecutando y la red es normal
3. **Verificar los registros del programa**: Verifique si el programa tiene registros anormales
4. **Reiniciar el programa**: Si ninguno de los métodos anteriores funciona, intente cerrar el programa y abrirlo nuevamente
";

    public override string Task => @"
# Tareas

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

    public override string Timer => @"
# Temporizadores

## ¿Qué son los temporizadores?

Los temporizadores son el mecanismo automático de recordatorio de los seres de silicio. Cuando se alcanza el tiempo establecido, el ser de silicio ejecutará automáticamente la tarea correspondiente.

## Dos tipos de temporizadores

### Temporizador único

Un temporizador que solo se activa una vez. Después de activarse, finaliza automáticamente.

**Escenarios aplicables:**
- Recordarle que haga algo en un momento específico
- Ejecutar una tarea única en una fecha específica

### Temporizador periódico

Un temporizador que se activará repetidamente. Después de cada activación, el sistema calculará automáticamente el tiempo de la próxima activación.

**Escenarios aplicables:**
- Tareas que se ejecutan diariamente a una hora fija (como un informe matutino todos los días a las 9 a. m.)
- Tareas que se ejecutan regularmente cada semana o mes
- Tareas que se ejecutan regularmente según las festividades del calendario lunar (como cada Año Nuevo lunar)

## Estados de los temporizadores

Los temporizadores tienen cuatro estados:

| Estado | Descripción |
|------|------|
| **En ejecución** | El temporizador funciona normalmente y espera la activación |
| **Pausado** | El temporizador está detenido temporalmente y no se activará |
| **Activado** | El temporizador único ha completado la activación |
| **Cancelado** | El temporizador ha sido cancelado y no se activará más |

## ¿Cómo ver los temporizadores?

### Ver los temporizadores de un ser de silicio específico

1. Vaya a la página **🧠 Ser de Silicio**
2. Seleccione el ser de silicio que desea ver
3. Encuentre la opción ""Temporizadores"" para ver todos los temporizadores de ese ser

## Información mostrada en los temporizadores

Al ver los temporizadores, puede ver la siguiente información:

| Elemento de información | Descripción |
|------|------|
| **Nombre** | El nombre del temporizador |
| **Descripción** | La descripción detallada del temporizador (si existe) |
| **Estado** | Estado actual (En ejecución, Pausado, etc.) |
| **Tipo** | Único o periódico |
| **Próxima activación** | La hora específica en que el temporizador se activará la próxima vez |
| **Sistema de calendario** | Calendario utilizado (como gregoriano, lunar, etc.) |
| **Número de activaciones** | Cuántas veces se ha activado el temporizador |
| **Hora de creación** | Cuándo se creó el temporizador |
| **Última activación** | La hora de la última activación (si ya se ha activado) |

## Sistemas de calendario admitidos

Los temporizadores admiten varios sistemas de calendario, incluyendo:

- **Calendario gregoriano** (Gregorian): Sistema solar de uso internacional
- **Calendario lunar** (Chinese Lunar): Calendario lunar tradicional chino
- **Otros sistemas de calendario**: Calendario islámico, calendario de troncos celestiales, etc.
- **Calendario de intervalo** (Interval): Activación en intervalos de tiempo fijos (como cada 2 horas)

## Historial de activación de los temporizadores

Cada vez que se activa un temporizador, el sistema registra información detallada de ejecución.

### Ver el historial de activación

1. Seleccione un temporizador específico en la lista de temporizadores
2. Vea el historial de ejecución de ese temporizador
3. Puede ver los detalles de cada activación:
   - Hora de activación
   - Estado de ejecución (éxito, fallo, etc.)
   - Mensajes de conversación durante la ejecución (si existen)
   - Información de error (si la ejecución falló)

### Estados del historial de ejecución

Cada ejecución tiene los siguientes estados:

| Estado | Descripción |
|------|------|
| **No iniciado** | La ejecución aún no ha comenzado |
| **Iniciado** | Se ha enviado la notificación de inicio |
| **En ejecución** | La tarea se está ejecutando |
| **Completado** | La tarea se ha completado con éxito |
| **Fallido** | La ejecución de la tarea ha fallado |

## Preguntas frecuentes

### P: ¿Cómo creo un nuevo temporizador?

**R:** Los temporizadores son gestionados automáticamente por los seres de silicio. Los seres de silicio crean temporizadores según sea necesario para ejecutar tareas periódicas.

### P: ¿Puedo eliminar o pausar temporizadores?

**R:** Sí. El sistema admite las siguientes operaciones:
- **Pausar temporizador**: Detener temporalmente el temporizador, se puede reanudar cuando sea necesario
- **Reanudar temporizador**: Restaurar un temporizador pausado al estado de ejecución. Si se perdió una hora de activación durante la pausa, el sistema calculará automáticamente la próxima hora de activación
- **Cancelar temporizador**: Cancelar permanentemente un temporizador, no se activará más
- **Eliminar temporizador**: Eliminar completamente un temporizador del sistema

### P: ¿Qué hago si el temporizador no se activa?

**Verifique lo siguiente:**
1. Si el temporizador está en estado **En ejecución** (no Pausado o Cancelado)
2. Si ha llegado la hora de activación (verifique ""Próxima activación"")
3. Si el ser de silicio se está ejecutando normalmente

### P: ¿Cómo sé cómo funcionan los temporizadores?

**Métodos:**
1. Verifique la lista de temporizadores para comprender las próximas operaciones
2. Verifique el historial de ejecución para comprender las actividades pasadas
3. Pregunte directamente al ser de silicio en la conversación.

### P: ¿Cómo calcula un temporizador periódico la próxima hora de activación?

**R:** Después de cada activación de un temporizador periódico, el sistema calcula automáticamente la próxima hora de activación según el sistema de calendario y las condiciones establecidas. Por ejemplo:
- Si se configura Gregoriano ""todos los días a las 9"", el sistema se activará todos los días a las 9 y luego calculará las 9 del día siguiente como la próxima hora de activación
- Si se configura Lunar ""primer día del primer mes de cada año"", el sistema se activará durante el Año Nuevo lunar y luego calculará la fecha del próximo Año Nuevo lunar

## Recomendaciones de uso

### Monitoreo diario

1. **Verificar regularmente**: Comprender la ejecución automática de los seres de silicio
2. **Prestar atención al historial de ejecución**: Comprender el modo de trabajo de la IA
3. **Verificar anomalías**: Verificar si hay registros de ejecución fallida

### Recomendaciones de optimización

1. **Combinar con conversación**: Discutir el estado de los temporizadores con el ser de silicio
2. **Observar los resultados de ejecución**: Comprender el efecto de los temporizadores a través del historial de ejecución
3. **Ajustar estrategias**: Si es necesario, haga que el ser de silicio ajuste la configuración de los temporizadores a través de la conversación
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
    
    public override string AIClients => @"
# Configuración de Cliente de IA

## Descripción General

Los clientes de IA son los ""conectores cerebrales"" del sistema Silicon Life, responsables de comunicarse con modelos de inteligencia artificial. El sistema admite múltiples clientes de IA, y puede elegir el servicio apropiado según sus necesidades.

## Clientes de IA Admitidos

El sistema admite múltiples clientes de IA, y puede elegir el servicio apropiado según sus necesidades.

### Cliente de IA Local

**Características:**
- 🏠 **Ejecución Local**: Los modelos de IA se ejecutan en su computadora
- 🔒 **Privacidad y Seguridad**: Los datos no se suben a la nube
- 💰 **Completamente Gratis**: Sin restricciones de uso
- ⚡ **Respuesta Rápida**: Sin latencia de red (modelos locales)

**Escenarios Adecuados:**
- Usuarios conscientes de la privacidad
- Mejor configuración de computadora (recomendado 16GB+ RAM)
- Uso ilimitado de IA
- Condiciones de red deficientes

**Requisitos de Configuración:**
- Necesita instalar software de servicio de IA correspondiente
- Necesita descargar archivos de modelo de IA (generalmente 4-20GB)
- Recomendado 16GB+ RAM, tarjeta gráfica dedicada es mejor

### Cliente de IA en la Nube

**Características:**
- ☁️ **Servicio en la Nube**: Los modelos de IA se ejecutan en servidores remotos
- 🚀 **Potente**: Puede usar modelos ultra grandes (200B+ parámetros)
- 💳 **Pago por Uso**: Tiene cuota gratuita, se cobra después de exceder
- 🌍 **Soporte Multi-Región**: Puede elegir servidores más cercanos

**Escenarios Adecuados:**
- Configuración de computadora más baja
- Necesita usar modelos de alta gama
- Uso ocasional, bajo uso
- Quiere comenzar rápidamente sin configuración local

**Requisitos de Configuración:**
- Necesita cuenta y clave API para plataforma correspondiente
- Necesita conexión a Internet
- Tiene límites de cuota de uso (puede actualizarse periódicamente)

## ¿Cómo Elegir un Cliente de IA?

### Diagrama de Flujo de Selección

```
¿Cuál es su configuración de computadora?
├─ Configuración alta (16GB+ RAM)
│  └─ ¿Consciente de la privacidad?
│     ├─ Sí → Elegir cliente local (ej. Ollama)
│     └─ No → Cualquiera está bien
└─ Configuración baja (8GB o menos)
   └─ Elegir cliente en la nube (ej. DashScope)
```

### Tabla Comparativa

| Característica | Cliente Local | Cliente en la Nube |
|------|--------------|-----------------|
| Dificultad de instalación | Media (necesita instalar software y modelos) | Simple (solo necesita clave API) |
| Costo de ejecución | Gratis (consume electricidad) | Tiene cuota gratuita, se cobra después de exceder |
| Protección de privacidad | ⭐⭐⭐⭐⭐ Completamente local | ⭐⭐⭐ Los datos pasan por la nube |
| Selección de modelos | Limitado por configuración de computadora | Puede elegir varios modelos grandes |
| Requisito de red | Solo necesario al descargar modelos | Siempre necesario |
| Velocidad de respuesta | Rápida (local) | Depende de la red |
| Límites de uso | Sin límites | Tiene límites de cuota |

## Configurar Cliente de IA

### Paso 1: Entrar a la Página de Configuración

1. Abra el sistema Silicon Life
2. Haga clic en el menú **⚙ Configuración** en la barra de navegación superior

### Paso 2: Seleccionar Tipo de Cliente de IA

1. Encuentre la opción **""Tipo de Cliente de IA""** en la página de configuración
2. Seleccione su cliente deseado del menú desplegable:
   - Cliente local (ej. `OllamaClient`)
   - Cliente en la nube (ej. `DashScopeClient`)

### Paso 3: Completar Información de Configuración

Después de seleccionar el cliente, aparecerán elementos de configuración correspondientes abajo:

#### Configuración de Cliente Local (ej. Ollama)

| Elemento de Configuración | Descripción | Valor Predeterminado | Ejemplo |
|--------|------|--------|------|
| **endpoint** | Dirección de servicio de IA | Depende del servicio específico | ej. `http://localhost:11434` |
| **model** | Nombre del modelo a usar | Depende del servicio específico | ej. `qwen3.5:8b` |
| **temperature** | Nivel de creatividad (0-1) | `0.7` | `0.5` más conservador, `0.9` más creativo |
| **maxTokens** | Longitud máxima de respuesta | `2048` | `4096` permite respuestas más largas |

**Ejemplo de Configuración:**
```
Tipo de Cliente de IA: Tipo de cliente local (ej. OllamaClient)
endpoint: http://localhost:11434 (completar según dirección de servicio real)
model: qwen3.5:8b (completar según modelo descargado)
temperature: 0.7
maxTokens: 2048
```

#### Configuración de Cliente en la Nube (ej. DashScope)

| Elemento de Configuración | Descripción | Valor Predeterminado | Ejemplo |
|--------|------|--------|------|
| **apiKey** | Clave API | Ninguna | Proporcionada por plataforma específica |
| **region** | Región de servidor | Depende de plataforma específica | ej. `beijing` |
| **model** | Modelo a usar | Depende de plataforma específica | ej. `qwen3.6-plus` |
| **temperature** | Nivel de creatividad (0-1) | `0.7` | `0.5` |
| **maxTokens** | Longitud máxima de respuesta | `2048` | `4096` |

**Regiones Disponibles:**

| Código de Región | Ubicación | Usuarios Adecuados |
|---------|------|---------|
| `beijing` | Pekín, China | Usuarios de China continental (recomendado) |
| `singapore` | Singapur | Usuarios del sudeste asiático |
| `hongkong` | Hong Kong, China | Usuarios de Hong Kong, Macao, Taiwán |
| `virginia` | EE.UU. | Usuarios de América del Norte |
| `frankfurt` | Alemania | Usuarios europeos |

**Modelos Disponibles:**

| Nombre del Modelo | Características | Escenarios Adecuados |
|---------|------|---------|
| `qwen3.6-plus` | Rendimiento equilibrado (recomendado) | Uso diario |
| `qwen3-max` | Capacidad más fuerte | Tareas complejas |
| `qwen3.6-flash` | Respuesta rápida | Preguntas y respuestas simples |
| `qwen-max` | Insignia de generación anterior | Razonamiento complejo |
| `qwen-plus` | Mejorado de generación anterior | Escenarios generales |
| `qwen-turbo` | Rápido de generación anterior | Tareas simples |
| `qwen3-coder-plus` | Específico de programación | Generación de código |
| `qwq-plus` | Específico de razonamiento | Matemáticas, lógica |
| `deepseek-v3.2` | Modelo de terceros | Escenarios generales |
| `deepseek-r1` | Modelo de razonamiento | Pensamiento profundo |
| `glm-5.1` | Modelo Zhipu | Escenarios en chino |
| `kimi-k2.5` | Contexto largo | Procesamiento de texto largo |
| `llama-4-maverick` | Modelo Meta | Escenarios en inglés |

**Ejemplo de Configuración:**
```
Tipo de Cliente de IA: Tipo de cliente en la nube (ej. DashScopeClient)
apiKey: Su clave API (obtenida de plataforma correspondiente)
region: beijing (elegir la región más cercana)
model: qwen3.6-plus (elegir según modelos disponibles)
temperature: 0.7
maxTokens: 2048
```

### Paso 4: Guardar Configuración

1. Después de completar toda la información necesaria
2. Haga clic en el botón **""Guardar Configuración""** en la parte inferior de la página
3. El sistema notificará que el guardado fue exitoso

### Paso 5: Probar Conexión

1. Haga clic en el menú **💬 Chat** en la barra de navegación superior
2. Seleccione un ser de silicio
3. Envíe un mensaje de prueba, como ""Hola""
4. Si recibe una respuesta, la configuración fue exitosa

## Preguntas Frecuentes

### P1: ¿No sé qué cliente elegir?

**Sugerencias:**
- Si es principiante, se recomienda usar primero **cliente en la nube**, configuración simple, inicio rápido
- Si valora la privacidad o tiene mejor configuración de computadora, elija **cliente local**

### P2: ¿Puedo usar dos clientes al mismo tiempo?

No. El sistema solo puede usar un cliente de IA a la vez. Pero puede cambiar en cualquier momento en la página de configuración.

### P3: ¿Se perderán los registros de chat después de cambiar clientes?

No. Los registros de chat se guardan en el sistema y son independientes del cliente de IA. Aún puede ver conversaciones históricas después de cambiar.

### P4: ¿El cliente local muestra error de conexión?

**Soluciones:**
1. Confirme que el software de servicio de IA correspondiente está ejecutándose (verificar bandeja del sistema o procesos)
2. Verifique si la dirección endpoint es correcta (referir configuración predeterminada de ese servicio)
3. Acceda a la dirección de servicio en un navegador, debería ver una respuesta
4. Confirme que los modelos están descargados: use el comando del servicio específico para ver la lista de modelos

### P5: ¿El cliente en la nube muestra error de autenticación?

**Soluciones:**
1. Verifique si la clave API es correcta (el formato se refiere a los requisitos de la plataforma correspondiente)
2. Confirme que la cuenta no está en mora
3. Verifique si se excedió la cuota de uso
4. Regenerar la clave API y actualizar la configuración

### P6: ¿Cómo obtener la clave API para cliente en la nube?

**Pasos Generales:**
1. Visite el sitio web de consola del proveedor de servicio de IA correspondiente
2. Inicie sesión en su cuenta
3. Entre a la página ""Gestión de Claves"" o ""Gestión de API""
4. Haga clic en ""Crear Clave"" o ""Generar Clave API""
5. Copie la clave API generada y guárdela adecuadamente

### P7: ¿Qué hacer si la respuesta es muy lenta?

**Cliente Local:**
- Verifique el uso de recursos de la computadora (CPU, RAM, tarjeta gráfica)
- Intente usar un modelo más pequeño (ej. 8B en lugar de 32B)
- Cierre otros programas que consumen recursos

**Cliente en la Nube:**
- Verifique la calidad de conexión de red
- Intente elegir una región de servidor más cercana
- Evite horas pico de red

### P8: ¿Qué es el parámetro temperature?

temperature controla la creatividad de las respuestas de IA:
- **0.0-0.3**: Muy conservador, respuestas predecibles, adecuado para preguntas factuales
- **0.4-0.7**: Modo equilibrado, adecuado para conversación diaria (recomendado)
- **0.8-1.0**: Muy creativo, respuestas diversas, adecuado para escritura creativa

### P9: ¿Cuánto debe configurarse maxTokens?

- **1024**: Respuestas cortas, adecuado para preguntas y respuestas simples
- **2048**: Longitud media, adecuado para conversación general (recomendado)
- **4096+**: Respuestas largas, adecuado para tareas complejas o generación de texto largo

Nota: Cuanto mayor sea la configuración, más recursos y tiempo consumirá.

### P10: ¿Pueden diferentes seres usar diferentes clientes?

Sí. Cada ser de silicio puede configurar independientemente el tipo de cliente de IA.

**Método de Configuración:**
1. Entre a la página de gestión de seres
2. Seleccione el ser a configurar
3. Configure AIClientType en la configuración de ese ser
4. Si el ser no está configurado, se usará la configuración global

## Mejores Prácticas

### 1. Elegir Modelos Basados en Tareas

- **Conversación diaria**: Use modelos pequeños a medianos (8B-14B o modelos rápidos)
- **Análisis complejo**: Use modelos grandes (32B+ o modelos avanzados)
- **Generación de código**: Use modelos de programación dedicados
- **Escritura creativa**: Use temperature más alto (0.8-0.9)

### 2. Optimizar Costos (Cliente en la Nube)

- Verifique regularmente el uso para evitar exceder la cuota gratuita
- Use modelos rápidos para tareas simples
- Use modelos avanzados para tareas complejas
- Configure maxTokens razonablemente para evitar desperdicio

### 3. Mejorar Rendimiento (Cliente Local)

- Priorice descargar modelos de uso frecuente para evitar descargar en tiempo de ejecución
- Mantenga el servicio de IA residente para reducir tiempo de inicio
- Use aceleración de tarjeta gráfica (si tiene tarjeta gráfica NVIDIA)
- Limpie regularmente modelos no usados para liberar espacio

### 4. Recomendaciones de Seguridad

- No comparta clave API con otros
- Cambie regularmente la clave API
- Los clientes locales están limitados solo a acceso local, no exponer a red pública
- Respalde archivos de configuración importantes

## Obtener Ayuda

Si encuentra problemas:
1. Verifique la sección [Preguntas Frecuentes](#preguntas-frecuentes)
2. Verifique [Gestión de Configuración](./config) para instrucciones de configuración del sistema
3. Verifique registros del sistema para detalles de error
";
    
    public override string BeingSoul => @"
# Archivo de Alma

## Descripción General

El Archivo de Alma es el archivo de configuración central de un Ser de Silicio, determinando la **personalidad, patrones de comportamiento, capacidades profesionales y métodos de trabajo** de cada ser.

Puede entender el archivo de alma como la ""configuración de personalidad"" o ""manual de trabajo"" del ser. Se carga automáticamente durante cada conversación de IA, guiando al ser de silicio para pensar y actuar de la manera que usted espera.

## Función del Archivo de Alma

El archivo de alma es la **fuerza impulsora central** de un ser de silicio, definiendo:

- 🎭 **Posicionamiento de Rol**: Quién es este ser de silicio, en qué campos destaca
- 📋 **Directrices de Comportamiento**: Cómo debería responder a los usuarios, qué principios seguir
- 🔄 **Flujo de Trabajo**: Cómo manejar tareas después de recibirlas, en cuántos pasos
- ⚠️ **Límites de Comportamiento**: Qué se puede hacer, qué no se debería hacer
- 💡 **Requisitos Profesionales**: Estándares de código, formato de salida, estilo de lenguaje, etc.

## Cómo Editar el Archivo de Alma

### Editar a través de la Interfaz Web

1. Vaya a la página **Seres de Silicio**
2. Haga clic en la tarjeta del ser de silicio que desea editar
3. Haga clic en el enlace **Archivo de Alma**
4. Modifique el contenido en el editor Markdown
5. Haga clic en el botón **Guardar**

### Editar a través del Asistente de IA

También puede conversar directamente con el **Curador de Silicio** y pedirle que le ayude a modificar el archivo de alma:

```
Por favor, ayúdeme a modificar el archivo de alma del asistente de programación para agregar soporte para desarrollo de Python
```

El Curador de Silicio le ayudará a actualizar el contenido del archivo de alma.

## Guía de Escritura de Archivos de Alma

### Estructura Básica

Los archivos de alma se escriben en **formato Markdown**. Se recomienda la siguiente estructura:

```markdown
# Configuración de Rol

Eres un [descripción de rol], especializado en:
- Habilidad 1
- Habilidad 2
- Habilidad 3

# Directrices de Comportamiento

1. Directriz 1
2. Directriz 2
3. Directriz 3

# Flujo de Trabajo

Al recibir una tarea:
1. Entender los requisitos
2. Analizar el enfoque
3. Ejecutar operaciones
4. Reportar resultados

# Estándares de Código

- Seguir ciertos estándares de codificación
- Proporcionar comentarios necesarios
- Considerar casos extremos
```

### Consejos de Escritura

1. **Definición Clara de Rol**: Especifique claramente las responsabilidades y áreas de especialización del ser de silicio
2. **Establecer Límites de Comportamiento**: Explique qué se puede hacer y qué no se debería hacer
3. **Proporcionar Flujo de Trabajo**: Guíe al ser de silicio sobre cómo manejar tareas
4. **Usar Formato Markdown**: Soporta encabezados, listas, bloques de código, etc.
5. **Ser Específico, No Vago**: Use ejemplos concretos en lugar de descripciones abstractas

## Ejemplos Prácticos

### Ejemplo 1: Asistente de Programación

```markdown
# Configuración de Rol

Eres un asistente de desarrollo full-stack profesional, especializado en:
- Desarrollo C# / .NET
- Diseño de arquitectura y revisión de código
- Diseño y optimización de bases de datos
- Desarrollo web frontend

# Directrices de Comportamiento

1. Siempre proporcionar ejemplos de código ejecutables
2. Explicar la lógica de código clave y el pensamiento de diseño
3. Proporcionar recomendaciones de mejores prácticas
4. Cuando no esté seguro, informar claramente al usuario

# Estándares de Código

- Seguir principios SOLID
- Usar nombres claros
- Agregar comentarios necesarios
- Considerar manejo de excepciones y casos extremos
```

### Ejemplo 2: Asistente de Servicio al Cliente

```markdown
# Configuración de Rol

Eres un asistente de servicio al cliente amigable, responsable de:
- Responder preguntas frecuentes de usuarios
- Manejar quejas simples
- Guiar a los usuarios a través de operaciones
- Recopilar comentarios de usuarios

# Directrices de Comportamiento

1. Mantenerse siempre cortés y paciente
2. Explicar en lenguaje simple y fácil de entender
3. Transferir a agente humano promptly cuando no pueda resolver
4. Registrar preguntas y comentarios de usuarios

# Flujo de Trabajo

1. Saludar al usuario
2. Entender las necesidades del usuario
3. Proporcionar soluciones
4. Confirmar si el problema está resuelto
5. Agradecer al usuario y finalizar la conversación
```

### Ejemplo 3: Asistente de Análisis de Datos

```markdown
# Configuración de Rol

Eres un experto en análisis de datos, especializado en:
- Limpieza y preprocesamiento de datos
- Análisis estadístico y visualización
- Predicción de tendencias y detección de anomalías
- Generar informes de datos

# Requisitos de Salida

1. Proporcionar conclusiones de análisis claras
2. Usar gráficos para explicación auxiliar
3. Anotar fuentes de datos y suposiciones
4. Dar recomendaciones accionables
```

## Ubicación de Almacenamiento del Archivo de Alma

El archivo de alma de cada ser de silicio se almacena en su directorio de datos:

```
DataDirectory/SiliconManager/{BeingGUID}/soul.md
```

El sistema gestiona automáticamente este archivo, y no necesita operar manualmente el sistema de archivos.

## Preguntas Frecuentes

### P: ¿Los cambios en el archivo de alma tendrán efecto inmediatamente?

**R:** Sí, tendrán efecto inmediatamente después de guardar. La próxima vez que el ser de silicio responda, usará el nuevo archivo de alma.

### P: ¿Hay un límite de tamaño para los archivos de alma?

**R:** No hay un límite de tamaño estricto, pero se recomienda mantenerlo dentro de un rango razonable (dentro de unos pocos miles de palabras). Los archivos de alma demasiado largos pueden afectar la velocidad de respuesta.

### P: ¿Puedo eliminar completamente el archivo de alma?

**R:** No se recomienda eliminar el archivo de alma. Si el contenido está vacío, el ser de silicio perderá la guía de comportamiento y puede producir respuestas inesperadas.

### P: ¿Cómo hago una copia de seguridad del archivo de alma?

**R:** Se recomienda hacer copias de seguridad regulares de los archivos de alma de seres de silicio importantes. Puede:
1. Copiar el contenido a un archivo local a través de la interfaz web
2. Usar la función de exportación del sistema (si es compatible)
3. Hacer una copia de seguridad directamente del directorio de datos

### P: ¿Cuál es la relación entre el archivo de alma y el sistema de memoria?

**R:** El archivo de alma define **patrones de comportamiento a largo plazo**, mientras que el sistema de memoria registra **historial de conversaciones a corto plazo**. Trabajan juntos:
- Archivo de alma: Le dice al ser de silicio ""qué tipo de rol eres""
- Sistema de memoria: Le dice al ser de silicio ""de qué hablamos antes""

### P: ¿Pueden diferentes seres de silicio usar el mismo archivo de alma?

**R:** Sí, pero no se recomienda. Cada ser de silicio debería tener un posicionamiento de rol único para evitar duplicación funcional.

## Mejores Prácticas

1. **Optimización Continua**: Optimice continuamente el archivo de alma basado en comentarios de uso real
2. **Gestión de Versiones**: Haga una copia de seguridad de la versión actual antes de modificaciones importantes
3. **Verificación de Pruebas**: Pruebe el efecto a través de la conversación después de la modificación
4. **Mantenerlo Conciso**: Exprese requisitos centrales en lenguaje conciso
5. **Evitar Contradicciones**: Asegúrese de que no haya conflictos entre varias directrices
6. **Revisión Regular**: Verifique regularmente si el archivo de alma sigue siendo aplicable

## Solución de Problemas

### Problema: El comportamiento del ser de silicio no cumple con las expectativas

**Lista de Verificación:**
1. ¿Es el contenido del archivo de alma claro y preciso?
2. ¿Ha agregado suficientes directrices de comportamiento?
3. ¿Hay instrucciones contradictorias?
4. ¿Ha proporcionado un flujo de trabajo específico?

**Soluciones:**
1. Reescribir descripciones vagas
2. Agregar más directrices de comportamiento específicas
3. Proporcionar ejemplos para ilustrar la salida esperada
4. Probar y optimizar continuamente

### Problema: Error al guardar el archivo de alma

**Causas Posibles:**
1. Problemas de permisos del sistema de archivos
2. Espacio en disco insuficiente
3. El archivo está ocupado por otro proceso

**Soluciones:**
1. Verificar registros del sistema para información detallada de errores
2. Confirmar que el directorio de datos es escribible
3. Reintentar después de reiniciar el sistema

## Funciones Relacionadas

- 🤖 [Gestión de Seres](being-management) - Crear y gestionar seres de silicio
- 💬 [Sistema de Chat](chat-system) - Conversar con seres de silicio
- 🧠 [Configuración de Cliente de IA](ai-clients) - Configurar servicios de IA
- 📝 [Sistema de Memoria](memory) - Gestionar historial de conversaciones
";

    public override string AuditLog => @"
# Registro de Auditoría

## Descripción General

El Registro de Auditoría (Audit Log) es el **sistema de monitoreo de uso de Tokenes** de la plataforma Silicon Life, que le ayuda a rastrear y gestionar el consumo de llamadas a IA de todos los seres de silicio.

Con el registro de auditoría, puede:
- 📊 **Ver estadísticas de consumo de Tokenes**: Saber cuántos Tokenes ha consumido cada ser de silicio
- 📈 **Analizar tendencias de uso**: Ver cambios en el uso de Tokenes por hora, día, mes
- 🔍 **Filtrar y comparar**: Filtrar datos por rango de tiempo, ser de silicio, tipo de cliente de IA
- 💾 **Exportar datos**: Exportar datos de auditoría a archivo CSV para análisis adicional

**¿Qué es un Token?**  
El Token es la unidad básica con la que los modelos de IA procesan texto. Cada llamada a IA de un ser de silicio consume Tokenes:
- **Prompt Tokens**: Texto enviado a la IA (incluyendo archivos de alma, historial de mensajes, mensajes de usuario, etc.)
- **Completion Tokens**: Texto generado por la respuesta de la IA
- **Total Tokens**: Total de Tokenes consumidos (Prompt + Completion)

## Acceso al Registro de Auditoría

### Entrar a la Página de Auditoría

1. Iniciar la plataforma Silicon Life
2. Hacer clic en ""Auditoría"" (icono 📊) en el menú de navegación izquierdo
3. Entrar al panel de control del registro de auditoría

### Requisitos de Permiso

- 📊 **Ver registro de auditoría**: Permiso de Curator (curador)
- 🔒 Los seres de silicio comunes no pueden acceder a la función de auditoría
- Esto asegura que solo los administradores puedan ver el uso global de Tokenes

## Funciones del Panel de Control de Auditoría

### 📈 Gráfico de Tendencias

La parte superior de la página de auditoría muestra el **gráfico de tendencia de uso de Tokenes**, que incluye:

- **Selector de rango de tiempo**:
  - Hoy (Today): Mostrar uso de Tokenes de hoy por hora
  - Esta semana (Week): Mostrar tendencia de uso de Tokenes de esta semana
  - Este mes (Month): Mostrar uso de Tokenes de este mes por día
  - Este año (Year): Mostrar uso de Tokenes de este año por mes

- **Datos de tendencia**:
  - El gráfico de líneas muestra el cambio en la cantidad de uso de Tokenes a lo largo del tiempo
  - Pasar el mouse sobre un punto de datos para ver el valor específico
  - Le ayuda a descubrir picos y valles de uso

### 📊 Resumen de Estadísticas

El panel de control muestra datos estadísticos clave:

- **Número total de solicitudes**: Número total de llamadas a IA
- **Número de éxitos/fallos**: Cuántas llamadas a IA fueron exitosas y cuántas fallaron
- **Consumo de Tokenes**:
  - Número total de Tokenes de entrada (Prompt Tokens)
  - Número total de Tokenes de salida (Completion Tokens)
  - Consumo total de Tokenes (Total Tokens)

### 🔍 Función de Filtros

Puede usar filtros para ver datos específicos con precisión:

- **Filtrar por ser de silicio**: Ver uso de Tokenes de un ser de silicio específico
- **Filtrar por cliente de IA**: Ver uso de un servicio de IA específico (como Ollama, Alibaba Cloud Bailian)
- **Filtros combinados**: Usar múltiples condiciones de filtro simultáneamente

### 📋 Categorización Detallada

#### Estadísticas por Tipo de Cliente de IA

Mostrar uso de Tokenes de cada cliente de IA:
- OllamaClient (servicio local Ollama)
- DashScopeClient (Alibaba Cloud Bailian)
- Otros clientes de IA configurados

Le ayuda a entender:
- Qué servicio de IA se usa más
- Comparación de uso entre modelos locales y modelos en la nube
- Si es necesario ajustar la configuración del cliente de IA

#### Estadísticas por Ser de Silicio

Mostrar uso de Tokenes de cada ser de silicio:
- Nombre del ser de silicio
- Consumo total de Tokenes
- Número de solicitudes

Le ayuda a descubrir:
- Qué seres de silicio son más activos
- Si el consumo de Tokenes es razonable
- Si es necesario ajustar la frecuencia de uso de algunos seres

### 💾 Exportación de Datos

Haciendo clic en el botón ""Exportar CSV"", puede:

- Exportar todos los datos de auditoría bajo las condiciones de filtro actuales a un archivo CSV
- El archivo CSV incluye:
  - Marca de tiempo
  - ID del ser de silicio
  - Tipo de cliente de IA
  - Número de Tokenes de entrada
  - Número de Tokenes de salida
  - Número total de Tokenes
  - Si fue exitoso

**Escenarios de uso**:
- Análisis de datos más profundo en Excel
- Creación de informes personalizados
- Conservación a largo plazo de registros de auditoría
- Compartir datos con otros miembros del equipo

## Escenarios de Uso

### Escenario 1: Monitoreo de Consumo de Tokenes

**Requisito**: Quiere saber el consumo total del servicio de IA este mes

**Pasos de operación**:
1. Entrar a la página de auditoría
2. Seleccionar ""Este mes"" en el rango de tiempo
3. Ver la estadística ""Consumo total de Tokenes""
4. Ver el gráfico de tendencias para entender los cambios diarios de consumo

**Resultado**: Puede ver claramente cuántos Tokenes se consumieron en total este mes y qué días tuvieron el uso más alto.

### Escenario 2: Solución de Consumo Anómalo

**Requisito**: Descubrió que los costos del servicio de IA son anormalmente altos y quiere encontrar la causa

**Pasos de operación**:
1. Entrar a la página de auditoría
2. Seleccionar el período de tiempo de costos anómalos
3. Ver estadísticas por ser de silicio
4. Encontrar el ser de silicio con mayor consumo
5. Hacer clic en este ser para ver la tendencia detallada
6. Si es necesario, exportar CSV para análisis más profundo

**Resultado**: Puede identificar qué ser de silicio consumió una gran cantidad de Tokenes en qué momento, y verificar más a fondo si su archivo de alma o configuración de tareas es razonable.

### Escenario 3: Comparación de Diferentes Servicios de IA

**Requisito**: Quiere comparar el uso de Ollama local y Alibaba Cloud Bailian

**Pasos de operación**:
1. Entrar a la página de auditoría
2. Seleccionar ""Este mes"" en el rango de tiempo
3. Ver estadísticas por tipo de cliente de IA
4. Comparar uso de Tokenes entre OllamaClient y DashScopeClient

**Resultado**: Puede entender:
- La proporción de uso entre modelos locales y modelos en la nube
- Qué servicio es más económico
- Si es necesario ajustar el cliente de IA predeterminado

### Escenario 4: Monitoreo de un Ser de Silicio Específico

**Requisito**: Creó un nuevo asistente de programación y quiere monitorear si su uso de Tokenes es razonable

**Pasos de operación**:
1. Entrar a la página de auditoría
2. Seleccionar el ID o nombre de este asistente de programación en el filtro
3. Ver su tendencia de uso de Tokenes
4. Comparar con el uso de otros seres de silicio

**Resultado**: Puede evaluar la eficiencia de este asistente de programación y determinar si es necesario optimizar su archivo de alma o forma de trabajo.

## Herramienta de Auditoría (Llamada de IA)

Los seres de silicio también pueden usar la herramienta **TokenAuditTool** para consultar el uso de Tokenes:

### Consulta de Resumen de Tokenes

**Ejemplo de comando**:
```
Consultar uso de Tokenes este mes
```

**Información devuelta**:
- Número total de solicitudes
- Número de éxitos/fallos
- Número de Tokenes de entrada/salida/totales
- Estadísticas por cliente de IA
- Estadísticas por ser de silicio

### Consulta de Tendencia de Uso

**Ejemplo de comando**:
```
Mostrar tendencia de uso de Tokenes esta semana
```

**Información devuelta**:
- Cantidad de uso de Tokenes por día o por hora
- Le ayuda a entender los patrones de uso

### Parámetros de la Herramienta

TokenAuditTool admite los siguientes parámetros:

- `time_range`: Rango de tiempo (today/week/month/year)
- `client_type`: Tipo de cliente de IA (opcional)
- `being_id`: ID del ser de silicio (opcional)

**Nota**: Esta es una herramienta `[SiliconManagerOnly]`, solo los curadores pueden usarla.

## Explicación de Datos

### Contenido de los Registros

Cada llamada a IA registra la siguiente información:

- **Marca de tiempo**: Hora de la llamada a IA (UTC)
- **ID del ser de silicio**: El ser que inició la llamada
- **Tipo de cliente de IA**: Servicio de IA utilizado (por ejemplo, OllamaClient)
- **Número de Tokenes de entrada**: Número de Tokenes del texto enviado a la IA
- **Número de Tokenes de salida**: Número de Tokenes del texto de respuesta de la IA
- **Número total de Tokenes**: Entrada + salida
- **ID de sesión**: La sesión de chat a la que pertenece
- **Si fue exitoso**: Si la llamada a IA fue exitosa

### Almacenamiento de Datos

- Los registros de auditoría usan **almacenamiento indexado por tiempo** (ITimeStorage)
- Admite consultas eficientes por rango de tiempo
- Los datos se almacenan persistentemente y no se pierden después del reinicio

### Explicación del Cálculo de Tokenes

**¿Qué es un Token?**
- El Token es la unidad básica con la que los modelos de IA procesan texto
- Un Token equivale aproximadamente a 0.75 palabras en inglés o 1-2 caracteres chinos
- El cálculo de Tokenes varía ligeramente entre diferentes modelos de IA

**¿Por qué es necesario monitorear Tokenes?**
- Los servicios de IA en la nube (como Alibaba Cloud Bailian) cobran por Token
- Entender el uso de Tokenes ayuda a controlar costos
- Descubrir patrones de uso anómalos (como llamadas en bucle infinito)
- Optimizar la configuración y archivos de alma de los seres de silicio

## Preguntas Frecuentes

### P: ¿Por qué no puedo ver la página de auditoría?

**R**: La función de auditoría requiere permiso de Curator (curador). Confirme:
- Es administrador o curador de la plataforma
- Los seres de silicio no tienen permiso para ver el registro de auditoría
- Si tiene problemas de permiso, contacte al administrador de la plataforma

### P: ¿Los datos de auditoría muestran ""Sin datos""?

**R**: Posibles razones:
- No hay llamadas a IA en el rango de tiempo seleccionado
- Las condiciones de filtro son demasiado estrictas, no hay registros coincidentes
- El ser de silicio aún no ha tenido diálogo con IA

**Solución**:
- Intente ajustar el rango de tiempo (por ejemplo, seleccionar ""Este mes"" en lugar de ""Hoy"")
- Limpiar las condiciones de filtro
- Primero tener algunos diálogos con el ser de silicio

### P: ¿Por qué el uso de Tokenes aumentó repentinamente?

**R**: Posibles razones:
- Se agregaron una gran cantidad de registros de diálogo (el historial se alarga, aumentan los Prompt Token)
- El archivo de alma se volvió muy complejo
- Algún ser de silicio llama frecuentemente a la IA
- Las tareas o temporizadores de IA se ejecutan en gran cantidad

**Método de solución**:
- Usar filtro para ver qué ser de silicio específico
- Ver gráfico de tendencias para ubicar el punto de tiempo específico
- Verificar el archivo de alma y configuración de tareas de este ser de silicio
- Exportar CSV para análisis detallado

### P: ¿Con qué software se puede abrir el archivo CSV exportado?

**R**: El archivo CSV se puede abrir con el siguiente software:
- Microsoft Excel
- Google Sheets
- LibreOffice Calc
- Cualquier editor de texto (como Bloc de notas, VS Code)

### P: ¿Cuánto tiempo se conservarán los datos de auditoría?

**R**: 
- Los registros de auditoría usan almacenamiento persistente en el sistema de archivos
- Por defecto, los datos se conservarán indefinidamente
- Si necesita limpiar datos antiguos, puede eliminar manualmente los archivos de registros de auditoría en el directorio de almacenamiento
- Se recomienda exportar regularmente datos importantes como respaldo

### P: ¿Por qué algunas llamadas a IA no están registradas?

**R**: Posibles razones:
- El cliente de IA no devolvió información de uso de Tokenes
- El servicio de registro de Tokenes no se inicializó correctamente
- La llamada falló y no se generaron estadísticas de Tokenes

**Nota**: Solo las respuestas de IA que contienen información de Tokenes serán registradas.

### P: ¿Puedo eliminar registros de auditoría?

**R**: 
- La versión actual no admite la eliminación de registros de auditoría a través de la interfaz
- Los registros de auditoría se almacenan en el directorio de índice de tiempo del sistema de archivos
- Si necesita limpiar, puede eliminar manualmente los archivos de auditoría en el directorio de almacenamiento
- **Se recomienda exportar un respaldo primero antes de eliminar**

## Mejores Prácticas

### 1. Monitorear Regularmente el Uso de Tokenes

- 📅 Ver el panel de control de auditoría una vez por semana o mes
- 📊 Seguir la tendencia de uso de Tokenes, resolver anomalías a tiempo
- 💰 Si usa servicios de IA en la nube, seguir la relación entre costos y uso de Tokenes

### 2. Usar la Función de Filtros para Ubicación Precisa

- 🔍 Cuando descubra un problema, primero use el filtro para reducir el alcance
- 🎯 Filtrar por ser de silicio, ubicar el ser específico
- 🤖 Filtrar por cliente de IA, comparar el uso de diferentes servicios

### 3. Exportar Datos Regularmente

- 💾 Exportar datos de auditoría una vez al mes como respaldo
- 📈 Crear gráficos e informes personalizados en Excel
- 📋 Conservar datos a largo plazo para análisis comparativo anual

### 4. Optimizar el Uso de Tokenes

Optimizar según los datos de auditoría:
- 📝 Simplificar archivos de alma (reducir configuraciones innecesarias)
- 🗂️ Controlar la cantidad de memoria (evitar historial demasiado largo)
- ⏱️ Ajustar la frecuencia de temporizadores (reducir llamadas a IA innecesarias)
- 🎯 Optimizar la descripción de tareas (aumentar eficiencia, reducir llamadas repetidas)

### 5. Configurar Alertas de Uso

- 📊 Seguir fluctuaciones anómalas en el uso de Tokenes
- 🔔 Si descubre que algún ser de silicio repentinamente consume una gran cantidad de Tokenes, verifique a tiempo
- 🛑 Si es necesario, pause este ser y resuelva el problema

## Funciones Relacionadas

- [Gestión de Seres de Silicio](/help/being-management) - Gestionar la creación y configuración de seres de silicio
- [Sistema de Tareas](/help/task) - Configurar y gestionar tareas programadas de seres de silicio
- [Sistema de Memoria](/help/memory) - Entender cómo la memoria afecta el uso de Tokenes
- [Gestión de Permisos](/help/permission) - Entender los permisos del curador y control de acceso
- [Gestión de Configuración](/help/config) - Configurar clientes de IA y otros ajustes del sistema
";
    
    public override string KnowledgeGraph => @"
# Grafo de Conocimiento

## Descripción General

El Grafo de Conocimiento es el **sistema de gestión y visualización de conocimiento** para seres de silicio, mostrando el conocimiento aprendido y acumulado por seres de silicio de manera gráfica. A través de nodos y conexiones intuitivos, puede ver claramente las relaciones entre el conocimiento.

## ¿Qué es un Triple de Conocimiento?

La unidad básica del grafo de conocimiento es el **triple de conocimiento**, usando una estructura ""sujeto-predicado-objeto"":

```
(sujeto) -[relación]-> (objeto)
```

**Ejemplos:**
- `(Xiao Ming) -[gusta]-> (manzanas)`
- `(Python) -[es]-> (lenguaje de programación)`
- `(Tierra) -[orbita]-> (Sol)`

Esta estructura permite a los seres de silicio entender y organizar el conocimiento como los humanos.

## Acceder al Grafo de Conocimiento

### Punto de Entrada

1. Haga clic en el icono **📚 Base de Conocimiento** en la barra de navegación izquierda
2. El sistema cargará y mostrará automáticamente la página de visualización del grafo de conocimiento

### Diseño de Interfaz

La página del grafo de conocimiento incluye:

- **Título Superior**: Muestra ""Visualización del Grafo de Conocimiento""
- **Panel de Estadísticas**: Muestra conteo de nodos, conteo de aristas y otras estadísticas
- **Área Principal de Canvas**: Muestra la visualización del grafo de conocimiento
- **Indicador de Carga**: Muestra mensaje de carga mientras se cargan los datos

## Visualización del Grafo

### Nodos

Los nodos representan **entidades** (sujetos u objetos) en el conocimiento:

- **Nodos Circulares**: Cada nodo representa una entidad
- **Etiquetas de Nodo**: Muestran el nombre de la entidad
- **Tamaño de Nodo**: Ajustado automáticamente según el conteo de conexiones (más conexiones = nodo más grande)
- **Colores de Nodo**:
  - Borde azul: Nodos regulares
  - Resaltado: Al pasar el mouse

### Aristas

Las aristas representan **relaciones** (predicados) en el conocimiento:

- **Líneas con Flechas**: Indican relaciones de sujeto a objeto
- **Color de Línea**: Azul semitransparente `rgba(100, 150, 255, 0.6)`
- **Marcadores de Flecha**: Muestran claramente la dirección de la relación

### Algoritmo de Diseño

El grafo de conocimiento usa un **algoritmo de diseño circular**:

- Todos los nodos están distribuidos uniformemente en órbitas circulares
- Espaciado razonable entre nodos
- Se adapta automáticamente al tamaño del canvas

## Funciones Interactivas

### Operaciones del Mouse

- **Pasar el mouse sobre nodo**: El nodo se resalta para fácil identificación
- **Ver relaciones**: Entender dirección de relaciones a través de flechas
- **Zoom del canvas**: Función de zoom nativa del navegador (Ctrl + rueda del mouse)

### Carga de Datos

- **Carga Automática**: Carga automáticamente datos de conocimiento del sistema al entrar a la página
- **Actualización en Tiempo Real**: Volver a visitar la página para obtener conocimiento más reciente
- **Indicador de Carga**: Muestra ""Cargando datos del grafo de conocimiento..."" mientras carga

## Gestión de Conocimiento

### ¿Cómo se Genera el Conocimiento?

Los seres de silicio acumulan conocimiento a través de:

1. **Aprendizaje por Conversación**: Extrayendo información clave de conversaciones con usuarios
2. **Invocación de Herramientas**: Agregando conocimiento a través de KnowledgeTool
3. **Análisis de Archivos**: Analizando contenido de archivos subidos para extraer conocimiento
4. **Ejecución de Tareas**: Acumulando conocimiento de dominio durante la ejecución de tareas

### Operaciones de KnowledgeTool

Los curadores de silicio o seres de silicio pueden pedir a la IA que realice operaciones de conocimiento a través de conversación:

**Agregar Conocimiento:**
```
Por favor agrega conocimiento: (Python) -[es]-> (lenguaje de programación)
```

**Consultar Conocimiento:**
```
Encontrar todo el conocimiento sobre ""Python""
```

**Buscar Conocimiento:**
```
Buscar conocimiento que contenga ""programación""
```

**Encontrar Ruta de Relación:**
```
Encontrar la ruta de relación entre ""Python"" y ""IA""
```

### Atributos de Conocimiento

Cada entrada de conocimiento contiene metadatos ricos:

- **ID Único**: Identificador único para cada conocimiento
- **Número de Versión**: Soporta actualizaciones de conocimiento y gestión de versiones
- **Confianza**: 0.0-1.0, indicando confiabilidad del conocimiento
- **Fuente**: Fuente del conocimiento (conversación, archivo, herramienta, etc.)
- **Categoría**: Dominio o categoría al que pertenece el conocimiento
- **Etiquetas**: Etiquetas personalizadas para fácil clasificación y búsqueda
- **Creador**: El ser de silicio que agregó este conocimiento
- **Hora de Creación**: Timestamp cuando se agregó el conocimiento
- **Estado de Validación**: No validado/Validado/Rechazado

## Estadísticas

La página del grafo de conocimiento muestra las siguientes estadísticas:

- **Total de Nodos**: Número de todas las entidades en el grafo de conocimiento
- **Total de Aristas**: Número de relaciones entre entidades
- **Entradas de Conocimiento**: Número total de triples de conocimiento en el sistema

Estas estadísticas le ayudan a entender el nivel de acumulación de conocimiento de los seres de silicio.

## Casos de Uso

### 1. Visualización de Conocimiento

Ver intuitivamente qué han aprendido los seres de silicio:

1. Entrar a la página del grafo de conocimiento
2. Observar distribución de nodos y conexiones
3. Entender estructura de organización de conocimiento y asociaciones

### 2. Validación de Conocimiento

Verificar resultados de aprendizaje de seres de silicio:

1. Ver nodos de conocimiento en dominios específicos
2. Validar precisión del conocimiento
3. Suplementar o corregir conocimiento a través de conversación

### 3. Exploración de Conocimiento

Descubrir asociaciones entre conocimiento:

1. Observar nodos altamente conectados (conceptos centrales)
2. Rastrear rutas de relación para descubrir asociaciones indirectas
3. Identificar brechas de conocimiento para guiar aprendizaje adicional

## Preguntas Frecuentes

### P: ¿Por qué el grafo de conocimiento está vacío?

**R:** Si los seres de silicio aún no han aprendido ningún conocimiento, el grafo se mostrará vacío. Puede agregar conocimiento mediante:
- Conversar con seres de silicio para que aprendan nueva información
- Subir archivos para que los seres de silicio analicen
- Pedir directamente a los seres de silicio que agreguen conocimiento específico

### P: ¿El grafo de conocimiento se actualiza automáticamente?

**R:** Los datos del grafo de conocimiento se almacenan en tiempo real, pero la página necesita actualizarse para mostrar los datos más recientes. Después de que los seres de silicio aprendan nuevo conocimiento, vuelva a visitar la página del grafo de conocimiento para ver actualizaciones.

### P: ¿Cómo ver cuando hay demasiados nodos?

**R:** Cuando el volumen de conocimiento es grande:
- Usar zoom del navegador (Ctrl + rueda del mouse)
- Enfocarse en nodos centrales altamente conectados
- Buscar conocimiento sobre temas específicos a través de herramientas de consulta

### P: ¿Cómo eliminar conocimiento incorrecto?

**R:** Puede pedir a los seres de silicio que eliminen o corrijan conocimiento a través de conversación:
```
Por favor elimina conocimiento incorrecto sobre ""...""
```
o
```
Por favor actualiza conocimiento: (conocimiento antiguo) a (nuevo conocimiento)
```

### P: ¿Dónde se almacenan los datos del grafo de conocimiento?

**R:** Los datos de conocimiento se almacenan en el directorio de datos del sistema en formato JSON. El sistema gestiona automáticamente almacenamiento, indexación y consulta de conocimiento.

### P: ¿Puedo exportar el grafo de conocimiento?

**R:** Actualmente, el grafo de conocimiento se usa principalmente para visualización en línea. Para exportar datos de conocimiento, puede pedir a los seres de silicio que consulten a través de herramientas y generen reportes.

## Mejores Prácticas

### 1. Revisar Regularmente la Acumulación de Conocimiento

- Revisar el grafo de conocimiento semanalmente para entender el progreso de aprendizaje de los seres de silicio
- Enfocarse en nodos y relaciones recién agregados
- Validar la precisión de conocimiento importante

### 2. Guiar el Aprendizaje de Conocimiento

- Proporcionar proactivamente entradas de conocimiento de alta calidad
- Corregir conocimiento incorrecto para mantener la precisión de la base de conocimiento
- Agregar categorías y etiquetas al conocimiento para facilitar la gestión

### 3. Usar el Grafo de Conocimiento para Asistir Conversaciones

- Referenciar conocimiento existente en conversaciones
- Pedir a los seres de silicio que razonen basándose en conocimiento existente
- Explorar asociaciones entre conocimiento para descubrir nuevos insights

### 4. Control de Calidad de Conocimiento

- Prestar atención a las puntuaciones de confianza del conocimiento
- Validar la precisión de conocimiento crítico
- Actualizar oportunamente conocimiento obsoleto o incorrecto

## Documentación Relacionada

- [Gestión de Seres](/help/being-management) - Gestionar creación y configuración de seres de silicio
- [Sistema de Chat](/help/chat-system) - Conversar con seres de silicio para aprender conocimiento
- [Sistema de Tareas](/help/task) - Configurar tareas de aprendizaje de conocimiento
- [Sistema de Memoria](/help/memory) - Entender cómo el conocimiento afecta la memoria de conversación
";
    
    public override string WorkNotes => @"
# Notas de Trabajo

## Descripción General

Las Notas de Trabajo son el**sistema de registro de conocimiento personal** de los Seres de Silicio, similar a un diario digital o registro de trabajo. Cada Ser de Silicio puede crear, gestionar y recuperar sus propias notas de trabajo, registrando experiencias de aprendizaje, ideas de trabajo, reflexiones y pensamientos.

Las Notas de Trabajo admiten dos modos:
- **Notas de Trabajo Personales**: Propiedad del Ser de Silicio, por defecto privadas
- **Notas de Trabajo de Proyecto**: Propiedad del espacio de proyecto, los miembros del proyecto pueden colaborar

## Funciones Principales

### Características Principales

- **Registro por Páginas**: Cada nota es una página independiente, organizada por número de página
- **Soporte Markdown**: El contenido admite formato Markdown, permite edición de texto enriquecido
- **Etiquetas de Palabras Clave**: Se pueden añadir palabras clave a las notas, facilitando búsqueda y clasificación
- **Control de Versiones**: Cada edición actualiza automáticamente el número de versión, registrando el historial de cambios
- **Seguimiento de Autor**: Registra información del creador y último editor
- **Vista de Línea de Tiempo**: Ver todas las notas en orden cronológico
- **Búsqueda de Texto Completo**: Buscar notas por palabras clave, resumen o contenido

### Dos Tipos de Notas de Trabajo

| Tipo | Propiedad | Visibilidad Predeterminada | Escenario de Uso |
|------|-----------|---------------------------|------------------|
| Notas de Trabajo Personales | Ser de Silicio | Privado | Diario personal, notas de aprendizaje, registro de pensamientos |
| Notas de Trabajo de Proyecto | Espacio de proyecto | Público | Progreso del proyecto, registro de colaboración, actas de reuniones |

## Acceso a Notas de Trabajo

### Puntos de Entrada

**Notas de Trabajo Personales**:
1. Haga clic en el icono **📝 Notas de Trabajo** en la barra de navegación izquierda
2. O encuentre el punto de entrada de notas de trabajo en la página de detalles del Ser de Silicio

**Notas de Trabajo de Proyecto**:
1. Entre en la página de detalles del proyecto
2. Haga clic en la pestaña **Notas de Trabajo**

### Descripción de la Interfaz

La página de notas de trabajo contiene dos áreas principales:

**Área de Cuadrícula de Notas**:
- Muestra todas las notas en formato de tarjeta
- Cada tarjeta muestra número de página, resumen, hora de actualización
- Efecto de animación de elevación al pasar el cursor
- Haga clic en una tarjeta para abrir los detalles de la nota

**Área de Detalles de Nota**:
- Muestra el contenido completo de la nota en pantalla completa
- Muestra metadatos (resumen, palabras clave, hora de actualización)
- El contenido se renderiza en formato Markdown
- Haga clic en el botón de cierre para volver a la lista

## Crear Notas

### Crear con Herramientas de IA

Los Seres de Silicio pueden crear notas mediante conversación con IA:

**Crear Nota Personal**:
```
Por favor, crea una nota de trabajo para mí:
- Resumen: Aprendí el concepto de grafo de conocimiento
- Contenido: Un grafo de conocimiento es una forma de representar conocimiento en estructura de grafos...
- Palabras clave: grafo de conocimiento, IA, aprendizaje
```

**Crear Nota de Proyecto**:
```
Por favor, crea una nota de trabajo en el proyecto XXX:
- Resumen: Actualización de progreso del proyecto - Semana 1
- Contenido: Esta semana se completó la fase de análisis de requisitos...
- Palabras clave: progreso, informe semanal
```

### Campos Obligatorios de la Nota

- **Resumen**: Breve descripción del contenido de la nota (obligatorio)
- **Contenido**: Contenido detallado de la nota, admite Markdown (obligatorio)
- **Palabras Clave**: Palabras clave separadas por comas (opcional)

### Información Registrada Automáticamente

- **Hora de Creación**: Registra automáticamente la hora UTC
- **Creador**: Registra automáticamente el GUID del creador
- **Número de Página**: El sistema asigna automáticamente un número de página único
- **Número de Versión**: La versión inicial es 1

## Ver y Gestionar Notas

### Explorar Lista de Notas

1. Abrir la página de notas de trabajo
2. Ver la cuadrícula de tarjetas de notas
3. La parte superior de la página muestra el recuento total de páginas
4. Las tarjetas están ordenadas por número de página

### Ver Detalles de la Nota

1. Haga clic en cualquier tarjeta de nota
2. Los detalles de la nota se expanden a pantalla completa
3. Ver la siguiente información: título de número de página, resumen, palabras clave, hora de actualización, contenido completo (renderizado Markdown)
4. Haga clic en **×** en la esquina superior derecha para cerrar los detalles

### Información de Tarjetas de Nota

Cada tarjeta muestra:
- **Page X**: Identificador de número de página
- **Contenido del Resumen**: Breve descripción de la nota
- **Hora de Actualización**: Hora de la última modificación

## Buscar Notas

### Métodos de Búsqueda

**Buscar con Herramientas de IA**: Buscar notas de trabajo que contengan palabras clave
**Ámbito de Búsqueda**: Resumen de la nota, contenido de la nota, campo de palabras clave

### Generar Índice de Notas

Puede pedir a la IA que genere un índice de todas las notas:
```
Por favor, genera un índice de mis notas de trabajo
```

## Actualizar Notas

### Editar Contenido de la Nota

Actualice notas con herramientas de IA, puede actualizar solo resumen, contenido o palabras clave por separado

### Características de Actualización

- **Actualización Selectiva**: Puede actualizar solo algunos campos
- **Incremento de Versión**: Cada edición incrementa automáticamente el número de versión +1
- **Seguimiento de Cambios**: Registra el GUID del último editor
- **Actualización de Hora**: La hora de actualización se refresca automáticamente

## Eliminar Notas

### Operación de Eliminación

Elimine notas con herramientas de IA, se requiere número de página o ID de nota

### Confirmación de Eliminación

- No se puede recuperar después de eliminar, opere con precaución
- La operación de eliminación se registra en el registro de auditoría

## Gestión de Permisos

### Permisos de Notas de Trabajo Personales

- **Creador**: Control total (crear, ver, editar, eliminar)
- **Curador de Silicio**: Puede gestionar notas de todos los Seres de Silicio
- **Otros Usuarios**: Sin permiso de acceso

### Permisos de Notas de Trabajo de Proyecto

- **Miembros del Proyecto**: Pueden crear, ver, editar notas del proyecto
- **No Miembros del Proyecto**: Sin permiso de acceso
- **Curador de Silicio**: Puede gestionar todas las notas del proyecto

## Escenarios de Uso

### Registro de Aprendizaje Personal

Registrar el proceso de aprendizaje de nuevos conocimientos, añadir resumen, contenido, palabras clave

### Seguimiento de Progreso del Proyecto

Registrar el progreso semanal del proyecto, hacer seguimiento de la finalización de tareas

### Resumen de Experiencia Laboral

Resumir experiencias y lecciones del trabajo, almacenar para referencia futura

### Registro de Actas de Reuniones

Registrar el contenido de reuniones del proyecto, almacenar decisiones y resultados de discusiones

## Mejores Prácticas

### 1. Redactar Resumen de Forma Estándar

- Limitar el resumen a menos de 20 caracteres, expresar claramente el contenido central de la nota

### 2. Usar Palabras Clave de Forma Razonable

- Usar 3-5 palabras clave, separadas por comas

### 3. Estructurar el Contenido

- Usar jerarquía de títulos Markdown, usar listas para información importante

### 4. Organizar Notas Regularmente

- Generar regularmente vista de índice, eliminar notas caducadas o innecesarias

### 5. Colaboración en Notas de Proyecto

- Registrar claramente información del autor, explicar motivo de cambios al editar

## Preguntas Frecuentes

### P1: ¿Cuál es la diferencia entre notas de trabajo y sistema de memoria?

**R**: Las notas de trabajo son contenido estructurado registrado activamente, similar a un diario; el sistema de memoria almacena fragmentos de conversación y hechos guardados automática o manualmente.

### P2: ¿Puedo proteger las notas con contraseña?

**R**: La versión actual no admite protección con contraseña, pero el acceso se controla mediante el sistema de permisos.

### P3: ¿Hay un límite para el número de notas?

**R**: No hay un límite estricto, pero se recomienda organizar regularmente para mantener la calidad de las notas.

### P4: ¿Puedo exportar las notas?

**R**: La versión actual no admite exportación directa, pero los datos de las notas se pueden obtener a través de la API.

### P5: ¿Puedo ver el historial de versiones de las notas?

**R**: La versión actual solo mantiene la última versión, no almacena versiones históricas.

### P6: ¿Se pueden convertir notas de proyecto y notas personales entre sí?

**R**: No se admite la conversión directa, puede crear una nueva nota y copiar el contenido.

## Documentación Relacionada

- [Gestión de Seres de Silicio](/help/being-management) - Entender cómo gestionar Seres de Silicio
- [Gestión de Proyectos](/help/projects) - Entender el espacio de proyecto y colaboración
- [Gestión de Tareas](/help/task) - Entender la asignación y seguimiento de tareas
- [Grafo de Conocimiento](/help/knowledge-graph) - Entender la visualización de conocimiento
- [Gestión de Permisos](/help/permission) - Entender el mecanismo de control de permisos
";
    
    public override string Projects => @"
# Gestión de Proyectos

## Descripción General

La Gestión de Proyectos es el**espacio de trabajo colaborativo** del sistema de Seres de Silicio, que proporciona un entorno para que múltiples Seres de Silicio trabajen juntos. A través del espacio de proyecto, el Curador de Silicio puede organizar y gestionar equipos de Seres de Silicio, asignar tareas, hacer seguimiento del progreso y lograr objetivos de colaboración complejos.

Los proyectos son la unidad organizativa central del sistema de Seres de Silicio, cada proyecto tiene un sistema de tareas independiente, notas de trabajo y lista de miembros.

## Funciones Principales

### Características Principales

- **Gestión del Ciclo de Vida del Proyecto**: Crear, archivar, restaurar, eliminar proyectos
- **Gestión de Miembros**: Asignar y eliminar miembros del proyecto
- **Colaboración de Tareas**: Sistema de gestión de tareas exclusivo del proyecto
- **Notas de Trabajo**: Notas de trabajo compartidas a nivel de proyecto
- **Información del Proyecto**: Gestión de metadatos como nombre, descripción, estado
- **Mecanismo de Archivo**: Admite archivo y restauración de proyectos, protege datos históricos

### Estado del Proyecto

| Estado | Descripción | Acciones Ejecutables |
|--------|-------------|---------------------|
| Activo | Proyecto en funcionamiento normal | Todas las acciones |
| Archivado | Proyecto pausado, datos conservados | Restaurar, ver |
| Eliminado | Proyecto eliminado permanentemente | Ninguna |

## Acceso a Gestión de Proyectos

### Puntos de Entrada

**Función Exclusiva del Curador de Silicio**:
- Las herramientas de gestión de proyectos solo están disponibles para el Curador de Silicio
- Pedir a la IA que ejecute operaciones de gestión de proyectos mediante conversación
- Ver la asociación de proyectos en la interfaz de gestión de Seres de Silicio

### Página de Proyecto

Cada proyecto tiene una página dedicada:
1. Visite `/project/{ID del Proyecto}` para ver detalles del proyecto
2. Ver información básica del proyecto (nombre, descripción, estado)
3. Acceder a notas de trabajo del proyecto
4. Acceder a lista de tareas del proyecto
5. Ver lista de miembros del proyecto

## Crear Proyecto

### Operación de Creación

Crear proyecto con herramientas de IA:
```
Por favor, crea un nuevo proyecto:
- Nombre: Proyecto de Desarrollo de Asistente de IA
- Descripción: Desarrollar funciones centrales del sistema de asistente inteligente
```

### Información del Proyecto

Requerido al crear el proyecto:
- **Nombre del Proyecto**: Expresar claramente el objetivo del proyecto (obligatorio)
- **Descripción del Proyecto**: Describir en detalle el contenido del proyecto (opcional, pero recomendado)

### Configuración Automática Después de Crear

- **ID del Proyecto**: El sistema genera automáticamente un GUID único
- **Hora de Creación**: Registra la hora de creación UTC
- **Estado Inicial**: Estado activo
- **Lista de Miembros**: Inicialmente vacía, necesita asignación posterior

## Gestionar Miembros del Proyecto

### Asignar Miembros al Proyecto

Añadir Ser de Silicio al proyecto:
```
Por favor, asigna el Ser de Silicio XXX al proyecto YYY
```

Requerido:
- **ID del Proyecto**: GUID del proyecto objetivo
- **ID del Ser de Silicio**: GUID del Ser de Silicio a asignar

### Eliminar Miembros del Proyecto

Eliminar Ser de Silicio del proyecto:
```
Por favor, elimina el Ser de Silicio XXX del proyecto YYY
```

### Ver Miembros del Proyecto

Listar todos los miembros del proyecto:
```
Por favor, lista todos los miembros del proyecto XXX
```

### Permisos de Miembros

- **Miembros del Proyecto**: Pueden crear tareas del proyecto, escribir notas del proyecto
- **No Miembros del Proyecto**: Sin acceso a recursos del proyecto
- **Curador de Silicio**: Puede gestionar miembros de todos los proyectos

## Actualizar Información del Proyecto

### Cambiar Nombre del Proyecto

```
Por favor, cambia el nombre del proyecto XXX a nuevo nombre del proyecto
```

### Cambiar Descripción del Proyecto

```
Por favor, cambia la descripción del proyecto XXX a nueva descripción del proyecto
```

### Actualizar Nombre y Descripción Simultáneamente

```
Por favor, actualiza el proyecto XXX:
- Nombre: Nuevo nombre
- Descripción: Nueva descripción
```

## Gestión del Ciclo de Vida del Proyecto

### Archivar Proyecto

Archivar proyectos temporalmente inactivos:
```
Por favor, archiva el proyecto XXX
```

**Efecto del Archivo**:
- El estado del proyecto cambia a ""Archivado""
- Los datos del proyecto se conservan completamente
- Los miembros siguen asociados
- Se puede restaurar desde la lista de archivos

**Escenarios de Archivo**:
- Proyecto temporalmente suspendido
- Proyecto por fases completado
- Necesidad de organizar la lista de proyectos

### Restaurar Proyecto

Restaurar proyecto desde estado archivado:
```
Por favor, restaura el proyecto XXX
```

**Efecto de Restauración**:
- El estado del proyecto vuelve a ""Activo""
- Todos los datos se restauran completamente
- Las relaciones de miembros permanecen sin cambios
- Todas las funciones se pueden usar normalmente

### Eliminar Proyecto

Eliminar proyecto permanentemente (no recuperable):
```
Por favor, elimina el proyecto XXX
```

**Efecto de Eliminación**:
- Los datos del proyecto se eliminan permanentemente
- Las tareas y notas de trabajo relacionadas se limpian
- Las relaciones de miembros se disuelven
- **La acción no se puede deshacer**

**Confirmar Antes de Eliminar**:
- Confirmar que el proyecto ya no es necesario
- Hacer copia de seguridad de datos importantes (si es necesario)
- Notificar a los miembros del proyecto

### Ver Lista y Detalles del Proyecto

**Listar Todos los Proyectos Activos**:
```
Por favor, lista todos los proyectos
```

**Incluyendo Proyectos Archivados**:
```
Por favor, lista todos los proyectos, incluyendo archivados
```

### Ver Detalles del Proyecto

Obtener información completa del proyecto:
```
Por favor, obtén los detalles del proyecto XXX
```

Información devuelta:
- ID del proyecto
- Nombre del proyecto
- Descripción del proyecto
- Estado del proyecto
- Hora de creación
- Lista de miembros
- Estadísticas de tareas

## Funciones de Colaboración del Proyecto

### Tareas del Proyecto

Cada proyecto tiene un sistema de tareas independiente:

**Crear Tarea del Proyecto**:
```
Por favor, crea una tarea en el proyecto XXX:
- Título: Implementar función de inicio de sesión de usuario
- Descripción: Completar módulo de autenticación y autorización de usuarios
- Prioridad: 1
```

**Prioridad de Tareas**:
- Número más pequeño = mayor prioridad
- Prioridad 1: Prioridad más alta
- Prioridad 5: Prioridad normal

**Flujo de Estado de Tareas**:
1. **Pendiente** (Pending): Tarea creada, esperando inicio
2. **En Ejecución** (Running): Tarea iniciada
3. **Completada** (Completed): Tarea completada exitosamente
4. **Fallida** (Failed): Ejecución de tarea fallida
5. **Cancelada** (Cancelled): Tarea cancelada

### Notas de Trabajo del Proyecto

Cada proyecto tiene un sistema de notas de trabajo compartido:

**Crear Nota del Proyecto**:
```
Por favor, crea una nota de trabajo en el proyecto XXX:
- Resumen: Actualización de progreso del proyecto - Semana 1
- Contenido: Esta semana se completó el análisis de requisitos...
- Palabras clave: progreso, informe semanal
```

**Características de Notas del Proyecto**:
- Todos los miembros del proyecto pueden crear y editar
- Organizado por número de página, conveniente para seguimiento
- Registra información del autor, seguimiento de contribuciones
- Admite formato Markdown

### Comparación de Recursos de Proyecto y Personales

| Tipo de Recurso | Personal | Proyecto |
|----------------|----------|----------|
| Notas de Trabajo | Diario privado | Notas de colaboración compartidas |
| Tareas | Tareas personales | Tareas de colaboración del proyecto |
| Grafo de Conocimiento | Conocimiento personal | Conocimiento del proyecto (futuro) |
| Memoria | Memoria personal | Memoria del proyecto (futuro) |

## Escenarios de Uso

### Proyecto de Desarrollo de Software

**Escenario**: Organizar equipo para desarrollar software

**Ejemplo**:
```
1. Crear proyecto: Desarrollo de aplicación web
2. Asignar miembros: Desarrollo frontend, desarrollo backend, pruebas
3. Crear tareas:
   - Implementar módulo de autenticación de usuarios (prioridad 1)
   - Diseñar arquitectura de base de datos (prioridad 1)
   - Crear documentación de API (prioridad 2)
4. Actualizar notas del proyecto semanalmente para registrar progreso
5. Hacer seguimiento de la finalización de tareas
```

### Proyecto de Investigación

**Escenario**: Colaborar en investigación de IA

**Ejemplo**:
```
1. Crear proyecto: Investigación de optimización de grafo de conocimiento
2. Asignar miembros: Investigador A, Investigador B
3. Crear tareas:
   - Revisión de literatura (prioridad 1)
   - Experimento de mejora de algoritmo (prioridad 2)
   - Crear informe de investigación (prioridad 3)
4. Registrar notas de experimento en el proyecto
5. Archivar fases de investigación completadas
```

### Proyecto de Aprendizaje

**Escenario**: Organizar grupo de estudio

**Ejemplo**:
```
1. Crear proyecto: Grupo de estudio de Python
2. Asignar miembros: Aprendiz A, Aprendiz B, Mentor
3. Crear tareas:
   - Semana 1: Aprender sintaxis básica (prioridad 1)
   - Semana 2: Programación orientada a objetos (prioridad 2)
   - Semana 3: Práctica de proyecto (prioridad 3)
4. Registrar notas de aprendizaje y reflexiones
5. Marcar hitos de aprendizaje completados
```

## Mejores Prácticas

### 1. Convención de Nombres de Proyectos

- Usar nombres de proyectos claros
- Incluir tipo u objetivo del proyecto
- Evitar abreviaturas ambiguas

**Buenos Ejemplos**:
- ""Desarrollo de Backend de Aplicación Web""
- ""Investigación de Optimización de Modelo de IA""
- ""Organización de Documentos Técnicos 2024""

**Malos Ejemplos**:
- ""Proyecto 1""
- ""Prueba""
- ""Nuevo Proyecto""

### 2. Describir el Proyecto en Detalle

- Describir en detalle el objetivo y alcance del proyecto
- Registrar hitos importantes
- Describir resultados esperados

### 3. Asignar Miembros de Forma Razonable

- Asignar miembros basándose en habilidades y roles
- Evitar que un miembro participe en demasiados proyectos
- Revisar regularmente la participación de miembros

### 4. Gestión de Prioridad de Tareas

- Establecer alta prioridad para tareas importantes (1-2)
- Establecer prioridad media para tareas normales (3-4)
- Establecer baja prioridad para tareas opcionales (5+)

### 5. Actualizar Regularmente Notas del Proyecto

- Actualizar progreso al menos una vez por semana
- Registrar decisiones y cambios importantes
- Mantener continuidad e integridad de las notas

### 6. Archivar Proyectos Oportunamente

- Archivar proyectos completados oportunamente
- Considerar archivar proyectos inactivos a largo plazo
- Confirmar que los datos están guardados antes de archivar

### 7. Usar la Función de Eliminación con Precaución

- Confirmar dos veces antes de eliminar
- Considerar archivar primero y observar por un tiempo
- Confirmar que los datos históricos no son necesarios

## Gestión de Permisos

### Permisos de Gestión de Proyectos

- **Curador de Silicio**:
  - Crear, archivar, restaurar, eliminar proyectos
  - Asignar y eliminar miembros del proyecto
  - Actualizar información del proyecto
  - Ver todos los proyectos

- **Miembros del Proyecto**:
  - Ver información del proyecto
  - Crear y editar tareas del proyecto
  - Crear y editar notas del proyecto
  - No puede gestionar el proyecto en sí

- **No Miembros del Proyecto**:
  - Sin acceso a recursos del proyecto
  - No puede ver información del proyecto

### Permisos de Herramientas

- Herramienta `project`: Marcada con `[SiliconManagerOnly]`, solo Curador de Silicio
- Herramienta `project_task`: Disponible para miembros del proyecto y Curador de Silicio
- Herramienta `project_work_note`: Disponible para miembros del proyecto y Curador de Silicio

## Preguntas Frecuentes

### P1: ¿Hay un límite superior para el número de proyectos?

**R**: La versión actual no tiene un límite estricto. Sin embargo, se recomienda mantener un número razonable de proyectos para facilitar la gestión. Demasiados proyectos pueden causar confusión en la gestión.

### P2: ¿Los proyectos archivados ocupan espacio de almacenamiento?

**R**: Sí, todos los datos de los proyectos archivados (tareas, notas, etc.) se conservan. Si el espacio de almacenamiento es limitado, puede considerar eliminar proyectos que ya no son necesarios.

### P3: ¿Puedo pertenecer a múltiples proyectos simultáneamente?

**R**: Sí. Un Ser de Silicio puede ser asignado a múltiples proyectos simultáneamente y cambiar entre diferentes proyectos.

### P4: ¿Los miembros del proyecto pueden abandonar el proyecto por sí mismos?

**R**: La versión actual no admite el abandono autónomo de miembros. El Curador de Silicio debe ejecutar la operación de eliminación.

### P5: ¿Se pueden añadir tareas a proyectos archivados?

**R**: No. Los proyectos archivados están en estado de solo lectura y deben restaurarse al estado activo para continuar añadiendo o modificando.

### P6: ¿Al eliminar un proyecto, también se eliminan las tareas y notas?

**R**: Sí. Eliminar un proyecto limpia todos los datos relacionados (tareas, notas de trabajo, etc.). Esta acción no es recuperable.

### P7: ¿Puedo copiar un proyecto?

**R**: La versión actual no admite la copia directa de proyectos. Debe crear un nuevo proyecto manualmente y reconfigurar miembros y tareas.

### P8: ¿Los proyectos tienen una fecha límite?

**R**: La versión actual no obliga a establecer fechas límite. Sin embargo, puede registrar el cronograma planificado en la descripción del proyecto o gestionar el tiempo mediante tareas.

## Documentación Relacionada

- [Gestión de Tareas](/help/task) - Entender la gestión de tareas personales
- [Notas de Trabajo](/help/work-notes) - Entender el sistema de notas de trabajo
- [Gestión de Seres de Silicio](/help/being-management) - Entender cómo gestionar Seres de Silicio
- [Gestión de Permisos](/help/permission) - Entender el mecanismo de control de permisos
- [Registro de Auditoría](/help/audit-log) - Ver registro de acciones del proyecto
";
    
    public override string Logging => @"
# Sistema de Registro de Eventos

## Descripción General

El sistema de registro de eventos es una infraestructura central de la plataforma Silicon Life Collective, utilizada para registrar el estado operativo del sistema, el comportamiento de los seres de silicio, mensajes de error y datos de depuración. El sistema produce salida simultánea a consola y sistema de archivos, y proporciona capacidades flexibles de filtrado por nivel de registro.

## Funciones Principales

- **Registro Multi-Nivel**: Soporta 6 niveles de registro (Trace, Debug, Information, Warning, Error, Critical)
- **Múltiples Destinos de Salida**: Soporta simultáneamente salida de consola coloreada y almacenamiento persistente en sistema de archivos
- **Filtrado Inteligente**: Soporta filtrado por nivel de registro, rango de tiempo y ser de silicio
- **Gestión Categorizada**: Organiza registros por categoría para facilitar la localización de problemas
- **Registro de Excepciones**: Registra automáticamente trazas de pila de excepciones para asistir en solución de problemas

## Descripción de Niveles de Registro

El sistema define los siguientes niveles de registro (en orden creciente de severidad):

| Nivel | Descripción | Caso de Uso |
|-------|-------------|-------------|
| **Trace** | Registros más detallados | Depuración de desarrollo, incluye datos sensibles |
| **Debug** | Información de depuración | Investigación interactiva, propósitos de depuración |
| **Information** | Información general | Rastreo del flujo normal de la aplicación, valor a largo plazo |
| **Warning** | Mensajes de advertencia | Eventos anormales o inesperados, pero no detiene la ejecución |
| **Error** | Mensajes de error | El flujo de ejecución actual se detiene debido a fallo |
| **Critical** | Errores críticos | Fallo del sistema o fallo catastrófico, requiere manejo inmediato |
| **None** | Sin registro | Usado para deshabilitar registro para una categoría específica |

## Guía de Uso

### ¿Dónde ver los registros?

El sistema proporciona dos formas de ver registros:

1. **Registros de Consola**: Al ejecutar el programa, la ventana de terminal muestra registros coloreados en tiempo real
   - Diferentes colores representan diferentes niveles de severidad (rojo=error, amarillo=advertencia, etc.)
   - Adecuado para depuración de desarrollo y monitoreo en tiempo real

2. **Registros de Archivo**: Todos los registros se guardan automáticamente en el directorio `data/Log/`
   - Guardados permanentemente, puede ver el historial en cualquier momento
   - Adecuado para rastreo de problemas y análisis posterior

## Opciones de Configuración

### Ajustar Detalle de Registro

Puede configurar el detalle de registro en el archivo de configuración:

- **Durante Desarrollo/Depuración**: Configurar a ""Debug"" o ""Trace"" para ver información más detallada
- **Uso Diario**: Configurar a ""Information"" para registrar operaciones normales
- **Entorno de Producción**: Configurar a ""Warning"" para registrar solo problemas y errores

## Visualización y Filtrado de Registros

Los archivos de registro se guardan en el directorio `data/Log/`, organizados automáticamente por tiempo para facilitar la búsqueda de historial.

## Formato de Visualización de Registros

### Estilo de Visualización en Consola

```
[2026-04-27 10:30:00.123] [INFO] [Category] [Being:guid] Mensaje de registro
```

- Marca de tiempo: Precisa a milisegundos
- Nivel: Alineado a la derecha, 4 caracteres de ancho
- Categoría: Identificador de fuente de registro
- ID de Ser de Silicio: Solo se muestra cuando el registro se relaciona con un ser específico

### Atributos de Entrada de Registro

Cada entrada de registro contiene la siguiente información:

- **BeingId**: ID del ser de silicio asociado (vacío para registros del sistema)
- **Timestamp**: Hora de creación del registro (UTC)
- **Level**: Nivel de registro
- **Category**: Categoría de registro
- **Message**: Mensaje de registro
- **Exception**: Información de excepción (si existe)

## Mejores Prácticas

### 1. Elegir Niveles de Registro Apropiados

- **Entorno de Desarrollo**: Usar niveles Debug o Trace
- **Entorno de Producción**: Usar niveles Information o Warning
- **Solución de Problemas**: Reducir temporalmente a nivel Debug

### 2. ¿En qué registros debe enfocarse?

Se recomienda重点关注 las siguientes situaciones:
- Información de inicio y apagado del sistema
- Creación de seres de silicio, anomalías en tiempo de ejecución y apagado
- Fallos de validación de permisos y problemas de seguridad
- Fallos de llamadas a IA y anomalías de respuesta
- Errores de ejecución de herramientas y timeouts

### 3. Evitar Impacto en el Rendimiento por Exceso de Registros

Si los archivos de registro son demasiado grandes, puede aumentar la configuración de detalle de registro (ej. cambiar a ""Warning"" o ""Error"") para reducir la cantidad de registros.

### 4. Protección de Información Sensible

El sistema NO registra la siguiente información sensible en los registros:
- Contraseñas y claves
- Información de identificación personal
- Claves y tokens de API
- Cadenas de conexión de base de datos

## Preguntas Frecuentes

### P: ¿Por qué no puedo ver algunos mensajes de registro?

**R**: Posibles razones:
1. La configuración de detalle de registro es demasiado alta, filtrando alguna información (se puede ajustar a ""Debug"" o ""Trace"" en configuración)
2. El sistema no inició correctamente, la funcionalidad de registro no se inicializó

### P: ¿Cómo ver registros más detallados?

**R**: Modifique la configuración de detalle de registro en el archivo de configuración a ""Debug"" o ""Trace"", luego reinicie el sistema.

### P: ¿Dónde están los archivos de registro?

**R**: Los archivos de registro se almacenan en el directorio `data/Log/`, indexados por tiempo.

## Solución de Problemas

### Problema: No se pueden encontrar registros históricos

**Síntomas**: No se pueden ver registros pasados

**Soluciones**:
1. Verificar si el directorio `data/Log/` existe y tiene permisos de lectura/escritura
2. Confirmar que hay suficiente espacio en disco disponible

## Notas

- El sistema de registro garantiza operación estable; la falla de un solo destino de salida no afectará otros destinos
- Si los archivos de registro son demasiado grandes, puede aumentar la configuración de detalle de registro (ej. cambiar a ""Warning"" o ""Error"") para reducir el volumen de registro
- Al escribir grandes cantidades de registros, se recomienda aumentar apropiadamente el nivel de registro
";
    
    #endregion
}
