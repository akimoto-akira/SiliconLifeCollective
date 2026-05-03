# Guía de Solución de Problemas

> **Versión: v0.1.0-alpha**

[English](../en/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | **Español** | [Deutsch](../de-DE/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md)

## Preguntas Frecuentes

### Construcción y Compilación

#### Problema: Construcción falla por dependencias faltantes

**Síntomas**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Solución**:
```bash
dotnet restore
dotnet build
```

#### Problema: SDK de .NET no encontrado

**Síntomas**:
```
The .NET SDK could not be found
```

**Solución**:
1. Instalar .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
2. Verificar instalación:
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
# Verificar que Ollama esté ejecutándose
ollama list

# Iniciar Ollama
ollama serve

# Probar conexión
curl http://localhost:11434/api/tags
```

#### Problema: Modelo no encontrado

**Síntomas**:
```
model "qwen2.5:7b" not found
```

**Solución**:
```bash
# Obtener modelo requerido
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
2. Comprobar que el nombre del modelo coincida con el catálogo de Bailian
3. Verificar que el endpoint de región sea correcto
4. Comprobar que la cuenta tenga acceso al modelo

---

### Problemas de Ejecución

#### Problema: Puerto en uso

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

**O cambiar puerto en configuración**.

#### Problema: Ser no se inicia

**Síntomas**:
- El estado del ser muestra "Error"
- Los registros muestran fallo de inicialización

**Solución**:
1. Verificar que archivo de alma existe y es válido
2. Comprobar que el cliente de IA esté configurado
3. Revisar registros para errores específicos:
```bash
tail -f logs/*.log
```

#### Problema: Memoria insuficiente

**Síntomas**:
```
OutOfMemoryException
```

**Solución**:
1. **SiliconLife.Default**: Aumentar tamaño del heap:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: La versión Fast tiene mayor uso de memoria inherente (~500MB), si la memoria sigue siendo insuficiente:
   - Reducir número de Seres Silicona concurrentes
   - Limpiar datos antiguos para liberar memoria

3. Limpiar datos antiguos:
```bash
# Archivar registros antiguos
mv logs/ logs-archive/
mkdir logs

# Limpiar memoria antigua
# A través de Web UI: Gestión de Memoria > Limpiar
```

> **Consejo**: SiliconLife.Default tiene menor uso de memoria (~200MB), adecuado para entornos con recursos limitados; SiliconLife.Fast tiene mayor uso de memoria pero mejor rendimiento, adecuado para entornos de producción.

---

### Problemas de Permisos

#### Problema: Permiso denegado

**Síntomas**:
```
Permission denied: disk:write
```

**Solución**:
1. Verificar permisos actuales:
```bash
curl http://localhost:8080/api/permissions
```

2. Conceder permiso:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. O usar Web UI: Gestión de Permisos

#### Problema: Permisos no expiran

**Síntomas**:
- Permisos siguen vigentes después del tiempo de expiración

**Solución**:
1. Verificar sincronización del reloj del sistema
2. Validar que el campo `expiresAt` esté configurado correctamente
3. Limpiar caché de permisos

---

### Problemas de Web UI

#### Problema: No se puede acceder a Web UI

**Síntomas**:
- Navegador muestra "Connection refused"

**Solución**:
1. Verificar que el servidor esté ejecutándose
2. Comprobar URL correcta: `http://localhost:8080`
3. Verificar configuración de firewall
4. Revisar registros para errores de inicio

#### Problema: SSE no funciona

**Síntomas**:
- Actualizaciones en tiempo real no aparecen
- Chat no se transmite en streaming

**Solución**:
1. Verificar que el navegador soporte SSE
2. Deshabilitar buffer de proxy para SSE
3. Verificar estabilidad de red
4. Probar con otro navegador

#### Problema: UI se ve rota

**Síntomas**:
- Estilos incorrectos
- Layout dañado

**Solución**:
1. Limpiar caché del navegador
2. Probar otra piel: Configuración > Piel
3. Verificar errores en consola del navegador
4. Deshabilitar extensiones del navegador

---

### Problemas de Almacenamiento

#### Problema: No se puede leer/escribir datos

**Síntomas**:
```
IOException: Access denied
```

**Solución**:
1. Verificar permisos de archivo
2. Validar que la ruta de almacenamiento exista
3. Verificar espacio en disco
4. Ejecutar con permisos apropiados

#### Problema: Datos corruptos

**Síntomas**:
- Errores de análisis JSON
- Pérdida de datos

**Solución**:
1. Restaurar desde respaldo
2. Verificar integridad del almacenamiento:
```bash
# A través de Web UI: Sistema > Verificación de almacenamiento
```

3. Reparar manualmente archivos corruptos

#### Problema: Archivo de almacenamiento SpeedyPack corrupto (versión Fast)

**Síntomas**:
- Archivo `.spk` no se puede cargar
- Inicialización de SpeedyStorage falla

**Solución**:
1. Usar herramienta `SiliconLife.Speedy.Manager` para verificar y reparar archivos `.spk`
2. Verificar que el archivo de índice `.spk.idx` coincida con el archivo `.spk`
3. Si el archivo de índice está corrupto, eliminar archivo `.spk.idx`, el sistema reconstruirá el índice automáticamente
4. Restaurar archivo `.spk` desde respaldo

#### Problema: Auto-compresión SpeedyPack falla (versión Fast)

**Síntomas**:
- Archivo `.spk` sigue creciendo
- Espacio en disco insuficiente

**Solución**:
1. Verificar que `SpeedyPackAutoCompactor` esté funcionando correctamente
2. Activar operación de compresión manualmente
3. Verificar configuración de umbral de compresión
4. Usar herramienta `SiliconLife.Speedy.Manager` para compresión manual

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

#### Problema: Herramienta devuelve error

**Síntomas**:
```
Tool execution failed: ...
```

**Solución**:
1. Verificar registros de la herramienta
2. Validar parámetros de entrada
3. Probar la herramienta de forma independiente
4. Verificar permisos

---

### Problemas de Plugins

#### Problema: Carga de plugin falla

**Síntomas**:
```
Plugin load failed: Security check failed
```

**Solución**:
1. Verificar que el plugin no referencie espacios de nombres prohibidos (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Validar que el plugin solo referencie ensamblados en la lista blanca de ensamblados de confianza
3. Verificar que el plugin implemente correctamente la interfaz `IPlugin`
4. Revisar registros para detalles de la verificación de seguridad fallida

#### Problema: Herramientas de plugin no registradas

**Síntomas**:
- El plugin se carga correctamente pero las herramientas no aparecen en la lista

**Solución**:
1. Confirmar que las clases de herramientas en el plugin implementan correctamente la interfaz `ITool`
2. Verificar que las clases de herramientas sean public
3. Validar que `ToolManager.ScanAllPluginAssemblies()` sea llamado
4. Reconstruir el plugin y reiniciar la aplicación

---

### Problemas de Notas de Trabajo

#### Problema: No se pueden crear notas de trabajo

**Síntomas**:
```
Failed to create work note
```

**Solución**:
1. Verificar que el ser exista y esté en estado de ejecución
2. Validar que la ruta de almacenamiento tenga permisos de escritura
3. Comprobar que el contenido no esté vacío (contenido es obligatorio)
4. Revisar registros para información detallada del error

#### Problema: Búsqueda de notas sin resultados

**Síntomas**:
- Búsqueda por palabra clave devuelve resultados vacíos
- Pero se sabe que hay notas relevantes

**Solución**:
1. Verificar ortografía de la palabra clave
2. Probar con palabra clave más general
3. Validar que la nota contenga la palabra clave (sensible a mayúsculas/minúsculas)
4. Aumentar valor del parámetro `max_results`

#### Problema: Generación de directorio de notas lenta

**Síntomas**:
- Tiempo de respuesta largo al generar directorio
- El ser tiene muchas notas (>1000 páginas)

**Solución**:
1. Esto es normal, requiere recorrer todas las notas
2. Considerar archivar notas antiguas periódicamente
3. Usar función de búsqueda en lugar de navegación por directorio
4. Optimización planificada: añadir mecanismo de caché de directorio

---

### Problemas de Red de Conocimiento

#### Problema: Consulta de conocimiento devuelve resultados vacíos

**Síntomas**:
```
No knowledge triples found
```

**Solución**:
1. Verificar ortografía del sujeto y predicado
2. Comprobar que el conocimiento haya sido añadido a la red
3. Usar función de búsqueda para coincidencia difusa:
```json
{
  "action": "search",
  "query": "palabra_clave"
}
```

#### Problema: Búsqueda de ruta de conocimiento falla

**Síntomas**:
```
No path found between concepts
```

**Solución**:
1. Verificar que ambos conceptos existan en la red de conocimiento
2. Comprobar si existe una ruta de asociación (puede no haber relación directa o indirecta)
3. Probar añadiendo más conocimiento para establecer conexiones
4. Reducir límite de longitud de ruta (si está configurado)

#### Problema: Validación de conocimiento falla

**Síntomas**:
```
Knowledge validation failed
```

**Solución**:
1. Verificar que el formato de la tripleta sea correcto (sujeto, predicado, objeto son obligatorios)
2. Validar que la confianza esté en el rango 0.0-1.0
3. Comprobar si hay tripletas duplicadas
4. Ver detalles del error de validación para entender el problema específico

#### Problema: Estadísticas de red de conocimiento inexactas

**Síntomas**:
- Números estadísticos no coinciden con lo esperado
- Estadísticas no se actualizan después de añadir conocimiento

**Solución**:
1. Las estadísticas pueden tardar unos segundos en actualizarse (caché)
2. Comprobar si operaciones de eliminación no se ejecutaron correctamente
3. Reiniciar aplicación para forzar actualización de estadísticas
4. Volver a consultar estadísticas a través de API

---

### Problemas de Gestión de Proyectos

#### Problema: No se puede crear proyecto

**Síntomas**:
```
Failed to create project
```

**Solución**:
1. Verificar que el nombre del proyecto no esté vacío (obligatorio)
2. Validar que el nombre del proyecto no esté duplicado
3. Comprobar que la ruta de almacenamiento tenga permisos de escritura
4. Revisar registros para información detallada del error

#### Problema: Datos de proyecto perdidos

**Síntomas**:
- Información del proyecto no se puede cargar
- Archivos de proyecto corruptos

**Solución**:
1. Verificar que el directorio de almacenamiento del proyecto exista
2. Restaurar datos del proyecto desde respaldo
3. Validar que el formato del archivo JSON sea correcto
4. Reparar manualmente archivos de proyecto corruptos

---

## Depuración

### Habilitar Registro Detallado

Editar configuración:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Verificar Registros

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

### Usar Depurador

**SiliconLife.Default (implementación predeterminada)**:
```bash
# Ejecutar con depurador
dotnet run --project src/SiliconLife.Default --configuration Debug

# Adjuntar depurador
# A través de IDE: Adjuntar al proceso > SiliconLife.Default
```

**SiliconLife.Fast (versión de alto rendimiento)**:
```bash
# Ejecutar con depurador
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Adjuntar depurador
# A través de IDE: Adjuntar al proceso > SiliconLife.Fast
```

> **Recomendación**: Para la fase de desarrollo y depuración, se recomienda usar SiliconLife.Default; después de verificar la arquitectura, usar SiliconLife.Fast para despliegue en producción.

---

## Problemas de Rendimiento

### Tiempo de Respuesta Lento

**Optimización**:
1. Reducir complejidad del modelo de IA
2. Habilitar caché
3. Limpiar datos antiguos
4. Aumentar recursos del sistema

### Uso Alto de CPU

**Verificar**:
- Demasiados seres ejecutándose
- Bucle infinito en herramientas
- Ejecución frecuente de temporizadores

**Solución**:
- Reducir seres concurrentes
- Optimizar código de herramientas
- Ajustar intervalos de temporizadores

### Uso Alto de Memoria

**Monitorear**:
```bash
# A través de Web UI: Dashboard > Memoria
```

**Optimizar**:
- Limpiar memoria antigua
- Reducir tamaño de contexto
- Implementar paginación

---

## Obtener Ayuda

### Ver Documentación

- [Guía de Inicio Rápido](getting-started.md)
- [Guía de Desarrollo](development-guide.md)
- [Referencia de API](api-reference.md)
- [Guía de Arquitectura](architecture.md)

### Verificar Registros

Siempre verificar registros primero para detalles de errores.

### Soporte de la Comunidad

- GitHub Issues: Reportar bugs
- Discussions: Hacer preguntas
- Documentación: Buscar soluciones

---

## Procedimientos de Emergencia

### Colapso del Sistema

1. Verificar registros para causa
2. Reiniciar aplicación:

**SiliconLife.Default (implementación predeterminada)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (versión de producción principal)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Si es necesario, restaurar desde respaldo

### Pérdida de Datos

1. Detener aplicación inmediatamente
2. Verificar archivos de respaldo
3. Restaurar datos
4. Verificar integridad

### Vulnerabilidad de Seguridad

1. Detener todos los seres
2. Revocar todos los permisos
3. Verificar registros de auditoría
4. Revisar control de acceso
5. Reiniciar con permisos restringidos

---

## Prevención

### Mejores Prácticas

1. **Respaldos regulares**
   - Respaldar directorio de datos
   - Respaldar configuración
   - Probar proceso de restauración

2. **Monitorear recursos**
   - Vigilar uso de CPU/memoria
   - Monitorear espacio en disco
   - Verificar conexiones de red

3. **Mantener actualizado**
   - Actualizar .NET SDK
   - Actualizar dependencias
   - Aplicar parches de seguridad

4. **Probar cambios**
   - Probar primero en desarrollo
   - Usar control de versiones
   - Documentar cambios

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Ver la [Guía de Desarrollo](development-guide.md)
- 🚀 Ver la [Guía de Inicio Rápido](getting-started.md)
- 🔒 Ver la [Documentación de Seguridad](security.md)
