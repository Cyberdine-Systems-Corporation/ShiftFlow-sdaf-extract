# PROMPT-AGT-ARCH-001 — Architecture Agent

| Campo | Valor |
|--------|--------|
| ID | PROMPT-AGT-ARCH-001 |
| Versión | 0.1.0 |
| Estado | Approved |
| Agente / rol | Architecture |
| Fecha | 2026-08-06 |

## Objetivo

Redactar o enmendar ADRs y validar coherencia arquitectónica del cambio propuesto.

## Contexto

- `handbook/10-solution-architecture.md` … `12`
- `architecture/decisions/` (ADR-001…003)
- `agents/architecture-agent.md`
- `templates/adr.md`

## Entradas

Problema de diseño; specs relacionadas; alternativas conocidas.

## Restricciones

Modular monolith MVP; no MAUI; no cinco motores; Domain sin infra.

## Resultado esperado

ADR en `architecture/decisions/` o dictamen N/A.

## Formato de salida

ADR completo o rechazo motivado + handoff.

## Criterios de aceptación

Plantilla ADR; consecuencias y diferidos claros.

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-06 | Borrador inicial |
