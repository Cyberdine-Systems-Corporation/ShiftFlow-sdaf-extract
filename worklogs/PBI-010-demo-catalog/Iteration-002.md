# PBI-010-demo-catalog / Iteration-002

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-14 |
| Agente | Domain+Application |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | `PROMPT-AGT-DOMAPP-001@0.1.1` |
| Contexto | Arranque local fallaba al persistir el catálogo: Npgsql rechaza `DateTimeOffset` con offset +02:00 en `timestamptz`. |
| Especificaciones utilizadas | PBI-010, ADR-007, Calendar.razor (ya usa `TimeSpan.Zero`) |
| Archivos leídos | DemoCatalogSeed, ShiftAssignmentConfiguration, AssignShift, Calendar.razor, CalendarAssignApiTests |
| Archivos modificados | `DemoCatalogSeed.cs` (`AtUtc`), runbook §3.2, este worklog |
| Resultado | Seed persiste instantes UTC (offset 0), alineado a UI y tests. El SaveChanges fallido no dejó orgs ancla; un rearranque vuelve a sembrar. |
| Tiempo | ~0.15 h |
| Coste | N/D |
| Observaciones | Skills: `csharp-adr006-slice@0.1.0`. No se activa `EnableLegacyTimestampBehavior`. |
| Pruebas ejecutadas | `dotnet test` filtro DemoCatalogSeed + CalendarAssign — 10 integration OK |
| Estado | hecho |
| Siguiente agente | humano (rearrancar Api) |
