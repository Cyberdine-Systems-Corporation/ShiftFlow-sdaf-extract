# PBI-014-ef-migrations / Iteration-004

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-13 |
| Agente | Testing+Review |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | `PROMPT-AGT-TESTREV-001@0.1.1` |
| Skills | `testing-review-pr@0.1.0`, `csharp-adr006-slice@0.1.0`, `sdaf-worklog-handoff@0.1.0` |
| Contexto | Gate 2 / confirmación previa a PR de ADR-007 Aceptado y slice de migraciones EF (PBI-014). |
| Especificaciones utilizadas | SPEC-PRD-001 (C-LOC) Approved; ADR-007 Aceptado; PBI-014; ADR-006 |
| Archivos leídos | ADR-007, PBI-014, DatabaseInitializer, IdentitySeed, factory, migraciones, runbook, handbook 17, worklogs 001–003 |
| Archivos modificados | este worklog |
| Resultado | **Gate 2 OK.** Dictamen: **merge sí**. 0 bloqueantes. |
| Tiempo | ~0.2 h |
| Coste | N/D |
| Observaciones | AC de historial en Postgres no tiene test automatizado (SQLite usa EnsureCreated; Testcontainers sigue diferido en ADR-004/007). Cobertura por revisión de código + integración existente. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` — 26 unit + 28 integration OK; 0 `var` en Infrastructure tocada |
| Estado | hecho |
| Siguiente agente | humano (merge del PR) |

## Quality gates

| Gate | Resultado |
|------|-----------|
| QG-Build | Verde (CS1591 en migraciones con `<summary>`) |
| QG-Unit | Verde (26) |
| QG-Accept | Verde en el camino de tests (SQLite EnsureCreated). AC Postgres/`__EFMigrationsHistory` no automatizado — menor |
| QG-Arch | OK — persistencia en Infrastructure; Domain sin EF; seed separado del esquema |
| QG-Docs / ADR-006 | OK XML en tipos públicos nuevos; 0 `var`; factory/initializer sin regiones (tipos triviales) |
| QG-Sec | OK — sin secretos nuevos; cadena default igual al runbook/DI ya existente |
| QG-Review | Checklist §3 abajo |

## Checklist review (handbook 17 §3)

### Gobierno
- [x] Gate 0: SPEC-PRD-001 C-LOC Approved; criterios en PBI-014; ADR-007 Aceptado (Iteration-003); worklogs 001–003
- [x] Sin alcance Out (sin worker Aspire, sin Testcontainers, sin prod)
- [x] Worklog + prompts citados

### Dominio / arquitectura
- [x] Sin reglas de negocio nuevas
- [x] Domain sin infra
- [x] N/A CQRS (infra de esquema)

### Calidad
- [x] Integración existente sigue verde (camino SQLite del AC)
- [x] Lenguaje de persistencia coherente (Migrate / EnsureCreated / provider)
- [x] Sin secretos
- [x] Logging de camino de esquema (Information)
- [x] ADR-006

### Producto
- [x] Auth/seed intactos (IdentitySeed sigue provisionando demo)
- [x] Runbook §3.1 y reset único documentados

### Seguridad (H20)
- [x] Sin secretos nuevos en el diff
- [x] Auth no tocada
- [x] Sin SQL concatenado (EF)

## Trazabilidad AC → evidencia

| Criterio PBI-014 | Evidencia |
|------------------|-----------|
| Postgres vacío → migraciones + historial | `DatabaseInitializer` → `MigrateAsync` si no SQLite; migración `InitialCreate` |
| Cambio aditivo sin wipe | Runbook §3.1; ADR-007 |
| Volumen EnsureCreated → un reset | Runbook §3.1 / §6 / troubleshooting |
| Tests SQLite EnsureCreated verdes | 28 integration OK |
| Prohibido EnsureCreated en Npgsql | Rama `IsSqlite` en `DatabaseInitializer` |

## Hallazgos

| Severidad | Hallazgo | Acción |
|-----------|----------|--------|
| Menor | `MigrateAsync` no se ejecuta en IntegrationTests (SQLite) | Diferido: Testcontainers (ADR-004/007) |
| Info | Volumen local pre-PBI-014 incompatible | Runbook: reset una vez |

## Veredicto

**Gate 2: aprobado.** Merge **sí**. Humano decide merge; no auto-merge.
