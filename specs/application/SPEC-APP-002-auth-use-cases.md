# SPEC-APP-002 — Casos de uso de autenticación y autorización

| Campo | Valor |
|--------|--------|
| ID | SPEC-APP-002 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-09 |
| Fuentes | SPEC-DOM-004, SPEC-PRD-001 (C-AUTH), SPEC-PRD-002 precondiciones |
| ADRs relacionados | ADR-001, **ADR-005** |
| Backlog | PBI-002 |
| Derivados | SPEC-ACC-001, host Api/Web Identity |

---

## 1. Contexto

Casos de uso mínimos para login/logout y autorización del rol Administrator en el MVP local.

---

## 2. Actor y precondiciones de entorno

- Runtime local con usuario demo (seed o creación documentada en runbook).  
- Credenciales solo en user-secrets / env local (nunca en git).

### Usuario demo (contrato de producto)

| Campo | Valor sugerido (local) |
|-------|------------------------|
| UserName | `demo.admin` |
| Role | `Administrator` |
| Password | Definida en runbook / user-secrets (no en esta spec) |

---

## 3. Casos de uso

### UC-AUTH-01 Login

| Campo | Valor |
|-------|--------|
| Actor | Visitante |
| Precondición | Usuario demo existe y está activo |
| Flujo | Envía credenciales válidas → sesión/cookie autenticada |
| Postcondición | Actor autenticado con rol Administrator |
| Alternativo | Credenciales inválidas → rechazo; sin sesión |

### UC-AUTH-02 Logout

| Campo | Valor |
|-------|--------|
| Actor | Usuario autenticado |
| Flujo | Cierra sesión |
| Postcondición | Deja de acceder a operaciones protegidas |

### UC-AUTH-03 Autorizar operación de maestros

| Campo | Valor |
|-------|--------|
| Actor | Usuario |
| Precondición | Intento de comando/query de SPEC-APP-001 |
| Flujo | Pipeline comprueba autenticación + rol Administrator |
| Postcondición | Permitido o rechazado (401/403 según corresponda) |

---

## 4. Criterios de aceptación (aplicación)

1. Login con credenciales demo válidas establece identidad con rol Administrator.  
2. Login inválido no autentica.  
3. Sin autenticación, los comandos de SPEC-APP-001 son rechazados.  
4. Logout invalida el acceso posterior a recursos protegidos en la misma sesión.

---

## 5. Fuera de alcance

- Registro público self-service, recuperación de contraseña, MFA, OAuth/OIDC externo.
- Gestión UI completa de usuarios (basta seed/demo + login).

---

## 6. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | Approved tras revisión humana |
| 0.1.0 | 2026-08-09 | Draft Sprint 1 (Specification Agent) |
