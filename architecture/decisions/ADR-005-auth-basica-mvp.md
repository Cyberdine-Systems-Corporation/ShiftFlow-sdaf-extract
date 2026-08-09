# ADR-005 — Autenticación y autorización básicas (MVP)

| Campo | Valor |
|--------|--------|
| Estado | Aceptado |
| Fecha | 2026-08-09 |
| Decisores | Architecture Agent / Director técnico |
| Relacionado | ADR-001, ADR-004, SPEC-DOM-004, SPEC-APP-002, SPEC-ACC-001, PBI-002 |

---

## Contexto

C-AUTH y las specs Approved (SPEC-DOM-004, SPEC-APP-002) exigen un usuario demo con rol que permita administrar y planificar.  
ADR-001 solo apunta a “Identity u opción equivalente”; hace falta fijar el mecanismo y los límites sin abrir un BC Identity ni auth enterprise.

Este ADR **decide** la dirección técnica. **No entrega implementación** (código, seed ni UI de login): eso queda en **PBI-002** cuando se ejecute.

---

## Decisión

1. **Mecanismo:** ASP.NET Core **Identity** + autenticación por **cookie** en el modular monolith (hosts Api y/o Web según composición del slice).
2. **Ubicación:** supporting / generic subdomain en **Infrastructure + composition root** (Api/Web). **No** se crea Bounded Context Identity separado en el MVP (`handbook/11` §2).
3. **Autorización:** un único rol de aplicación **`Administrator`**, alineado a SPEC-DOM-004 (maestros en Sprint 1; planificación en Sprint 2).
4. **Usuario demo:** al menos uno (`demo.admin` o equivalente documentado en runbook), provisionado por seed o setup local; contraseña solo en **user-secrets / env**, nunca en git.
5. **Domain:** no referencia Identity/EF Identity; Application autoriza vía abstracciones del host o políticas, sin filtrar tipos de Identity al Domain.
6. **Alcance de este ADR:** decisión y restricciones. La materialización en `src/` es **PBI-002** (y tests ACC-S1-01/02/07), no un entregable implícito de aceptar este documento.

### Diferido explícitamente (Out de este ADR / post-MVP salvo enmienda)

| Tema | Estado |
|------|--------|
| SSO / OIDC / OAuth externos | Diferido |
| MFA, reset password por email, registro self-service | Diferido |
| BC Identity separado, multi-tenant IAM | Diferido |
| Roles adicionales (Viewer, Planner-only, etc.) | Diferido |
| Autorización fina por Organization/Department | Diferido |
| Proveedores cloud de identidad como camino de demo | Diferido (C-LOC) |

---

## Alternativas consideradas

| Alternativa | Motivo de rechazo (MVP) |
|-------------|-------------------------|
| JWT bearer como único esquema + SPA pura | Blazor Web App + cookie Identity reduce fricción de demo local |
| Auth custom sin Identity | Más código y superficie de seguridad sin beneficio en MVP |
| Sin auth hasta post-MVP | Contradice SPEC-PRD-001 C-AUTH y SPEC-DOM-004 Approved |
| IdentityServer / Keycloak / Entra ID obligatorios | Complejidad y dependencia externa; viola runtime local autocontenido |
| BC Identity desde el día 1 | Sobre-partición prematura (handbook 11) |

---

## Consecuencias

### Positivas

- Gate 0 técnico cerrado para auth: PBI-002 tiene ADR de referencia.
- Alineado a stack .NET (ADR-001) y a specs Approved.
- Límites Out claros (sin SSO/MFA/BC Identity en la demo).

### Negativas / costes

- Identity añade paquetes y esquema de tablas cuando se implemente PBI-002.
- Cookie auth acopla sesión al host Web/Api; habrá que cuidar CORS/antiforgery en el slice.

### Relación con implementación

| Artefacto | Responsabilidad |
|-----------|-----------------|
| Este ADR | Decidir mecanismo, rol, ubicación, diferidos |
| PBI-002 | Implementar login/logout, seed demo, autorización de maestros |
| Runbook | Documentar usuario demo y secretos locales tras PBI-002 |

Hasta que PBI-002 se complete, el skeleton puede seguir sin Identity; **no** se considera violación de este ADR carecer aún del código.

---

## Cumplimiento

- Toda implementación de C-AUTH en `mvp-0.1` debe respetar este ADR o enmendarlo.
- Desvíos hacia SSO/OIDC/BC Identity requieren nuevo ADR o enmienda explícita.

---

## Historial

| Fecha | Cambio |
|-------|--------|
| 2026-08-09 | Aceptado: Identity + cookie + rol Administrator; implementación diferida a PBI-002 |
