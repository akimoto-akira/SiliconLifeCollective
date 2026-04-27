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
- Cambiar puerto en configuración
- O terminar proceso que usa puerto 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

#### Problema: Web UI no carga

**Síntomas**:
- Navegador muestra error de conexión
- Página en blanco

**Solución**:
1. Verificar que la aplicación se esté ejecutando
2. Comprobar que el puerto 8080 sea accesible
3. Verificar que no haya errores en registros del servidor
4. Intentar limpiar caché del navegador

---

### Problemas de Permisos

#### Problema: Permisos denegados inesperadamente

**Síntomas**:
- Operaciones bloqueadas sin razón aparente
- Mensajes de "Permiso denegado"

**Solución**:
1. Verificar estado de IsCurator del usuario
2. Comprobar configuración de límites de velocidad
3. Revisar reglas de GlobalACL
4. Verificar lógica de callback
5. Comprobar timeout de respuesta de usuario

#### Problema: Solicitudes de permisos excesivas

**Síntomas**:
- Preguntas frecuentes de permisos para mismas operaciones

**Solución**:
- Usar caché de frecuencia de usuario
- Añadir reglas a GlobalACL para recursos frecuentes
- Implementar callback personalizado para lógica inteligente

---

### Problemas de Ser Silicona

#### Problema: Ser no se inicia

**Síntomas**:
- Estado permanece en "Created" o "Starting"
- Errores en registros

**Solución**:
1. Verificar que archivo de alma existe y es válido
2. Comprobar que configuración sea correcta
3. Revisar registros para errores específicos
4. Verificar que backend de IA sea accesible

#### Problema: Ser se comporta inesperadamente

**Síntomas**:
- Respuestas incorrectas
- No ejecuta herramientas correctamente

**Solución**:
1. Revisar archivo de alma - ¿define comportamiento claramente?
2. Verificar que herramientas estén disponibles
3. Comprobar que permisos estén configurados correctamente
4. Revisar historial de chat para contexto

---

## Registro y Diagnóstico

### Habilitar Registro Detallado

```csharp
config.Logging.Level = LogLevel.Debug;
```

### Ubicaciones de Registro

- **Registros del sistema**: `data/logs/`
- **Registros de ser**: `data/beings/{being-id}/logs/`
- **Registros de auditoría**: `data/audit/`

### Analizar Registros

Buscar patrones:
- Errores repetitivos
- Advertencias de rendimiento
- Fallos de permisos

---

## Problemas Comunes y Soluciones

### Ollama Lento

**Causa**: Modelo demasiado grande o hardware insuficiente

**Soluciones**:
- Usar modelo más pequeño (ej. `qwen2.5:1.5b`)
- Aumentar recursos del sistema
- Considerar usar servicio en la nube

### Consumo Excesivo de Tokens

**Causa**: Prompts ineficientes o bucles de herramientas

**Soluciones**:
- Optimizar archivo de alma
- Establecer límites de tokens en configuración
- Monitorear uso a través de panel de auditoría

### Almacenamiento Lleno

**Causa**: Historial de chat y registros acumulan

**Soluciones**:
- Implementar estrategia de limpieza
- Archivar datos antiguos
- Aumentar espacio en disco

---

## Herramientas de Diagnóstico

### Verificar Estado del Sistema

```bash
curl http://localhost:8080/api/dashboard
```

### Verificar Estado de Ser

```bash
curl http://localhost:8080/api/beings/{being-id}
```

### Verificar Permisos

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

---

## Contactar Soporte

Si el problema persiste:

1. 📚 Leer [documentación completa](docs/)
2. 🐛 Reportar problema en [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
3. 💬 Participar en discusiones de comunidad
4. 📧 Contactar al maintainer

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🔒 Ver la [Documentación de Seguridad](security.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
