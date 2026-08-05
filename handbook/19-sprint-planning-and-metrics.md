# 19 — Sprint Planning and Metrics

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | VI — Operación |
| **Norma superior** | [04-product-roadmap.md](04-product-roadmap.md), [09-development-workflow.md](09-development-workflow.md), [15-agent-traceability.md](15-agent-traceability.md) |
| **Deriva hacia** | `backlog/`, worklogs, retrospectiva MVP |

---

## 1. Propósito

Operacionalizar el roadmap (cap. 04): cómo planificar sprints bajo capacidad real y qué métricas importan (sin vanity).

---

## 2. Capacidad

| Día | Horas |
|-----|------:|
| L–V | 5 |
| S–D | 3 |

La planificación de PBIs **no** puede asumir más de esta capacidad humana de supervisión/integración, aunque los agentes aceleren la generación.

---

## 3. Rituales mínimos

| Ritual | Cuándo | Salida |
|--------|--------|--------|
| Planificación de sprint | Inicio de sprint | PBIs con Gate 0 viable o plan para cerrarlo |
| Refinement | Según necesidad | Specs Draft → camino a Approved |
| Cierre diario breve | Fin de bloque | Worklog actualizado |
| Retrospectiva | 22 ago / fin MVP | Lecciones SDAF + producto |

No se impone ceremonial Scrum completo.

---

## 4. Priorización dentro del sprint

1. Cerrar Gate 0 de lo que se vaya a codear.  
2. Flujo demo / DoD del sprint.  
3. Deuda que bloquee el siguiente sprint.  
4. Pulido no bloqueante.

Política de recorte: capítulo 04 §6 (nunca SDAF gate, demo mínima, local, slides+vídeo).

---

## 5. Métricas (equilibradas)

| Métrica | Objetivo orientativo |
|---------|----------------------|
| % PBIs con spec + acceptance antes de código | ≥ 95 % |
| % commits de producto con worklog referenciado | ≥ 80 % (sprints 1–3) |
| Acceptance críticos del MVP verdes | 100 % del set definido |
| Horas reales vs plan | Alerta si desvío > ±20 % |
| Violaciones Gate 0 en merge/demo | 0 sin ADR de excepción |

No optimizar cobertura de líneas como KPI primario del MVP.

---

## 6. Informe de cierre MVP

Al `mvp-0.1` registrar brevemente:

- Métricas §5 obtenidas.
- Desvíos y recortes aplicados.
- Estado del handbook (qué Approved / Draft).
- Enlace a slides + vídeo.

---

## 7. Criterios de aceptación de este capítulo (H8)

- [ ] Capacidad y rituales son realistas.
- [ ] Métricas son auditables vía specs/worklogs/tests.
- [ ] Alineado al roadmap Approved.

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H8) |
