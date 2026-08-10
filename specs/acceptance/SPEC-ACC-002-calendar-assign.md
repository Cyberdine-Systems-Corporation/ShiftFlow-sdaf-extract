# SPEC-ACC-002 — Aceptación calendario y asignación (PBI-005)

| Campo | Valor |
|--------|--------|
| ID | SPEC-ACC-002 |
| Versión | 0.1.0 |
| Estado | Draft |
| Fecha | 2026-08-10 |
| Fuentes | SPEC-PRD-002 AC-02/AC-03, SPEC-DOM-005, SPEC-DOM-006 (HR-01), SPEC-APP-003 |
| ADRs relacionados | ADR-003, ADR-004 |
| Backlog | PBI-005 |
| Derivados | tests Acceptance / Integration del calendario y AssignShift |

---

## 1. Contexto

Escenarios Given/When/Then del calendario mensual y la asignación manual.  
Refinan AC-02 y AC-03 de SPEC-PRD-002.  
**No** cubren leave (AC-04) ni descanso mínimo (paso 8 opcional): PBI-006/007.

Precondición común: runtime local; Administrator autenticado; Organization con Department, Employee activo y ShiftType activo (SPEC-ACC-001).

---

## 2. Escenarios

### ACC-S2-01 Abrir calendario mensual

```text
Dado un Administrator autenticado y una Organization existente
Cuando consulta el calendario del mes M (GetMonthCalendar / UI Calendario)
Entonces obtiene una vista del mes M sin error (lista vacía permitida)
```

### ACC-S2-02 Asignación válida

```text
Dado un Employee activo sin turnos Assigned en el intervalo I y un ShiftType activo de la misma Organization
Cuando se ejecuta AssignShift para ese Employee, tipo e intervalo I válido
Entonces existe un ShiftAssignment Assigned consultable y visible en el calendario del mes que contiene I
```

### ACC-S2-03 Rechazo por solape (HR-01)

```text
Dado un Employee con un ShiftAssignment Assigned en [10:00, 14:00) del día D
Cuando se intenta AssignShift al mismo Employee en [12:00, 16:00) del día D
Entonces la asignación se rechaza con RuleViolation de solape y no se crea un segundo Assigned
```

### ACC-S2-04 Turnos adyacentes permitidos

```text
Dado un Employee con ShiftAssignment Assigned en [10:00, 14:00) del día D
Cuando se ejecuta AssignShift al mismo Employee en [14:00, 18:00) del día D
Entonces la asignación se acepta (no hay solape en el borde)
```

### ACC-S2-05 Rechazo estructural — ShiftType inactivo

```text
Dado un ShiftType inactivo de la Organization y un Employee activo sin conflictos
Cuando se intenta AssignShift usando ese ShiftType
Entonces la operación se rechaza por invariante estructural (no se persiste)
```

### ACC-S2-06 Cancelar asignación

```text
Dado un ShiftAssignment Assigned visible en el calendario del mes M
Cuando se ejecuta CancelShift sobre esa asignación
Entonces deja de aparecer como Assigned en GetMonthCalendar de M
```

### ACC-S2-07 Escritura anónima rechazada

```text
Dado un cliente sin autenticación
Cuando intenta AssignShift
Entonces la operación se rechaza y no se crea asignación
```

---

## 3. Trazabilidad

| Escenario | Spec / AC |
|-----------|-----------|
| ACC-S2-01 | C-CAL, SPEC-APP-003 GetMonthCalendar |
| ACC-S2-02 | SPEC-PRD-002 AC-02 |
| ACC-S2-03 | SPEC-PRD-002 AC-03, SPEC-DOM-006 HR-01 |
| ACC-S2-04 | SPEC-DOM-006 §2.1 borde |
| ACC-S2-05 | SPEC-DOM-005 INV-ASN-03 |
| ACC-S2-06 | SPEC-DOM-005 INV-ASN-06 |
| ACC-S2-07 | SPEC-DOM-004 / SPEC-APP-002 |

---

## 4. Fuera de alcance

- AC-04 leave, HR-02/HR-03.  
- Journey E2E completo en un solo test (PBI-009).  
- NFR visual SPEC-PRD-003 (ya cubierto por PBI-013).

---

## 5. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-10 | Draft PBI-005 (Specification Agent) |
