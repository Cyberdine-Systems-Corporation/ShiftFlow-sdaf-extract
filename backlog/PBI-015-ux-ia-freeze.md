# PBI-015 — Jerarquía UX freeze (Blazor Web)

| Campo | Valor |
|--------|--------|
| Sprint | 3 (pulido demo antes del freeze / etiqueta) |
| Prioridad | 15 |
| Specs | [SPEC-PRD-003](../specs/product/SPEC-PRD-003-ui-demo-nfr.md) v0.2.0 Approved (AC-UX-06…10; 01…05 vigentes) |
| DoD | AC-UX-06…10; worklog ATF; sin kit UI externo; sin cambio de API/Domain ni de SPEC-PRD-002 |
| Estado | Gate 2 OK; pendiente merge humano (#36) |

## Descripción

Reordenar la información en la UI demo: pestañas en detalle de organización, inventario antes que alta, calendario como protagonista, organización activa en el shell y copy de producto. No sustituye PBI-013 (tokens/shell); lo complementa.

## Notas

- Fuente canónica de NFR: SPEC-PRD-003. No duplicar en handbook/ADRs.
- ADR no requerido (CSS propio; ADR-002 Web-only sigue vigente).
- Kind de leave: literales de dominio (`Vacation` / `Other`); la UI solo muestra etiquetas en castellano.
- Freeze/tag (`PBI-010`) queda fuera de este slice.
