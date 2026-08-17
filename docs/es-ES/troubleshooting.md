# Guía de Solución de Problemas

> **Versión: v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | **Español** | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Русский](../ru-RU/troubleshooting.md)

## Preguntas Frecuentes

### Construcción y Compilación

#### Problema: La construcción falla, faltan dependencias

**Síntomas**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Solución**:
```bash
dotnet restore
dotnet build
```

#### Problema: No se encuentra el SDK de .NET

**Síntomas**:
```
The .NET SDK could not be found
```

**Solución**:
1. Instalar .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
2. Verificar la instalación:
```bash
dotnet --version
```

---

### Problemas de Conexión de IA

#### Problema: Conexión a Ollama rechazada

**Síntomas**:
```
Failed to connect to Ollama at http://localhost:11434
```

**Solución**:
```bash
# Verificar si Ollama está ejecutándose
ollama list

# Iniciar Ollama
ollama serve

# Probar la conexión
curl http://localhost:11434/api/tags
```

#### Problema: Modelo no encontrado

**Síntomas**:
```
model "qwen2.5:7b" not found
```

**Solución**:
```bash
# Extraer el modelo requerido
ollama pull qwen2.5:7b

# Listar modelos disponibles
ollama list
```

#### Problema: Error 404 de Bailian

**Síntomas**:
```
HTTP 404: Model not found
```

**Solución**:
1. Verificar que la clave API sea correcta
2. Comprobar que el nombre del modelo coincida con el directorio de Bailian
3. Verificar que el endpoint regional sea correcto
4. Comprobar que la cuenta tenga acceso al modelo

#### Problema: Fallo de conexión con Volcengine Ark

**Síntomas**:
```
HTTP 401: Unauthorized
o
HTTP 404: Endpoint not found
```

**Solución**:
1. Verificar que la clave API sea correcta
2. Comprobar que el formato de la URL del endpoint sea correcto (predeterminado: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Confirmar que el parámetro Model use el ID del punto de acceso de inferencia (por ejemplo, `ep-20241212123456-abcde`), no el nombre del modelo
4. Comprobar que la cuenta tenga acceso al punto de acceso

#### Problema: Fallo de conexión con DeepSeek / Zhipu / Ernie / Hunyuan / MiniMax / Moonshot / SiliconFlow

**Síntomas**:
```
HTTP 401: Unauthorized
o
HTTP 404: Model not found
```

**Solución**:
1. Verificar que la clave API sea correcta
2. Comprobar que el endpoint sea accesible desde su red
3. Verificar que el nombre del modelo sea válido:
   - DeepSeek: `deepseek-v4-flash`, `deepseek-v4-pro`
   - Zhipu: `glm-4-flash` (gratuito), `glm-5`, `glm-4-long` (1M)
   - Ernie: `ernie-5.1`, `ernie-speed-128k` (gratuito), `ernie-tiny-8k` (gratuito)
   - Hunyuan: `hy3` (TokenHub), otros modelos (Legacy)
   - MiniMax: `MiniMax-M3`, `MiniMax-M2.7`, endpoint `domestic`/`international`
   - Moonshot: `kimi-k2.6`, `kimi-k2.7-code`
   - SiliconFlow: Lista dinámica — usar API `/models` para ver modelos disponibles
4. Para Hunyuan: Verificar que el endpoint coincida con el modelo (hy3/hy3-preview → TokenHub, otros → Legacy)
5. Para MiniMax: Verificar que el endpoint seleccionado (`domestic`/`international`) sea accesible

---

### Problemas en Tiempo de Ejecución

#### Problema: Puerto ya en uso

**Síntomas**:
```
HttpListenerException: Address already in use
```

**Solución**:

**Windows**:
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
lsof -ti:8080 | xargs kill -9
```

**O cambie el puerto en la configuración.**

#### Problema: El Ser de Silicio no puede iniciar

**Síntomas**:
- El estado del Ser de Silicio muestra "Error"
- Los registros muestran fallo de inicialización

**Solución**:
1. Verificar que el Archivo de Alma exista y sea válido
2. Verificar que el cliente de IA esté configurado
3. Revisar los registros para obtener errores específicos:
```bash
tail -f logs/*.log
```

#### Problema: Memoria insuficiente

**Síntomas**:
```
OutOfMemoryException
```

**Solución**:
1. **SiliconLife.Default**: Aumentar el tamaño del heap:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: La versión Fast tiene un consumo de memoria inherente más alto (~500MB), si la memoria sigue siendo insuficiente, se recomienda:
   - Reducir la cantidad de Seres de Silicio concurrentes
   - Limpiar datos antiguos para liberar memoria

3. Limpiar datos antiguos:
```bash
# Archivar registros antiguos
mv logs/ logs-archive/
mkdir logs

# Limpiar memorias antiguas
# A través de Web UI: Gestión de Memoria > Limpiar
```

> **Consejo**: SiliconLife.Default tiene un consumo de memoria menor (~200MB), adecuado para entornos con memoria limitada; SiliconLife.Fast tiene un consumo mayor pero mejor rendimiento, adecuado para entornos de producción.

---

### Problemas de Permisos

#### Problema: Permiso denegado

**Síntomas**:
```
Permission denied: FileAccess C:\Windows
```

**Solución**:
1. Verificar los permisos actuales:
```bash
curl http://localhost:8080/api/permissions/list
```

2. Otorgar permisos:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

3. O usar la Web UI: Gestión de Permisos

#### Problema: Los permisos no expiran

**Síntomas**:
- Los permisos siguen siendo válidos después del tiempo de expiración

**Solución**:
1. Verificar la sincronización del reloj del sistema
2. Comprobar que el campo `expiresAt` esté configurado correctamente
3. Limpiar la caché de permisos

---

### Problemas de Web UI

#### Problema: No se puede acceder a la Web UI

**Síntomas**:
- El navegador muestra "Connection refused"

**Solución**:
1. Verificar que el servidor esté ejecutándose
2. Comprobar la URL correcta: `http://localhost:8080`
3. Verificar la configuración del firewall
4. Revisar los registros para errores de inicio

#### Problema: SSE no funciona

**Síntomas**:
- Las actualizaciones en tiempo real no aparecen
- El chat no se transmite en flujo

**Solución**:
1. Verificar que el navegador soporte SSE
2. Desactivar el almacenamiento en búfer del proxy para SSE
3. Comprobar la estabilidad de la red
4. Probar con un navegador diferente

#### Problema: La UI se ve rota

**Síntomas**:
- Los estilos son incorrectos
- El diseño está desordenado

**Solución**:
1. Limpiar la caché del navegador
2. Probar una piel diferente: Configuración > Piel
3. Revisar errores en la consola del navegador
4. Desactivar las extensiones del navegador

---

### Problemas de Almacenamiento

#### Problema: No se pueden leer/escribir datos

**Síntomas**:
```
IOException: Access denied
```

**Solución**:
1. Verificar los permisos de archivo
2. Comprobar que la ruta de almacenamiento exista
3. Verificar el espacio en disco
4. Ejecutar con los permisos adecuados

#### Problema: Datos corruptos

**Síntomas**:
- Errores de análisis JSON
- Pérdida de datos

**Solución**:
1. Restaurar desde copia de seguridad
2. Verificar la integridad del almacenamiento:
```bash
# A través de Web UI: Sistema > Verificación de almacenamiento
```

3. Reparar manualmente los archivos corruptos

#### Problema: Archivo de almacenamiento SpeedyPack corrupto (versión Fast)

**Síntomas**:
- El archivo `.spk` no se puede cargar
- La inicialización de SpeedyStorage falla

**Solución**:
1. Usar la herramienta `SiliconLife.Speedy.Manager` para verificar y reparar archivos `.spk`
2. Comprobar que el archivo de índice `.spk.idx` coincida con el archivo `.spk`
3. Si el archivo de índice está corrupto, eliminar el archivo `.spk.idx`, el sistema reconstruirá automáticamente el índice
4. Restaurar el archivo `.spk` desde una copia de seguridad

#### Problema: Fallo en la compactación automática de SpeedyPack (versión Fast)

**Síntomas**:
- El archivo `.spk` sigue creciendo
- Espacio en disco insuficiente

**Solución**:
1. Verificar que `SpeedyPackAutoCompactor` esté funcionando correctamente
2. Activar manualmente la operación de compactación
3. Verificar la configuración del umbral de compactación
4. Usar la herramienta `SiliconLife.Speedy.Manager` para compactar manualmente

---

### Problemas de Ejecución de Herramientas

#### Problema: Herramienta no encontrada

**Síntomas**:
```
Tool "xyz" not found
```

**Solución**:
1. Verificar que el nombre de la herramienta sea correcto
2. Comprobar que la herramienta esté en el directorio Tools
3. Reconstruir el proyecto
4. Verificar que la herramienta esté correctamente implementada

#### Problema: La herramienta devuelve un error

**Síntomas**:
```
Tool execution failed: ...
```

**Solución**:
1. Revisar los registros de la herramienta
2. Verificar los parámetros de entrada
3. Probar la herramienta de forma independiente
4. Verificar los permisos

---

### Problemas de Plugins

#### Problema: Fallo en la carga del plugin

**Síntomas**:
```
Plugin load failed: Security check failed
```

**Solución**:
1. Verificar que el plugin no referencie espacios de nombres prohibidos no declarables (ej.: `System.Runtime.InteropServices`, `System.Reflection.Emit`, `Microsoft.CodeAnalysis`)
2. Si el plugin necesita `System.IO` o `System.Net.Http`, verificar que haya declarado las capacidades `FileIO` o `Network` mediante `[PluginCapability]`
3. Comprobar que el plugin solo referencie ensamblados en la lista blanca de ensamblados de confianza
4. Verificar que el plugin implemente correctamente la interfaz `IPlugin`
5. Revisar los registros para obtener detalles del fallo en la verificación de seguridad

#### Problema: Las herramientas del plugin no se registran

**Síntomas**:
- El plugin se carga correctamente pero las herramientas no aparecen en la lista de herramientas

**Solución**:
1. Confirmar que las clases de herramientas del plugin implementan correctamente la interfaz `ITool`
2. Verificar que las clases de herramientas sean públicas
3. Comprobar que se haya llamado a `ToolManager.ScanAllPluginAssemblies()`
4. Reconstruir el plugin y reiniciar la aplicación

---

### Problemas con las Habilidades

#### Problema: La habilidad no aparece en la lista de habilidades o no es visible para la IA

**Síntomas**:
- La página de habilidades de Web UI guarda correctamente, pero la lista no muestra / la IA no invoca la habilidad

**Solución**:
1. Comprobar que `id` y `description` de la habilidad no estén vacíos (los borradores no se exponen a la IA)
2. Las habilidades con metadatos incompletos (`NeedsCompletion`) no se inyectan en la IA——completar los metadatos del front matter YAML o dejar que la IA los complete antes de guardar
3. Comprobar si la matriz de permisos deshabilita `{skillId}:execute` (las habilidades deshabilitadas no son visibles para la IA)
4. Confirmar que el interruptor global `SkillEnabled` es true
5. La recarga en caliente tarda hasta 30 segundos en efectarse, esperar y refrescar o reiniciar

#### Problema: La ejecución de la habilidad falla con "not in whitelist"

**Síntomas**:
```
Tool 'xxx' is not available in skill 'yyy' (not in whitelist)
```

**Solución**:
- Añadir la herramienta al `tool_whitelist` de la habilidad, o vaciar la lista blanca para heredar todas las herramientas del Ser de Silicio

#### Problema: Límite de cantidad de habilidades alcanzado

**Síntomas**:
```
Custom skill limit reached (50)
```

**Solución**:
1. Eliminar habilidades personalizadas que ya no se usan
2. O aumentar la configuración `MaxCustomSkillsPerBeing`

---

### Problemas MCP

#### Problema: Fallo de conexión del servidor MCP

**Síntomas**:
- El estado del servidor muestra `error` o `disconnected`, `lastError` no está vacío

**Solución**:
1. Servidor stdio: confirmar que `command` es ejecutable (ej. `npx` está en PATH), `arguments` correctos
2. Servidor http: comprobar que la URL del `endpoint` es alcanzable (firewall, proxy)
3. Hacer clic en **Reconectar** en la página /mcp
4. Revisar los detalles de `lastError`, comúnmente comando inexistente, versión incompatible, endpoint 404

#### Problema: Las herramientas MCP no se inyectan en el Ser de Silicio

**Síntomas**:
- El servidor está conectado (`connected`) pero la IA no puede llamar la herramienta `mcp_xxx_yyy`

**Solución**:
1. Confirmar que `enabled` del servidor es true
2. Confirmar que el interruptor global `McpEnabled` es true
3. Comprobar la matriz de permisos: si `mcp_{serverId}_{toolName}:execute` está deshabilitado
4. En la conversación del Ser de Silicio se puede usar la herramienta `mcp` (`list_tools`) para verificar los nombres de herramientas inyectadas

#### Problema: Añadir servidor devuelve error de formato de ID

**Síntomas**:
```
Server id must contain only lowercase letters, digits and underscores
```

**Solución**:
- El ID del servidor solo permite letras minúsculas, números y guiones bajos (ej. `filesystem`, `github_tools`)

---

### Problemas con la Plataforma IM

#### Problema: No se reciben mensajes de Feishu

**Solución**:
1. Comprobar la dirección de callback y el puerto de la configuración de suscripción de eventos en la plataforma abierta de Feishu (`listenPort` + `callbackPath`)
2. Confirmar que `Encrypt Key` / `Verification Token` coinciden con la configuración
3. Para desarrollo local se puede usar el asistente de autorización OAuth (autorización de un clic en la página de configuración); el callback de eventos requiere red pública accesible o usar túnel de intranet
4. Revisar errores de verificación de firma/descifrado en los registros

#### Problema: Timeout de autorización OAuth

**Síntomas**:
- La página de autorización muestra estado `timeout`

**Solución**:
1. La sesión de autorización es válida por 5 minutos, tras el timeout hacer clic nuevamente en el botón de autorización
2. Confirmar que la dirección de callback `/im/feishu/callback` es accesible por Feishu (`redirectBaseUrl` configurado correctamente)
3. La visualización del estado frontend depende de SSE; si SSE se desconecta se puede consultar `/im/{platform}/status` como respaldo

#### Problema: Marcador de posición `${ENV_VAR}` no resuelto

**Síntomas**:
- La conexión de la plataforma IM falla, el valor de configuración sigue siendo texto del marcador de posición

**Solución**:
1. Confirmar que la variable de entorno se estableció antes de iniciar el proceso (reiniciar la aplicación para que surta efecto)
2. Comprobar la ortografía del nombre de la variable (solo soporta `[A-Za-z_][A-Za-z0-9_]*`)
3. Nota: conservar el marcador de posición en config.json es el comportamiento esperado, la resolución ocurre en la copia en memoria

#### Problema: Solo una de varias plataformas IM recibe mensajes

**Solución**:
- Los mensajes salientes se difunden a todas las plataformas activadas, el fallo de envío a una plataforma individual se aísla silenciosamente——comprobar si el token de esa plataforma ha expirado (reautorizar o actualizar la clave)

---

### Problemas de Notas de Trabajo

#### Problema: No se pueden crear notas de trabajo

**Síntomas**:
```
Failed to create work note
```

**Solución**:
1. Verificar que el Ser de Silicio exista y esté en estado de ejecución
2. Comprobar que la ruta de almacenamiento tenga permisos de escritura
3. Verificar que el contenido no esté vacío (el contenido es obligatorio)
4. Revisar los registros para obtener información detallada del error

#### Problema: La búsqueda de notas no devuelve resultados

**Síntomas**:
- La búsqueda por palabra clave devuelve resultados vacíos
- Pero se está seguro de que hay notas relevantes

**Solución**:
1. Verificar la ortografía de las palabras clave
2. Probar con palabras clave más generales
3. Comprobar si las notas contienen la palabra clave (distingue mayúsculas de minúsculas)
4. Aumentar el valor del parámetro `max_results`

#### Problema: Generación lenta del directorio de notas

**Síntomas**:
- Tiempo de respuesta largo al generar el directorio
- El Ser de Silicio tiene una gran cantidad de notas (>1000 páginas)

**Solución**:
1. Este es un comportamiento normal, requiere recorrer todas las notas
2. Considerar archivar periódicamente las notas antiguas
3. Usar la función de búsqueda en lugar de navegación por directorio
4. Optimización planificada: agregar mecanismo de caché de directorio

---

### Problemas de Red de Conocimiento

#### Problema: Las consultas de conocimiento devuelven resultados vacíos

**Síntomas**:
```
No knowledge triples found
```

**Solución**:
1. Verificar la ortografía del sujeto y el predicado
2. Comprobar que el conocimiento se haya agregado a la red
3. Usar la función de búsqueda para coincidencia difusa:
```json
{
  "action": "search",
  "query": "palabra clave"
}
```

#### Problema: Fallo en la búsqueda de rutas de conocimiento

**Síntomas**:
```
No path found between concepts
```

**Solución**:
1. Verificar que ambos conceptos existan en la red de conocimiento
2. Comprobar si existe una ruta de asociación (puede que no haya relación directa o indirecta)
3. Intentar agregar más conocimiento para establecer conexiones
4. Reducir el límite de longitud de ruta (si se ha configurado)

#### Problema: Fallo en la validación de conocimiento

**Síntomas**:
```
Knowledge validation failed
```

**Solución**:
1. Comprobar que el formato de la tripleta sea correcto (sujeto, predicado, objeto son obligatorios)
2. Verificar que la confianza esté en el rango 0.0-1.0
3. Comprobar si hay tripletas duplicadas
4. Revisar los detalles del error de validación para entender el problema específico

#### Problema: Las estadísticas de la red de conocimiento son inexactas

**Síntomas**:
- Los números estadísticos no coinciden con lo esperado
- Las estadísticas no se actualizan después de agregar conocimiento

**Solución**:
1. Las estadísticas pueden tardar unos segundos en actualizarse (caché)
2. Comprobar si alguna operación de eliminación no se ejecutó correctamente
3. Reiniciar la aplicación para forzar la actualización de las estadísticas
4. Volver a consultar las estadísticas a través de la API

---

### Problemas de Gestión de Proyectos

#### Problema: No se puede crear un proyecto

**Síntomas**:
```
Failed to create project
```

**Solución**:
1. Verificar que el nombre del proyecto no esté vacío (obligatorio)
2. Comprobar que el nombre del proyecto no esté duplicado
3. Verificar que la ruta de almacenamiento tenga permisos de escritura
4. Revisar los registros para obtener información detallada del error

#### Problema: Pérdida de datos del proyecto

**Síntomas**:
- La información del proyecto no se puede cargar
- Los archivos del proyecto están corruptos

**Solución**:
1. Comprobar que el directorio de almacenamiento del proyecto exista
2. Restaurar los datos del proyecto desde una copia de seguridad
3. Verificar que el formato del archivo JSON sea correcto
4. Reparar manualmente los archivos del proyecto corruptos

#### Problema: Fallo en la asignación de roles del proyecto

**Síntomas**:
```
Failed to assign role
```

**Solución**:
1. Confirmar que el Ser de Silicio se ha unido al proyecto
2. Comprobar que el nombre del rol sea válido
3. Verificar que el operador sea el Curador de Silicio
4. Revisar los registros para obtener información detallada del error

#### Problema: El flujo de trabajo no puede iniciar

**Síntomas**:
- La creación de la instancia del flujo de trabajo falla
- Las transiciones de estado no se ejecutan

**Solución**:
1. Comprobar que la plantilla del flujo de trabajo esté definida
2. Verificar que el estado inicial esté configurado correctamente
3. Confirmar que el proyecto tenga una plantilla de flujo de trabajo vinculada
4. Revisar los registros del flujo de trabajo para obtener errores de transición

---

### Problemas de Permisos de Herramientas

#### Problema: Operación de herramienta denegada

**Síntomas**:
```
Tool operation denied: network:post
```

**Solución**:
1. Verificar la configuración de permisos de herramientas del Ser de Silicio:
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. Actualizar los permisos de herramientas:
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. O usar la Web UI: Ser de Silicio → Permisos de Herramientas

#### Problema: Los permisos de herramientas a nivel de proyecto no surten efecto

**Síntomas**:
- Los permisos de herramientas a nivel de proyecto no funcionan como se esperaba

**Solución**:
1. Confirmar que los permisos a nivel de proyecto estén correctamente configurados
2. Comprobar si hay conflictos entre los permisos a nivel del Ser de Silicio y a nivel de proyecto
3. Los permisos a nivel de proyecto son independientes de los permisos a nivel del Ser de Silicio, se toma la intersección de ambos
4. Revisar los registros de auditoría para confirmar los resultados de la verificación de permisos

---

## Depuración

### Habilitar Registro Detallado

Editar la configuración:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Revisar Registros

Los registros se almacenan en:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Ver en tiempo real:
```bash
tail -f logs/*.log
```

### Usar el Depurador

**SiliconLife.Default (implementación por defecto)**:
```bash
# Ejecutar con el depurador
dotnet run --project src/SiliconLife.Default --configuration Debug

# Adjuntar depurador
# A través del IDE: Adjuntar al proceso > SiliconLife.Default
```

**SiliconLife.Fast (versión de alto rendimiento)**:
```bash
# Ejecutar con el depurador
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Adjuntar depurador
# A través del IDE: Adjuntar al proceso > SiliconLife.Fast
```

> **Recomendación**: Durante la fase de desarrollo y depuración, se recomienda usar SiliconLife.Default; una vez verificada la arquitectura, usar SiliconLife.Fast para el despliegue en producción.

---

## Problemas de Rendimiento

### Tiempo de Respuesta Lento

**Optimización**:
1. Reducir la complejidad del modelo de IA
2. Habilitar la caché
3. Limpiar datos antiguos
4. Aumentar los recursos del sistema

### Uso Alto de CPU

**Verificar**:
- Demasiados Seres de Silicio ejecutándose
- Bucles infinitos en las herramientas
- Ejecución frecuente de temporizadores

**Solución**:
- Reducir los Seres de Silicio concurrentes
- Optimizar el código de las herramientas
- Ajustar los intervalos de los temporizadores

### Uso Alto de Memoria

**Monitorear**:
```bash
# A través de Web UI: Panel > Memoria
```

**Optimizar**:
- Limpiar memorias antiguas
- Reducir el tamaño del contexto
- Implementar paginación

---

## Obtener Ayuda

### Consultar la Documentación

- [Guía de Inicio Rápido](getting-started.md)
- [Guía de Desarrollo](development-guide.md)
- [Referencia de API](api-reference.md)
- [Guía de Arquitectura](architecture.md)

### Revisar Registros

Siempre revise primero los registros para obtener detalles de los errores.

### Soporte de la Comunidad

- GitHub Issues: Reportar bugs
- Discussions: Hacer preguntas
- Documentación: Buscar soluciones

---

## Procedimientos de Emergencia

### Fallo del Sistema

1. Revisar los registros para determinar la causa
2. Reiniciar la aplicación:

**SiliconLife.Default (implementación por defecto)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (versión de producción recomendada)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Si es necesario, restaurar desde una copia de seguridad

### Pérdida de Datos

1. Detener la aplicación inmediatamente
2. Comprobar los archivos de copia de seguridad
3. Restaurar los datos
4. Verificar la integridad

### Vulnerabilidad de Seguridad

1. Detener todos los Seres de Silicio
2. Revocar todos los permisos
3. Revisar los registros de auditoría
4. Verificar los controles de acceso
5. Reiniciar con permisos restringidos

---

## Prevención

### Mejores Prácticas

1. **Copias de Seguridad Regulares**
   - Respaldar el directorio de datos
   - Respaldar la configuración
   - Probar el proceso de restauración

2. **Monitorear Recursos**
   - Vigilar el uso de CPU/memoria
   - Monitorear el espacio en disco
   - Verificar las conexiones de red

3. **Mantener Actualizado**
   - Actualizar el SDK de .NET
   - Actualizar las dependencias
   - Aplicar parches de seguridad

4. **Probar Cambios**
   - Probar primero en desarrollo
   - Usar control de versiones
   - Documentar los cambios

---

## Siguientes Pasos

- 📚 Leer la [guía de arquitectura](architecture.md)
- 🛠️ Consultar la [guía de desarrollo](development-guide.md)
- 🚀 Ver la [guía de inicio rápido](getting-started.md)
- 🔒 Consultar la [documentación de seguridad](security.md)
