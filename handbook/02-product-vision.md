# 02 — Product Vision

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | I — Constitución de producto |
| **Norma superior** | [01-product-charter.md](01-product-charter.md) |
| **Deriva hacia** | [03-mvp-definition.md](03-mvp-definition.md), [04-product-roadmap.md](04-product-roadmap.md), `specs/product/` |

---

## 1. Propósito de este capítulo

Describir **hacia dónde** va ShiftFlow como producto, sin confundirlo con el alcance inmediato del MVP (capítulo 03).

La visión orienta prioridades a medio plazo. El MVP es un corte demostrable en el camino.

---

## 2. Visión

ShiftFlow será la plataforma de referencia para la planificación inteligente de personal en organizaciones que trabajan por turnos: configurable, auditable y asistida por IA, donde el responsable de planificación decide y el sistema valida, explica y propone.

No aspira a ser solo un gestor de cuadrantes. Aspira a ser un **colaborador de planificación** que reduce fricción operativa sin sustituir la responsabilidad humana.

---

## 3. Posicionamiento

| Somos | No somos |
|-------|----------|
| Plataforma configurable multi-sector | Vertical cerrado de un único sector |
| Sistema de reglas + asignación con trazabilidad | Caja negra que “publica turnos sola” |
| Asistente de IA supervisado | Autopiloto sin confirmación humana |
| Producto + metodología SDAF | Solo código sin gobierno |

---

## 4. Experiencia objetivo (norte)

Un responsable de planificación debería poder:

1. Modelar su organización (estructura, personas, tipos de turno) sin desarrollo a medida.
2. Ver un calendario mensual claro y asignar turnos con feedback inmediato de reglas.
3. Entender **por qué** una asignación es inválida o frágil.
4. Pedir ayuda a la IA (explicación, detección de conflictos, propuestas) y **aceptar o rechazar** el resultado.
5. Confiar en que el historial de decisiones es reconstruible.

---

## 5. Horizonte de producto (post-MVP)

Sin compromiso de fecha en este capítulo; dirección de evolución:

- Reglas avanzadas configurables (hard/soft) derivadas de knowledge real.
- Generación asistida de cuadrantes con confirmación humana.
- Optimización bajo restricciones.
- Colaboración en tiempo real entre roles.
- Superficies adicionales (p. ej. MAUI Hybrid) cuando el Web esté consolidado.
- Multitenancy e integraciones empresariales (ERP, directorio) según demanda.

El detalle de qué entra o no en el corte actual está en el capítulo 03.

---

## 6. Éxito de la visión (señales)

La visión se considera en buen camino si, a lo largo de las versiones:

- Un nuevo sector se incorpora principalmente por configuración y specs, no por fork.
- Las reglas críticas se evalúan de forma explícita y explicable.
- La IA aumenta productividad sin erosionar la autoridad del planificador.
- El handbook y las specs permiten continuar el producto con nuevos agentes o personas.

---

## 7. Criterios de aceptación de este capítulo (H3)

- [ ] La visión distingue claramente plataforma vs vertical y asistencia vs autopiloto.
- [ ] No mezcla lista de features del MVP (eso es cap. 03).
- [ ] El horizonte post-MVP es dirección, no compromiso de entrega del 22 ago.

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H3) |
