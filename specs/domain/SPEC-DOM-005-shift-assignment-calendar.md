# SPEC-DOM-005 — ShiftAssignment y calendario mensual

| Campo | Valor |
|--------|--------|
| ID | SPEC-DOM-005 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-10 |
| Fuentes | SPEC-PRD-001 (C-CAL, C-ASN), SPEC-PRD-002 AC-02/AC-03, SPEC-DOM-001…003, `handbook/03-mvp-definition.md` §7, `handbook/11-ddd-and-bounded-contexts.md`, `handbook/12-cqrs-vertical-slices.md`, ADR-003 |
| ADRs relacionados | ADR-001, ADR-003, ADR-004 |
| Backlog | PBI-005 |
| Derivados | SPEC-APP-003, SPEC-ACC-002; SPEC-DOM-006 (reglas hard del Rule Engine) |

---

## 1. Contexto

Modelo de dominio del **Scheduling Engine** (ADR-003): ciclo de vida de turnos asignados y proyección de calendario mensual.  
La asignación es **manual** (sin optimización). Antes de persistir una asignación, Application **debe** invocar el Rule Engine v1 (SPEC-DOM-006).

---

## 2. Glossary (enmienda Sprint 2)

Complementa SPEC-DOM-001:

| Término | Definición | Sinónimos prohibidos en Domain |
|---------|------------|--------------------------------|
| **ShiftAssignment** | Turno concreto asignado a un Employee en un intervalo temporal, tipificado por ShiftType. | Shift row, Booking, Appointment |
| **CalendarMonth** | Proyección de lectura de ShiftAssignment (y más adelante Leaves) para una Organization en un mes civil. | Schedule grid (salvo UI) |
| **RuleViolation** | Resultado de evaluación hard que bloquea una asignación; incluye código de regla y mensaje observable. | Error genérico sin traza de regla |

**No** es aggregate de escritura el “calendario”: es vista/query sobre asignaciones (y ausencias en PBI-007).

---

## 3. Aggregate ShiftAssignment

| Campo conceptual | Regla |
|------------------|--------|
| Id | Identidad estable |
| OrganizationId | Obligatorio; coherente con Employee y ShiftType |
| EmployeeId | Obligatorio; Employee existente, activo, de la misma Organization |
| ShiftTypeId | Obligatorio; ShiftType existente, activo, de la misma Organization (INV-STT-05) |
| StartAt | Instantánea/local de inicio del turno (DateTimeOffset o Date+Time acordado en implementación; sin overnight en MVP) |
| EndAt | Fin del turno; **EndAt > StartAt** |
| Status | `Assigned` \| `Cancelled` (MVP); cancelado no cuenta para solapes futuros |

Notas MVP:

- Un turno ocupa un único intervalo continuo (sin multi-día fragmentado).
- Zona horaria: una por Organization **diferida**; MVP asume reloj homogéneo local del runtime.
- Overnight (End ≤ Start cruzando medianoche): **Out** (alineado a SPEC-DOM-003).

---

## 4. Invariantes estructurales (Hard)

| ID | Regla | Hard/Soft | Sprint / PBI |
|----|-------|-----------|--------------|
| INV-ASN-01 | Organization, Employee y ShiftType existen y pertenecen a la misma Organization | Hard | 2 / PBI-005 |
| INV-ASN-02 | Employee activo al asignar | Hard | 2 / PBI-005 |
| INV-ASN-03 | ShiftType activo al asignar (INV-STT-05) | Hard | 2 / PBI-005 |
| INV-ASN-04 | EndAt > StartAt (sin overnight) | Hard | 2 / PBI-005 |
| INV-ASN-05 | Actor autenticado con permiso de planificación (Administrator, SPEC-DOM-004) | Hard | 2 / PBI-005 |
| INV-ASN-06 | Cancelación solo sobre asignación `Assigned` existente | Hard | 2 / PBI-005 |

Las hard rules de negocio del Rule Engine (**solape**, **leave**, **descanso mínimo**) **no** se duplican aquí: viven en SPEC-DOM-006.  
Scheduling rechaza si el Rule Engine devuelve alguna RuleViolation.

---

## 5. Calendario mensual (lectura)

| Concepto | Regla |
|----------|--------|
| Alcance | Organization + año-mes (calendario gregoriano) |
| Contenido mínimo | ShiftAssignment con Status=`Assigned` cuyo intervalo intersecta el mes |
| Orden | Por StartAt ascendente (estable) |
| Leaves | Fuera de PBI-005; se añaden en PBI-007 sin romper el contrato de proyección |

Criterio de producto (C-CAL): la vista permite iniciar una asignación manual (C-ASN).

---

## 6. Criterios de aceptación (dominio)

1. No se persiste ShiftAssignment que viole INV-ASN-01…04.  
2. No se persiste ShiftAssignment si el Rule Engine reporta RuleViolation.  
3. Cancelar una asignación la deja fuera de la proyección de calendario y de evaluaciones de solape posteriores.  
4. GetMonthCalendar (aplicación) solo refleja asignaciones `Assigned` del mes pedido.

---

## 7. Fuera de alcance

- Optimización / auto-scheduling / IA que escribe cuadrantes.  
- Cambios de turno entre empleados, pools, plantillas multi-día.  
- Leaves (PBI-007) y hard rules leave/descanso (detalle en SPEC-DOM-006 / PBI-006).  
- Colores obligatorios, drag-and-drop, vistas semana/día como DoD.

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-10 | Approved tras revisión humana |
| 0.1.0 | 2026-08-10 | Draft PBI-005 (Specification Agent) |
