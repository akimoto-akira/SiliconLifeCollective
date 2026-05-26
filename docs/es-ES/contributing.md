# Guía de Contribución

> **Versión: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | **Español** | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

¡Gracias por su interés en contribuir a SiliconLifeCollective!

## Contribución a Doble Versión

Este proyecto tiene dos versiones de implementación, puede elegir la dirección de contribución según sus intereses:

### SiliconLife.Default (Versión por defecto)
- **Stack tecnológico**: Aplicación de consola .NET 9
- **Dirección de contribución**: Desarrollo de funcionalidades principales, implementación de herramientas, localización, documentación
- **Adecuado para**: Todos los desarrolladores

### SiliconLife.Fast (Versión de alto rendimiento)
- **Stack tecnológico**: Aplicación de escritorio multiplataforma .NET 9 (Avalonia UI)
- **Dirección de contribución**: Optimización de rendimiento, almacenamiento SpeedyPack, bandeja del sistema, concurrencia sin bloqueos
- **Adecuado para**: Desarrolladores con experiencia en desarrollo de escritorio e interés en optimización de rendimiento

> **Nota importante**: Ambas versiones comparten los proyectos SiliconLife.Core y SiliconLife.Common, las mejoras a las interfaces principales afectarán a ambas versiones simultáneamente.

## Código de Conducta

Este proyecto sigue la licencia Apache 2.0. Mantenga el respeto y la profesionalidad en todas las interacciones.

---

## Inicio Rápido

### 1. Hacer Fork del repositorio

Haga clic en el botón "Fork" en GitHub para crear su propia copia.

### 2. Clonar su Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Configurar el entorno de desarrollo

```bash
# Instalar .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Restaurar dependencias
dotnet restore

# Construir el proyecto
dotnet build

# Ejecutar pruebas
dotnet test
```

### 4. Crear una rama de funcionalidad

```bash
git checkout -b feature/your-feature-name
```

### 5. Elegir el proyecto de desarrollo

Según su tipo de contribución, elija el proyecto apropiado:

- **Interfaces principales/clases abstractas** → Modificar `SiliconLife.Core`
- **Implementaciones compartidas** → Modificar `SiliconLife.Common`
- **Específico de la versión Default** → Modificar `SiliconLife.Default`
- **Específico de la versión Fast** → Modificar `SiliconLife.Fast`
- **Motor de almacenamiento** → Modificar `SiliconLife.Speedy`
- **Herramienta de gestión de almacenamiento** → Modificar `SiliconLife.Speedy.Manager`
- **Desarrollo de plugins** → Modificar `SiliconLife.Core/Plugins`
- **Documentación multilingüe** → Modificar el directorio `docs/`

---

## Flujo de Trabajo de Desarrollo

### Estilo de Código

- Seguir las convenciones de codificación de C#
- Nombres de clases en PascalCase
- Parámetros de métodos en camelCase
- Campos privados en `_camelCase`
- Todas las APIs públicas deben tener documentación XML

### Mensajes de Confirmación

Seguir el formato de **Commits Convencionales**:

```
<type>(<scope>): <description>
```

**Tipos**:
- `feat`: Nueva funcionalidad
- `fix`: Corrección de bug
- `docs`: Cambios en documentación
- `style`: Formato de código
- `refactor`: Refactorización de código
- `test`: Cambios en pruebas
- `chore`: Cambios de construcción/herramientas

**Ejemplos**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Realizar Cambios

1. **Escribir código**
   - Seguir los patrones existentes
   - Agregar pruebas para nuevas funcionalidades
   - Actualizar la documentación

2. **Probar sus cambios**
   ```bash
   # Ejecutar todas las pruebas
   dotnet test
   
   # Construir en modo release
   dotnet build --configuration Release
   ```

3. **Formatear el código**
   ```bash
   dotnet format
   ```

4. **Confirmar los cambios**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **Empujar a su Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Crear un Pull Request**
   - Ir al repositorio original
   - Hacer clic en "Compare & pull request"
   - Completar la plantilla de PR
   - Enviar

---

## Guía de Pull Requests

### Título del PR

Usar el mismo formato que los mensajes de confirmación:
```
feat(localization): add Korean language support
```

### Descripción del PR

Incluir:

1. **Qué** - ¿Qué hace este PR?
2. **Por qué** - ¿Por qué se necesita este cambio?
3. **Cómo** - ¿Cómo lo implementó?
4. **Pruebas** - ¿Cómo se probó?

### Ejemplo de Descripción de PR

```markdown
## Qué
Agregar localización en coreano para todos los componentes UI y documentación.

## Por qué
Ampliar la accesibilidad del proyecto para usuarios de coreano.

## Cómo
- Crear archivo de localización KoKR.cs
- Agregar 500+ claves de traducción
- Actualizar todas las vistas para usar localización
- Crear documentación en coreano en docs/ko-KR/

## Pruebas
- Verificar que todos los elementos UI muestren coreano correctamente
- Probar la funcionalidad de cambio de idioma
- Revisar traducciones con hablantes nativos
```

---

## Tipos de Contribución

### 1. Corrección de Bugs

**Proceso**:
1. Verificar issues existentes
2. Crear un issue si no existe
3. Corregir el bug
4. Agregar casos de prueba
5. Enviar PR

**Requisitos**:
- Descripción clara del bug
- Pasos para reproducir
- Pruebas para prevenir regresiones

### 2. Nuevas Funcionalidades

**Proceso**:
1. Discutir la funcionalidad en Issues/Discussions
2. Obtener aprobación del mantenedor
3. Implementar la funcionalidad
4. Agregar pruebas exhaustivas
5. Actualizar la documentación
6. Enviar PR

**Requisitos**:
- Propuesta de funcionalidad aprobada
- Cobertura completa de pruebas
- Documentación actualizada
- Compatible hacia atrás

### 3. Documentación

**Proceso**:
1. Identificar vacíos en la documentación
2. Escribir/actualizar documentación
3. Enviar PR

**Requisitos**:
- Clara y concisa
- Incluir ejemplos
- Soporte multilingüe si aplica

### 4. Refactorización de Código

**Proceso**:
1. Proponer la refactorización en un Issue
2. Obtener aprobación
3. Refactorizar el código
4. Asegurar que todas las pruebas pasen
5. Enviar PR

**Requisitos**:
- Sin cambios de funcionalidad
- Todas las pruebas pasan
- Mejora la calidad del código
- Explicación clara

---

## Guía de Pruebas

### Pruebas Unitarias

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Preparar
    var service = new MyService();
    
    // Ejecutar
    var result = service.DoSomething();
    
    // Verificar
    Assert.IsTrue(result.Success);
}
```

### Pruebas de Integración

Probar flujos de trabajo completos:
- Interacción con IA
- Ejecución de herramientas
- Verificación de permisos
- Operaciones de almacenamiento

### Pruebas Manuales

Para cambios en UI:
- Probar en múltiples navegadores
- Verificar diseño responsivo
- Comprobar accesibilidad

---

## Guía de Documentación

### Comentarios de Código

- Usar comentarios XML para todas las APIs públicas
- Usar comentarios en línea para lógica compleja
- Los comentarios de código deben estar en inglés

### Archivos de Documentación

- Colocar en `docs/{language}/`
- Actualizar todas las versiones de idioma
- Seguir la estructura existente

### Documentación Multilingüe

Al agregar documentación:
1. Crear primero la versión en inglés
2. Traducir a otros idiomas
3. Mantener el contenido sincronizado

---

## Proceso de Revisión

### Qué revisan los mantenedores

1. **Calidad del código**
   - Sigue las convenciones
   - Claro y legible
   - Bien documentado

2. **Pruebas**
   - Cobertura adecuada
   - Todas las pruebas pasan
   - Cubre casos límite

3. **Documentación**
   - Actualizada
   - Explicaciones claras
   - Multilingüe

4. **Compatibilidad**
   - Compatible hacia atrás
   - Sin cambios disruptivos (a menos que se notifique)
   - Sigue versionado semántico

### Cronología de Revisión

- Revisión inicial: 1-3 días
- Integración de comentarios: según sea necesario
- Fusión: tras aprobación

---

## Preguntas Frecuentes

### PR Rechazado

**Causas**:
- No sigue las guías
- Pruebas insuficientes
- Cambios disruptivos no notificados
- Calidad de código deficiente

**Solución**:
- Resolver los comentarios
- Actualizar el PR
- Volver a enviar

### Conflictos de Fusión

**Solución**:
```bash
# Actualizar su rama
git fetch origin
git rebase origin/master

# Resolver conflictos
# Editar archivos en conflicto
git add .
git rebase --continue

# Empujar con fuerza
git push --force-with-lease
```

---

## Obtener Ayuda

### Recursos

- **Documentación**: [docs/](../)
- **Problemas**: GitHub Issues
- **Discusiones**: GitHub Discussions
- **Código de Conducta**: CODE_OF_CONDUCT.md

### Contacto

- Crear un Issue para bugs
- Iniciar una Discussion para preguntas
- Mencionar a los mantenedores para asuntos urgentes

---

## Agradecimientos

Los contribuyentes serán reconocidos en:
- La sección de contribuyentes del README.md
- Las notas de versión
- La documentación del proyecto

---

## Licencia

Al contribuir, usted acepta que sus contribuciones serán licenciadas bajo la licencia Apache 2.0.

---

## Siguientes Pasos

- 📚 Leer la [documentación](../)
- 🐛 Ver los [issues abiertos](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Iniciar una [discusión](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 ¡Hacer Fork y comenzar a contribuir!

¡Gracias por contribuir a SiliconLifeCollective! 🎉
