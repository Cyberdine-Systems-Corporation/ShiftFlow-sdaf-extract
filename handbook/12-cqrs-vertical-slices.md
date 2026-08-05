# 12 — CQRS and Vertical Slices

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | III — Arquitectura |
| **Norma superior** | [10-solution-architecture.md](10-solution-architecture.md), [11-ddd-and-bounded-contexts.md](11-ddd-and-bounded-contexts.md) |
| **Deriva hacia** | `*.Application`, MediatR, tests por feature |

---

## 1. Propósito

Definir cómo se combinan **CQRS** y **Vertical Slice Architecture** en Application para el MVP: sin dos bases de datos, sin sobre-ingeniería.

---

## 2. CQRS en ShiftFlow (MVP)

| Aspecto | Decisión |
|---------|----------|
| Separación | Sí: **Commands** (escriben) y **Queries** (leen) como tipos distintos |
| Modelo de lectura | Proyecciones/DTOs vía queries; pueden usar EF `AsNoTracking` |
| Modelo de escritura | Aggregates de dominio + repositorios |
| Bus / dos SQL stores | **No** |
| Event sourcing | **No** |

CQRS aquí es **separación de responsabilidades y contratos**, no partición de infraestructura.

---

## 3. Vertical slices

Cada capacidad se organiza como **slice** (carpeta de feature), no solo por capa técnica horizontal.

Estructura orientativa dentro de Application:

```text
Application/
  Features/
    Employees/
      CreateEmployee/
        CreateEmployeeCommand.cs
        CreateEmployeeHandler.cs
        CreateEmployeeValidator.cs
      GetEmployee/
        GetEmployeeQuery.cs
        GetEmployeeHandler.cs
    Shifts/
      AssignShift/
        ...
      GetMonthCalendar/
        ...
```

Reglas:

1. Un slice agrupa lo necesario para ese use case (comando/query + handler + validator).
2. Código compartido entre slices solo con justificación (evitar “Common” prematuro).
3. El Domain permanece transversal y estable; los slices **orquestan**, no duplican invariantes.

---

## 4. MediatR (u opción ADR equivalente)

- Commands/Queries implementan contratos de MediatR (o pipeline compatible).
- Behaviors opcionales MVP: logging, validación, unit of work — sin pipeline barroco.
- Handlers de comando: cargan aggregate → mutan → persisten → publican domain events in-process.
- Handlers de query: no mutan dominio.

Si se elige otra librería, ADR obligatorio; el estilo slice+CQRS se mantiene.

---

## 5. Relación con specs

| Spec | Artefacto Application |
|------|------------------------|
| Use case (application spec) | Slice Command o Query |
| Acceptance | Test que ejercita API/handler/flujo |
| Domain rule | Inv limpia en Domain; el handler solo la dispara |

Nombrar commands/queries alineados al lenguaje ubicuo (`AssignShift`, no `UpdateTableRows`).

---

## 6. Features MVP (mapa inicial de slices)

Orientativo; el backlog prioriza:

| Área | Commands (ej.) | Queries (ej.) |
|------|----------------|---------------|
| Organización | CreateOrganization, … | GetOrganization, List… |
| Departamento | CreateDepartment, … | ListDepartments |
| Empleado | CreateEmployee, … | GetEmployee, List… |
| ShiftType | CreateShiftType, … | ListShiftTypes |
| Calendario / turno | AssignShift, CancelShift, … | GetMonthCalendar |
| Leave | RegisterLeave, … | ListLeaves |
| Auth/roles | Register/Login según ADR | CurrentUser |

---

## 7. Anti-patrones

| Anti-patrón | Mitigación |
|-------------|------------|
| God ApplicationService por entidad | Un handler por use case |
| Query que muta | Prohibido |
| Command que devuelve vista compleja | Devolver id/ack; consultar con query |
| Slice que salta Domain y escribe con SQL ad hoc en reglas de negocio | Reglas en Domain; infra solo persiste |
| Micro-handlers triviales ×100 sin specs | Spec primero; agrupar solo si el use case lo es |

---

## 8. Criterios de aceptación de este capítulo (H6)

- [ ] CQRS sin dos bases ni event sourcing queda explícito.
- [ ] Vertical slices son la organización de Application.
- [ ] Commands mutan dominio; queries no.
- [ ] MediatR (o ADR equivalente) encaja en el estilo.

---

## 9. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H6) |
