# ADR-008 — Extracción de SDAF a núcleo reutilizable (core)

| Campo | Valor |
|--------|--------|
| Estado | Propuesto |
| Fecha | 2026-08-22 |
| Decisores | Director técnico (humano) |
| Relacionado | `handbook/05-sdaf-framework.md` §8, `handbook/07-repository-organization.md`, `skills/README.md`, worklog `skills-sdaf-portable`, fork `ShiftFlow-sdaf-extract` |

---

## Contexto

ShiftFlow (repo origen) contiene **producto** y **método (SDAF)** en el mismo árbol. El handbook ya declara que SDAF debe ser reutilizable fuera de este producto (H05 §8), y las skills `sdaf-*` ya se diseñaron tool-agnostic.

Se necesita un núcleo de gobernanza reusable sin alterar el repo origen del MVP. Este fork (`ShiftFlow-sdaf-extract`) es el **laboratorio de extracción**; el destino estable será un repositorio nuevo `sdaf-core`.

## Decisión

1. **Tres capas de pertenencia** para cada artefacto de gobernanza:
   - **Core** — metodología SDAF, independiente de producto y de stack.
   - **Stack** — normas/playbooks ligados a tecnología (p. ej. .NET / Blazor / ADR-006).
   - **Product** — constitución, dominio, código y trazabilidad de ShiftFlow.

2. **Estrategia de repos (opción C / híbrida):**
   - El **origen ShiftFlow** permanece intacto como referencia del MVP.
   - Este **fork** es workspace temporal de clasificación y limpieza.
   - El entregable estable es un repo nuevo **`sdaf-core`** (semver), no un segundo ShiftFlow permanente.
   - Opcional posterior: pack **`sdaf-stack-dotnet`** y plantilla **`sdaf-project-template`**.
   - La adopción en el origen (submodule/subtree) queda **fuera** de este ADR; requiere decisión explícita posterior.

3. **Contenido mínimo de `sdaf-core` v0.1:**
   - Handbook: Partes II (05–09), IV (13–15), apéndice B (plantillas); Partes V genéricas (16–18, 20) solo tras scrub de referencias a ShiftFlow/ADR-006.
   - **Norma canónica:** en todo proyecto bajo SDAF, la verdad operativa para implementar está en `specs/` del **repo consumidor** (H05/H08). El core exige y estandariza esa carpeta; no la rellena con specs de un producto.
   - **Gobernanza de stack:** el core exige ADRs y Gate 0 ante cambios de stack/límites; no fija .NET, Blazor, PostgreSQL ni ADR-006.
   - `templates/`, skills `sdaf-gate0`, `sdaf-agent-router`, `sdaf-worklog-handoff`, `spec-draft-pbi`, `adr-propose`.
   - Contratos/prompts de agentes **genéricos** (sin acoplar a Blazor/C#).
   - `AGENTS.md.template` + esquema `sdaf.config` (nombre de proyecto, idioma, agentes activos, pack de stack opcional).
   - Regla IDE genérica de idioma (castellano), no coding standards C#.

4. **Qué no entra en `sdaf-core` (contenido de producto / stack concreto):**
   - `src/`, `tests/`, `knowledge/`, **contenido** de `specs/` de ShiftFlow, `backlog/`, `postman/`, `docs/presentation/`.
   - Handbook Parte I (01–04), Parte III específica de solución (10–12), glosario de dominio (A), métricas de sprint del producto (19) salvo que se generalicen después.
   - ADRs de producto/stack de ShiftFlow (ADR-001…007) y skills de dominio/stack (`csharp-adr006-slice`, `blazor-bff-slice`, `rule-engine-hr`, etc.).
   - Worklogs históricos de ShiftFlow.
   - Clarificación: excluir `specs/` pobladas ≠ excluir la norma “la verdad operativa está en `specs/`”.

5. **Inventario canónico** de esta extracción: [`docs/sdaf-extraction/inventory-core-stack-product.md`](../../docs/sdaf-extraction/inventory-core-stack-product.md).

6. **Generalización:** ningún artefacto en `sdaf-core` debe mencionar “ShiftFlow” como nombre de producto ni fijar Blazor/PostgreSQL/ADR-006 como norma del método. Usar placeholders / config (`{{PROJECT_NAME}}`, etc.).

## Alternativas consideradas

| Alternativa | Motivo de rechazo / aplazamiento |
|-------------|----------------------------------|
| Extraer dentro del repo origen | Viola el requisito de no alterar el código/gobernanza actual del MVP. |
| Fork permanente como segundo ShiftFlow migrado | Compite con el origen; el valor reusable es el core, no el clon. |
| Solo GitHub Template (copia) sin repo core | Facilita onboarding pero genera drift; no hay upgrade versionado del método. |
| Submodule inmediato en el origen | Prematuro; primero hay que estabilizar el core en el laboratorio. |

## Consecuencias

### Positivas
- Origin del MVP queda congelado como referencia.
- Otros proyectos pueden adoptar SDAF sin arrastrar dominio de turnos.
- Separación Core/Stack evita que .NET sea “la constitución” del método.

### Negativas / costes
- Duplicación temporal (fork + futuro `sdaf-core`) hasta archivar el laboratorio.
- Scrub de texto y enlaces relativos en handbook/prompts.
- Los proyectos consumidores necesitarán un mecanismo de upgrade (submodule, subtree o release copy) documentado más adelante.

### Seguimiento
1. Aprobar este ADR (humano).
2. Completar scrub y publicar `sdaf-core@0.1.0` desde el trabajo de este fork.
3. (Opcional) `sdaf-stack-dotnet` y `sdaf-project-template`.
4. Decidir adopción en el origen ShiftFlow en un ADR posterior.

## Revisión (paso 1)

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-22 |
| Resultado | Coherente con opción C; listo para aprobación humana (paso 3) |
| Ajustes | Explicitar norma `specs/` como verdad operativa del consumidor; stack concreto fuera / gobernanza de stack dentro |

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-22 | Revisión: norma specs + gobernanza de stack vs contenido excluido |
| 0.1.0 | 2026-08-22 | Propuesta inicial en fork `ShiftFlow-sdaf-extract` |
