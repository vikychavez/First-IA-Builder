---
name: conventional-commit
description: Genera mensajes de commit siguiendo Conventional Commits. Se usa cuando el usuario va a crear un commit o pide explícitamente un mensaje de commit.
---

# Conventional Commit

Genera el mensaje de commit a partir de los cambios en stage (`git diff --staged`), siguiendo el formato Conventional Commits.

## Formato

```
tipo(scope): descripción en imperativo
```

- **tipo**: uno de `feat`, `fix`, `refactor`, `docs`, `style`, `test`, `chore`, `perf`, `build`, `ci`.
- **scope** (opcional): módulo o área afectada (ej. `auth`, `novedades`, `api`). Omitir el scope y los paréntesis si el cambio no es específico de un módulo.
- **descripción**: en imperativo ("agregar", "corregir", "eliminar" — no "agregado", "agregando"), en minúscula, sin punto final, máx. 72 caracteres en total.

## Reglas

1. Analizar `git diff --staged` (o los archivos que el usuario indique) para determinar el tipo de cambio real, no lo que el usuario describe de palabra.
2. Elegir el tipo según la naturaleza del cambio:
   - `feat`: nueva funcionalidad para el usuario final.
   - `fix`: corrección de un bug.
   - `refactor`: cambio de código que no altera comportamiento externo.
   - `docs`: cambios solo en documentación.
   - `style`: formato, espacios, sin cambio de lógica.
   - `test`: agregar o corregir tests.
   - `chore`: mantenimiento, dependencias, configuración.
   - `perf`: mejora de rendimiento.
   - `build`: cambios en el sistema de build o dependencias externas.
   - `ci`: cambios en configuración de integración continua.
3. Si hay cambios de distinta naturaleza mezclados en el stage, priorizar el cambio principal y avisar al usuario que podría convenir dividir el commit.
4. No incluir emojis, prefijos de herramientas ni menciones de archivos en el mensaje.
5. Si el cambio rompe compatibilidad hacia atrás, agregar `!` después del tipo/scope (ej. `feat(api)!: cambiar formato de respuesta`) y explicar el breaking change en el cuerpo del commit.
6. Proponer el mensaje al usuario antes de ejecutar `git commit`; no commitear sin confirmación.

## Ejemplos

- `feat(novedades): agregar carga de horas feriado`
- `fix(calculo): corregir redondeo en zona desfavorable`
- `refactor(personal): extraer validación de provincia`
- `docs: actualizar PRD con criterios de aceptación`
- `chore: actualizar dependencias de angular`
