# ShiftFlow

Planificación y gestión de turnos con reglas configurables y trazabilidad.

MVP **enterprise demostrable** (no producción): Blazor Web + API + PostgreSQL en entorno **local autocontenido**.

## Qué es

| Cara | Contenido |
|------|-----------|
| **Producto** | Auth/roles, maestros, tipos de turno, calendario, asignación validada (hard rules), ausencias |
| **Método** | SDAF: handbook, specs, ADRs, agentes/prompts, skills, worklogs y tests derivados de aceptación |

- Constitución: [handbook/README.md](handbook/README.md)
- Charter: [handbook/01-product-charter.md](handbook/01-product-charter.md)
- Alcance MVP: [handbook/03-mvp-definition.md](handbook/03-mvp-definition.md)

## Arranque rápido

Prerrequisitos: **.NET SDK 10**, Docker (PostgreSQL vía Aspire), Git.

```powershell
git clone https://github.com/mortiz-iadev/ShiftFlow.git
cd ShiftFlow
dotnet restore ShiftFlow.sln
dotnet run --project src/ShiftFlow.AppHost --launch-profile https
```

Pasos completos (URLs, usuario demo, Compose, parada): **[docs/runbook-local.md](docs/runbook-local.md)**.

API de contrato (Postman): [postman/README.md](postman/README.md).

## Estructura

| Ruta | Rol |
|------|-----|
| `handbook/` | Constitución del proyecto |
| `specs/`, `architecture/`, `backlog/` | Specs, ADRs, PBIs |
| `agents/`, `prompts/`, `skills/`, `worklogs/` | Ingeniería IA (SDAF) |
| `src/`, `tests/` | Solución .NET y pruebas |
| `docs/` | Runbooks / presentación (no sustituyen handbook) |
| [AGENTS.md](AGENTS.md) | Router de agentes |

## Contribuir

Ver [CONTRIBUTING.md](CONTRIBUTING.md). **Gate 0** obligatorio antes de código de producto.

## Seguridad

Ver [SECURITY.md](SECURITY.md) para reportar vulnerabilidades (sin issue público con PoC).

## Licencia

[MIT](LICENSE) © 2026 [mortiz-iadev](https://github.com/mortiz-iadev).
