# PBI-011-ai-explain-stub / Iteration-005-testing-review

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-14 |
| Agente | Testing+Review |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | `PROMPT-AGT-TESTREV-001@0.1.1` |
| Skills | `testing-review-pr@0.1.0`, `csharp-adr006-slice@0.1.0`, `security-review-mvp@0.1.0`, `sdaf-worklog-handoff@0.1.0` |
| Contexto | Gate 2 de PBI-011: API mergeada (#34) + UI abierta (#35). |
| Especificaciones utilizadas | SPEC-APP-005 Approved, SPEC-ACC-005 ACC-S3-X01…X07, ADR-003/006, handbook 09/16/17/20 |
| Archivos leídos | RuleExplainEndpoints, ExplainRule, StubRuleExplanation, AssignShift, SchedulingEndpoints, Calendar.razor, MastersApiClient, RuleExplainApiTests, worklogs 001–004 |
| Archivos modificados | este worklog; backlog PBI-011 |
| Resultado | **Gate 2 OK.** 0 bloqueantes. Dictamen: **merge sí** (#35). |
| Tiempo | ~0.25 h |
| Coste | N/D |
| Observaciones | Rebuild de Api bloqueado por `ShiftFlow.Api.exe` en local; integration verdes sobre DLL ya compilada de esta rama. Playwright Out (H16). |
| Pruebas ejecutadas | `dotnet test` Unit 26 OK; Integration 35 OK (DLL). 0 `var` en `.cs`/`.razor` tocados. |
| Estado | hecho |
| Siguiente agente | humano (merge PR #35) |

## Quality gates

| Gate | Resultado |
|------|-----------|
| QG-Build | Verde en el slice (CS1591 OK en #34; Web compiló en Iteration-004). Rebuild sln ahora falló por file lock de Api en ejecución — no es defecto de código. |
| QG-Unit | Verde (26) |
| QG-Accept | Verde ACC-S3-X01…X06 (API). X07 cubierto por revisión de Calendar (sin Playwright, H16). |
| QG-Arch | OK — Rule Engine en Domain; explicación en Infrastructure; query sin escrituras; UI no reimplementa HR. |
| QG-Docs / ADR-006 | OK XML en tipos públicos nuevos; regiones en endpoints; 0 `var` C# |
| QG-Sec | OK — `GET /api/rules/explain` con `Administrator`; Calendar `[Authorize]`; sin secretos; stub sin red/LLM |
| QG-Review | Checklist §3 abajo |

## Checklist review (handbook 17 §3)

### Gobierno
- [x] Gate 0: SPEC-APP-005 / SPEC-ACC-005 Approved; ADR-003; worklogs 001–004
- [x] Sin alcance Out (sin LLM, sin propuestas de cuadrante)
- [x] Worklogs + prompts citados

### Dominio / arquitectura
- [x] Hard rules siguen en `RuleEngine`; stub solo explica
- [x] Domain sin infra; `IRuleExplanation` en Application
- [x] `ExplainRule` es query; `AssignShift` no persiste si hay `RuleViolation`

### Calidad
- [x] Tests X01–X06; X07 por código UI (H16)
- [x] Lenguaje HR-01/02/03
- [x] Sin secretos
- [x] ADR-006

### Producto
- [x] Auth Administrator en API y `/calendar`
- [x] Runbook no exige cambio (endpoint nuevo documentado en Postman #34)

### Seguridad (H20)
- [x] Sin secretos en el diff
- [x] Autorización coherente; anónimo → 401 (X05)
- [x] Sin SQL; input `code` no se interpola a comandos

## Trazabilidad ACC → evidencia

| Escenario | Evidencia |
|-----------|-----------|
| ACC-S3-X01 HR-01 sin mutar | `ACC_S3_X01_explicacion_HR01_sin_mutar` |
| ACC-S3-X02 HR-02 | `ACC_S3_X02_explicacion_HR02` |
| ACC-S3-X03 HR-03 | `ACC_S3_X03_explicacion_HR03` |
| ACC-S3-X04 no soportado | `ACC_S3_X04_codigo_no_soportado` |
| ACC-S3-X05 anónimo | `ACC_S3_X05_anonimo_rechazado` |
| ACC-S3-X06 no bypass | `ACC_S3_X06_stub_no_bypassea_rule_engine` |
| ACC-S3-X07 UI calendario | `Calendar.razor` alerta título/cuerpo; no recarga mes si AssignShift falla |

## Hallazgos

| Severidad | Hallazgo | Acción |
|-----------|----------|--------|
| Menor | No hay test API de `code` vacío → `INV-RUL-01` | Deuda; no está en ACC numerado |
| Info | ACC-S3-X07 sin Playwright | Alineado a H16; smoke manual en el PR |
| Info | El stub ignora contexto opcional (empleado/intervalo) | Determinista; spec lo permite |

## Veredicto

**Gate 2: aprobado.** Merge **sí** de https://github.com/mortiz-iadev/ShiftFlow/pull/35. API ya en main (#34). Humano decide merge; no auto-merge.
