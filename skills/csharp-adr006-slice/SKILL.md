---
name: csharp-adr006-slice
description: Aplica estándares C# ADR-006 (sin var, contrato vs implementación, regiones, XML docs, CS1591) en diffs de src/tests. Usar al escribir o revisar código C#/.razor.
---

# csharp-adr006-slice

| Campo | Valor |
|--------|--------|
| ID | csharp-adr006-slice |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | alta |
| Fecha | 2026-08-12 |
| Norma | [ADR-006](../../architecture/decisions/ADR-006-coding-standards.md), [handbook/17](../../handbook/17-code-review-and-quality-gates.md) |

## Disparadores

- Cualquier diff en `src/**/*.cs`, `src/**/*.razor`, `tests/**/*.cs`.

## Pasos

1. Leer ADR-006 y regla `.cursor/rules/coding-standards-csharp.mdc` (apunta a la norma).
2. Tipado explícito: **prohibido `var`**. Contrato (interfaz/puerto) si se consume por abstracción; implementación si es concreto estable.
3. Regiones en tipos no triviales (Factory/Behavior/Invariants; Commands/Queries; Endpoints; Repositories).
4. XML docs públicos en `src/` (`CS1591` = error).
5. Comentarios solo si aportan invariante no obvia.
6. Verificar: `rg --glob "*.cs" --glob "*.razor" "\bvar\b" src tests` → 0; `dotnet test ShiftFlow.sln`.
7. QG-Docs si toca docs de calidad (H17).

## Definition of Done

- [ ] 0 `var` en archivos tocados (idealmente solución).
- [ ] Compilación/tests verdes pertinentes.
- [ ] XML docs / regiones según ADR-006.

## Restricciones

- No entregar diff de `src/` sin ADR-006.
- `out`/`TryGet*` deben usar el tipo real de la API (p. ej. `HeaderStringValues`).

## Referencias

- [ADR-006](../../architecture/decisions/ADR-006-coding-standards.md)
- [.editorconfig](../../.editorconfig)
