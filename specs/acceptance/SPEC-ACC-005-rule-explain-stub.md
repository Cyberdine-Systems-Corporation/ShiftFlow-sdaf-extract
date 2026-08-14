# SPEC-ACC-005 — Aceptación del stub de explicación de reglas (PBI-011)

| Campo | Valor |
|--------|--------|
| ID | SPEC-ACC-005 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-14 |
| Fuentes | SPEC-APP-005, SPEC-DOM-006, SPEC-PRD-002 pasos 6–8, handbook 03 §3 (IA stub), H16 |
| ADRs relacionados | ADR-003 |
| Backlog | PBI-011 |
| Derivados | Tests de aceptación/integración API; slice UI calendario (Frontend) |

---

## 1. Contexto

Escenarios Given/When/Then del **stub de explicación**.  
No sustituyen ACC-002/003/004 (el bloqueo HR-* ya está cubierto).  
Cobertura canónica: **API** (H16). La UI se cubre con un escenario observable en Blazor, sin Playwright obligatorio.

Precondición común salvo ACC-S3-X05: runtime local; Administrator autenticado.

---

## 2. Escenarios

### ACC-S3-X01 Explicación de HR-01 sin mutar

```text
Dado un Employee con un ShiftAssignment Assigned que provoca HR-01 al reasignar en solape
Cuando el cliente pide explicación de Code=HR-01 (ExplainRule y/o cuerpo de error de AssignShift)
Entonces recibe Title y Body en castellano que identifican el solape (HR-01)
Y no se crea un segundo Assigned
```

### ACC-S3-X02 Explicación de HR-02

```text
Dado un Leave Active que cubre el intervalo candidato (HR-02)
Cuando el cliente pide explicación de Code=HR-02
Entonces el Body identifica la ausencia / leave
Y no se persiste AssignShift en ese intervalo
```

### ACC-S3-X03 Explicación de HR-03

```text
Dado una Organization con umbral de descanso que provoca HR-03
Cuando el cliente pide explicación de Code=HR-03
Entonces el Body identifica el descanso mínimo
Y no se persiste la asignación rechazada
```

### ACC-S3-X04 Código no soportado

```text
Dado un Administrator autenticado
Cuando pide ExplainRule con Code distinto de HR-01, HR-02 y HR-03 (p. ej. HR-99)
Entonces recibe una explicación de código no soportado (sin inventar una hard rule)
Y MutatesSchedule es false / no hay escrituras
```

### ACC-S3-X05 Anónimo rechazado

```text
Dado un cliente sin autenticación
Cuando intenta ExplainRule
Entonces la operación se rechaza y no se expone explicación de negocio
```

### ACC-S3-X06 El stub no bypassea el Rule Engine

```text
Dado un conflicto HR-01 vigente
Cuando se obtiene una explicación y a continuación se reintenta el mismo AssignShift inválido
Entonces AssignShift sigue rechazando con Code=HR-01
Y el cuadrante no incorpora el turno
```

### ACC-S3-X07 UI calendario muestra la explicación

```text
Dado un Administrator en el calendario Web y un intento de asignación que viola HR-01 (u HR-02 / HR-03)
Cuando la Api rechaza la asignación
Entonces la UI muestra una explicación en castellano que identifica la regla
Y el mes no pinta el turno rechazado
```

---

## 3. Trazabilidad

| Escenario | Spec / AC |
|-----------|-----------|
| ACC-S3-X01 | SPEC-APP-005 §5.1, SPEC-PRD-002 paso 6, HR-01 |
| ACC-S3-X02 | SPEC-APP-005, SPEC-PRD-002 paso 7, HR-02 |
| ACC-S3-X03 | SPEC-APP-005, SPEC-PRD-002 paso 8, HR-03 |
| ACC-S3-X04 | SPEC-APP-005 §3 código desconocido |
| ACC-S3-X05 | SPEC-DOM-004 / SPEC-APP-002 |
| ACC-S3-X06 | ADR-003: explicación no escribe ni bypassea Evaluate |
| ACC-S3-X07 | SPEC-APP-005 §4 UI; Frontend PBI-011 |

---

## 4. Fuera de alcance

- LLM real / red a proveedor de IA.  
- Propuestas de cuadrante o persistencia asistida.  
- Playwright E2E (H16).  
- Re-especificar el bloqueo HR-* (ACC-002/003/004).

---

## 5. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-14 | Approved tras revisión humana |
| 0.1.0 | 2026-08-14 | Draft PBI-011 (Specification Agent) |
