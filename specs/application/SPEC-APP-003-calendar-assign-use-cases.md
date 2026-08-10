# SPEC-APP-003 — Casos de uso de calendario y asignación

| Campo | Valor |
|--------|--------|
| ID | SPEC-APP-003 |
| Versión | 0.1.0 |
| Estado | Draft |
| Fecha | 2026-08-10 |
| Fuentes | SPEC-DOM-005, SPEC-DOM-006, SPEC-PRD-001 (C-CAL, C-ASN, C-RUL), SPEC-PRD-002 AC-02/AC-03, `handbook/12-cqrs-vertical-slices.md` |
| ADRs relacionados | ADR-003, ADR-004 |
| Backlog | PBI-005 (calendario + AssignShift); PBI-006 (completar Rule Engine) |
| Derivados | SPEC-ACC-002, slices Application, endpoints API, UI Blazor calendario |

---

## 1. Contexto

Casos de uso CQRS del Scheduling Engine y de la consulta de calendario mensual.  
Actor: **Administrator** autenticado (SPEC-DOM-004 / SPEC-APP-002).

Orchestración de escritura:

```text
Validar entrada → cargar referencias → construir candidato ShiftAssignment
  → RuleEngine.Evaluate → si ok, persistir → si no, rechazar con RuleViolation
```

---

## 2. Comandos

| Comando | Precondiciones | Postcondiciones | Errores observables |
|---------|----------------|-----------------|---------------------|
| `AssignShift` | Actor Administrator; Employee y ShiftType válidos (INV-ASN-*); intervalo válido | ShiftAssignment `Assigned` persistido y visible en `GetMonthCalendar` | Violación estructural; **RuleViolation** (p. ej. HR-01 solape); no autenticado |
| `CancelShift` | Asignación `Assigned` existe; mismo permiso | Status=`Cancelled`; ya no aparece como asignada en calendario | No encontrada; ya cancelada |

`AssignShift` **debe** invocar Rule Engine v1 antes de persistir (ADR-003 / SPEC-DOM-006).

---

## 3. Consultas

| Query | Parámetros | Resultado |
|-------|------------|-----------|
| `GetMonthCalendar` | OrganizationId, Year, Month | Lista de asignaciones `Assigned` que intersectan el mes (y metadatos mínimos: empleado, tipo, intervalo) |

Consultas de planificación requieren autenticación (mismo rol MVP).

---

## 4. Flujos

### 4.1 Asignación válida (demo paso 5)

1. `GetMonthCalendar` (mes objetivo).  
2. `AssignShift` con empleado sin conflictos y tipo activo.  
3. `GetMonthCalendar` muestra el nuevo turno.

### 4.2 Rechazo por solape (demo paso 6)

1. Empleado con turno `Assigned` en un intervalo.  
2. `AssignShift` solapado → rechazo; **no** hay nueva fila `Assigned`.  
3. El cliente recibe identificación de regla de solape (HR-01).

### 4.3 Alternativos estructurales

- Employee/ShiftType inactivo o de otra Organization → rechazo INV-ASN-* sin llamar a reglas de negocio (o short-circuit antes de Evaluate).  
- EndAt ≤ StartAt → rechazo INV-ASN-04.

---

## 5. Criterios de aceptación (aplicación)

1. `AssignShift` / `CancelShift` / `GetMonthCalendar` viven como slices Application; Domain no conoce HTTP.  
2. Ningún `AssignShift` exitoso omite la evaluación del Rule Engine.  
3. Errores de regla se distinguen de errores de validación estructural en la respuesta de aplicación/API (códigos o tipos observables).  
4. Tras 4.1, la query de calendario incluye el turno; tras 4.2, el estado previo se conserva.

---

## 6. Fuera de alcance

- `RegisterLeave` / listado de leaves (PBI-007).  
- HR-02/HR-03 completos (PBI-006; HR-02 necesita Leave).  
- Drag-and-drop, edición in-place masiva, notificaciones SignalR.  
- Paginación avanzada del mes (MVP: mes completo razonable para demo).

---

## 7. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-10 | Draft PBI-005 (Specification Agent) |
