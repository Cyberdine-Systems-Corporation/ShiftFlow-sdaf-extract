# SPEC-APP-004 — Casos de uso de Leave

| Campo | Valor |
|--------|--------|
| ID | SPEC-APP-004 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-12 |
| Fuentes | SPEC-DOM-007, SPEC-DOM-006 (HR-02), SPEC-APP-003, SPEC-PRD-001 (C-LEA), SPEC-PRD-002 AC-04, `handbook/12-cqrs-vertical-slices.md` |
| ADRs relacionados | ADR-003, ADR-004 |
| Backlog | PBI-007; PBI-006 (HR-02 en AssignShift) |
| Derivados | SPEC-ACC-003, slices Application, endpoints API, UI Blazor (registro / listado / calendario) |

---

## 1. Contexto

Casos de uso CQRS para registrar y consultar ausencias.  
Actor: **Administrator** autenticado (SPEC-DOM-004 / SPEC-APP-002).

HR-02 se ejercita en el comando existente `AssignShift` (SPEC-APP-003): tras existir Leave, `RuleEngine.Evaluate` **debe** considerar Leaves `Active`.

---

## 2. Comandos

| Comando | Precondiciones | Postcondiciones | Errores observables |
|---------|----------------|-----------------|---------------------|
| `RegisterLeave` | Actor Administrator; Employee válido (INV-LEA-*); EndOn ≥ StartOn | Leave `Active` persistido; consultable; participa en HR-02 | Violación INV-LEA-*; no autenticado |
| `CancelLeave` | Leave `Active` existe; mismo permiso | Status=`Cancelled`; ya no bloquea ni aparece como activo | No encontrado; ya cancelado |

No hay `ApproveLeave` / `RejectLeave` en MVP.

---

## 3. Consultas

| Query | Parámetros | Resultado |
|-------|------------|-----------|
| `ListLeaves` | OrganizationId; filtros opcionales EmployeeId, mes/año | Leaves (mínimo los `Active`) del alcance pedido |
| `GetMonthCalendar` (existente) | OrganizationId, Year, Month | Además de asignaciones, puede incluir Leaves `Active` que intersectan el mes (SPEC-DOM-005/007) sin romper clientes que ignoren el campo nuevo |

Consultas de planificación requieren autenticación (mismo rol MVP).

---

## 4. Flujos

### 4.1 Registro y bloqueo (demo paso 7 / AC-04)

1. `RegisterLeave` para Employee E, fechas que cubren el día D.  
2. `AssignShift` a E con intervalo que cae en D → rechazo **HR-02** (RuleViolation por ausencia); no se persiste asignación.  
3. `ListLeaves` / calendario muestra el Leave activo.

### 4.2 Cancelación desbloquea

1. Leave `Active` que bloqueaba D.  
2. `CancelLeave`.  
3. `AssignShift` válido en D (sin otros conflictos HR-01/HR-03) → se acepta.

### 4.3 Alternativos estructurales

- Employee inactivo u otra Organization → rechazo INV-LEA-* sin efectos en el Rule Engine.  
- EndOn < StartOn → rechazo INV-LEA-03.  
- Escritura anónima → rechazo de autenticación/autorización.

---

## 5. Criterios de aceptación (aplicación)

1. `RegisterLeave` / `CancelLeave` / `ListLeaves` viven como slices Application; Domain no conoce HTTP.  
2. Tras `RegisterLeave`, un `AssignShift` en cobertura activa recibe RuleViolation **HR-02** (código/tipo observable distinto de HR-01).  
3. Tras `CancelLeave`, el mismo intervalo deja de fallar por HR-02 (salvo otro Leave activo).  
4. Errores estructurales de Leave se distinguen de RuleViolation.

---

## 6. Fuera de alcance

- Aprobación / rechazo multi-paso.  
- Edición in-place de fechas (MVP: cancelar y registrar de nuevo).  
- Notificaciones, SignalR, autoservicio empleado.  
- Implementación de HR-03 (PBI-006).

---

## 7. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-12 | Approved tras revisión humana |
| 0.1.0 | 2026-08-12 | Draft PBI-007 (Specification Agent) |
