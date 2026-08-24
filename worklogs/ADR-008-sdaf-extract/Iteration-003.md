# ADR-008 extracción SDAF — Iteration 003

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-24 |
| Agente | Architecture / docs |
| Modelo | Cursor agent |
| Skills | `sdaf-worklog-handoff@0.1.0` |
| Contexto | ADR-008 Aceptado; arranque de scrub hacia `sdaf-core@0.1.0` |
| Archivos modificados | `sdaf-core/**`; `docs/sdaf-extraction/README.md`; este worklog |
| Resultado | Árbol candidato Draft: handbook método, templates, skills core, agentes/prompts genéricos, schema de config |
| Pruebas ejecutadas | Grep de ShiftFlow/Blazor/ADR-006 en `sdaf-core/` (solo menciones de laboratorio en README) |
| Estado | hecho |
| Siguiente agente | humano (revisar Draft del handbook del core); luego publicar repo `sdaf-core` o enmiendas |

## Notas

- Producto del fork (`src/`, `specs/` pobladas, Parte I) **no** se ha borrado: el laboratorio sigue siendo clon completo + árbol `sdaf-core/`.
- Handbook del core en **Draft** (no se auto-aprueba la constitución extraída).
- Fuera de v0.1: Partes V, pack stack, template de proyecto, CLI.
