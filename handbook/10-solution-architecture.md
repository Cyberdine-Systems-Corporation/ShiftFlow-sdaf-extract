# 10 — Solution Architecture

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | III — Arquitectura |
| **Norma superior** | Parte I (MVP), Parte II (SDAF), [01-product-charter.md](01-product-charter.md) |
| **Deriva hacia** | [11-ddd-and-bounded-contexts.md](11-ddd-and-bounded-contexts.md), [12-cqrs-vertical-slices.md](12-cqrs-vertical-slices.md), ADRs, `src/` |

---

## 1. Propósito

Definir la arquitectura de solución del MVP: estilo, capas, componentes, motores, runtime local y límites explícitos.  
El detalle de DDD y de CQRS/slices está en los capítulos 11 y 12. Las elecciones cerradas viven en ADRs.

---

## 2. Estilo arquitectónico (MVP)

| Decisión | Valor |
|----------|--------|
| Estilo | Modular monolith |
| Organización lógica | Clean Architecture + Vertical Slices en Application |
| Persistencia | Un PostgreSQL; EF Core en Infrastructure |
| UI | Blazor Web App (única superficie de cliente en MVP) |
| API | ASP.NET Core (Minimal APIs o controllers — ADR) |
| Orquestación local | .NET Aspire AppHost y/o Docker Compose |
| Mensajería distribuida | No (sin bus externo en MVP) |
| Multitenancy avanzado | No |

Un solo deployable lógico de aplicación (+ base de datos) para la demo local.

---

## 3. Diagrama lógico

```text
┌─────────────────────────────────────────────┐
│ Presentation                                 │
│  Blazor Web App  →  ASP.NET API              │
└──────────────────────┬──────────────────────┘
                       │
┌──────────────────────▼──────────────────────┐
│ Application (Vertical Slices + MediatR)      │
│  Commands / Queries / Handlers / Validators  │
└──────────────────────┬──────────────────────┘
                       │
┌──────────────────────▼──────────────────────┐
│ Domain (WorkforceScheduling)                 │
│  Aggregates · VOs · Domain Events            │
│  Ports: Rule evaluation · Scheduling ops     │
└───────────────┬──────────────────────────────┘
                │
┌───────────────▼──────────────────────────────┐
│ Infrastructure                                │
│  EF Core · PostgreSQL · Serilog · AI stub    │
│  (Identity/auth adapters)                    │
└──────────────────────────────────────────────┘
```

Dependencias: Presentation → Application → Domain ← Infrastructure.  
El Domain **no** referencia EF, Blazor, OpenAI ni ASP.NET.

---

## 4. Proyectos esperados en `src/` (orientativo)

Nombres finales vía ADR de solución; estructura mínima:

| Proyecto | Rol |
|----------|-----|
| `*.AppHost` | Aspire: orquesta API, Web, Postgres |
| `*.Web` | Blazor Web App |
| `*.Api` | Host HTTP / composition root (puede unificarse con Web si ADR lo justifica) |
| `*.Application` | Slices CQRS |
| `*.Domain` | Modelo y puertos de dominio |
| `*.Infrastructure` | EF, AI stub, implementaciones de puertos |
| `*.ServiceDefaults` | Defaults Aspire/Serilog (si aplica) |

`tests/`: unitarios de dominio, integración con Testcontainers, acceptance del flujo crítico.

---

## 5. Motores (MVP)

| Motor | Responsabilidad MVP | Ubicación |
|-------|---------------------|-----------|
| **Scheduling** | Asignación manual, ciclo de vida del turno/calendario | Domain + Application |
| **Rule Engine v1** | Evaluar ≤3 hard rules (solape, ausencia, descanso mínimo) | Domain (puertos/servicios de dominio); sin DSL externo |
| Compliance | Absorbido en Rule Engine | — |
| Optimization | Out | — |
| AI Recommendation | Stub infra: explicación; no escribe cuadrante | Infrastructure |

No se crean microservicios por motor.

---

## 6. Cross-cutting

| Concern | Enfoque MVP |
|---------|-------------|
| Auth / roles | Básico (ASP.NET Identity u opción ADR equivalente) |
| Logging | Serilog |
| Validación de entrada | En Application (FluentValidation u opción ADR) |
| Errores de dominio | Resultados/excepciones de dominio explícitas; mapeo en API |
| IA | Solo adaptadores; prompts de producto versionados cuando existan |

---

## 7. Runtime local (arquitectura de despliegue MVP)

```text
Máquina evaluador
  ├── Docker / Aspire
  │     ├── PostgreSQL
  │     ├── Api (+ Web) 
  └── Runbook (docs/ o README)
```

Prohibido como arquitectura de cierre: “solo funciona en cloud”.

---

## 8. ADRs mínimos a registrar (Sprint 0/1)

1. Stack .NET / Postgres / Aspire  
2. Web-only (diferir MAUI)  
3. Motores MVP (Rule + Scheduling; resto diferido)  
4. Layout de solución y composición Api/Web  
5. Estrategia de auth básica  

---

## 9. Fuera de esta arquitectura (MVP)

- Event bus, CQRS físico con dos bases, microservicios.
- Redis, SignalR, OTel completo.
- MAUI, offline, multitenancy avanzado.

---

## 10. Criterios de aceptación de este capítulo (H6)

- [ ] Modular monolith + Clean + slices queda normativo para el MVP.
- [ ] Un BC y dos motores (Rule + Scheduling) sin microservicios.
- [ ] Runtime local es parte de la arquitectura de solución, no un afterthought.
- [ ] Lista de ADRs mínimos es aceptable.

---

## 11. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H6) |
