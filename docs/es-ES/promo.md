# Promoción

[English](promo.md) | [简体中文](docs/zh-CN/promo.md) | [繁體中文](docs/zh-HK/promo.md) | [Español](docs/es-ES/promo.md) | [日本語](docs/ja-JP/promo.md) | [한국어](docs/ko-KR/promo.md) | [Čeština](docs/cs-CZ/promo.md)

## Silicon Life Collective

**Una plataforma de colaboración multiagente donde los agentes de IA se autoevolucionan**

---

## ¿Qué es Silicon Life Collective?

Silicon Life Collective es una plataforma innovadora de colaboración multiagente construida en .NET 9, donde los agentes de IA llamados **Seres de Silicio** pueden autoevolucionarse mediante compilación dinámica Roslyn.

### Características Principales

✨ **Arquitectura Cuerpo-Cerebro** - Seres de Silicio con cuerpo que detecta eventos y cerebro que procesa IA

🔄 **Compilación Dinámica** - Los seres pueden generar y ejecutar código C# dinámicamente

🛡️ **Seguridad de 5 Niveles** - Cadena de permisos granular para todas las operaciones

🌍 **Localización Completa** - Soporte para 20 variantes de idioma incluyendo español

📅 **32 Calendarios** - Soporte multi-calendario desde Gregoriano hasta Maya

💬 **Interfaz Web Completa** - 18 controladores, 4 pieles intercambiables, actualizaciones en tiempo real

---

## Demo en Vivo

### Iniciar la Aplicación

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
dotnet run --project src/SiliconLife.Default
```

La interfaz web se abrirá automáticamente en `http://localhost:8080`

### Primeros Pasos

1. **Crear un Ser de Silicio** con personalidad personalizada
2. **Chatear** con el ser y ver cómo responde
3. **Observar** cómo usa herramientas para completar tareas
4. **Configurar** permisos y seguridad
5. **Explorar** las múltiples funcionalidades

---

## Casos de Uso

### Asistente Personal

Crea un asistente personalizado con tu propio archivo de alma:

```markdown
# Asistente Personal

Eres mi asistente personal experto en productividad.

## Capacidades
- Gestionar tareas
- Enviar recordatorios
- Investigar temas
- Resumir documentos
```

### Equipo Multiagente

Configura múltiples seres trabajando juntos:

- **Curador**: Gestiona y coordina
- **Investigador**: Busca información
- **Escritor**: Genera contenido
- **Programador**: Escribe código

### Herramientas Personalizadas

Extiende las capacidades con herramientas propias:

```csharp
public class MyTool : ITool
{
    public string Name => "MiHerramienta";
    public ToolResult Execute(string method, object parameters)
    {
        // Tu lógica aquí
    }
}
```

---

## Tecnología

| Componente | Tecnología |
|-----------|-----------|
| Runtime | .NET 9 |
| Lenguaje | C# |
| IA | Ollama, DashScope |
| Compilación | Roslyn |
| Web | HttpListener + SSE |
| Almacenamiento | JSON (file-based) |

---

## Por Qué Silicon Life Collective

### 🚀 Autoevolución

Los seres pueden mejorar su propio código mediante compilación dinámica Roslyn.

### 🔒 Seguridad Primero

Todas las operaciones pasan por una cadena de permisos de 5 niveles.

### 🌐 Multi-idioma

20 variantes de idioma con localización completa.

### 📦 Sin Dependencias

- Sin base de datos requerida
- Sin framework web pesado
- Solo .NET 9 y Roslyn

### 🎨 Personalizable

- Pieles intercambiables
- Herramientas personalizadas
- Calendarios extensibles
- Configuración flexible

---

## Comunidad

### Autor

**Hoshino Kennji**

- GitHub: [akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

### Contribuir

¡Las contribuciones son bienvenidas!

1. Fork el repositorio
2. Crear rama de característica
3. Enviar pull request

Ver [Guía de Contribución](contributing.md) para detalles.

---

## Licencia

**Apache License 2.0**

Uso comercial permitido
Modificación permitida
Distribución permitida
Uso de patente permitido

---

## Recursos

### Documentación

- [Documentación Principal](../es-ES/README.md)
- [Guía de Arquitectura](architecture.md)
- [Primeros Pasos](getting-started.md)
- [Referencia de API](api-reference.md)
- [Guía de Desarrollo](development-guide.md)

### Guías

- [Guía de Seres de Silicio](silicon-being-guide.md)
- [Sistema de Calendario](calendar-system.md)
- [Sistema de Permisos](permission-system.md)
- [Interfaz Web](web-ui-guide.md)

### Referencia

- [Herramientas](tools-reference.md)
- [Seguridad](security.md)
- [Resolución de Problemas](troubleshooting.md)
- [Registro de Cambios](changelog.md)

---

## Hoja de Ruta

### Completado ✅

- Fases 1-10.5
- Chat básico hasta interfaz web completa
- 32 calendarios, 20 idiomas
- Compilación dinámica

### Planificado 🔜

- **Fase 11**: Integración con IM externos (Feishu, WhatsApp, Telegram)
- **Fase 12**: Gráfico de conocimiento, sistema de plugins, ecosistema de habilidades

---

## Únete a Nosotros

**GitHub**: [Star](https://github.com/akimoto-akira/SiliconLifeCollective) el repositorio

**YouTube**: Suscríbete para tutoriales y demos

**Bilibili**: Videos en chino

**Contribuye**: Envía pull requests y reporta issues

---

**Silicon Life Collective** - Donde la IA cobra vida
