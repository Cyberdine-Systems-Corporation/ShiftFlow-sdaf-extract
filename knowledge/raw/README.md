# Knowledge — raw (append-only)

Fuentes primarias de expertos y artefactos históricos **sin reinterpretar**.

Según el handbook (cap. 07): esta carpeta es inmutable en intención. No se “corrige” al experto aquí; las interpretaciones van a `specs/` y las extracciones estructuradas a `knowledge/curated/`.

## Inventario (2026-08-05)

| Archivo | Origen | Notas |
|---------|--------|--------|
| `Domain-Specs-V1.docx` | `IA Project/Domain Specs.V1.docx` | Documento funcional de reglas de turnos (contratos, noches, bolsa, etc.). **No implementar tal cual** — transformar vía pipeline de dominio. |
| `2026-07-ShiftFlow-Engineering-Handbook-seed.md` | Borrador externo jul 2026 | Semilla del Product Charter; la constitución vigente es `handbook/`. |
| `2026-07-ShiftFlow-AI-Engineering-Master-Prompt.md` | Master prompt v1.0 | Semilla del rol Director técnico; la biblioteca vigente irá en `prompts/`. |
| `2026-07-ShiftFlow-Plan-consolidado.md` | Plan consolidado jul 2026 | Contexto histórico; el roadmap vigente es `handbook/04-product-roadmap.md`. |

## Reglas

1. Añadir solo con fecha/nombre claro; no sobrescribir silenciosamente.
2. Si hay nueva versión de un DOCX, añadir archivo nuevo (p. ej. `Domain-Specs-V2.docx`) y dejar el anterior.
3. No borrar entradas para “alinearlas” con el código.
