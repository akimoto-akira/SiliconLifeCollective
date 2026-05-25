# Guía de Contribución

> **Versión: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | **Español** | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

¡Gracias por tu interés en contribuir a SiliconLifeCollective!

## Contribución de Versión Dual

Este proyecto tiene dos versiones de implementación. Puedes elegir tu dirección de contribución según tus intereses:

### SiliconLife.Default (Versión Predeterminada)
- **Pila Tecnológica**: Aplicación de consola .NET 9
- **Dirección de Contribución**: Desarrollo de funciones principales, implementación de herramientas, localización, documentación
- **Adecuado Para**: Todos los desarrolladores

### SiliconLife.Fast (Versión de Alto Rendimiento)
- **Pila Tecnológica**: Aplicación de formularios Windows .NET 9
- **Dirección de Contribución**: Optimización de rendimiento, almacenamiento SpeedyPack, bandeja del sistema, concurrencia sin bloqueo
- **Adecuado Para**: Desarrolladores con experiencia en Windows e interés en optimización de rendimiento

> **Nota Importante**: Ambas versiones comparten los proyectos SiliconLife.Core y SiliconLife.Common. Las mejoras a las interfaces centrales afectarán a ambas versiones simultáneamente.

## Código de Conducta

Este proyecto sigue la licencia Apache 2.0. Mantén el respeto y la profesionalidad en todas las interacciones.

---

## Inicio Rápido

### 1. Hacer Fork del Repositorio

Haz clic en el botón "Fork" en GitHub para crear tu propia copia.

### 2. Clonar tu Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Configurar Entorno de Desarrollo

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

### 4. Crear Rama de Característica

```bash
git checkout -b feature/nombre-de-caracteristica
```

### 5. Elegir Proyecto de Desarrollo

Selecciona el proyecto apropiado según tu tipo de contribución:

- **Interfaces centrales/clases abstractas** → Modificar `SiliconLife.Core`
- **Implementaciones compartidas** → Modificar `SiliconLife.Common`
- **Específico de la versión Default** → Modificar `SiliconLife.Default`
- **Específico de la versión Fast** → Modificar `SiliconLife.Fast`
- **Motor de almacenamiento** → Modificar `SiliconLife.Speedy`
- **Herramienta de gestión de almacenamiento** → Modificar `SiliconLife.Speedy.Manager`
- **Desarrollo de plugins** → Modificar `SiliconLife.Core/Plugins`
- **Documentación multi-idioma** → Modificar directorio `docs/`

---

## Flujo de Trabajo de Desarrollo

### Estilo de Código

- Seguir convenciones de codificación C#
- Usar PascalCase para nombres de clases
- Usar camelCase para parámetros de métodos
- Usar `_camelCase` para campos privados
- Todas las APIs públicas deben tener documentación XML

### Mensajes de Commit

Seguir el formato de **Commits Convencionales**:

```
<tipo>(<alcance>): <descripción>
```

**Tipos**:
- `feat`: Nueva característica
- `fix`: Corrección de bug
- `docs`: Cambios de documentación
- `style`: Formato de código
- `refactor`: Refactorización de código
- `test`: Cambios de pruebas
- `chore`: Cambios de construcción/herramientas

**Ejemplos**:
```bash
feat(localization): añadir soporte de idioma coreano
fix(permission): corregir puntero nulo en callback
docs: actualizar guía de contribución
refactor(web): simplificar estructura de controladores
```

### Realizar Cambios

1. **Escribir código**
   - Seguir patrones existentes
   - Añadir pruebas para nueva funcionalidad
   - Actualizar documentación

2. **Probar tus cambios**
   ```bash
   # Ejecutar todas las pruebas
   dotnet test
   
   # Construir en modo release
   dotnet build --configuration Release
   ```

3. **Formatear código**
   ```bash
   dotnet format
   ```

4. **Confirmar cambios**
   ```bash
   git add .
   git commit -m "feat(alcance): descripción"
   ```

5. **Push a tu Fork**
   ```bash
   git push origin feature/nombre-de-caracteristica
   ```

6. **Crear Pull Request**
   - Ir al repositorio original
   - Hacer clic en "Compare & pull request"
   - Completar la plantilla de PR
   - Enviar

---

## Guía de Pull Request

### Título del PR

Usar el mismo formato que los mensajes de commit:
```
feat(localization): añadir soporte de idioma coreano
```

### Descripción del PR

Incluir:

1. **Qué** - ¿Qué hace este PR?
2. **Por qué** - ¿Por qué se necesita este cambio?
3. **Cómo** - ¿Cómo lo implementaste?
4. **Pruebas** - ¿Cómo se probó?

### Ejemplo de Descripción de PR

```markdown
## Qué
Añadir localización al coreano para todos los componentes UI y documentación.

## Por qué
Ampliar la accesibilidad del proyecto a usuarios coreanos.

## Cómo
- Crear archivo de localización KoKR.cs
- Añadir 500+ claves de traducción
- Actualizar todas las vistas para usar localización
- Crear documentación en coreano en docs/ko-KR/

## Pruebas
- Verificar que todos los elementos UI muestren coreano correctamente
- Probar funcionalidad de cambio de idioma
- Revisar traducciones con hablantes nativos
```

---

## Tipos de Contribución

### 1. Corrección de Bugs

**Proceso**:
1. Verificar issues existentes
2. Crear un issue si no existe
3. Corregir el bug
4. Añadir casos de prueba
5. Enviar PR

**Requisitos**:
- Descripción clara del bug
- Pasos para reproducir
- Pruebas para prevenir regresión

### 2. Nuevas Características

**Proceso**:
1. Discutir la característica en Issues/Discussions
2. Obtener aprobación del mantenedor
3. Implementar la característica
4. Añadir pruebas completas
5. Actualizar documentación
6. Enviar PR

**Requisitos**:
- Propuesta de característica aprobada
- Cobertura de pruebas completa
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
- Soportar múltiples idiomas si aplica

### 4. Refactorización de Código

**Proceso**:
1. Proponer refactorización en un Issue
2. Obtener aprobación
3. Refactorizar código
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
public void MiFuncionalidad_DeberiaFuncionar_ComoseEspera()
{
    // Organizar
    var servicio = new MiServicio();
    
    // Ejecutar
    var resultado = servicio.HacerAlgo();
    
    // Afirmar
    Assert.IsTrue(resultado.Success);
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
- Usar inglés para comentarios de código

### Archivos de Documentación

- Colocar en `docs/{idioma}/`
- Actualizar todas las versiones de idiomas
- Seguir la estructura existente

### Documentación Multi-idioma

Al añadir documentación:
1. Crear primero la versión en inglés
2. Traducir a otros idiomas
3. Mantener el contenido sincronizado

---

## Proceso de Revisión

### Qué Verifican los Mantenedores

1. **Calidad del Código**
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
   - Multi-idioma

4. **Compatibilidad**
   - Compatible hacia atrás
   - Sin cambios disruptivos (a menos que se notifique)
   - Sigue versionado semántico

### Timeline de Revisión

- Revisión inicial: 1-3 días
- Integración de feedback: según sea necesario
- Fusión: tras aprobación

---

## Preguntas Frecuentes

### PR Rechazado

**Razones**:
- No sigue las guías
- Pruebas insuficientes
- Cambios disruptivos no notificados
- Calidad de código deficiente

**Solución**:
- Resolver el feedback
- Actualizar el PR
- Reenviar

### Conflictos de Fusión

**Solución**:
```bash
# Actualizar tu rama
git fetch origin
git rebase origin/master

# Resolver conflictos
# Editar archivos en conflicto
git add .
git rebase --continue

# Push forzado
git push --force-with-lease
```

---

## Obtener Ayuda

### Recursos

- **Documentación**: [docs/](../)
- **Issues**: GitHub Issues
- **Discusiones**: GitHub Discussions
- **Código de Conducta**: CODE_OF_CONDUCT.md

### Contacto

- Crear un Issue para bugs
- Iniciar una Discussion para preguntas
- Mencionar al mantenedor para asuntos urgentes

---

## Reconocimientos

Los contribuidores serán reconocidos en:
- Sección de contribuidores del README.md
- Notas de lanzamiento
- Documentación del proyecto

---

## Licencia

Al contribuir, aceptas que tus contribuciones estén licenciadas bajo la licencia Apache 2.0.

---

## Próximos Pasos

- 📚 Leer la [documentación](../)
- 🐛 Ver [issues abiertos](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Iniciar una [discusión](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Hacer fork y comenzar a contribuir

¡Gracias por contribuir a SiliconLifeCollective!🎉
