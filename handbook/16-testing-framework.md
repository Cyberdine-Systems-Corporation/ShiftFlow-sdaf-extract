# 16 — Testing Framework

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | V — Calidad y entrega |
| **Norma superior** | [08-specification-standard.md](08-specification-standard.md), [09-development-workflow.md](09-development-workflow.md), [03-mvp-definition.md](03-mvp-definition.md) |
| **Deriva hacia** | `tests/`, Gate 2–3, [17-code-review-and-quality-gates.md](17-code-review-and-quality-gates.md) |

---

## 1. Propósito

Definir la estrategia de pruebas del MVP: pirámide, origen en specs, herramientas y qué es bloqueante para demo.

---

## 2. Principio rector

Los tests de aceptación se **derivan** de `specs/acceptance/` (y criterios en specs Approved).  
Un test que no traza a spec es útil como caracterización, pero no sustituye acceptance del PBI.

---

## 3. Pirámide (MVP)

| Nivel | Qué cubre | Herramientas |
|-------|-----------|--------------|
| Unitario (Domain) | Invariantes, Rule Engine v1, VOs | xUnit, FluentAssertions |
| Application | Handlers con dobles/fakes cuando aporte | xUnit |
| Integración | EF + PostgreSQL real | Testcontainers |
| Aceptación / API | Flujo crítico del MVP | xUnit (+ HTTP client) o equivalente ADR |
| UI E2E | Opcional en MVP si el tiempo aprieta | Playwright solo si no pone en riesgo el DoD |

Prioridad: **Domain + acceptance del flujo demo** > UI E2E exhaustivo.

---

## 4. Cobertura mínima bloqueante (DoD MVP)

Deben existir y estar verdes:

1. Tests de las ≤3 hard rules (solape, ausencia, descanso mínimo).
2. Acceptance del flujo: maestros → asignar válido → rechazar inválido → leave bloquea.
3. Al menos un smoke de arranque/persistencia con Postgres (Testcontainers o entorno local documentado).

---

## 5. Organización en `tests/`

Orientativa (ADR de solución puede ajustar nombres):

```text
tests/
  ShiftFlow.Domain.Tests/
  ShiftFlow.Application.Tests/
  ShiftFlow.Architecture.Tests/   # opcional: NetArchTest / similares
  ShiftFlow.Acceptance.Tests/
```

Nombrar tests según escenario de acceptance (`AssignShift_WhenOverlap_ShouldReject`).

---

## 6. Datos de prueba

- Builders/mothers en tests; evitar BD compartida sucia entre pruebas.
- Seed de demo (runtime) ≠ fixtures de test; no acoplarlos sin querer.

---

## 7. Criterios de aceptación de este capítulo (H8)

- [ ] Tests desde specs queda normativo.
- [ ] Pirámide y cobertura bloqueante del MVP son claras.
- [ ] UI E2E no es puerta única del DoD.

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H8) |
