# 03 — MVP Definition

| Campo | Valor |
|--------|--------|
| **Versión** | 0.3.0 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | I — Constitución de producto |
| **Norma superior** | [01-product-charter.md](01-product-charter.md), [02-product-vision.md](02-product-vision.md) |
| **Deriva hacia** | [04-product-roadmap.md](04-product-roadmap.md), `specs/product/`, `specs/acceptance/`, ADRs de alcance |

---

## 1. Propósito de este capítulo

Fijar el **corte demostrable** del MVP: qué debe existir el **22 de agosto de 2026**, qué queda fuera, y con qué criterios se da por cerrado.

Este capítulo es frontera de alcance. Ampliarlo requiere enmienda explícita.

---

## 2. Objetivo del MVP

Obtener un **MVP enterprise demostrable** (no producción) que combine:

1. **Producto:** flujo Web de maestros + calendario + asignación manual validada + ausencias + auth/roles + API.
2. **Metodología:** SDAF operable (handbook, specs, ADRs, agentes/prompts mínimos, worklogs, tests derivados de aceptación).

No se persigue despliegue productivo en cloud ni optimización automática de cuadrantes.

**Obligatorio:** el MVP debe poder instalarse y ejecutarse de forma **local y autocontenida** para pruebas y valoraciones (véase §3.1 y §4.4).

---

## 3. Decisiones de corte (obligatorias)

| Decisión | Valor |
|----------|--------|
| Cliente de usuario | **Solo Blazor Web App** |
| MAUI Hybrid | Diferido (ADR) |
| Entregable dual | SDAF mínimo profesional + producto demostrable (punto intermedio) |
| Capacidad humana de referencia | L–V 5 h, fines 3 h (~96 h en 1–22 ago) |
| Bounded context inicial | Uno: WorkforceScheduling |
| Motores | Rule Engine v1 + Scheduling (asignación manual); resto diferido |
| IA en MVP | Stub de infraestructura: explicación de reglas; **sin** mutar cuadrantes |
| Ejecución para demo/pruebas | **Local autocontenida**; cloud no es requisito ni puerta de entrada |
| Cierre comunicativo del MVP | **Vídeo de presentación** + **slides** (producto, evolución, arquitectura) |

### 3.1 Ejecución local autocontenida (mandatory)

Para realizar pruebas, demos y valoraciones del MVP **debe** existir un camino de ejecución local que:

1. Se instale y arranque en una máquina de desarrollo o evaluación **sin depender de un despliegue en cloud** (Azure, AWS, GCP u otro PaaS/SaaS de hosting).
2. Incluya la **infraestructura autocontenida** necesaria (como mínimo aplicación + PostgreSQL) mediante Docker y/o .NET Aspire (u equivalente documentado), de modo que un evaluador no tenga que provisionar servicios externos de pago o cuentas cloud.
3. Disponga de un **runbook local** (pasos reproducibles: prerrequisitos, arranque, URL, usuario demo, parada).
4. Permita completar el flujo demo del §7 íntegramente en ese entorno local.

Queda **prohibido** como DoD del MVP:

- Exigir despliegue en cloud para poder probar o valorar.
- Dejar la demo solo “en la máquina del autor” sin script/compose/AppHost reproducible.

Cloud puede existir más adelante como opción de producto; **no** forma parte del cierre del MVP.

---

## 4. Incluido (In)

### 4.1 Producto

- Gestionar organizaciones, departamentos y empleados.
- Gestionar tipos de turno.
- Calendario mensual.
- Asignación manual de turnos.
- Validación de reglas **hard mínimas** (como máximo tres en implementación):
  - no solapes de turno para la misma persona,
  - ausencia bloquea asignación,
  - descanso mínimo configurable entre turnos.
- Gestión básica de leaves (vacaciones/ausencias).
- Usuarios y roles básicos.
- API REST.
- Cliente Blazor Web App demostrable.
- Persistencia PostgreSQL vía EF Core.
- Observabilidad mínima con Serilog.

### 4.2 Ingeniería / SDAF

- Handbook Partes I–II en estado usable (Approved o Draft estable acordado).
- Specs de producto y de dominio necesarias para lo In.
- ADRs de stack, Web-only y motores MVP.
- Acceptance tests derivados de specs para el flujo crítico.
- Worklogs de iteraciones clave.
- Biblioteca de prompts: agentes activos + stubs mínimos.

### 4.3 Reglas avanzadas del knowledge

Las reglas del documento funcional experto (contratos parciales, noches pares/impares, bolsa mensual, ocho días, cuotas nocturnas, validaciones intermensuales, etc.):

- **Deben** entrar en `knowledge/` y, progresivamente, en specs de dominio documentadas.
- **No deben** implementarse en el MVP salvo que se elijan explícitamente como una de las ≤3 hard rules del §4.1.

### 4.4 Runtime local autocontenido

- AppHost Aspire y/o `docker compose` (o ambos) que levanten API + Blazor Web + PostgreSQL en local.
- Documentación de arranque local en el repositorio (runbook).
- Datos o seed mínimos opcionales para acortar la demo, sin sustituir el flujo del §7.
- Verificación de que el stack arranca en frío en una máquina limpia con los prerrequisitos declarados (SDK .NET, Docker Desktop o motor compatible).

### 4.5 Presentación del MVP (mandatory)

Como **punto final del entregable MVP** deben existir ambos artefactos, alineados entre sí:

1. **Deck de slides** — presentación reutilizable (fuente en el repo, p. ej. `docs/presentation/mvp-0.1/`), en castellano.
2. **Vídeo de presentación** — grabación que recorre el mismo relato que las slides (el archivo puede vivir fuera de Git si es pesado; el repo **debe** referenciar URL o ruta y checksum/fecha en el runbook o README de presentación).

Contenido mínimo obligatorio (slides y vídeo):

| Bloque | Debe cubrir |
|--------|-------------|
| Producto | Problema, propuesta de valor, alcance del MVP, demo del flujo crítico |
| Evolución | Camino 1–22 ago (sprints), qué queda post-MVP, doble entregable producto + SDAF |
| Arquitectura | Stack, Clean/CQRS/slices a alto nivel, Rule Engine + Scheduling, runtime local autocontenido, IA como infra sin mutación automática |

Duración orientativa del vídeo: 8–15 minutos.  
Las slides deben poder usarse en una defensa oral sin el vídeo.

---

## 5. Excluido (Out)

- Optimización automática de cuadrantes.
- Generación de cuadrantes por IA con escritura automática.
- Machine Learning / predicción de demanda.
- MAUI Blazor Hybrid y apps nativas independientes.
- Redis y SignalR (hasta necesidad demostrada).
- OpenTelemetry completo.
- Multitenancy avanzado.
- Integraciones ERP / Active Directory.
- Informes avanzados.
- Modo offline.
- Compliance/Optimization/AI Recommendation como motores separados.
- **Despliegue cloud obligatorio** para pruebas, demos o valoración del MVP.
- Dependencia de servicios gestionados en cloud (Postgres cloud, App Service, etc.) como único camino de ejecución.

---

## 6. Stack del MVP

| Incluido | Diferido |
|----------|----------|
| .NET 10, ASP.NET Core, EF Core, PostgreSQL | MAUI Hybrid |
| Blazor Web App | Redis, SignalR |
| MediatR, xUnit, FluentAssertions, Testcontainers | OTel completo |
| Serilog | CI/CD cloud / pipelines avanzados |
| **Docker + Aspire mínimos como runtime local obligatorio** | Hosting cloud como camino de demo |
| | IA generativa de cuadrantes |

Contingencia: si .NET 10 bloquea el avance, ADR de fallback a .NET 9 LTS.  
El runtime local (§3.1 / §4.4) **no** tiene contingencia “pasar a cloud”: si Aspire falla, se documenta `docker compose` equivalente; si Docker no está disponible en un entorno concreto, se declara prerrequisito — no se sustituye por despliegue cloud.

---

## 7. Flujo demo (Definition of Demo)

**Prerrequisito:** entorno local arrancado según runbook (§4.4), sin pasos de cloud.

En menos de 15 minutos debe poder mostrarse:

1. Crear/consultar organización, departamento y empleados.
2. Definir tipos de turno.
3. Abrir calendario mensual.
4. Asignar un turno válido.
5. Intentar una asignación inválida y ver el rechazo/explicación de regla.
6. Registrar una ausencia y comprobar que bloquea.
7. Identificar rol/usuario básico y API detrás del flujo.

---

## 8. Criterios de cierre del MVP (DoD)

El MVP se considera cerrado cuando:

1. Todo lo **In** de producto del §4.1 está implementado o explícitamente recortado con ADR de excepción fechado.
2. Los acceptance tests del flujo demo están en verde.
3. Existe trazabilidad spec → código → test → worklog para el camino crítico.
4. El handbook y la estructura SDAF permiten continuar sin pérdida de contexto.
5. La demo del §7 es reproducible con **runbook local** breve (§4.4).
6. Un evaluador puede levantar la infraestructura autocontenida en local **sin cuenta ni despliegue cloud**.
7. Existen **slides** y **vídeo de presentación** del MVP (§4.5) cubriendo producto, evolución y arquitectura.
8. Lo **Out** no se ha colado como dependencia bloqueante.

---

## 9. Criterios de aceptación de este capítulo (H3)

- [ ] Web-only y diferidos (MAUI, Redis, SignalR, optimización, IA que escribe) quedan explícitos.
- [ ] Runtime local autocontenido es mandatory; cloud no es requisito de demo/pruebas.
- [ ] Vídeo + slides de cierre son mandatory (§4.5).
- [ ] La lista In es demostrable en ~96 h; no reintroduce el alcance de 176 h.
- [ ] Las reglas avanzadas del DOCX quedan en knowledge/specs, no en código obligatorio.
- [ ] El DoD y el flujo demo son verificables.

---

## 10. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.3.0 | 2026-08-05 | Enmienda Approved: vídeo + slides de presentación como cierre mandatory del MVP |
| 0.2.1 | 2026-08-05 | Approved tras revisión humana (incluye runtime local mandatory) |
| 0.2.0 | 2026-08-05 | Runtime local autocontenido mandatory; cloud excluido como camino de demo |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H3); corte equilibrado SDAF + producto |
