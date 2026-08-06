# SPEC-PRD-002 — Journey de demo del MVP

| Campo | Valor |
|--------|--------|
| ID | SPEC-PRD-002 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-05 |
| Fuentes | `handbook/03-mvp-definition.md` §7–§8 |
| ADRs relacionados | ADR-001, ADR-002, ADR-003 |
| Backlog | PBI-010 (demo E2E), depende de PBI-001…009 |
| Derivados | `specs/acceptance/` (cuando se detallen), tests Acceptance |

---

## 1. Contexto

Journey que un evaluador debe completar en **menos de 15 minutos** sobre runtime local (C-LOC), sin cloud.

---

## 2. Precondiciones

- Stack arrancado según runbook (API + Blazor Web + PostgreSQL).  
- Usuario con rol que permita administrar y planificar (C-AUTH).

---

## 3. Flujo principal

| Paso | Acción | Resultado observable |
|------|--------|----------------------|
| 1 | Crear o abrir una organización | Organización visible |
| 2 | Crear departamento y al menos un empleado | Empleado listado bajo la org/dept |
| 3 | Crear al menos un tipo de turno | Tipo disponible para asignar |
| 4 | Abrir calendario mensual | Vista del mes con capacidad de asignación |
| 5 | Asignar un turno válido a un empleado | Turno visible en calendario; sin error de regla |
| 6 | Intentar asignación que viole solape (misma persona, solapada) | Sistema rechaza; se comunica la violación |
| 7 | Registrar un leave y reintentar asignación en fechas bloqueadas | Sistema rechaza por ausencia |
| 8 | (Opcional en demo oral) Mostrar rechazo por descanso mínimo si hay datos | Violación de descanso comunicada |
| 9 | Identificar que existe API detrás del flujo | Endpoint(s) o evidencia de API REST usable |

---

## 4. Criterios de aceptación (Given / When / Then)

### AC-01 Organización y maestros

```text
Dado un usuario autenticado con permisos de administración
Cuando crea organización, departamento, empleado y tipo de turno
Entonces puede consultarlos en la UI Web
```

### AC-02 Asignación válida

```text
Dado un empleado sin conflictos en el intervalo
Cuando se asigna un turno válido
Entonces el turno queda registrado y visible en el calendario mensual
```

### AC-03 Rechazo por solape

```text
Dado un empleado con un turno ya asignado en un intervalo
Cuando se intenta asignar otro turno solapado
Entonces la asignación se rechaza y se informa la regla de solape
```

### AC-04 Leave bloquea

```text
Dado un leave activo que cubre una fecha
Cuando se intenta asignar un turno en esa fecha al mismo empleado
Entonces la asignación se rechaza por ausencia
```

### AC-05 Runtime local

```text
Dado solo prerrequisitos locales (SDK .NET, Docker) y el runbook del repo
Cuando se sigue el arranque documentado
Entonces el journey AC-01…AC-04 puede ejecutarse sin desplegar en cloud
```

---

## 5. Fuera de alcance

- Optimización automática, generación por IA, MAUI, colaboración SignalR.

---

## 6. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial Sprint 0 |
