# PBI-008-blazor-crud / Iteration-001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-09 |
| Agente | Frontend |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-FE-001@0.1.1 |
| Contexto | Shell Blazor + CRUD maestros (AC-01); rama `feat/pbi-008-blazor-crud` |
| Especificaciones utilizadas | SPEC-PRD-002 AC-01, SPEC-APP-001, ADR-002/006 |
| Archivos leídos | PBI-008, Login/AuthNavBar, Program Web, Masters API |
| Archivos modificados | MastersApiClient, páginas Org/Detail/Calendar/Home/Login, Routes, nav, CSS, backlog, runbook, este worklog |
| Resultado | Nav + login → organizaciones → detalle con dept/emp/shift type; calendario placeholder Sprint 2 |
| Tiempo | ~1.5 h |
| Coste | N/D |
| Observaciones | Errores de Api (INV-*) visibles en UI. Calendario funcional = PBI-005. Fix auth Web: AddAuthentication passthrough; CookieContainer singleton; PropagateAllCookiesHandler (host Aspire); RemoveAllResilienceHandlers en cliente api. |
| Pruebas ejecutadas | `dotnet build` Web OK; `dotnet test` 8+9 OK |
| Estado | hecho |
| Siguiente agente | humano (commit/PR) → Sprint 2 specs (PBI-005…) o Testing+Review smoke UI |
