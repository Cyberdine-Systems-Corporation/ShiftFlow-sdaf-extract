# SPEC-DOM-006 — Rule Engine v1 (hard rules)

| Campo | Valor |
|--------|--------|
| ID | SPEC-DOM-006 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-10 |
| Fuentes | SPEC-PRD-001 §2.2 (C-RUL), SPEC-PRD-002 AC-03/AC-04, ADR-003, SPEC-DOM-005, `handbook/03-mvp-definition.md`, `knowledge/raw/Domain-Specs-V1.docx` (filtrado Out) |
| ADRs relacionados | ADR-003 |
| Backlog | PBI-006 (completo); **HR-01** requerido por AssignShift en PBI-005 |
| Derivados | SPEC-APP-003 (invocación), SPEC-ACC-002 (solape); acceptance leave/descanso en PBI-006/007 |

---

## 1. Contexto

Rule Engine v1 es un **mecanismo dentro** del BC WorkforceScheduling (no microservicio ni BC aparte).  
Evalúa hard rules que **bloquean** asignaciones. Soft preferences del DOCX: documentadas como diferidas, no implementadas.

Contrato conceptual:

```text
Evaluate(candidate ShiftAssignment, contexto de plan) → ok | RuleViolation[]
```

`AssignShift` (y equivalentes) **debe** invocar esta evaluación **antes** de persistir (ADR-003).

---

## 2. Hard rules MVP (máximo tres)

| ID | Regla | Bloquea si | Mensaje observable (mínimo) | PBI |
|----|-------|------------|------------------------------|-----|
| **HR-01** | No solape de turnos para la misma persona | Existe ShiftAssignment `Assigned` del mismo Employee cuyo intervalo se solapa con `[StartAt, EndAt)` del candidato | Violación de solape (misma persona, intervalo solapado) | PBI-005 (mínimo) / PBI-006 |
| **HR-02** | Leave activo bloquea asignación | Existe Leave activo del mismo Employee que cubre cualquier instante del intervalo candidato | Violación por ausencia | PBI-006 + PBI-007 |
| **HR-03** | Descanso mínimo entre turnos | Tiempo entre el fin de un turno `Assigned` adyacente y el inicio del candidato (o viceversa) es &lt; umbral configurable de la Organization | Violación de descanso mínimo | PBI-006 |

### 2.1 Solape (HR-01) — detalle

- Intervalos semiabiertos recomendados: `[StartAt, EndAt)`; dos turnos que se tocan en el borde (A.End == B.Start) **no** solapan.
- Solo Status=`Assigned` participa.
- Comparación por `EmployeeId`.

### 2.2 Leave (HR-02)

- Depende del aggregate Leave (PBI-007). Hasta entonces el motor puede exponer el hook y devolver vacío para HR-02.
- Criterio de producto: AC-04 de SPEC-PRD-002.

### 2.3 Descanso mínimo (HR-03)

- Umbral: valor configurable por Organization (p. ej. minutos); default de producto a fijar en implementación (≥ 0).
- Demo oral puede omitir si no hay datos (SPEC-PRD-002 paso 8 opcional).

---

## 3. Soft / diferidas (Out de código MVP)

Etiqueta **Implementación: diferida (DOCX / Out MVP)**:

- Preferencias blandas, pares/impares, bolsa mensual de horas, tipologías sectoriales fijas, fairness scoring, compliance engine separado.

---

## 4. Criterios de aceptación (dominio)

1. Si HR-01 detecta solape, `Evaluate` no es ok y no se persiste la asignación.  
2. Varias violaciones pueden devolverse juntas; al menos una basta para rechazar.  
3. Sin violaciones y con invariantes estructurales OK (SPEC-DOM-005), la asignación puede persistirse.  
4. HR-02/HR-03 no se inventan con datos mock en dominio: requieren modelo Leave / config explícitos cuando se activen.

---

## 5. Fuera de alcance

- Motor de optimización, IA que muta el cuadrante.  
- Reglas soft como bloqueo.  
- Motor Compliance/Optimization separados (ADR-003).

---

## 6. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-10 | Approved tras revisión humana |
| 0.1.0 | 2026-08-10 | Draft (HR-01…03); HR-01 acoplado a PBI-005 AssignShift |
