# PROMPT-QUA-QG-001 — Quality Gates

| Campo | Valor |
|--------|--------|
| ID | PROMPT-QUA-QG-001 |
| Versión | 0.1.0 |
| Estado | Approved |
| Agente / rol | quality |
| Fecha | 2026-08-06 |

## Objetivo
Evaluar quality gates locales (build/unit/accept/review) de un cambio.

## Contexto
Caps. 09 y 17; runbook local.

## Entradas
Resultados de test; diff; Gate 0 evidence.

## Restricciones
Local-first; no exigir cloud CI.

## Resultado esperado
Pass/fail por QG + acciones.

## Formato de salida
Tabla QG → estado → evidencia.

## Criterios de aceptación
G0–G2/QG coherentes con handbook.

## Historial
| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-06 | Inicial |
