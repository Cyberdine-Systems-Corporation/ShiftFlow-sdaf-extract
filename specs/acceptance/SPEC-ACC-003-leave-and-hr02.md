# SPEC-ACC-003 — Aceptación Leave y HR-02 (PBI-007)

| Campo | Valor |
|--------|--------|
| ID | SPEC-ACC-003 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-12 |
| Fuentes | SPEC-PRD-002 AC-04, SPEC-DOM-007, SPEC-DOM-006 (HR-02), SPEC-APP-004, SPEC-APP-003 |
| ADRs relacionados | ADR-003, ADR-004 |
| Backlog | PBI-007; PBI-006 (cierre HR-02 en motor) |
| Derivados | tests Acceptance / Integration de Leave y rechazo por ausencia |

---

## 1. Contexto

Escenarios Given/When/Then de registro de ausencias y bloqueo de asignación (HR-02).  
Refinan AC-04 de SPEC-PRD-002 y el paso 7 del journey.

Precondición común: runtime local; Administrator autenticado; Organization con Employee activo (SPEC-ACC-001); Rule Engine invocado en `AssignShift` (SPEC-ACC-002).

**No** cubren HR-03 (descanso mínimo): PBI-006 / acceptance futura.

---

## 2. Escenarios

### ACC-S2-L01 Registrar leave

```text
Dado un Administrator autenticado y un Employee activo de la Organization
Cuando se ejecuta RegisterLeave con StartOn=EndOn=D (rango válido)
Entonces existe un Leave Active consultable (ListLeaves o calendario del mes de D)
```

### ACC-S2-L02 Leave bloquea asignación (HR-02)

```text
Dado un Leave Active del Employee E que cubre el día D y un ShiftType activo
Cuando se intenta AssignShift a E con un intervalo que cae en D
Entonces la asignación se rechaza con RuleViolation HR-02 (ausencia) y no se crea ShiftAssignment Assigned
```

### ACC-S2-L03 Asignación fuera del leave permitida

```text
Dado un Leave Active de E que cubre solo el día D
Cuando se ejecuta AssignShift a E en un intervalo íntegramente en D+1 (u otro día no cubierto) sin otros conflictos
Entonces la asignación se acepta
```

### ACC-S2-L04 Cancelar leave desbloquea

```text
Dado un Leave Active de E que cubre D y que provocaría HR-02
Cuando se ejecuta CancelLeave y después AssignShift a E en un intervalo en D sin otros conflictos
Entonces el Leave queda Cancelled y la asignación se acepta
```

### ACC-S2-L05 Rechazo estructural — EndOn < StartOn

```text
Dado un Employee activo
Cuando se intenta RegisterLeave con EndOn anterior a StartOn
Entonces la operación se rechaza por invariante estructural y no se persiste Leave
```

### ACC-S2-L06 Escritura anónima rechazada

```text
Dado un cliente sin autenticación
Cuando intenta RegisterLeave
Entonces la operación se rechaza y no se crea Leave
```

### ACC-S2-L07 HR-01 y HR-02 distinguibles

```text
Dado un Employee E con un ShiftAssignment Assigned en [10:00, 14:00) del día D y, en otro caso, solo un Leave Active que cubre D (sin solape previo)
Cuando se intenta AssignShift solapado (primer caso) o AssignShift bajo leave (segundo caso)
Entonces el rechazo expone códigos/tipos distintos (HR-01 vs HR-02)
```

---

## 3. Trazabilidad

| Escenario | Spec / AC |
|-----------|-----------|
| ACC-S2-L01 | C-LEA, SPEC-APP-004 RegisterLeave |
| ACC-S2-L02 | SPEC-PRD-002 AC-04, SPEC-DOM-006 HR-02 |
| ACC-S2-L03 | SPEC-DOM-007 §3.1 cobertura |
| ACC-S2-L04 | SPEC-DOM-007 INV-LEA-05, SPEC-APP-004 CancelLeave |
| ACC-S2-L05 | SPEC-DOM-007 INV-LEA-03 |
| ACC-S2-L06 | SPEC-DOM-004 / SPEC-APP-002 |
| ACC-S2-L07 | SPEC-DOM-006 HR-01 vs HR-02 |

---

## 4. Fuera de alcance

- HR-03 descanso mínimo.  
- Approve/Reject leave.  
- Journey E2E monolítico (PBI-009).  
- Autocancelación de turnos ya Assigned al registrar leave.

---

## 5. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-12 | Approved tras revisión humana |
| 0.1.0 | 2026-08-12 | Draft PBI-007 (Specification Agent) |
