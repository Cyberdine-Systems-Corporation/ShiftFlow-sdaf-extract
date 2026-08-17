# PBI-015-ux-ia-freeze / Iteration-005-testing-review

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-17 |
| Agente | Testing+Review |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | `PROMPT-AGT-TESTREV-001@0.1.1` |
| Skills | `testing-review-pr@0.1.0`, `csharp-adr006-slice@0.1.0`, `sdaf-worklog-handoff@0.1.0` |
| Contexto | Gate 2 del PR #36 (jerarquía UX freeze, SPEC-PRD-003 0.2.0). |
| Especificaciones utilizadas | SPEC-PRD-003 Approved 0.2.0 (AC-UX-01…10); SPEC-PRD-002 Approved (journey no cambia); ADR-002/006; handbook 09/16/17/20 |
| Archivos leídos | Diff Web (`WorkspaceContext`, `App.razor`, `AuthNavBar`, páginas, `app.css`); PBI-015; worklogs 001–004; `ShiftFlow.Web.csproj` |
| Archivos modificados | este worklog; `backlog/PBI-015-ux-ia-freeze.md`; `backlog/README.md` |
| Resultado | **Gate 2 OK.** 0 bloqueantes. Dictamen: **merge sí** (#36). |
| Tiempo | ~0.4 h |
| Coste | N/D |
| Observaciones | Sin tests E2E Blazor (Playwright Out, H16). AC-UX-06…10 cubiertos por revisión de markup/CSS + smoke manual. UI no reimplementa HR: `AssignShiftAsync` sigue yendo a la API. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` (2026-08-17) → Unit 26 OK; Integration 35 OK. 0 `var` en `src/`. Web csproj sin Mud/Fluent. |
| Estado | hecho |
| Siguiente agente | humano (merge PR #36) |

## Quality gates

| Gate | Resultado |
|------|-----------|
| QG-Build | Verde (`ShiftFlow.Web` + sln; CS1591 OK; `WorkspaceContext` con XML) |
| QG-Unit | Verde (26) |
| QG-Accept | N/A de ACC de dominio nuevos. AC-UX-06…10 por código UI (H16). Suite de journey/API sin regresiones (35). |
| QG-Arch | OK — CSS propio; Web-only; un circuito `InteractiveServer` en `Routes`; `WorkspaceContext` scoped compartido; sin reglas HR en Razor |
| QG-Docs / ADR-006 | OK — 0 `var`; XML en tipo público nuevo; regiones no exigibles (tipo trivial) |
| QG-Sec | OK — sin secretos nuevos; páginas de maestros/planificación con `[Authorize(Roles = Administrator)]`; login no cambia el contrato de sesión |
| QG-Review | Checklist §3 abajo |

## Checklist review (handbook 17 §3)

### Gobierno
- [x] Gate 0: SPEC-PRD-003 0.2.0 Approved; PBI-015; ADR N/A; worklogs 001–004
- [x] Sin Out (sin Mud/Fluent, MAUI, dark mode, drag-and-drop, APIs nuevas)
- [x] Worklogs + `PROMPT-AGT-TESTREV-001@0.1.1` citados

### Dominio / arquitectura
- [x] Hard rules siguen en Domain; Calendar solo muestra rechazo de API
- [x] Domain/Application/Infra no tocados
- [x] Sin commands disfrazados de query

### Calidad
- [x] Nombres de producto en UI; Kind `Vacation`/`Other` sin cambiar dominio
- [x] Sin secretos en el diff
- [x] ADR-006 en `.cs`/`.razor` tocados

### Producto
- [x] Auth Administrator en `/organizations`, detalle, `/calendar`, `/leaves`
- [x] Runbook local válido (misma composición Aspire; no hay endpoints nuevos)

### Seguridad (H20)
- [x] Sin secretos nuevos
- [x] Autorización UI coherente con el rol demo
- [x] Sin injection; org id vía `Guid`/select

## Trazabilidad AC-UX → evidencia

| AC | Evidencia |
|----|-----------|
| AC-UX-01 | `Login.razor`: `sf-auth-brand` ShiftFlow + formulario + Entrar |
| AC-UX-02 | `AuthNavBar.razor`: marca, NavLink, usuario, Salir; `sf-shell-max` 88rem |
| AC-UX-03 | empty/loading/alert en Organizations, detalle, Calendar, Leaves |
| AC-UX-04 | `@media` 960/720/640; grilla → listado &lt;720px; aside apilado |
| AC-UX-05 | `ShiftFlow.Web.csproj` solo Serilog + project refs |
| AC-UX-06 | `OrganizationDetail.razor`: tabs; `_tab` default `personal` |
| AC-UX-07 | Organizations/Leaves: listado en `.sf-section`; alta en `.sf-panel` colapsable |
| AC-UX-08 | `sf-cal-layout` mes + aside; `OnDayClick`; alerta en aside |
| AC-UX-09 | `WorkspaceContext` + select en nav; Calendar/Leaves sin select de org en el cuerpo; `Routes @rendermode InteractiveServer` (un circuito) |
| AC-UX-10 | Ledes de producto; Home `sf-page-title` Planificación (sin `sf-hero-brand`) |

## Hallazgos

| Severidad | Hallazgo | Acción |
|-----------|----------|--------|
| Menor | AC-UX-06…10 sin automatizar (sin Playwright, H16) | Smoke manual en el PR; mismo criterio que PBI-013 |
| Menor | `AuthNavBar.OnLocationChanged` es `async void` | Deuda; no bloquea demo |
| Menor | Pestaña Personal del detalle sigue con alta encima de las tablas | Fuera de AC-UX-07 (solo listados orgs/ausencias); deuda post-merge opcional |
| Info | WCAG AA formal Out de SPEC-PRD-003 | Ya registrado en PBI-013 |

## Veredicto

**Gate 2: aprobado.** Merge **sí** de https://github.com/mortiz-iadev/ShiftFlow/pull/36. Humano decide merge; no auto-merge. Smoke recomendado: cambiar org en el shell desde Calendario y desde el detalle.
