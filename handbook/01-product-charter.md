# 01 — Product Charter

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | I — Constitución de producto |
| **Norma superior** | [00-preface.md](00-preface.md) |
| **Deriva hacia** | [02-product-vision.md](02-product-vision.md), [03-mvp-definition.md](03-mvp-definition.md), [04-product-roadmap.md](04-product-roadmap.md), specs de producto |
| **Semilla** | Borrador externo Product Charter (jul 2026); reescrito, no importado tal cual |

---

## 1. Propósito de este capítulo

Definir de forma inequívoca:

- Qué problema resuelve ShiftFlow.
- La misión del producto.
- Los principios de producto que obligan a todas las decisiones.
- A quién sirve y qué valor ofrece.
- Restricciones y riesgos de carácter estratégico.

El detalle de visión narrativa, alcance fino del MVP y calendario viven en los capítulos 02–04.  
Este charter no se contradice con ellos: los enmarca.

Ninguna decisión arquitectónica, funcional o técnica debe contradecir este capítulo Approved sin enmienda formal.

---

## 2. Misión

Desarrollar una **plataforma** de planificación y gestión de turnos que permita a cualquier organización que trabaje por turnos configurar sus reglas, asignar personal con trazabilidad y asistirse progresivamente con IA, reduciendo errores y complejidad operativa.

ShiftFlow no es un desarrollo a medida para un único sector.

---

## 3. Problema

Hoy muchas organizaciones planifican con:

- Hojas de cálculo.
- Aplicaciones heredadas.
- Productos rígidos que obligan a adaptar el negocio al software.
- Procesos manuales frágiles.

Consecuencias típicas:

- Errores de cobertura y solapes.
- Conflictos entre reglas y realidad operativa.
- Incumplimientos de restricciones laborales o internas.
- Bajo tiempo de reacción ante cambios.
- Poca trazabilidad de por qué un turno quedó asignado.

---

## 4. Oportunidad

Cada organización tiene reglas distintas. Los sectores iniciales de interés (hospitales, policía, bomberos, protección civil, seguridad privada) son **ejemplos de configuración**, no el modelo de dominio embebido en código.

La oportunidad de producto es:

> Reglas como datos configurables, no como forks de código por cliente.

La extensión a transporte, industria, logística, hoteles, call centers u aeropuertos debe ser posible sin reescribir el núcleo.

---

## 5. Objetivos estratégicos

El producto debe, a lo largo de su evolución:

1. Simplificar la planificación diaria y mensual.
2. Reducir errores y violaciones de reglas.
3. Acortar el tiempo para construir y ajustar un cuadrante.
4. Facilitar cambios con impacto visible (quién, qué, por qué).
5. Permitir colaboración humano–sistema (y más adelante entre roles).
6. Mantener trazabilidad de decisiones de planificación.
7. Incorporar IA de forma progresiva y siempre supervisada.

---

## 6. Principios de producto (obligatorios)

### 6.1 Configuración sobre programación

Las reglas de negocio del cliente deben poder configurarse.  
No deben requerir un desarrollo específico por organización para el caso común.

### 6.2 IA como asistente

La IA asiste al responsable de planificación.  
**No debe** modificar un cuadrante de forma automática sin confirmación humana.

### 6.3 Plataforma, no vertical cerrada

El dominio se modela de forma configurable.  
Términos y reglas de un sector concreto viven en configuración/knowledge/specs, no como supuestos fijos del núcleo.

### 6.4 Arquitectura antes que feature

No se acepta una funcionalidad que rompa los límites arquitectónicos acordados.  
Si hay tensión, se revisa alcance o se enmienda la arquitectura con ADR + impacto en handbook.

### 6.5 El dominio manda

Ante conflicto entre comodidad técnica y corrección de negocio, prevalece el dominio — expresado en specs derivadas de `knowledge/`.

### 6.6 Trazabilidad

Toda decisión relevante de producto, arquitectura e implementación asistida por IA debe poder reconstruirse (handbook, ADR, spec, worklog, tests).

### 6.7 Documentación como producto

La documentación normativa y las specs no son un entregable posterior al código: son prerrequisito y parte del valor.

### 6.8 Especificación antes que implementación

No se implementa funcionalidad de producto sin especificación con criterios de aceptación.  
El gate operativo se detalla en Parte II.

### 6.9 Calidad sobre velocidad bruta

La IA acelera; no autoriza a saltarse tests, reviews ni gates.

### 6.10 Doble entregable

Cada incremento debe proteger tanto el **producto** como la **metodología SDAF** (véase preface).

---

## 7. Público objetivo

### Inicial (go-to-market de referencia)

- Hospitales y servicios sanitarios con turnos.
- Policía y fuerzas de seguridad.
- Bomberos y emergencias.
- Protección civil.
- Seguridad privada.

### Extensible (sin cambio de núcleo)

- Transporte, industria, logística, hoteles, call centers, aeropuertos, y cualquier organización basada en turnos.

### Roles de usuario (producto)

| Rol | Necesidad principal |
|-----|---------------------|
| Responsable de planificación | Construir y ajustar cuadrantes con reglas claras |
| Administrador de organización | Configurar estructura, tipos de turno, usuarios |
| Empleado (futuro / parcial) | Consultar turnos y ausencias |
| Stakeholders técnicos | API, auditoría, evolución de plataforma |

---

## 8. Propuesta de valor

- Configuración flexible de reglas y estructura organizativa.
- Planificación asistida con validación explícita de restricciones.
- Arquitectura preparada para evolucionar (no un prototipo desechable).
- IA integrada como infraestructura de asistencia, no como dueña del cuadrante.
- Trazabilidad de decisiones.
- Cliente web como superficie principal del MVP; otras superficies según roadmap.

---

## 9. Stakeholders

### Internos (ingeniería del producto)

- Product Owner / responsable de producto.
- Arquitecto / Director técnico.
- Desarrolladores (humanos y agentes bajo supervisión).
- QA / Testing.
- DevOps.

### Externos

- Organizaciones cliente.
- Responsables de planificación.
- Administradores.
- Empleados (usuarios finales).

---

## 10. Restricciones estratégicas

### Producto

- Orientación SaaS / plataforma multi-organización a medio plazo; el MVP puede usar tenancy simple sin multitenancy avanzado (detalle en cap. 03).
- Sector-agnóstico en el núcleo.

### Metodología

- Desarrollo gobernado por SDAF (Parte II).
- Specs y ADRs obligatorios antes de implementación de features.

### Tecnología (dirección de plataforma)

Stack de referencia del producto (no todo es alcance MVP; el corte fino está en cap. 03):

| Incluido en dirección | Diferido / no núcleo MVP |
|-----------------------|---------------------------|
| .NET, ASP.NET Core, EF Core, PostgreSQL | MAUI Hybrid (diferido por ADR de MVP) |
| Blazor Web App | Redis, SignalR hasta necesidad demostrada |
| Docker, .NET Aspire (mínimo) | Integraciones ERP / AD |
| MediatR, tests automatizados | Optimización automática e IA generativa de cuadrantes |

### Arquitectura (dirección)

- DDD, Clean Architecture, CQRS con Vertical Slices.
- Un bounded context inicial de scheduling; crecimiento por contexto cuando el dominio lo exija.
- IA fuera del dominio (adaptadores de infraestructura).

---

## 11. Riesgos estratégicos

| Riesgo | Mitigación de charter |
|--------|------------------------|
| Complejidad del dominio de reglas | Knowledge → specs; implementar pocas hard rules al inicio |
| Scope creep del MVP | Cap. 03 como frontera; cambios solo con enmienda |
| Dependencia excesiva de la IA | Human supervised; gates SDAF |
| Specs incompletas o ambiguas | Parar implementación; Specification Agent + revisión humana |
| Solución “hospitalaria” encubierta | Principio 6.3; review de lenguaje ubicuo |
| Sobre-arquitectura | Simplicidad priorizada; un BC; diferir motores avanzados |

---

## 12. Factores críticos de éxito (producto + ingeniería)

El proyecto va bien si:

1. La arquitectura admite evolución sin reescritura.
2. El dominio refleja el negocio configurable, no un vertical rígido.
3. Existe trazabilidad spec → código → test → worklog.
4. Los agentes pueden continuar el trabajo sin pérdida de contexto normativo.
5. El MVP es demostrable y honestamente acotado.
6. La calidad no se sacrifica por velocidad de generación de código.

Los criterios de cierre del MVP se fijan en el capítulo 03.

---

## 13. Relación con el resto del handbook

```text
01 Product Charter (este documento)
    → 02 Product Vision
    → 03 MVP Definition
    → 04 Product Roadmap
    → Parte II SDAF (cómo se construye)
    → specs/product y specs/domain
```

Este charter no sustituye al SDAF Framework (cap. 05) ni al detalle de MVP (cap. 03).

---

## 14. Criterios de aceptación de este capítulo (H2)

- [ ] La misión describe una plataforma configurable, no un vertical único.
- [ ] Los principios 6.1–6.10 son aceptables como norma de producto.
- [ ] El corte “dirección tecnológica” vs “alcance MVP” queda claro (detalle en cap. 03).
- [ ] MAUI / Redis / SignalR / optimización automática no aparecen como obligatorios del MVP aquí.
- [ ] Visión narrativa larga y lista fina de features MVP se dejan para caps. 02–03.

---

## 15. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-01 | Reescritura H2 desde semilla externa; alineado a SDAF y MVP Web-only |
