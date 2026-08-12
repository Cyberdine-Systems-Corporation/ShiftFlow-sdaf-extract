# PBI-005 — Calendario UI — Iteration 004 (Frontend)

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-12 |
| Agente | Frontend |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-FE-001@0.1.1 |
| Contexto | Tras merge #18 (API AssignShift/calendar); UI Blazor en rama `feat/pbi-005-calendar-ui` |
| Especificaciones utilizadas | SPEC-APP-003, SPEC-ACC-002, SPEC-PRD-003 (design system) |
| Archivos leídos | MastersApiClient, OrganizationDetail, Calendar placeholder, SchedulingEndpoints |
| Archivos modificados | MastersApiClient (GetMonthCalendar/Assign/Cancel), Calendar.razor, app.css, Home.razor, backlog, este worklog |
| Resultado | Calendario mensual con selector de org, assign, cancel y alertas de regla; sin commit/PR (orden humana) |
| Tiempo | ~1 h |
| Coste | N/D |
| Observaciones | Errores `HR-01` / `INV-ASN-*` vía `sf-alert`. Grid desktop + listado móvil. Sin lógica de reglas en UI. |
| Pruebas ejecutadas | `dotnet build src/ShiftFlow.Web` |
| Estado | hecho |
| Siguiente agente | Testing+Review |

## Entregado

- Cliente: `GetMonthCalendarAsync`, `AssignShiftAsync`, `CancelShiftAsync`
- `/calendar`: org + mes + form assign + grid/list + cancel
