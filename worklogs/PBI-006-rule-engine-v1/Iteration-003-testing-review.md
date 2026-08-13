# PBI-006 — Testing+Review PR #28 — Iteration 003 (retro)

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-13 |
| Agente | Testing+Review |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-TESTREV-001@0.1.1 |
| Skills | `testing-review-pr@0.1.0`, `security-review-mvp@0.1.0`, `csharp-adr006-slice@0.1.0` |
| Contexto | Gate 2 del slice HR-03; PR #28 ya mergeado (`8c3e2f0`) — review retroactivo |
| Especificaciones utilizadas | SPEC-DOM-006 §2.3, SPEC-ACC-004, SPEC-PRD-002 paso 8; ADR-003/006; handbook 09/17/20 |
| Archivos leídos | RuleEngine, Organization, AssignShift, SetOrganizationMinimumRest, MasterDataEndpoints, OrganizationDetail.razor, CalendarAssignApiTests, SPEC-ACC-004 |
| Archivos modificados | este worklog; backlog PBI-006 → Hecho |
| Resultado | **Gate 2 OK** (0 bloqueantes). Merge ya realizado; el dictamen habría sido **sí**. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` → Unit 26 OK, Integration 26 OK; 0 `var` en src/tests |
| Estado | hecho |
| Siguiente agente | humano (PBI-009 journey) |

## Quality gates

| Gate | Resultado |
|------|-----------|
| QG-Build | Verde (CS1591 corregido en `MastersApiClient` antes del merge) |
| QG-Unit | Verde (26; HR-03 dominio + INV-ORG-02) |
| QG-Accept | Verde ACC-S2-R01/R02 (API); R03 cubierto por ACC-S2-04 + unit `HR03_no_aplica_si_umbral_cero` |
| QG-Arch | OK — HR-03 en Domain; AssignShift invoca Evaluate antes de persistir; comando de umbral no consulta |
| QG-Docs / ADR-006 | OK XML en API pública tocada; Organization con regiones; 0 `var` |
| QG-Sec | OK — `PUT .../minimum-rest` bajo grupo Organizations con `Administrator`; sin secretos; input entero validado en dominio (≥ 0) |
| QG-Review | Checklist §3 abajo |

## Checklist review (handbook 17 §3)

### Gobierno
- [x] Gate 0: SPEC-DOM-006 Approved; SPEC-ACC-004 Approved (Iteration-002); ADR-003 vigente; worklogs 001–002
- [x] Sin alcance Out (sin soft rules, sin compliance sectorial)
- [x] Worklog implementación + aprobación ACC citados

### Dominio / arquitectura
- [x] Regla en `RuleEngine`; umbral en `Organization`; UI solo llama API
- [x] Domain sin infra
- [x] `SetOrganizationMinimumRest` es comando; AssignShift muta tras Evaluate

### Calidad
- [x] Tests alineados a ACC-004
- [x] Lenguaje ubicuo (MinimumRestMinutes, HR-03)
- [x] Sin secretos
- [x] ADR-006 (tipado explícito, XML)

### Producto
- [x] Auth del grupo `/api/organizations` cubre el nuevo PUT
- [x] Runbook: `EnsureCreated` + reset de volumen (§6) sigue válido para columna nueva

### Seguridad (H20)
- [x] Sin secretos en el diff
- [x] Autorización coherente API/UI (`[Authorize]` en detalle org)
- [x] Sin SQL concatenado

## Trazabilidad ACC → test

| Escenario | Test |
|-----------|------|
| ACC-S2-R01 rechazo gap &lt; umbral | `ACC_S2_R01_rechazo_por_descanso_minimo_HR03` + unit `HR03_rechaza_gap_inferior_al_umbral` |
| ACC-S2-R02 gap = umbral permitido | `ACC_S2_R02_permite_gap_igual_al_umbral` + unit `HR03_permite_gap_igual_al_umbral` |
| ACC-S2-R03 umbral 0 / adyacentes | `ACC_S2_04_turnos_adyacentes_permitidos` + unit `HR03_no_aplica_si_umbral_cero` |

## Hallazgos

| Severidad | Hallazgo | Acción |
|-----------|----------|--------|
| Menor | ACC-S2-R01 no aserta que el calendario sigue con una sola asignación (el spec lo pide) | Deuda: añadir GET calendar en el test (PBI-009 o follow-up) |
| Menor | Colección Postman sin `PUT .../minimum-rest` | `postman-contract-sync` en PBI-009 o freeze demo |
| Menor | `RuleEngine` sin `#region` pese a ser tipo no trivial | Estilo; no bloquea (patrón previo al slice) |
| Info | Default umbral 0: demo oral de HR-03 exige configurar minutos en UI | Alineado a SPEC-DOM-006 / ACC-004; documentar en PBI-010 |

## Veredicto

**Gate 2 del slice HR-03: aprobado** (review tras merge de https://github.com/mortiz-iadev/ShiftFlow/pull/28).  
PBI-006 (HR-01+02+03) puede marcarse **Hecho**. Siguiente: PBI-009.
