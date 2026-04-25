# Resolución de Problemas

[English](troubleshooting.md) | [简体中文](docs/zh-CN/troubleshooting.md) | [繁體中文](docs/zh-HK/troubleshooting.md) | [Español](docs/es-ES/troubleshooting.md) | [日本語](docs/ja-JP/troubleshooting.md) | [한국어](docs/ko-KR/troubleshooting.md) | [Čeština](docs/cs-CZ/troubleshooting.md)

## Problemas Comunes

---

## La Aplicación No Se Inicia

### Síntoma

Error al ejecutar `dotnet run`

### Soluciones

#### 1. Verificar .NET SDK

```bash
dotnet --version
```

Debe ser 9.0 o superior. Si no está instalado:
- Descargar desde [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/9.0)

#### 2. Restaurar Dependencias

```bash
dotnet restore
```

#### 3. Compilar Primero

```bash
dotnet build
```

#### 4. Verificar Puerto

Si el puerto 8080 está en uso:

```json
// config.json
{
  "WebHost": {
    "Port": 8081
  }
}
```

---

## Ollama No Funciona

### Síntoma

Error de conexión a Ollama

### Soluciones

#### 1. Verificar que Ollama Está Ejecutándose

```bash
ollama list
```

Si no está instalado, descargar desde [ollama.com](https://ollama.com/)

#### 2. Descargar Modelo

```bash
ollama pull qwen2.5:7b
```

#### 3. Verificar URL

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434"
    }
  }
}
```

---

## Error de Clave API DashScope

### Síntoma

Error de autenticación con DashScope

### Soluciones

#### 1. Verificar Clave API

Obtener clave desde [Bailian Console](https://bailian.console.aliyun.com/)

#### 2. Configurar Correctamente

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "tu-clave-api-aqui",
      "Model": "qwen-plus"
    }
  }
}
```

#### 3. Verificar Región

```json
{
  "Region": "cn-hangzhou"
}
```

---

## Ser No Responde

### Síntoma

El Ser de Silicio no responde a mensajes

### Soluciones

#### 1. Verificar Estado

Ir a Dashboard y verificar que el ser está "Ejecutando"

#### 2. Revisar Logs

```bash
# Ver logs en interfaz web
http://localhost:8080/logs
```

#### 3. Reiniciar Ser

1. Ir a `/beings`
2. Detener ser
3. Iniciar ser

#### 4. Verificar Archivo de Alma

```bash
# Verificar que soul.md existe
ls DataDirectory/SiliconManager/{being-guid}/soul.md
```

---

## Permisos Denegados

### Síntoma

Operaciones bloqueadas por permisos

### Soluciones

#### 1. Revisar Solicitud de Permiso

Ir a `/permission-requests` y aprobar/denegar

#### 2. Verificar ACL

```bash
# Ver configuración de permisos
http://localhost:8080/permissions
```

#### 3. Configurar como Curador

Si el ser debería tener acceso total:

```csharp
being.IsCurator = true;
```

---

## Error de Compilación Dinámica

### Síntoma

Error al ejecutar código dinámico

### Soluciones

#### 1. Verificar Código

Asegurar que el código C# es válido:

```csharp
public class Program 
{
    public static string Run() 
    {
        return "Hola";
    }
}
```

#### 2. Revisar Escaneo de Seguridad

El código puede ser bloqueado por SecurityScanner. Ver logs para detalles.

#### 3. Verificar Dependencias

Solo se permiten ciertas bibliotecas en código dinámico.

---

## Memoria Llena

### Síntoma

Alto uso de memoria

### Soluciones

#### 1. Limpiar Memoria Antigua

Usar MemoryTool para eliminar datos antiguos:

```csharp
memoryTool.Execute("Delete", new { key = "old_data" });
```

#### 2. Configurar Límites

```json
{
  "Memory": {
    "MaxMessages": 50,
    "MaxAgeDays": 30
  }
}
```

#### 3. Reiniciar Aplicación

```bash
# Detener y reiniciar
dotnet run --project src/SiliconLife.Default
```

---

## Chat No Muestra Mensajes

### Síntoma

La interfaz de chat no muestra mensajes

### Soluciones

#### 1. Verificar SSE

Abrir consola del navegador y verificar conexión SSE:

```javascript
console.log(EventSource);
```

#### 2. Recargar Página

Presionar F5 para recargar

#### 3. Verificar Canal

Asegurar que el canal existe:

```bash
GET /api/channels
```

---

## Error de Localización

### Síntoma

Textos no se muestran en idioma correcto

### Soluciones

#### 1. Verificar Configuración

```json
{
  "Language": "EsES"
}
```

#### 2. Verificar Implementación

Asegurar que la clase de localización existe:

```bash
# Verificar archivo
src/SiliconLife.Default/Localization/EsES.cs
```

---

## Problemas de Rendimiento

### Síntoma

Aplicación lenta

### Soluciones

#### 1. Monitorear Métricas

Ir a Dashboard para ver:
- Uso de CPU
- Uso de memoria
- Tiempos de respuesta

#### 2. Optimizar Seres

- Reducir número de seres activos
- Aumentar intervalos de tick
- Limitar historial de chat

#### 3. Configurar Circuit Breaker

```json
{
  "CircuitBreaker": {
    "MaxFailures": 3,
    "CooldownMinutes": 1
  }
}
```

---

## Logs Útiles

### Ubicación

```
data/logs/
```

### Niveles de Log

- Trace: Máximo detalle
- Debug: Depuración
- Information: Info general
- Warning: Advertencias
- Error: Errores
- Critical: Errores críticos

### Ver Logs en Tiempo Real

```bash
http://localhost:8080/logs
```

---

## Obtener Ayuda

### Documentación

- [Documentación Principal](../es-ES/README.md)
- [Guía de Arquitectura](architecture.md)
- [Guía de Desarrollo](development-guide.md)

### Reportar Bugs

GitHub Issues: [github.com/akimoto-akira/SiliconLifeCollective/issues](https://github.com/akimoto-akira/SiliconLifeCollective/issues)

### Comunidad

- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)
