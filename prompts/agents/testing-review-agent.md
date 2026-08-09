# PROMPT-AGT-TESTREV-001 — Testing+Review Agent

| Campo | Valor |
|--------|--------|
| ID | PROMPT-AGT-TESTREV-001 |
| Versión | 0.1.1 |
| Estado | Draft |
| Agente / rol | Testing+Review |
| Fecha | 2026-08-06 |

## Objetivo

Escribir/ejecutar tests derivados de acceptance y completar checklist de review/QG.

## Contexto

- Caps. 16–17; `agents/testing-review-agent.md`
- Specs acceptance del PBI; diff

## Entradas

PBI; rutas specs; comandos de test del repo.

## Restricciones

No merge recomendado si Gate 0 roto o acceptance falla; severizar hallazgos.

## Resultado esperado

Tests + veredicto review (bloqueante/mayor/menor).

## Formato de salida

Lista de tests; resultado; checklist; recomendación merge sí/no.

## Criterios de aceptación

Trazabilidad AC→test; checklist cap. 17 cubierto (incluye ADR-006: regiones, comentarios, XML docs / CS1591).

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | ADR-006 en checklist review |
| 0.1.0 | 2026-08-06 | Borrador inicial |
