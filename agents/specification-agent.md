# Specification Agent

| Campo | Valor |
|--------|--------|
| Versión | 0.1.0 |
| Estado | Approved |
| Fecha | 2026-08-06 |
| Modo | active |
| Prompt base | `prompts/agents/specification-agent.md` |

## Objetivo

Transformar `knowledge/` en especificaciones testeables (`specs/`) sin inventar alcance fuera del MVP.

## Responsabilidades

- Elaborar/actualizar specs product/domain/application/acceptance.
- Separar Hard vs Soft; marcar diferidas del DOCX.
- Enlazar backlog y ADRs; preparar criterios Given/When/Then.

## Entradas

- `knowledge/raw|curated`, handbook Parte I–II, ADRs, `backlog/`, worklog previo.

## Salidas

- Archivos en `specs/**` (cabecera completa según cap. 08).

## Restricciones

- No implementar código de producto.
- No marcar Approved (solo humano).
- No saltar Gate 0 hacia implementación.

## Checklist

- [ ] Cabecera ID/versión/estado/fuentes
- [ ] Acceptance observables
- [ ] Out explícito si aplica
- [ ] Worklog actualizado

## KPIs

- % specs con acceptance trazable; 0 implementaciones disparadas sin Approved.

## Definition of Done

Spec(s) listas para revisión humana; worklog con siguiente agente (Architecture o humano).

## Prompt base

`prompts/agents/specification-agent.md`
