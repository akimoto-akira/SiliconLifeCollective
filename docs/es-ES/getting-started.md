# Inicio Rápido

> **Versión: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | **Español** | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md)

## Elegir Versión

Este proyecto proporciona dos versiones de implementación:

### SiliconLife.Default (Versión por defecto)
- **Posicionamiento**: Implementación por defecto, principalmente para verificar la viabilidad de la arquitectura
- **Modo de ejecución**: Aplicación de consola
- **Método de almacenamiento**: Almacenamiento JSON en sistema de archivos
- **Escenarios aplicables**: Seguridad de datos prioritaria, volúmenes de datos pequeños, depuración de desarrollo, verificación de arquitectura
- **Soporte de plataforma**: Windows, Linux, macOS
- **Descripción del rol**: Como implementación de referencia para verificación de arquitectura, proporciona una forma de ejecución simple y fiable, adecuada para primeros contactos con este proyecto o para depuración de desarrollo

### SiliconLife.Fast (Versión de alto rendimiento)
- **Posicionamiento**: Versión de producción recomendada
- **Modo de ejecución**: Aplicación de escritorio (bandeja del sistema en Windows/macOS / ventana de estado en Linux)
- **Método de almacenamiento**: Almacenamiento en memoria SpeedyPack + persistencia asíncrona (formato de archivo .spk)
- **Escenarios aplicables**: Alta concurrencia, baja latencia, grandes volúmenes de datos, ejecución de producción a largo plazo
- **Soporte de plataforma**: Windows/macOS (funcionalidad completa, incluyendo bandeja del sistema), Linux (ventana de estado, sin icono en bandeja)
- **Descripción del rol**: Implementación de grado de producción profundamente optimizada, es la opción preferida para ejecución a largo plazo y entornos de producción reales

> **Recomendación para principiantes**: Para el primer uso, se recomienda comenzar con **SiliconLife.Default** para verificar rápidamente la viabilidad de la arquitectura; una vez familiarizado con el sistema, se recomienda encarecidamente migrar a **SiliconLife.Fast** como versión de ejecución para el entorno de producción.

## Requisitos Previos

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

### 3. Configurar el Backend de IA

Edite `src/SiliconLife.Default/Config/DefaultConfigData.cs` o modifique la configuración en tiempo de ejecución a través de la Web UI.

#### Opción A: Ollama (local)

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

#### Opción B: Bailian (nube)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **Regiones disponibles**: `beijing` (Pekín), `virginia` (Virginia), `singapore` (Singapur), `hongkong` (Hong Kong), `frankfurt` (Fráncfort)

#### Opción C: Volcengine Ark (nube)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Nota**: El parámetro Model de Volcengine Ark acepta un ID de punto de acceso de inferencia (por ejemplo, `ep-20241212123456-abcde`), no un nombre de modelo.

### 4. Ejecutar la Aplicación

#### Ejecutar la versión Default

```bash
cd src/SiliconLife.Default
dotnet run
```

El servidor Web se iniciará en `http://localhost:8080`

#### Ejecutar la versión Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: La aplicación se iniciará en modo ventana, se minimizará a la bandeja del sistema, y el servidor Web también se iniciará en `http://localhost:8080`

**Linux**: La aplicación mostrará una ventana de estado (sin icono en la bandeja del sistema) y abrirá automáticamente el navegador para acceder a la Web UI. También se puede usar el parámetro `--no-tray` para omitir la apertura automática del navegador:

```bash
dotnet run -- --no-tray
```

### 5. Acceder a la Web UI

Abra el navegador y navegue a:

```
http://localhost:8080
```

Verá un panel que incluye:
- Gestión de Seres de Silicio
- Interfaz de chat
- Panel de configuración
- Monitoreo del sistema

## Primer Ser de Silicio

### Crear su Primer Ser

1. En la Web UI, navegue a **Gestión de Seres**
2. Haga clic en **Crear Nuevo Ser**
3. Configure el Archivo de Alma (`soul.md`) con personalidad y comportamiento
4. Inicie el ser

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

### Conexión a Ollama rechazada

**Problema**: No se puede conectar a Ollama en `http://localhost:11434`

**Solución**:
```bash
# Verificar si Ollama está ejecutándose
ollama list

# Iniciar Ollama si es necesario
ollama serve
```

### Modelo no encontrado

**Problema**: `model "qwen2.5:7b" not found`

**Solución**:
```bash
# Extraer el modelo requerido
ollama pull qwen2.5:7b
```

### Puerto ya en uso

**Problema**: `HttpListenerException: Address already in use`

**Solución**:
- Cambie el puerto en la configuración
- O termine el proceso que usa el puerto 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Siguientes Pasos

- 📚 Lea la [guía de arquitectura](architecture.md) para comprender el diseño del sistema
- 🛠️ Consulte la [guía de desarrollo](development-guide.md) para extender el sistema
- 📖 Explore la [referencia de API](api-reference.md) para detalles de integración
- 🔒 Consulte la [documentación de seguridad](security.md) para el sistema de permisos
- 🧰 Consulte la [referencia de herramientas](tools-reference.md) para todas las herramientas integradas
- 🌐 Consulte la [guía de Web UI](web-ui-guide.md) para las funciones de la interfaz

## Estructura del Proyecto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces y clases abstractas principales
│   ├── SiliconLife.Common/          # Implementaciones compartidas (usadas por ambas versiones)
│   ├── SiliconLife.App/             # Capa de aplicación compartida por Default y Fast
│   ├── SiliconLife.Default/         # Implementación por defecto + punto de entrada (versión de consola)
│   ├── SiliconLife.Fast/            # Implementación de alto rendimiento + punto de entrada (versión de escritorio)
│   ├── SiliconLife.Speedy/          # Motor de almacenamiento de alto rendimiento SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Herramienta de gestión SpeedyPack (Avalonia UI)
├── docs/                            # Documentación (multilingüe, 34 variantes de idioma)
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

## ¿Necesita Ayuda?

- 📖 Consulte el [sistema de documentación de ayuda](web-ui-guide.md#sistema-de-documentacion-de-ayuda-nuevo) (soporte multilingüe)
- 📚 Lea la [documentación completa](docs/)
- 🐛 Reporte problemas en [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Participe en las discusiones de la comunidad
