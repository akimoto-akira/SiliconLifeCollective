# Primeros Pasos

[English](getting-started.md) | [简体中文](docs/zh-CN/getting-started.md) | [繁體中文](docs/zh-HK/getting-started.md) | [Español](docs/es-ES/getting-started.md) | [日本語](docs/ja-JP/getting-started.md) | [한국어](docs/ko-KR/getting-started.md) | [Čeština](docs/cs-CZ/getting-started.md)

## Requisitos Previos

- **.NET 9 SDK** - [Descargar](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Descargar](https://git-scm.com/)
- **Ollama** (opcional, para IA local) - [Descargar](https://ollama.com/)
- **Clave API de DashScope** (opcional, para IA en la nube) - [Solicitar](https://bailian.console.aliyun.com/)

## Inicio Rápido

### 1. Clonar el Repositorio

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Compilar el Proyecto

```bash
dotnet build
```

### 3. Configurar Backend de IA

Edita `src/SiliconLife.Default/Config/DefaultConfigData.cs` o modifica la configuración en tiempo de ejecución a través de la interfaz web.

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

#### Opción B: DashScope (Nube)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "tu-clave-api-aqui",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. Ejecutar la Aplicación

```bash
dotnet run --project src/SiliconLife.Default
```

La aplicación se iniciará y abrirá automáticamente la interfaz web en tu navegador predeterminado.

## Primer Ser de Silicio

Después de iniciar la aplicación, puedes crear tu primer Ser de Silicio a través de la interfaz web:

1. Ve a `http://localhost:8080`
2. Haz clic en "Crear Ser" en el panel
3. Ingresa un nombre y personalidad
4. Haz clic en "Crear"

Alternativamente, usa la API:

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{"name": "Mi Asistente", "soul": "# Personalidad\nEres un asistente útil."}'
```

## Explorar el Sistema

### Panel de Control

El panel muestra:
- Estados de todos los Seres de Silicio
- Uso de memoria
- Estado del planificador
- Métricas de rendimiento

### Chat

Interactúa con los Seres de Silicio a través de la interfaz de chat:
- Selecciona un Ser del menú desplegable
- Escribe un mensaje
- Observa cómo piensa y responde

### Gestión de Código

Navega y edita el código del sistema en tiempo real:
- Navegador de código para explorar la estructura
- Editor de código con resaltado de sintaxis
- Edición del archivo de alma para personalizar el comportamiento

## Solución de Problemas

### Ollama no se está ejecutando

Si usas Ollama, asegúrate de que esté instalado y ejecutándose:

```bash
ollama list
ollama pull qwen2.5:7b
```

### Error de Clave API

Si usas DashScope, verifica que tu clave API sea válida y esté correctamente configurada.

### El Puerto 8080 está en uso

Cambia el puerto en la configuración:

```json
{
  "WebHost": {
    "Port": 8081
  }
}
```

## Próximos Pasos

- Lee la [Guía de Arquitectura](architecture.md) para entender el diseño del sistema
- Explora el [Sistema de Herramientas](tools-reference.md) para ver capacidades disponibles
- Revisa el [Sistema de Permisos](permission-system.md) para entender la seguridad
- Consulta la [Guía de Desarrollo](development-guide.md) para contribuir

## Recursos Adicionales

- [Documentación Completa](../es-ES/README.md)
- [Guía de Seres de Silicio](silicon-being-guide.md)
- [Referencia de API](api-reference.md)
- [Resolución de Problemas](troubleshooting.md)
