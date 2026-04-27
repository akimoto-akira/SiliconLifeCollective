# Guía de Contribución

> **Versión: v0.1.0-alpha**

[English](../en/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | **Español** | [Deutsch](../de-DE/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md)

## ¡Bienvenido!

¡Gracias por tu interés en contribuir a Silicon Life Collective! Aceptamos todas las formas de contribución.

## Cómo Contribuir

### Reportar Bugs

1. Verificar que el bug no haya sido reportado
2. Crear un issue en [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
3. Incluir:
   - Descripción clara del problema
   - Pasos para reproducir
   - Comportamiento esperado
   - Capturas de pantalla si es aplicable

### Sugerir Mejoras

1. Crear un issue con etiqueta "enhancement"
2. Describir la mejora propuesta
3. Explicar por qué sería útil
4. Proporcionar ejemplos si es posible

### Contribuir con Código

#### Flujo de Trabajo de Desarrollo

1. **Hacer fork del repositorio**

2. **Crear rama de característica**:
   ```bash
   git checkout -b feature/nombre-de-caracteristica
   ```

3. **Implementar cambios**
   - Seguir convenciones de código del proyecto
   - Añadir pruebas para nueva funcionalidad
   - Actualizar documentación si es necesario

4. **Confirmar cambios**:
   ```bash
   git add .
   git commit -m "feat: descripción clara del cambio"
   ```

5. **Push a la rama**:
   ```bash
   git push origin feature/nombre-de-caracteristica
   ```

6. **Enviar Pull Request**

### Convenciones de Commit

Seguir [Conventional Commits](https://www.conventionalcommits.org/):

```
<tipo>(<alcance>): <descripción>
```

**Tipos**:
- `feat`: Nueva característica
- `fix`: Corrección de bug
- `docs`: Cambios de documentación
- `style`: Formato de código
- `refactor`: Refactorización
- `test`: Añadir o modificar pruebas
- `chore`: Tareas de mantenimiento

**Ejemplos**:
```bash
feat(localization): añadir soporte para español
fix(permission): corregir validación de permisos nulos
docs: actualizar documentación de arquitectura
```

## Estilo de Código

### C#

- Seguir convenciones de .NET
- Usar PascalCase para clases y métodos
- Usar camelCase para parámetros y variables locales
- Incluir comentarios XML para APIs públicas

```csharp
/// <summary>
/// Descripción del método.
/// </summary>
/// <param name="param">Descripción del parámetro</param>
/// <returns>Descripción del valor de retorno</returns>
public async Task<string> MiMetodoAsync(string param)
{
    // Implementación
}
```

### Markdown

- Usar encabezados consistentes
- Incluir ejemplos de código cuando sea apropiado
- Mantener listas formateadas correctamente

### Documentación

- Mantener documentación actualizada con cambios de código
- Proporcionar ejemplos claros
- Traducir a múltiples idiomas si es posible

## Pruebas

### Ejecutar Pruebas

```bash
dotnet test
```

### Escribir Pruebas

- Crear pruebas unitarias para nueva funcionalidad
- Incluir pruebas de integración para flujos complejos
- Asegurar que todas las pruebas pasen antes de enviar PR

### Cobertura de Pruebas

Objetivo: >80% de cobertura de código

```bash
dotnet test /p:CollectCoverage=true
```

## Documentación

### Actualizar Documentación

Cuando añadas nuevas características:

1. Actualizar guía relevante en `docs/`
2. Actualizar README si es necesario
3. Añadir ejemplos de uso
4. Actualizar referencias de API

### Traducciones

Ayuda a traducir documentación a múltiples idiomas:

- `docs/en/` - Inglés
- `docs/zh-CN/` - Chino simplificado
- `docs/es-ES/` - Español
- `docs/ja-JP/` - Japonés
- `docs/ko-KR/` - Coreano
- Y más...

## Revisión de Código

### Criterios de Revisión

- Código sigue convenciones de estilo
- Todas las pruebas pasan
- Documentación actualizada
- Sin regresiones de rendimiento
- Sin problemas de seguridad

### Proceso de Revisión

1. Mantenedor revisa PR
2. Solicita cambios si es necesario
3. Contribuidor actualiza PR
4. Mantenedor aprueba y fusiona

## Licencia

Al contribuir, aceptas que tus contribuciones estén bajo la licencia Apache 2.0.

## Código de Conducta

- Ser respetuoso con otros contribuidores
- Aceptar retroalimentación constructiva
- Enfocarse en lo que es mejor para la comunidad
- Mostrar empatía hacia otros miembros de la comunidad

## Reconocimiento

Los contribuidores serán reconocidos en:

- Archivo CONTRIBUTORS.md
- Notas de lanzamiento
- Página de agradecimientos en Web UI

## Preguntas Frecuentes

### ¿Necesito permiso para contribuir?

¡No! Simplemente envía un PR y lo revisaremos.

### ¿Puedo contribuir sin ser experto en C#?

¡Sí! Aceptamos contribuciones de documentación, traducciones, reportes de bugs, etc.

### ¿Cuánto tiempo toma revisar un PR?

Generalmente dentro de 1-2 semanas, dependiendo de la complejidad.

### ¿Puedo trabajar en múltiples PRs?

¡Por supuesto! Solo asegúrate de usar ramas separadas.

## Contactar

- 🐛 [Reportar problema](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 [Discusiones de GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 📧 Email del maintainer

---

¡Gracias por contribuir a Silicon Life Collective!
