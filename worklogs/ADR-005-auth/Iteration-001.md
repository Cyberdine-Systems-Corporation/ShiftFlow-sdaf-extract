# ADR-005-auth / Iteration-001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-09 |
| Agente | Architecture |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-ARCH-001@0.1.0 |
| Contexto | Formalizar auth MVP sin implementar código; rama `docs/adr-005-auth` |
| Especificaciones utilizadas | SPEC-DOM-004, SPEC-APP-002, SPEC-ACC-001, ADR-001, ADR-004 |
| Archivos leídos | handbook/11 §2, ADR-001 auth, templates/adr.md, backlog PBI-002 |
| Archivos modificados | `architecture/decisions/ADR-005-auth-basica-mvp.md`, README ADRs, SPEC-DOM-004/APP-002, backlog PBI-002 + README, este worklog |
| Resultado | ADR-005 Aceptado: Identity + cookie + rol Administrator; Out SSO/MFA/BC Identity; implementación explícitamente en PBI-002 |
| Tiempo | ~0.25 h |
| Coste | N/D |
| Observaciones | No se tocó `src/`. C-AUTH sigue en MVP vía PBI-002 cuando se priorice. |
| Pruebas ejecutadas | N/A |
| Estado | hecho |
| Siguiente agente | humano (merge ADR) → Domain+Application cuando se active PBI-002; o maestros PBI-003 si se antepone |
