---
name: blazor-bff-slice
description: Implementa o extiende UI Blazor Server con sesión BFF/cookies y clientes API sin mutar Domain desde la UI. Usar en slices Frontend tras Gate 0.
---

# blazor-bff-slice

| Campo | Valor |
|--------|--------|
| ID | blazor-bff-slice |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | media |
| Fecha | 2026-08-12 |
| Norma | ADR auth (ADR-005), [agents/frontend-agent.md](../../agents/frontend-agent.md) |

## Disparadores

- Páginas Blazor, nav, CRUD UI, calendario, leaves UI; cambios en `src/ShiftFlow.Web`.

## Pasos

1. Confirmar Gate 0 (`sdaf-gate0`) y specs UI/API aplicables.
2. Reutilizar patrones existentes: `Auth/`, `Api/*ApiClient.cs`, cookies/`CookieContainerHolder`, handlers.
3. UI solo orquesta HTTP hacia API; **reglas de negocio en Domain/Application**, no reimplementar en Razor.
4. Proyección (p. ej. ausencias en calendario) ≠ mutación (assign/cancel/register).
5. Aplicar `csharp-adr006-slice` (incluye `.razor`).
6. Smoke manual o tests de integración existentes; actualizar worklog Frontend.

## Definition of Done

- [ ] Flujo auth/sesión coherente con BFF existente.
- [ ] Sin reglas HR/leave inventadas en UI.
- [ ] ADR-006 cumplido; worklog cerrado.

## Restricciones

- No secretos en cliente.
- No ampliar Out del MVP (IA producto, etc.).

## Referencias

- [prompts/agents/frontend-agent.md](../../prompts/agents/frontend-agent.md)
- `src/ShiftFlow.Web/`
