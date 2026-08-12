# Backlog MVP (`mvp-0.1`)

Priorizado para el roadmap Approved (`handbook/04-product-roadmap.md`).  
Cada PBI debe enlazar specs; **no implementar** features de producto sin Gate 0 (spec Approved + acceptance + worklog; ADR si aplica).

## Orden de prioridad

| Orden | ID | Título | Sprint | Specs | Estado |
|------:|----|--------|--------|-------|--------|
| 1 | [PBI-001](PBI-001-skeleton-solucion.md) | Skeleton solución .NET + runtime local | 0 | SPEC-PRD-001 Approved (C-LOC, C-API, C-WEB), ADR-004 | Hecho (mergeado) |
| 2 | [PBI-002](PBI-002-auth-roles.md) | Auth y roles básicos | 1 | SPEC-DOM-004, SPEC-APP-002, SPEC-ACC-001, ADR-005 Approved | Hecho (mergeado) |
| 3 | [PBI-003](PBI-003-organization-department-employee.md) | Organization, Department, Employee | 1 | SPEC-DOM-001/002, SPEC-APP-001, SPEC-ACC-001 Approved | Hecho (mergeado) |
| 4 | [PBI-004](PBI-004-shift-types.md) | Shift types | 1 | SPEC-DOM-003, SPEC-APP-001, SPEC-ACC-001 Approved | Hecho (mergeado) |
| 5 | [PBI-005](PBI-005-calendar-assign-shift.md) | Calendario y asignación manual | 2 | SPEC-DOM-005/006, SPEC-APP-003, SPEC-ACC-002 Approved | Hecho (mergeado #18+#19; Gate 2 OK) |
| 6 | [PBI-006](PBI-006-rule-engine-v1.md) | Rule Engine v1 (3 hard rules) | 2 | SPEC-DOM-006/007 Approved, ADR-003 | Ready (HR-02/03) |
| 7 | [PBI-007](PBI-007-leaves.md) | Leaves / ausencias | 2 | SPEC-DOM-007, SPEC-APP-004, SPEC-ACC-003 Approved | Ready (Gate 0 OK) |
| 8 | [PBI-008](PBI-008-blazor-shell-crud.md) | Blazor shell + CRUD maestros | 1–2 | SPEC-PRD-002 | Hecho (mergeado #13; Gate 2 OK) |
| 9 | [PBI-009](PBI-009-acceptance-tests-demo.md) | Acceptance tests del journey | 2–3 | SPEC-PRD-002 | Tras reglas |
| 10 | [PBI-010](PBI-010-runbook-demo-freeze.md) | Runbook, seed opcional, freeze demo | 3 | SPEC-PRD-001 (C-LOC) | — |
| 11 | [PBI-011](PBI-011-ai-explain-stub.md) | Stub IA explicación de reglas | 3 | ADR-003, SPEC-PRD-001 | — |
| 12 | [PBI-012](PBI-012-presentacion-slides-video.md) | Slides + vídeo de presentación | 3 | SPEC-PRD-001 (C-PRE) | — |
| 13 | [PBI-013](PBI-013-ux-blazor-redesign.md) | Rediseño UX demo (Blazor) | 2–3 | SPEC-PRD-003 Approved | Hecho (mergeado; Gate 2 OK) |

Specs de producto **Approved** (incl. SPEC-PRD-003 NFR UI). Specs Sprint 1 maestros/auth **Approved**. Specs Sprint 2 calendario/asignación (**SPEC-DOM-005/006**, **SPEC-APP-003**, **SPEC-ACC-002**) **Approved**. Specs Leave (**SPEC-DOM-007**, **SPEC-APP-004**, **SPEC-ACC-003**) **Approved** — Gate 0 listo para PBI-007 / HR-02 (y PBI-006 HR-03).  
Gate 2 retroactivo documentado (2026-08-12): PBI-008 (#13), PBI-013 (#14), PBI-005 API (#18) — ver worklogs `*-testing-review-retro*`.

## Leyenda de estado Gate 0

| Estado | Significado |
|--------|-------------|
| Spec dominio pendiente | Falta `specs/domain` (+ app/acceptance) Approved |
| Ready… | Puede iniciarse trabajo técnico acotado |
