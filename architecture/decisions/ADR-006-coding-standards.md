# ADR-006 — Estándares de código (legibilidad, regiones, XML docs, tipado explícito)

| Campo | Valor |
|--------|--------|
| Estado | Aceptado |
| Fecha | 2026-08-12 |
| Decisores | Product Owner / Engineering |
| Relacionado | ADR-001, ADR-004, `handbook/07-repository-organization.md`, `handbook/17-code-review-and-quality-gates.md` |

## Contexto

El código del MVP debe ser legible para humanos y agentes. Hasta ahora no había norma explícita de regiones conceptuales, comentarios ni documentación XML, ni un gate de build que la exija. El handbook (§7) anticipaba un ADR de coding standards.  
Además, el uso de `var` oscurece el contrato o la implementación en reviews y en handoffs entre agentes.

## Decisión

1. **Identificadores:** inglés idiomático .NET (tipos, miembros, namespaces).  
2. **Comentarios y XML docs:** castellano.  
3. **Regiones conceptuales** (`#region` / `#endregion`) en tipos no triviales, con nombres de frontera de responsabilidad (p. ej. `Factory`, `Invariants`, `Behavior`, `Commands`, `Queries`, `Endpoints`, `Mapping`). Orden estable dentro de la clase. Prohibidas regiones “misc” o de una sola línea sin frontera real.  
4. **Comentarios de línea/bloque:** solo en lógica de alto impacto o no obvia (invariantes sutiles, trade-offs, porqués Application vs Domain). No narrar lo que el identificador ya dice.  
5. **XML documentation** obligatoria en la API pública e `internal` visible de proyectos bajo `src/` (`<summary>`; `<param>` / `<returns>` / `<exception>` cuando aporten).  
6. **Tipado explícito (sin `var`):** las declaraciones locales y de campo deben usar el tipo escrito — el **contrato** (interfaz/puerto) cuando el símbolo se trata por su abstracción (p. ej. `ILeaveRepository`, `IReadOnlyList<T>`), o la **implementación** cuando el valor es un concreto estable (p. ej. `Leave`, `List<T>`, `HttpClient`). Prohibido `var` en `src/` y `tests/`.  
7. **Enforcement:** `GenerateDocumentationFile` + warning `CS1591` tratado como error en `src/` (`Directory.Build.props`). Preferencias de estilo en `.editorconfig` (`csharp_style_var_* = false`). `tests/` queda exento de CS1591; **no** de tipado explícito.  
8. **Review / PR:** checklist de `handbook/17` incluye regiones, comentarios, XML docs y tipado explícito; sin `CS1591` limpio ni `var` nuevo en el diff no hay merge.

Código tocado en un PR debe cumplir la norma en el diff. La deuda residual de archivos no tocados se salda al modificarlos o en chores dedicados (este chore elimina `var` en la solución).

## Alternativas consideradas

| Alternativa | Por qué no |
|-------------|------------|
| Solo checklist humano sin analizador | Se olvida; no bloquea build |
| StyleCop completo / EditorConfig agresivo | Coste alto para MVP; se puede ampliar post-MVP |
| Regiones prohibidas (estilo “flat”) | El equipo pide fronteras conceptuales explícitas para legibilidad |
| Permitir `var` cuando el tipo es “aparente” | Sigue ocultando contrato vs implementación en reviews |

## Consecuencias

- Builds de `src/` fallan sin XML docs en miembros públicos/`internal`.  
- Declaraciones más verbosas; reviews y agentes ven el tipo de un vistazo.  
- Agentes deben aplicar ADR-006 (regla Cursor + prompts).  
- `.editorconfig` guía IDE/`dotnet format` para no reintroducir `var`.
