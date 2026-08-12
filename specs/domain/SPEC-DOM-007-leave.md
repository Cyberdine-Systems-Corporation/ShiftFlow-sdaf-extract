# SPEC-DOM-007 — Leave (ausencias / vacaciones)

| Campo | Valor |
|--------|--------|
| ID | SPEC-DOM-007 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-12 |
| Fuentes | SPEC-PRD-001 (C-LEA, C-RUL), SPEC-PRD-002 AC-04 / paso 7, SPEC-DOM-002, SPEC-DOM-005, SPEC-DOM-006 §2.2, `handbook/03-mvp-definition.md` §4.1/§7, `handbook/11-ddd-and-bounded-contexts.md`, `handbook/12-cqrs-vertical-slices.md`, `knowledge/raw/2026-07-ShiftFlow-Plan-consolidado.md` (filtrado Out) |
| ADRs relacionados | ADR-003, ADR-004 |
| Backlog | PBI-007 (modelo + registro); PBI-006 (activación HR-02) |
| Derivados | SPEC-APP-004, SPEC-ACC-003; Rule Engine HR-02; proyección CalendarMonth |

---

## 1. Contexto

Aggregate **Leave**: ausencia o vacación de un Employee que **bloquea** nuevas asignaciones en el rango cubierto (hard rule HR-02, SPEC-DOM-006).

MVP = **gestión básica**: registrar y cancelar; sin workflow de aprobación.  
El knowledge consolidado menciona `ApproveLeave` / `RejectLeave` / eventos `LeaveApproved` — **Out** de implementación MVP (documentado aquí; no inventar alcance).

---

## 2. Glossary (enmienda)

Complementa SPEC-DOM-001 / SPEC-DOM-005:

| Término | Definición | Sinónimos prohibidos en Domain |
|---------|------------|--------------------------------|
| **Leave** | Ausencia o vacación registrada de un Employee que cubre un intervalo de fechas civiles y, si está activa, participa en HR-02. | Vacation request (salvo UI), Time-off ticket, PTO balance |
| **LeaveCoverage** | Intervalo temporal derivado de las fechas del Leave usado por el Rule Engine para comparar con el candidato `ShiftAssignment`. | — |

---

## 3. Aggregate Leave

| Campo conceptual | Regla |
|------------------|--------|
| Id | Identidad estable |
| OrganizationId | Obligatorio; coherente con Employee |
| EmployeeId | Obligatorio; Employee existente de la misma Organization |
| StartOn | Fecha civil de inicio (inclusive) |
| EndOn | Fecha civil de fin (inclusive); **EndOn ≥ StartOn** |
| Status | `Active` \| `Cancelled` (MVP) |
| Kind | Opcional (`Vacation` \| `Other` u otro literal acotado); **no** afecta HR-02 |
| Reason | Texto opcional corto (demo / auditoría mínima) |

### 3.1 Cobertura temporal (LeaveCoverage)

Para evaluar HR-02 contra un candidato `[StartAt, EndAt)`:

- El Leave `Active` cubre desde `StartOn 00:00:00` hasta el instante exclusivo `EndOn + 1 día 00:00:00` en el reloj homogéneo del runtime (misma convención que SPEC-DOM-005: sin TZ por Organization en MVP).
- HR-02 viola si **cualquier instante** del intervalo candidato cae dentro de esa cobertura (intersección no vacía).

Ejemplo: Leave `StartOn=2026-08-15`, `EndOn=2026-08-15` bloquea un turno el día 15; un turno que empieza el 16 a las 00:00 **no** queda cubierto.

### 3.2 Notas MVP

- Un Leave es un único rango continuo de fechas (sin fragmentos).
- Varios Leaves `Active` del mismo Employee pueden solaparse entre sí; todos cuentan para HR-02.
- Registrar un Leave **no** cancela ni modifica `ShiftAssignment` ya `Assigned` en el rango (solo bloquea **nuevas** asignaciones). Limpieza retroactiva: **Out**.

---

## 4. Invariantes estructurales (Hard)

| ID | Regla | Hard/Soft | Sprint / PBI |
|----|-------|-----------|--------------|
| INV-LEA-01 | Organization y Employee existen y pertenecen a la misma Organization | Hard | 2 / PBI-007 |
| INV-LEA-02 | Employee activo al registrar | Hard | 2 / PBI-007 |
| INV-LEA-03 | EndOn ≥ StartOn | Hard | 2 / PBI-007 |
| INV-LEA-04 | Actor autenticado con permiso de planificación (Administrator, SPEC-DOM-004) | Hard | 2 / PBI-007 |
| INV-LEA-05 | Cancelación solo sobre Leave `Active` existente | Hard | 2 / PBI-007 |

La regla de negocio **HR-02** no se duplica aquí: vive en SPEC-DOM-006 y se evalúa en `AssignShift`.

---

## 5. Relación con calendario y Rule Engine

| Concepto | Regla |
|----------|--------|
| CalendarMonth | Puede incluir Leaves `Active` cuyo rango de fechas intersecta el mes (además de asignaciones); no rompe el contrato de proyección de SPEC-DOM-005 |
| Rule Engine | Con Leave disponible, HR-02 deja de ser no-op: `Evaluate` debe consultar Leaves `Active` del Employee |

---

## 6. Criterios de aceptación (dominio)

1. No se persiste Leave que viole INV-LEA-01…03.  
2. Solo Status=`Active` participa en HR-02 y en la proyección de ausencias del calendario.  
3. Cancelar un Leave lo excluye de HR-02 y de la proyección como activo.  
4. Un Leave `Active` cuya cobertura intersecta el intervalo candidato provoca RuleViolation HR-02 (vía Rule Engine).

---

## 7. Fuera de alcance

- Workflow `ApproveLeave` / `RejectLeave` y estados Pending/Approved/Rejected del plan consolidado.  
- Saldos, bolsas de horas, tipos sectoriales, medias jornadas con granularidad horaria propia.  
- Autocancelación o conflicto automático con turnos ya asignados.  
- Autoservicio del empleado; roles granulares por departamento.  
- HR-03 (descanso mínimo): PBI-006, no este aggregate.

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-12 | Approved tras revisión humana |
| 0.1.0 | 2026-08-12 | Draft PBI-007 (Specification Agent) |
