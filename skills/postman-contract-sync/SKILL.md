---
name: postman-contract-sync
description: Alinea la colección Postman y su README con endpoints API nuevos o cambiados. Usar tras slices de Application/API o en Testing.
---

# postman-contract-sync

| Campo | Valor |
|--------|--------|
| ID | postman-contract-sync |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | media |
| Fecha | 2026-08-12 |
| Norma | [postman/README.md](../../postman/README.md); specs APP/ACC del PBI |

## Disparadores

- Nuevos endpoints; cambios de contrato; cierre de PBI API.

## Pasos

1. Diff de endpoints en `src/ShiftFlow.Api` vs colección en `postman/`.
2. Añadir/actualizar requests: auth previa, paths, cuerpos mínimos, códigos esperados.
3. Actualizar [postman/README.md](../../postman/README.md) (variables, orden de uso).
4. **Sin secretos** ni tokens reales en la colección.
5. Worklog: archivos tocados + `postman-contract-sync@0.1.0`.

## Definition of Done

- [ ] Colección cubre el contrato del PBI.
- [ ] README coherente.
- [ ] Sin credenciales.

## Restricciones

- Postman no sustituye specs ni tests automatizados.
- No inventar endpoints no implementados.

## Referencias

- `postman/`
- Specs `specs/application/`, `specs/acceptance/`
