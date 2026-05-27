# Demo de Plugin Mínimo

Una implementación mínima de `IPlugin` que demuestra el ciclo de vida del plugin con valores codificados.

## Resumen de la interfaz IPlugin

Cada plugin de SiliconLife debe implementar la interfaz `IPlugin` definida en `SiliconLife.Collective`：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Resumen de propiedades

| Miembro | Tipo | Descripción |
|---------|------|-------------|
| `Id` | `string` | Identificador único, debe ser estable entre versiones (ej：`"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | Nombre para mostrar legible, localizado por la enumeración `Language` |
| `Version` | `string` | Cadena de versión semántica (ej：`"1.0.0"`) |
| `GetDescription(Language)` | `string` | Breve descripción de la funcionalidad del plugin |
| `GetAuthor(Language)` | `string` | Nombre del autor u organización |

## Orden de llamada del ciclo de vida

El host llama a los métodos del ciclo de vida en un orden estricto：

```
OnLoad → OnStart → [En ejecución] → OnStop → OnUnload
```

| Método | Cuándo se llama | Uso típico |
|--------|----------------|------------|
| `OnLoad()` | Una vez, cuando la DLL del plugin se carga en el host | Validar configuración, registrar tipos, preparar recursos |
| `OnStart()` | Cuando el host se ha iniciado completamente y todos los plugins están cargados | Interactuar con otros plugins, iniciar tareas en segundo plano |
| `OnStop()` | Cuando el host se cierra correctamente | Liberar recursos, vaciar búferes, guardar estado |
| `OnUnload()` | Cuando el plugin se descarga del proceso del host | Limpieza final |

## Esta demo

Este plugin devuelve valores codificados para todas las propiedades y deja los métodos del ciclo de vida vacíos. Es el punto de partida más simple para el desarrollo de plugins.

## Nota de seguridad

Los plugins se cargan en un `AssemblyLoadContext` aislado y se analizan en busca de referencias a espacios de nombres prohibidos (ej：`System.IO`, `System.Net.Http`). Consulte la[documentación de seguridad](../../docs/es-ES/security.md) para más detalles.
