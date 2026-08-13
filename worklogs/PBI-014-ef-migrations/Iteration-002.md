# PBI-014-ef-migrations / Iteration-002

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-13 |
| Agente | Domain+Application (Infrastructure Agent en stub; persistencia EF ya la cubre este agente en el MVP) |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | `PROMPT-AGT-DOMAPP-001@0.1.1` |
| Contexto | Materializar ADR-007 / PBI-014: baseline EF, `MigrateAsync` en Npgsql, `EnsureCreated` solo en SQLite de tests, runbook. Entrada: Iteration-001. |
| Especificaciones utilizadas | ADR-007 Propuesto, PBI-014, SPEC-PRD-001 (C-LOC), ADR-006 |
| Archivos leídos | Iteration-001, IdentitySeed, DbContext, Api/Infrastructure csproj, ShiftFlowApiFactory, runbook, configuraciones Fluent API |
| Archivos modificados | `DatabaseInitializer.cs`, `ShiftFlowDbContextFactory.cs`, `IdentitySeed.cs`, csproj Api+Infrastructure, `Persistence/Migrations/*` (`InitialCreate`), `.config/dotnet-tools.json`, `docs/runbook-local.md`, `README.md`, este worklog |
| Resultado | Esquema Postgres vía `MigrateAsync`; SQLite de tests vía `EnsureCreated`; herramienta local `dotnet-ef` 10.0.2; runbook §3.1. Tests 26 unit + 28 integration verdes. |
| Tiempo | ~0.8 h |
| Coste | N/D |
| Observaciones | Skills: `sdaf-gate0@0.1.0`, `csharp-adr006-slice@0.1.0`, `sdaf-worklog-handoff@0.1.0`. 0 `var` en archivos tocados. Volumen Aspire/Compose creado con EnsureCreated exige **un** reset (§6). ADR-007 sigue **Propuesto** hasta aceptación humana. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` — 26 unit + 28 integration OK |
| Estado | hecho |
| Siguiente agente | humano (aceptar ADR-007) y Testing+Review (`PROMPT-AGT-TESTREV` / Gate 2-PR) |
