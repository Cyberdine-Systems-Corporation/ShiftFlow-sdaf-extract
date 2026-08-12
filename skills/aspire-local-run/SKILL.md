---
name: aspire-local-run
description: Arranca y verifica el runtime local Aspire/AppHost para demo o smoke. Usar al diagnosticar entorno o preparar demo MVP.
---

# aspire-local-run

| Campo | Valor |
|--------|--------|
| ID | aspire-local-run |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | media |
| Fecha | 2026-08-12 |
| Norma | [handbook/18](../../handbook/18-devops.md), AppHost en `src/ShiftFlow.AppHost` |

## Disparadores

- “Levantar local”, demo, smoke post-merge, fallo de puertos/Postgres.

## Pasos

1. Revisar runbooks/`docs/` y configuración AppHost (puertos; Postgres host **5433** si está fijado en el repo).
2. Arrancar desde solución Aspire/AppHost según README del proyecto.
3. Smoke mínimo: API health/status y Web login o home según exista.
4. Registrar en worklog: comando, URLs, fallos, remedios (sin secretos).
5. Si el fallo es de diseño (puerto, dependencia) → `adr-propose` o fix con PBI; no parches silenciosos sin traza.

## Definition of Done

- [ ] Entorno levantado o bloqueo documentado con causa.
- [ ] Smoke anotado en worklog.

## Restricciones

- No commitear connection strings con secretos.
- No inventar pipeline CI aquí (`devops-ci-gate`).

## Referencias

- `src/ShiftFlow.AppHost/`
- [handbook/03-mvp-definition.md](../../handbook/03-mvp-definition.md) (runtime local)
