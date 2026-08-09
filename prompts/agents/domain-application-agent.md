# PROMPT-AGT-DOMAPP-001 — Domain+Application Agent

| Campo | Valor |
|--------|--------|
| ID | PROMPT-AGT-DOMAPP-001 |
| Versión | 0.1.1 |
| Estado | Approved |
| Agente / rol | Domain+Application |
| Fecha | 2026-08-06 |

## Objetivo

Implementar dominio y slices CQRS según specs Approved y ADRs.

## Contexto

- Caps. 11–12; ADR-001/003; `agents/domain-application-agent.md`
- Specs Approved del PBI; `src/` skeleton cuando exista

## Entradas

PBI; rutas de specs; límites del aggregate/slice.

## Restricciones

**STOP** si Gate 0 incompleto. Sin UI. Sin hard rules extra. Castellano en mensajes/commits.

## Resultado esperado

Código Domain/Application + notas de tests necesarios.

## Formato de salida

Resumen de cambios + archivos + handoff Frontend/Testing+Review.

## Criterios de aceptación

Invariantes en Domain; command/query separados; worklog citado `PROMPT-AGT-DOMAPP-001@0.1.1`; cumplimiento **ADR-006** (regiones conceptuales, comentarios de impacto, XML docs; build sin CS1591).

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | ADR-006 legibilidad / XML docs |
| 0.1.0 | 2026-08-06 | Borrador inicial |
