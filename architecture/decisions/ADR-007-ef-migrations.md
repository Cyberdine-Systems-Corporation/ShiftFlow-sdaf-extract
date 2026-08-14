# ADR-007 — Evolución de esquema con migraciones EF Core

| Campo | Valor |
|--------|--------|
| Estado | Aceptado |
| Fecha | 2026-08-13 |
| Decisores | Director técnico / Architecture Agent |
| Relacionado | ADR-001, ADR-004 (diferido «Migraciones EF»), ADR-005, ADR-006, `handbook/10-solution-architecture.md`, `handbook/18-devops.md`, PBI-014, SPEC-PRD-001 (C-LOC) |

---

## Contexto

El MVP persiste en un PostgreSQL orquestado por Aspire/Compose (ADR-001, ADR-004). Hasta ahora el esquema se materializa con `Database.EnsureCreatedAsync` en el arranque (`IdentitySeed`).

`EnsureCreated`:

- no escribe `__EFMigrationsHistory`;
- no aplica cambios incrementales si la base ya existe;
- obliga a resetear el volumen Docker en cada PBI con tablas o columnas nuevas (`docs/runbook-local.md`).

ADR-004 difería explícitamente «Migraciones EF con datos de dominio (Sprint 1+)». El modelo de dominio del Sprint 1–2 ya está en Fluent API; hace falta un historial de esquema distinto de `EnsureCreated` sin introducir un segundo motor de migraciones.

Los tests de integración usan SQLite in-memory (`ShiftFlowApiFactory`). Las migraciones Npgsql no son aplicables ahí.

---

## Decisión

Adoptar **migraciones de EF Core** como única fuente de esquema en PostgreSQL.

1. **Ubicación.** `src/ShiftFlow.Infrastructure/Persistence/Migrations/`. Infrastructure sigue siendo dueño de EF (ADR-004).
2. **Baseline.** Una migración `InitialCreate` que captura el modelo actual (Identity + agregados de dominio). No se reconstruye el pasado PBI a PBI.
3. **Aplicación en runtime (MVP).** Al arrancar la Api, si el provider es Npgsql: `MigrateAsync`. El seed de Identity (rol `Administrator`, usuario demo) queda separado del esquema.
4. **Tests SQLite.** Si el provider es SQLite: `EnsureCreatedAsync`. No hay un segundo set de migraciones. Testcontainers Postgres permanece diferido (ADR-004).
5. **Diseño.** `IDesignTimeDbContextFactory<ShiftFlowDbContext>` para `dotnet ef` sin levantar Aspire. Herramienta local `dotnet-ef` en `.config/dotnet-tools.json`.
6. **Prohibido.** Mezclar `EnsureCreated` y `Migrate` sobre la misma base PostgreSQL. Un volumen creado con `EnsureCreated` debe resetearse **una vez** al adoptar este ADR.
7. **Arranque.** `MigrateAsync` en el host Api es aceptable en el MVP (demo local). Worker Aspire o `dotnet ef migrations bundle` quedan **fuera** de este ADR.
8. **XML docs (ADR-006).** Las clases públicas generadas por `dotnet ef` llevan `<summary>` breve. No se relaja CS1591 en el folder de migraciones.

`EnsureCreated` contra el volumen Aspire/Compose queda prohibido.

---

## Alternativas consideradas

| Alternativa | Motivo de rechazo (MVP) |
|-------------|-------------------------|
| Seguir con `EnsureCreated` | Sin historial; reset de volumen en cada cambio de esquema |
| `EnsureDeleted` + `EnsureCreated` | Borra datos en cada arranque; sigue sin historial |
| DbUp / Flyway / Evolve (SQL versionado aparte) | Segunda fuente de verdad frente al modelo Fluent API; ceremonia de más para el MVP |
| Worker Aspire solo de migraciones | Mejor aislamiento en prod; pieza extra innecesaria para demo local |
| Sets duales de migraciones (Npgsql + SQLite) | Mantenimiento alto; el modelo ya se mantiene provider-agnóstico en Fluent API |
| Testcontainers Postgres ya en este PBI | Deseable a medio plazo; fuera de alcance de cerrar el historial de esquema |

---

## Consecuencias

### Positivas

- El esquema evoluciona sin wipe del volumen cuando la migración es aditiva.
- `__EFMigrationsHistory` documenta qué revisión está aplicada.
- El runbook deja de pedir reset «si faltan tablas» como operación rutinaria.

### Negativas / costes

- Hay que commitear archivos generados y añadir migraciones en cada cambio de modelo.
- Un volumen local previo (EnsureCreated) es incompatible: un último reset.
- Tests de integración no ejercitan `MigrateAsync` (solo EnsureCreated en SQLite).

### Diferido explícitamente

- Worker Aspire / bundle de migraciones para no mutar esquema desde la Api.
- Testcontainers PostgreSQL en IntegrationTests (cierra el hueco SQLite).
- Política de migraciones en entornos compartidos o prod (fuera del MVP).

---

## Cumplimiento

- PBI-014 materializa esta decisión.
- Cualquier otro mecanismo de esquema sobre Postgres requiere enmendar este ADR.

---

## Historial

| Versión / fecha | Cambio |
|-----------------|--------|
| 2026-08-13 | Propuesto: migraciones EF Core + `MigrateAsync` en Npgsql; `EnsureCreated` solo en SQLite de tests |
| 2026-08-13 | Aceptado por Director técnico (encargo «Aprovar drafts»). |
