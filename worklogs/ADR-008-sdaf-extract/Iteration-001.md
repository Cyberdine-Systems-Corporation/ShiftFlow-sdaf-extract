# ADR-008 extracción SDAF — Iteration 001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-22 |
| Agente | Architecture / docs (humano vía chat) |
| Modelo | Cursor agent |
| Skills | `sdaf-agent-router@0.1.0` (contexto), planificación extracción |
| Contexto | Fork `ShiftFlow-sdaf-extract`; origin MVP intacto; opción C (lab → `sdaf-core`) |
| Archivos modificados | `architecture/decisions/ADR-008-extraccion-sdaf-core.md`; `docs/sdaf-extraction/**`; este worklog |
| Resultado | ADR Propuesto + inventario Core/Stack/Product + branch `chore/sdaf-extract` |
| Pruebas ejecutadas | N/A (solo docs) |
| Estado | hecho |
| Siguiente agente | humano (aprobar ADR-008 = paso 3); luego Architecture/docs para scrub y bootstrap `sdaf-core` |

## Notas

- Remoto: `https://github.com/Cyberdine-Systems-Corporation/ShiftFlow-sdaf-extract.git`
- No se ha tocado `src/` ni el repo origen ShiftFlow.
- Inventario canónico: `docs/sdaf-extraction/inventory-core-stack-product.md`
- MVP `sdaf-core@0.1.0` acotado en el inventario (sección final).

## Revisión paso 1 (2026-08-22)

- Clasificación Core/Stack/Product **confirmada**.
- Ajuste ADR/inventario v0.1.1: norma `specs/` y gobernanza de stack **dentro** del core; contenido de specs de ShiftFlow y stack concreto **fuera**.
- Pendiente paso 3: estado ADR-008 → Aceptado (humano).
- Paso 2: commit + push de esta rama.
