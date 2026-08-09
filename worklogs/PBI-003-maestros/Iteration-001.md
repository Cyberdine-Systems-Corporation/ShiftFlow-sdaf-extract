# PBI-003-maestros / Iteration-001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-09 |
| Agente | Domain+Application |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-DOMAPP-001@0.1.0 |
| Contexto | Implementar maestros Org/Dept/Employee según SPEC-DOM-002 / SPEC-APP-001; rama `feat/pbi-003-maestros` |
| Especificaciones utilizadas | SPEC-DOM-001/002, SPEC-APP-001, SPEC-ACC-001 (ACC-S1-03 parcial, 04, 05), ADR-001/004 |
| Archivos leídos | Specs Sprint 1, Program Api, stub maestros, DbContext, IdentitySeed, backlog PBI-003 |
| Archivos modificados | Domain aggregates + repos; Application CQRS; Infrastructure EF/repos/DI; Api MasterDataEndpoints; tests; runbook; backlog; este worklog |
| Resultado | Persistencia real Organization/Department/Employee; API admin; stub eliminado; tests de invariantes y ACC-S1-03/04/05 (sin ShiftType) |
| Tiempo | ~1.5 h |
| Coste | N/D |
| Observaciones | ShiftType = PBI-004. EnsureCreated: reset volumen Postgres si faltan tablas nuevas. Sin UI (PBI-008). |
| Pruebas ejecutadas | `dotnet test` — 6 unit + 8 integration OK |
| Estado | hecho |
| Siguiente agente | humano (commit/PR) → PBI-004 ShiftType (o Frontend ligero si se antepone listados) |
