# SPEC-APP-005 — Stub de explicación de reglas

| Campo | Valor |
|--------|--------|
| ID | SPEC-APP-005 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-14 |
| Fuentes | SPEC-PRD-001 (C-RUL), SPEC-PRD-002 pasos 6–8, SPEC-DOM-006, `handbook/03-mvp-definition.md` §3 (IA stub), `handbook/10-solution-architecture.md` §5, `handbook/13-ai-agent-framework.md` §2, `skills/product-ia-prompt` |
| ADRs relacionados | ADR-003 (AI Recommendation: stub de explicación; no escribe cuadrante) |
| Backlog | PBI-011 |
| Derivados | SPEC-ACC-005, slice Application (query + puerto), adaptador Infrastructure, UI calendario (Frontend) |

---

## 1. Contexto

El Rule Engine v1 ya **bloquea** asignaciones inválidas (`RuleViolation` con código `HR-01` / `HR-02` / `HR-03` y mensaje corto; SPEC-APP-003).  
PBI-011 añade una **capa de explicación** para el planificador: texto en castellano, más legible que el mensaje de dominio, **sin mutar** el cuadrante.

Esto es **IA de producto** (adaptador en Infrastructure), no un agente de ingeniería del repo (H13 §2). En el MVP el adaptador es un **stub determinista** (plantillas por código de regla). No se exige un LLM real.

Actor: **Administrator** autenticado (SPEC-DOM-004 / SPEC-APP-002).

```text
AssignShift → RuleEngine.Evaluate → si violación: NO persistir
  → (opcional) IRuleExplanation.Explain(code, contexto) → texto al cliente
ExplainRule (query) → IRuleExplanation.Explain → texto; cero escrituras
```

---

## 2. Puerto de aplicación

| Puerto | Responsabilidad |
|--------|-----------------|
| `IRuleExplanation` (nombre orientativo) | Dado un código de hard rule y contexto opcional, devolver una explicación. **No** persiste agregados. **No** invoca `AssignShift` / `CancelShift` / `RegisterLeave`. **No** sustituye `RuleEngine.Evaluate`. |

Implementación MVP: clase en **Infrastructure** (stub). Domain permanece dueño de las hard rules.

### 2.1 Entrada

| Campo | Obligatorio | Notas |
|-------|-------------|--------|
| `Code` | Sí | Código estable (`HR-01`, `HR-02`, `HR-03`) |
| `OrganizationId` | No | Permite contextualizar (p. ej. umbral HR-03) |
| `EmployeeId` | No | No se usa para escribir; solo para redactar |
| Intervalo candidato | No | `StartAt` / `EndAt` si el cliente los tiene |

### 2.2 Salida

| Campo | Notas |
|-------|--------|
| `Code` | Eco del código pedido (o el reconocido) |
| `Title` | Título corto en castellano |
| `Body` | Párrafo(s) que identifican la regla, el porqué del rechazo y una pista de qué cambiar. No propone un nuevo turno persistible. |
| `MutatesSchedule` | Siempre `false` en MVP |

Misma entrada → misma salida (stub determinista; testeable).

---

## 3. Consulta

| Query | Precondiciones | Postcondiciones | Errores observables |
|-------|----------------|-----------------|---------------------|
| `ExplainRule` | Actor Administrator; `Code` no vacío | Explicación devuelta; **ningún** cambio en ShiftAssignment, Leave ni maestros | No autenticado; `Code` vacío |

`ExplainRule` es **solo lectura**. Un código no reconocido no inventa una hard rule nueva: explicación de «código no soportado en el stub» y `MutatesSchedule=false`.

---

## 4. Relación con `AssignShift`

`AssignShift` sigue siendo el único camino que evalúa y (si ok) persiste.

Al rechazar por `RuleViolation` HR-*:

1. El código y el mensaje corto de dominio **siguen** siendo observables (SPEC-APP-003).
2. El handler **puede** adjuntar `Title`/`Body` del mismo puerto `IRuleExplanation` en el error de aplicación/API, para que la demo no exija un segundo round-trip.
3. Adjuntar explicación **no** cambia el resultado del Rule Engine ni autoriza persistir.

La UI de calendario (Frontend, PBI-011) muestra esa explicación al usuario cuando el rechazo es HR-*.

---

## 5. Flujos

### 5.1 Rechazo + explicación (demo pasos 6–8)

1. `AssignShift` viola HR-01, HR-02 o HR-03 → no hay nueva fila `Assigned`.
2. El cliente recibe el código de regla y una explicación en castellano (en el error y/o vía `ExplainRule`).
3. El calendario del mes no incluye el turno rechazado.

### 5.2 Consulta directa

1. Administrator llama `ExplainRule` con `Code=HR-01` (contexto opcional).
2. Recibe título y cuerpo; el estado de planificación es idéntico al de antes de la query.

### 5.3 Alternativos

- Anónimo → rechazo de autenticación; sin explicación de negocio.
- Código desconocido → cuerpo de fallback; sin alta de reglas.

---

## 6. Criterios de aceptación (aplicación)

1. `ExplainRule` vive como slice Application (query); el stub vive en Infrastructure; Domain no conoce HTTP ni el adaptador de explicación.
2. Ni `ExplainRule` ni la generación de texto en el rechazo de `AssignShift` persisten asignaciones, leaves ni maestros.
3. La explicación de `HR-01` / `HR-02` / `HR-03` identifica la regla correspondiente (solape, ausencia, descanso mínimo) en castellano.
4. Un código desconocido no se trata como hard rule nueva ni como autorización para asignar.
5. `AssignShift` sigue bloqueando igual que SPEC-APP-003 / SPEC-DOM-006 aunque exista el stub.

---

## 7. Fuera de alcance

- LLM real, RAG, MCP o llamadas de red a un proveedor de IA.
- Propuestas de turnos, auto-scheduling u Optimization Engine.
- Mutar o bypassear el Rule Engine.
- Soft preferences; reglas avanzadas del DOCX.
- Explicar invariantes estructurales `INV-ASN-*` / `INV-LEA-*` (opcional post-MVP).
- UI E2E Playwright (H16: API primero; UI Blazor en el slice Frontend del mismo PBI).

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-14 | Approved tras revisión humana |
| 0.1.0 | 2026-08-14 | Draft PBI-011 (Specification Agent) |
