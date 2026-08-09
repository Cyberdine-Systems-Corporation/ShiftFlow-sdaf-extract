# SPEC-ACC-001 — Aceptación Sprint 1 (auth + maestros)

| Campo | Valor |
|--------|--------|
| ID | SPEC-ACC-001 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-09 |
| Fuentes | SPEC-PRD-002 AC-01, SPEC-DOM-002…004, SPEC-APP-001/002 |
| ADRs relacionados | ADR-001, ADR-004 |
| Backlog | PBI-002, PBI-003, PBI-004, PBI-008 |
| Derivados | tests Acceptance / Integration del Sprint 1 |

---

## 1. Contexto

Escenarios Given/When/Then del núcleo Sprint 1. Refinan AC-01 de SPEC-PRD-002 y añaden auth.  
No cubren calendario ni reglas (Sprint 2).

---

## 2. Escenarios

### ACC-S1-01 Login demo

```text
Dado el runtime local arrancado y el usuario demo.admin provisionado
Cuando inicia sesión con credenciales válidas
Entonces queda autenticado con rol Administrator
```

### ACC-S1-02 Rechazo anónimo en escritura

```text
Dado un cliente sin autenticación
Cuando intenta CreateOrganization (API o UI)
Entonces la operación se rechaza (no se crea la organización)
```

### ACC-S1-03 Alta de maestros

```text
Dado un Administrator autenticado
Cuando crea Organization, Department, Employee y ShiftType válidos
Entonces puede listarlos en API y/o UI Web
```

### ACC-S1-04 Unicidad de departamento

```text
Dado una Organization con un Department llamado "Urgencias"
Cuando se intenta crear otro Department "urgencias" en la misma Organization
Entonces la creación se rechaza por nombre duplicado
```

### ACC-S1-05 Employee coherente con Organization

```text
Dado Department D1 en Organization A y Department D2 en Organization B
Cuando se intenta crear o mover un Employee de D1 a D2 cruzando organizaciones de forma inconsistente
Entonces la operación se rechaza
```

### ACC-S1-06 ShiftType con horario inválido

```text
Dado un Administrator autenticado y una Organization activa
Cuando crea un ShiftType con DefaultStartTime=22:00 y DefaultEndTime=06:00
Entonces la creación se rechaza (overnight diferido; End debe ser > Start)
```

### ACC-S1-07 Logout

```text
Dado un Administrator autenticado
Cuando cierra sesión
Entonces las operaciones protegidas posteriores en esa sesión se rechazan
```

---

## 3. Trazabilidad

| Escenario | Specs |
|-----------|--------|
| ACC-S1-01, 02, 07 | SPEC-DOM-004, SPEC-APP-002 |
| ACC-S1-03 | SPEC-PRD-002 AC-01, SPEC-APP-001 |
| ACC-S1-04, 05 | SPEC-DOM-002 |
| ACC-S1-06 | SPEC-DOM-003 |

---

## 4. Fuera de alcance

- AC-02…AC-04 de SPEC-PRD-002 (asignación, solape, leave).
- UI pixel-perfect; solo observabilidad funcional.

---

## 5. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | Approved tras revisión humana |
| 0.1.0 | 2026-08-09 | Draft Sprint 1 (Specification Agent) |
