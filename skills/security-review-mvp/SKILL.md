---
name: security-review-mvp
description: Revisa diffs Api/Web/auth contra el baseline de seguridad MVP (H20, OWASP-aware, ADR-005, sin secretos). Usar en Testing+Review o al tocar auth, cookies, endpoints o secretos.
---

# security-review-mvp

| Campo | Valor |
|--------|--------|
| ID | security-review-mvp |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | media |
| Fecha | 2026-08-13 |
| Norma | [handbook/20](../../handbook/20-security-standards.md), [SECURITY.md](../../SECURITY.md), ADR-005 |

## Disparadores

- Diff que toca auth, roles, cookies, login, endpoints API, manejo de secretos o input externo.
- Gate 2 / review de PR con superficie de seguridad.
- Pedido explícito de “security review” del MVP.

## Pasos

1. Leer [handbook/20-security-standards.md](../../handbook/20-security-standards.md) §4–§5 (no pegar OWASP entero).
2. Comprobar secretos en el diff (passwords, connection strings, tokens).
3. Comprobar authz: rutas de producto vs públicas; coherencia con ADR-005 y ACC.
4. Buscar injection obvia (SQL crudo, comandos con input).
5. Si hay hallazgo explotable nuevo: orientar a [SECURITY.md](../../SECURITY.md); no publicar PoC en issue abierto.
6. Registrar en worklog `security-review-mvp@0.1.0` + dictamen (bloqueante / mayor / menor según H17).

## Definition of Done

- [ ] Checklist H20 §5.1 recorrido en el alcance del diff.
- [ ] QG-Sec evaluado (pass/fail).
- [ ] Worklog actualizado.

## Restricciones

- No exigir MFA/SSO ni ASVS completo (Out MVP).
- No marcar como bug el Out documentado en ADR-005.
- No inventar controles de producción fuera de norma.

## Referencias

- [handbook/17-code-review-and-quality-gates.md](../../handbook/17-code-review-and-quality-gates.md)
- [architecture/decisions/ADR-005-auth-basica-mvp.md](../../architecture/decisions/ADR-005-auth-basica-mvp.md)
