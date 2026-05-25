# Inicio Rápido

> **Versión: v0.2.0-alpha**

[English](../en/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | **Español** | [Deutsch](../de-DE/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md)

## Elegir Versión

Este proyecto proporciona dos versiones de implementación:

### SiliconLife.Default (Versión Predeterminada)
- **Posicionamiento**: Implementación predeterminada, utilizada principalmente para verificación de viabilidad de arquitectura
- **Modo de Ejecución**: Aplicación de consola
- **Almacenamiento**: Almacenamiento JSON del sistema de archivos
- **Escenario de Aplicación**: Prioridad de seguridad de datos, bajo volumen de datos, depuración de desarrollo, verificación de arquitectura
- **Soporte de Plataforma**: Windows, Linux, macOS
- **Descripción de Rol**: Implementación de referencia para verificación de arquitectura que proporciona una ejecución simple y confiable, adecuada para primer contacto o depuración de desarrollo

### SiliconLife.Fast (Versión de Alto Rendimiento)
- **Posicionamiento**: Versión de producción principal
- **Modo de Ejecución**: Aplicación de escritorio (Windows/macOS bandeja del sistema / Linux ventana de estado)
- **Almacenamiento**: Almacenamiento en memoria SpeedyPack + persistencia asíncrona (formato de archivo .spk)
- **Escenario de Aplicación**: Alta concurrencia, baja latencia, gran volumen de datos, operaciones de producción a largo plazo
- **Soporte de Plataforma**: Windows/macOS (funciones completas, incluyendo bandeja del sistema), Linux (ventana de estado, sin icono en bandeja)
- **Descripción de Rol**: Implementación de nivel de producción con optimización profunda, la mejor opción para operaciones a largo plazo y entornos de producción reales

> **Sugerencia para Principiantes**: Se recomienda que los usuarios primerizos comiencen con **SiliconLife.Default** para verificar rápidamente la viabilidad de la arquitectura. Después de familiarizarse con el sistema, se recomienda encarecidamente migrar a **SiliconLife.Fast**.

## Prerrequisitos

- **.NET 9 SDK** - [Descargar](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Descargar](https://git-scm.com/)
- **Ollama** (opcional, para IA local) - [Descargar](https://ollama.com/)
- **Clave API de Bailian** (opcional, para IA en la nube) - [Solicitar](https://bailian.console.aliyun.com/)
- **Clave API de Volcengine Ark** (opcional, para IA en la nube) - [Solicitar](https://console.volcengine.com/ark)

## Inicio Rápido

### 1. Clonar el Repositorio

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Construir el Proyecto

```bash
dotnet build
```

### 3. Configurar Backend de IA

Editar `src/SiliconLife.Default/Config/DefaultConfigData.cs` o modificar la configuración en tiempo de ejecución a través de la Web UI.

#### Opción A: Ollama (Local)

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### Opción B: Bailian (Nube)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "tu-clave-api-aqui",
      "Model": "qwen3.6-plus",
      "Region": "beijing"
    }
  }
}
```

#### Opción C: Volcengine Ark (Nube)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "tu-clave-api-aqui",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Nota**: El parámetro Model de Volcengine Ark acepta un ID de endpoint de inferencia (ej. `ep-20241212123456-abcde`), no un nombre de modelo.

### 4. Ejecutar la Aplicación

#### Ejecutar Versión Default

```bash
cd src/SiliconLife.Default
dotnet run
```

El servidor web se iniciará en `http://localhost:8080`

#### Ejecutar Versión Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: La aplicación se iniciará en modo formulario, minimizada en la bandeja del sistema, con el servidor web también iniciándose en `http://localhost:8080`

**Linux**: La aplicación mostrará una ventana de estado (sin icono en la bandeja del sistema) y abrirá automáticamente el navegador para acceder a la Web UI. Puede usar el parámetro `--no-tray` para omitir la apertura automática del navegador:

```bash
dotnet run -- --no-tray
```

### 5. Acceder a la Web UI

Abrir un navegador y navegar a:

```
http://localhost:8080
```

Verá un panel de control que incluye:
- Gestión de Seres Silicona
- Interfaz de chat
- Panel de configuración
- Monitoreo del sistema

## Tu Primer Ser Silicona

### Crear Tu Primer Ser

1. Navegar a **Gestión de Seres** en la Web UI
2. Hacer clic en **Crear Nuevo Ser**
3. Configurar el archivo de alma (`soul.md`) con personalidad y comportamiento
4. Iniciar el ser

### Ejemplo de soul.md

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## Preguntas Frecuentes

### Conexión a Ollama Rechazada

**Problema**: No se puede conectar a Ollama en `http://localhost:11434`

**Solución**:
```bash
# Verificar que Ollama esté ejecutándose
ollama list

# Para iniciar Ollama
ollama serve
```

### Modelo No Encontrado

**Problema**: `model "qwen2.5:7b" not found`

**Solución**:
```bash
# Obtener el modelo requerido
ollama pull qwen2.5:7b
```

### Puerto en Uso

**Problema**: `HttpListenerException: Address already in use`

**Solución**:
- Cambiar el puerto en la configuración
- O terminar el proceso que usa el puerto 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md) para entender el diseño del sistema
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md) para extender el sistema
- 📖 Explorar la [Referencia de API](api-reference.md) para detalles de integración
- 🔒 Ver la [Documentación de Seguridad](security.md) para el sistema de permisos
- 🧰 Consultar la [Referencia de Herramientas](tools-reference.md) para todas las herramientas integradas
- 🌐 Ver la [Guía de Web UI](web-ui-guide.md) para funciones de la interfaz

## Estructura del Proyecto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces centrales y clases abstractas
│   ├── SiliconLife.Common/          # Implementación compartida (común a ambas versiones)
│   ├── SiliconLife.App/             # Capa de aplicación compartida entre Default y Fast
│   ├── SiliconLife.Default/         # Implementación predeterminada + punto de entrada (versión consola)
│   ├── SiliconLife.Fast/            # Implementación de alto rendimiento + punto de entrada (versión formularios)
│   ├── SiliconLife.Speedy/          # Motor de almacenamiento de alto rendimiento SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Herramienta de gestión SpeedyPack (Windows Forms)
├── docs/                            # Documentación (multilingüe, 29 variantes de idioma)
│   ├── en/                          # Inglés
│   ├── zh-CN/                       # Chino simplificado
│   ├── zh-HK/                       # Chino tradicional
│   ├── es-ES/                       # Español
│   ├── ja-JP/                       # Japonés
│   ├── ko-KR/                       # Coreano
│   └── cs-CZ/                       # Checo
├── 总文档/                           # Documentos de requisitos y arquitectura (chino)
└── README.md                        # Descripción del proyecto
```

## ¿Necesitas Ayuda?

- 📖 Consultar el [Sistema de Documentos de Ayuda](web-ui-guide.md#帮助文档系统新增) (soporte multilingüe)
- 📚 Leer la [Documentación Completa](docs/)
- 🐛 Reportar problemas en [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Participar en discusiones de la comunidad
