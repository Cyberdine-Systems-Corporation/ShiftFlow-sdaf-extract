# PBI-007 — Leave UI + proyección calendario — Iteration 004 (Frontend)

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-12 |
| Agente | Frontend |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-FE-001@0.1.1 |
| Contexto | API Leave/HR-02 en main (#22); UI + proyección CalendarMonth |
| Especificaciones utilizadas | SPEC-PRD-002 AC-04, SPEC-PRD-003, SPEC-APP-004, SPEC-DOM-007 Approved; ADR-002/006 |
| Archivos leídos | Calendar.razor, MastersApiClient, GetMonthCalendar, LeaveEndpoints, AuthNavBar |
| Archivos modificados | GetMonthCalendar → MonthCalendarDto; Calendar.razor; Leaves.razor; MastersApiClient; AuthNavBar/Home; app.css; tests calendario/leave; Postman; backlog; este worklog |
| Resultado | Página `/leaves`; calendario proyecta ausencias; AssignShift muestra HR-02 vía sf-alert |
| Tiempo | ~1.25 h |
| Coste | N/D |
| Observaciones | Contrato GET calendar cambia a `{ assignments, leaves }` (tests actualizados). HR-03 fuera. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` → Unit 21 OK, Integration 24 OK; `dotnet build` Web OK |
| Estado | hecho |
| Siguiente agente | Testing+Review / merge |

## Entregado

- Application: `MonthCalendarDto` + `CalendarLeaveDto` en GetMonthCalendar
- Web: `/leaves` (register/list/cancel); Calendar con leaves; nav + CTA Home
- Cliente: métodos Leave + GetMonthCalendar tipado
