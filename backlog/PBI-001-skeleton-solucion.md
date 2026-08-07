# PBI-001 — Skeleton solución .NET + runtime local

| Campo | Valor |
|--------|--------|
| Sprint | 0 |
| Prioridad | 1 |
| Specs | SPEC-PRD-001 (C-LOC, C-API, C-WEB), ADR-001, ADR-004 |
| DoD | Solución compila; AppHost/Compose levanta Postgres (+ hosts); runbook mínimo |
| Estado | DoD cumplido (net10.0; AppHost+Postgres validados 2026-08-07) |

## Descripción

Crear la solución modular monolith (Domain, Application, Infrastructure, Api/Web, AppHost, Tests) sin features de negocio, lista para slices del Sprint 1.
