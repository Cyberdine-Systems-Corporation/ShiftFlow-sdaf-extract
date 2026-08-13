# Política de seguridad — ShiftFlow

## Alcance

ShiftFlow es un **MVP demostrable** (no producción). Aun así, se aceptan reportes de vulnerabilidades que afecten a:

- Autenticación / sesión (cookie, tokens de acceso de desarrollo)
- Autorización por roles
- Exposición de secretos o datos sensibles en el repositorio o en respuestas de API
- Inyección, XSS u otras fallas explotables en Api o Web locales

## Cómo reportar

**No** abras un issue público con detalles explotables.

1. Contacta al mantenedor vía GitHub: [mortiz-iadev](https://github.com/mortiz-iadev) (Security advisory privado del repo, o mensaje/issue **sin** PoC pública).
2. Incluye: resumen, impacto, pasos de reproducción, versión/commit afectado, entorno (local Aspire, etc.).
3. Si es posible, propone mitigación.

Respuesta orientativa: acuse en unos días laborables; corrección o plan según severidad y prioridad del MVP.

## Prácticas del proyecto

- Baseline de diseño/implementación: [handbook/20-security-standards.md](handbook/20-security-standards.md).
- No commitear secretos (`.env`, passwords, connection strings con credenciales, user-secrets).
- Contraseña del usuario demo: override con **user-secrets** o variables de entorno (ver [docs/runbook-local.md](docs/runbook-local.md) y [ADR-005](architecture/decisions/ADR-005-auth-basica-mvp.md)).
- Auth del MVP es básica a propósito (sin MFA, etc.); no se considera “bug” el alcance diferido documentado en ADR/specs, salvo que haya bypass inesperado de lo **sí** especificado.

## Versiones soportadas

Solo la rama `main` (y releases etiquetados, si existen). No hay SLA de producción.

## Licencia

El software se distribuye bajo [MIT](LICENSE). El reporte de seguridad no modifica la licencia ni las renuncias de garantía del MIT.
