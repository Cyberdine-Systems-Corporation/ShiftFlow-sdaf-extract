---
name: rule-engine-hr
description: Guía cambios de reglas HR/Leave (HR-01/02/03), RuleEngine y proyección calendario vs mutación. Usar en Domain+Application al tocar ausencias o hard rules.
---

# rule-engine-hr

| Campo | Valor |
|--------|--------|
| ID | rule-engine-hr |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | baja |
| Fecha | 2026-08-12 |
| Norma | Specs Leave/HR (p. ej. SPEC-DOM Leave, SPEC-ACC-003); `RuleEngine` |

## Disparadores

- Leave register/cancel; Evaluate con `activeLeaves`; HR-02 bloqueo assign; futuro HR-03; proyección leaves en calendario.

## Pasos

1. Leer specs Approved del PBI (DOM/APP/ACC); respetar **Out** (p. ej. leave sobre turno Assigned permitido si la spec lo dice; sin autocancel).
2. Reglas en Domain (`RuleEngine` / aggregates); Application orquesta; API/UI no reinventan.
3. Distinguir: **mutación** (Assign/Register/Cancel) vs **proyección** calendario (`GetMonthCalendar`).
4. Tests unitarios de dominio + acceptance ACC citados; aplicar `csharp-adr006-slice`.
5. Si la regla es nueva (HR-03) → Gate 0 + specs antes de código.

## Definition of Done

- [ ] Comportamiento alineado a spec (In/Out).
- [ ] Tests ACC/unidad verdes pertinentes.
- [ ] Worklog con specs citadas.

## Restricciones

- No “arreglar” Out de producto inventando cancelaciones.
- No lógica HR solo en Blazor.

## Referencias

- `src/ShiftFlow.Domain/Rules/RuleEngine.cs`
- `src/ShiftFlow.Domain/Leaves/`
- `specs/acceptance/SPEC-ACC-003-leave-and-hr02.md` (y specs DOM/APP Leave)
