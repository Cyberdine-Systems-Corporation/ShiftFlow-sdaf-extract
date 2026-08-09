# SPEC-APP-001 — Casos de uso de maestros (Org / Dept / Employee / ShiftType)

| Campo | Valor |
|--------|--------|
| ID | SPEC-APP-001 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-09 |
| Fuentes | SPEC-DOM-002, SPEC-DOM-003, SPEC-PRD-001 (C-ORG…C-STT), SPEC-PRD-002 AC-01 |
| ADRs relacionados | ADR-004 |
| Backlog | PBI-003, PBI-004, PBI-008 (consumo UI) |
| Derivados | SPEC-ACC-001, comandos/queries en Application, endpoints API |

---

## 1. Contexto

Casos de uso de aplicación (CQRS) para el núcleo de maestros del Sprint 1.  
Actor por defecto: **Administrator** autenticado (SPEC-DOM-004 / SPEC-APP-002).

---

## 2. Comandos

| Comando | Precondiciones | Postcondiciones | Errores observables |
|---------|----------------|-----------------|---------------------|
| `CreateOrganization` | Actor Administrator; Name válido | Organization activa creada y consultable | Name vacío / inválido |
| `RenameOrganization` | Org existe | Name actualizado | No encontrada; Name inválido |
| `SetOrganizationActive` | Org existe | IsActive actualizado | No encontrada |
| `CreateDepartment` | Org activa; Name único en org | Department creado | Org inexistente/inactiva; Name duplicado |
| `RenameDepartment` | Dept existe | Name actualizado | No encontrado; Name duplicado |
| `SetDepartmentActive` | Dept existe | IsActive actualizado | No encontrado |
| `CreateEmployee` | Dept existe y activo; DisplayName válido | Employee creado | Dept inválido; Email duplicado si aplica |
| `UpdateEmployee` | Employee existe | Campos actualizados; Dept misma Org | Violación INV-EMP-* |
| `SetEmployeeActive` | Employee existe | IsActive actualizado | No encontrado |
| `CreateShiftType` | Org activa; Name único | ShiftType creado | Violación INV-STT-* |
| `UpdateShiftType` | ShiftType existe | Campos actualizados | Violación INV-STT-* |
| `SetShiftTypeActive` | ShiftType existe | IsActive actualizado | No encontrado |

---

## 3. Consultas

| Query | Resultado |
|-------|-----------|
| `GetOrganizationById` / `ListOrganizations` | Detalle o lista |
| `ListDepartmentsByOrganization` | Departamentos de una org |
| `ListEmployeesByDepartment` / `ListEmployeesByOrganization` | Empleados |
| `ListShiftTypesByOrganization` | Catálogo de tipos |

Las consultas de administración requieren autenticación (SPEC-DOM-004).

---

## 4. Flujos

### 4.1 Alta de estructura mínima de demo

1. `CreateOrganization`  
2. `CreateDepartment`  
3. `CreateEmployee`  
4. `CreateShiftType`  

Resultado: datos listos para AC-01 (SPEC-PRD-002) y SPEC-ACC-001.

### 4.2 Alternativos

- Fallo de invariante → comando rechazado; no hay persistencia parcial del aggregate afectado.
- Organización inactiva → rechazo de altas hijas.

---

## 5. Criterios de aceptación (aplicación)

1. Cada comando de escritura pasa por Application (MediatR/slice); Domain no conoce HTTP.  
2. Los errores de invariante se mapean a respuestas API 4xx comprensibles (detalle de contrato HTTP puede refinarse en implementación).  
3. Tras el flujo 4.1, las queries listan los cuatro recursos.

---

## 6. Fuera de alcance

- Endpoints de calendario/asignación (Sprint 2).
- Import CSV, paginación avanzada, búsqueda full-text.

---

## 7. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | Approved tras revisión humana |
| 0.1.0 | 2026-08-09 | Draft Sprint 1 (Specification Agent) |
