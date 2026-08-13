# 20 — Security Standards (MVP)

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.0 |
| **Estado** | Approved |
| **Fecha** | 2026-08-13 |
| **Parte** | V — Calidad y entrega |
| **Norma superior** | [06-engineering-principles.md](06-engineering-principles.md), [09-development-workflow.md](09-development-workflow.md), [10-solution-architecture.md](10-solution-architecture.md), [03-mvp-definition.md](03-mvp-definition.md) |
| **Deriva hacia** | [17-code-review-and-quality-gates.md](17-code-review-and-quality-gates.md), [SECURITY.md](../SECURITY.md), ADR-005, Testing+Review, skill `security-review-mvp` |

---

## 1. Propósito

Fijar el **baseline de seguridad** obligatorio para humanos y agentes en el MVP demostrable: qué hay que cumplir ya, qué queda diferido, y cómo se revisa.

Este capítulo **no** convierte ShiftFlow en producto hardened de producción. Complementa auth (ADR-005), quality gates (H17) y la política de reporte ([SECURITY.md](../SECURITY.md)).

---

## 2. Alcance y no-alcance

### 2.1 En alcance (MVP)

- Api ASP.NET + Blazor Web + Identity/cookie (ADR-005).
- Secretos y credenciales en repo / runbooks / Postman / worklogs.
- Controles básicos alineados a riesgos del OWASP Top 10 relevantes al stack.
- Checklist de review y gate técnico mínimo (QG-Sec).

### 2.2 Fuera de alcance (salvo enmienda / post-MVP)

- Certificación ASVS completa, pentest formal obligatorio, bug bounty.
- SSO/OIDC/MFA, IAM multi-tenant, WAF, SIEM (coherente con Out de ADR-005 y H03).
- Exigir cloud hardening como DoD del MVP (C-LOC).

Lo diferido en ADR/specs **no** es “bug” por sí solo; sí lo es un bypass de lo **sí** especificado (p. ej. endpoint de maestros sin auth cuando ACC lo exige).

---

## 3. Referencias externas (marco, no checklist interminable)

| Referencia | Uso en ShiftFlow |
|------------|------------------|
| [OWASP Top 10](https://owasp.org/www-project-top-ten/) | Mapa de riesgos; baseline §4 |
| OWASP ASVS (nivel 1, selectivo) | Inspiración para controles de auth/sesión/input; **no** se exige cobertura ASVS completa |
| ADR-005 | Decisión de auth/roles del MVP |
| SECURITY.md | Cómo reportar vulnerabilidades |

Los agentes **enlazan** estas referencias; no pegan el Top 10 entero en cada PR.

---

## 4. Baseline obligatorio (MVP)

### 4.1 Secretos y datos sensibles

- Prohibido commitear passwords, connection strings con credenciales, API keys, `.env` con secretos, user-secrets.
- Usuario demo: password por user-secrets / env ([runbook](../docs/runbook-local.md)); no como verdad en git salvo placeholder documentado de desarrollo.
- Postman/colecciones: sin tokens reales.
- Logs y worklogs: sin secretos ni PII innecesaria.

### 4.2 Autenticación y autorización

- Cumplir [ADR-005](../architecture/decisions/ADR-005-auth-basica-mvp.md) y specs ACC de auth.
- Endpoints de producto protegidos según rol (`Administrator` en MVP) salvo rutas explícitamente públicas (`/api/status`, health, login).
- No filtrar tipos de Identity al Domain.
- No “abrir” autorización en UI sin la misma regla en API.

### 4.3 Mapa OWASP Top 10 → controles ShiftFlow

| Riesgo (categoría) | Control mínimo MVP |
|--------------------|--------------------|
| Broken Access Control | Policies/roles en API; tests ACC de auth; no confiar solo en ocultar UI |
| Cryptographic Failures | HTTPS en perfil Aspire `https`; no almacenar secretos en claro en repo |
| Injection | EF Core parametrizado; sin SQL concatenado con input; validar entradas de API |
| Insecure Design | Specs + ADR antes de features sensibles; no inventar auth ad hoc |
| Security Misconfiguration | No exponer stack traces sensibles en demos “públicas”; secretos fuera de git |
| Vulnerable Components | Tras actualizar paquetes, build/tests verdes; anotar vulnerabilidades conocidas críticas si aparecen |
| Identification / Auth Failures | Cookie Identity según ADR-005; logout; no registrar contraseñas |
| Software / Data Integrity | No ejecutar scripts no versionados con secretos; PRs revisados (H17) |
| Security Logging Failures | Errores de auth sin filtrar secretos; no loguear passwords |
| SSRF | No introducir fetch a URLs controladas por usuario sin spec/ADR |

Blazor Server: respetar antiforgery / patrones del framework en formularios interactivos; no desactivar protecciones sin ADR.

### 4.4 Cookies y sesión (MVP)

- Preferir cookies de auth con flags adecuados al hosting local (HttpOnly; Secure cuando el perfil es HTTPS).
- Documentar en runbook el flujo BFF/cookie Web ↔ Api si cambia.

---

## 5. Gates y review

### 5.1 Checklist (añadir a H17)

Al tocar auth, cookies, endpoints, secretos o input externo:

- [ ] Sin secretos nuevos en el diff.
- [ ] Autorización coherente API (y UI si aplica).
- [ ] Sin injection obvia (SQL/comandos).
- [ ] Alineado a ADR-005 / specs ACC de auth si el PBI las toca.
- [ ] Runbook/SECURITY no contradichos.

### 5.2 QG-Sec

| Gate | Condición | Bloquea |
|------|-----------|---------|
| QG-Sec | Diff no introduce secreto en claro ni bypass de auth de lo especificado | Merge |

Hallazgos QG-Sec son **bloqueantes** (equivalente a H17 §5).

### 5.3 Reporte

Vulnerabilidades descubiertas: [SECURITY.md](../SECURITY.md) (sin PoC en issue público).

---

## 6. Roles

| Actor | Responsabilidad |
|-------|-----------------|
| Architecture | ADR de auth/sesión/controles nuevos |
| Domain+Application / Frontend / Infra | Cumplir baseline en el slice |
| Testing+Review | Checklist §5 + ACC auth; dictamen merge |
| Humano | Aprobar enmiendas de este capítulo; severidad en demos externas |

Skill operativa: [`skills/security-review-mvp`](../skills/security-review-mvp/SKILL.md).

---

## 7. Criterios de aceptación de este capítulo

- [ ] Queda claro MVP vs producción / diferidos.
- [ ] Baseline §4 es accionable en review sin exigir ASVS completo.
- [ ] Relación con H17, ADR-005 y SECURITY.md es explícita.

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-13 | Approved: baseline OWASP-aware para MVP |
