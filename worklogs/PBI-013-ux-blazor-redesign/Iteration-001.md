# PBI-013 — Rediseño UX demo (Blazor) — Iteration 001

| Campo | Valor |
|--------|--------|
| Agente | Frontend |
| Fecha | 2026-08-10 |
| Specs | SPEC-PRD-003 Approved, SPEC-PRD-002 |
| Rama | `feat/ux-blazor-redesign` |
| Siguiente | Testing+Review / merge |

## Objetivo

Registrar NFR UI demo (fuente canónica) e implementar design system CSS + pantallas Web sin kit externo.

## Hecho

- `SPEC-PRD-003` Approved (0.1.1) + `PBI-013` + índice backlog; PBI-008 marcado mergeado.
- Tokens y primitivas en `wwwroot/app.css` (Fraunces/Manrope, teal, atmósfera, motion).
- Shell: `App.razor` lang=es + fonts; `MainLayout` / `AuthNavBar` con NavLink activo.
- Pantallas: Login, Home, Organizations, OrganizationDetail, Calendar (estados vacío/error).

## No hecho / riesgos

- Sin audit WCAG formal; smoke responsive recomendado en navegador tras merge.

## Archivos clave

- `src/ShiftFlow.Web/wwwroot/app.css`
- `src/ShiftFlow.Web/Components/**/*.razor` (layout + pages)
- `specs/product/SPEC-PRD-003-ui-demo-nfr.md`
