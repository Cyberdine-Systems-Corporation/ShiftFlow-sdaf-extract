# PBI-011-ai-explain-stub / Iteration-004-frontend

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-14 |
| Agente | Frontend |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | `PROMPT-AGT-FE-001@0.1.1` |
| Skills | `blazor-bff-slice@0.1.0`, `csharp-adr006-slice@0.1.0`, `sdaf-worklog-handoff@0.1.0` |
| Contexto | Tras slice API (Iteration-003 / PR #34). Mostrar explicación HR-* en calendario (ACC-S3-X07). |
| Especificaciones utilizadas | SPEC-APP-005 §4, SPEC-ACC-005 ACC-S3-X07, SPEC-PRD-003 (alerta legible) |
| Archivos leídos | Calendar.razor, MastersApiClient, app.css, worklog 003, ACC-005 |
| Archivos modificados | `MastersApiClient`/`ApiResult` (title/body), `Calendar.razor`, `app.css`, backlog PBI-011, este worklog |
| Resultado | El rechazo de AssignShift en `/calendar` muestra código · título y cuerpo del stub. Sin segundo round-trip. Sin reglas en UI. |
| Tiempo | ~0.35 h |
| Coste | N/D |
| Observaciones | Playwright Out (H16). La UI no llama `GET /api/rules/explain`; usa el adjunto del 400. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` — 26 unit + 35 integration OK; `dotnet build` Web OK |
| Estado | hecho |
| Siguiente agente | Testing+Review (Gate 2 del PBI-011: API #34 + esta UI) |

## ACC-S3-X07

Dado rechazo HR-* en calendario, la alerta muestra título y cuerpo en castellano; `LoadCalendarAsync` no se llama si AssignShift falla, así que el mes no pinta el turno rechazado.
