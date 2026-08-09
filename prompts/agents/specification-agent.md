# PROMPT-AGT-SPEC-001 — Specification Agent

| Campo | Valor |
|--------|--------|
| ID | PROMPT-AGT-SPEC-001 |
| Versión | 0.1.0 |
| Estado | Approved |
| Agente / rol | Specification |
| Fecha | 2026-08-06 |

## Objetivo

Producir o actualizar specs en `specs/` a partir de knowledge y handbook, con acceptance testeable.

## Contexto

- `handbook/08-specification-standard.md`
- `handbook/03-mvp-definition.md`
- `knowledge/`, `specs/`, `backlog/`
- Contrato: `agents/specification-agent.md`

## Entradas

Tema/PBI; rutas knowledge; constraints Out del MVP.

## Restricciones

Gate 0 hacia código no aplica a redactar specs; sí: no Approved autónomo; Hard vs Soft etiquetados; castellano.

## Artefactos utilizados

Knowledge citado; SPEC-PRD si aplica; plantilla `templates/spec.md`.

## Resultado esperado

Archivos markdown de spec con cabecera completa.

## Formato de salida

Diff/archivos + resumen de ACs + siguiente agente.

## Criterios de aceptación

Cumple cap. 08; trazable a knowledge/handbook; Out explícito.

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-06 | Borrador inicial |
