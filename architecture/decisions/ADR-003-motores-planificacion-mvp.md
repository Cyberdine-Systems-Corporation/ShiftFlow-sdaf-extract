# ADR-003 — Motores de planificación del MVP

| Campo | Valor |
|--------|--------|
| Estado | Aceptado |
| Fecha | 2026-08-05 |
| Decisores | Director técnico / Architecture Agent |
| Relacionado | `handbook/03-mvp-definition.md`, `handbook/10-solution-architecture.md`, `handbook/11-ddd-and-bounded-contexts.md`, ADR-001 |

---

## Contexto

Se propuso dividir el sistema en cinco motores (Scheduling, Rule, Compliance, Optimization, AI Recommendation).  
Para el MVP eso es sobre-diseño: un solo bounded context (**WorkforceScheduling**), asignación manual y ≤3 hard rules.

Hay que fijar qué motores existen de verdad en `mvp-0.1` y cuáles se difieren.

---

## Decisión

### Incluidos en el MVP

| Motor | Responsabilidad | Forma |
|-------|-----------------|--------|
| **Scheduling Engine** | Ciclo de vida de calendario/turnos; **asignación manual** | Lógica de Domain + use cases Application |
| **Rule Engine v1** | Evaluar hard rules que bloquean asignaciones inválidas | Mecanismo **dentro** del BC WorkforceScheduling (no microservicio, no BC aparte) |

Hard rules implementadas en MVP (máximo tres):

1. No solapes de turno para la misma persona.  
2. Ausencia (leave) bloquea asignación.  
3. Descanso mínimo configurable entre turnos.

### Diferidos (Out del MVP como motores separados)

| Motor | Tratamiento MVP |
|-------|-----------------|
| Compliance Engine | Absorbido en Rule Engine v1; separar solo con ADR futuro si crece |
| Optimization Engine | No implementar |
| AI Recommendation Engine | **No** como motor de escritura; solo **stub de infraestructura** para explicación de reglas (sin mutar cuadrante) |

### Reglas de evolución

- Soft preferences y reglas avanzadas del knowledge (`Domain-Specs-V1.docx`) se documentan en specs; no se implementan salvo las hard rules anteriores.
- Abrir un nuevo motor o BC requiere ADR + necesidad demostrada (cap. 11).

---

## Alternativas consideradas

| Alternativa | Motivo de rechazo (MVP) |
|-------------|-------------------------|
| Cinco motores como proyectos/servicios | Complejidad y coste de integración injustificados |
| Auto-scheduling / optimización en MVP | Fuera del DoD; riesgo de scope creep |
| IA que genera y persiste cuadrantes | Viola principio de producto (IA asistente, confirmación humana) |
| Rule engine externo (Drools, etc.) | Exceso de infra para ≤3 reglas |

---

## Consecuencias

### Positivas

- Alcance acotado y demostrable (asignar / rechazar / leave).
- Un solo modelo mental: dominio + evaluación de reglas.
- Preparado para ampliar reglas vía specs sin inventar microservicios.

### Negativas / costes

- No hay optimización ni generación automática en la demo.
- Compliance “de marca” no existe como módulo aparte (puede confundir a quien espere cinco cajas).

### Cumplimiento

- Los slices `AssignShift` (y equivalentes) **deben** invocar Rule Engine v1 antes de persistir.
- Tests de acceptance del flujo crítico cubren las tres hard rules.
