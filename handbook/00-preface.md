# 00 — Preface

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.0 |
| **Estado** | Approved |
| **Fecha** | 2026-08-01 |
| **Parte** | Front matter |
| **Norma superior** | Ninguna (este documento inaugura la constitución) |
| **Deriva hacia** | Todo el handbook; en especial Partes I y II |

---

## 1. Por qué existe este handbook

ShiftFlow no se construye solo como una aplicación de turnos.

Se construye también como un **sistema de ingeniería** reutilizable: Spec-Driven AI Development Framework (SDAF).

Este handbook existe para que:

- Las decisiones importantes sean explícitas y auditables.
- Humanos y agentes IA compartan las mismas reglas.
- El código derive de especificaciones, no al revés.
- Un nuevo colaborador (o un nuevo agente) pueda retomar el proyecto sin depender de conversaciones perdidas.

Sin constitución, la velocidad con IA produce deuda opaca. Con constitución, la velocidad es gobernada.

---

## 2. Qué es y qué no es

### Es

- La **constitución** del proyecto y de la metodología.
- La fuente de principios, alcance de producto (cuando esté Approved) y reglas de trabajo.
- El contrato que deben respetar specs, ADRs, prompts, agentes e implementación.

### No es

- Un documento de requisitos detallados (eso vive en `specs/`).
- Un registro de decisiones tácticas (eso vive en ADRs).
- Un log de trabajo de agentes (eso vive en `worklogs/`).
- Un manual de usuario final del producto.
- Un sustituto del knowledge de expertos (`knowledge/` permanece inmutable y primario para el dominio).

---

## 3. Autoridad

1. Mientras un capítulo esté en **Draft**, orienta el trabajo pero puede corregirse en la misma sesión de co-creación sin ceremonia.
2. Cuando un capítulo pase a **Approved**, solo puede cambiarse mediante:
   - propuesta explícita de enmienda,
   - revisión humana,
   - actualización de versión del capítulo,
   - entrada en `handbook/CHANGELOG.md`.
3. Ningún agente IA puede autodeclarar un capítulo como Approved.
4. Ninguna implementación puede contradecir un capítulo Approved. Si la realidad del código lo exige, primero se enmienda la norma o se registra un ADR de excepción temporal con fecha de caducidad.

---

## 4. Doble entregable del proyecto

ShiftFlow persigue, en paralelo y con el mismo rigor:

| Entregable | Descripción |
|------------|-------------|
| **Producto** | Plataforma SaaS configurable de planificación de turnos (MVP demostrable) |
| **Metodología** | SDAF: conocimiento → specs → arquitectura → implementación trazable con agentes |

Ninguno de los dos justifica sacrificar al otro sin decisión explícita en Parte I (MVP) y Parte II (SDAF).

---

## 5. Cómo leer este handbook

1. Empieza por este preface y el [índice](README.md).
2. Lee la **Parte I** para entender el producto y el alcance.
3. Lee la **Parte II** antes de escribir código o prompts de implementación.
4. Consulta Partes III–V según tu rol (arquitectura, agentes, calidad).
5. Usa la Parte VI para ritmo, métricas y planificación.
6. Usa los apéndices como referencia rápida (glosario, plantillas).

Regla práctica: si vas a implementar y no puedes citar spec + ADR + criterio de aceptación, **párate** y vuelve a la Parte II.

---

## 6. Cómo se escribe (co-creación)

Este handbook se elabora en sesiones numeradas (H1, H2, …):

1. Se redacta un borrador de capítulo.
2. El responsable humano revisa, corrige o veta.
3. Se marca Draft o Approved en la cabecera y en el índice.
4. Solo entonces se avanza al siguiente bloque.

Prioridad de profundidad para el MVP:

- Partes **I y II**: profundas y bloqueantes.
- Partes **III–V**: mínimas pero operables.
- Parte **VI** y apéndices: breves.

---

## 7. Idioma y estilo

- Idioma oficial del handbook: **español**.
- Estilo: normativo, corto, verificable (“debe”, “no debe”, “puede”).
- Evitar ensayos, marketing vacío y reglas imposibles de auditar.
- Los términos de dominio se alinearán con el glosario (Apéndice A) cuando exista.

---

## 8. Audiencia

- Arquitecto / Director técnico humano  
- Product Owner  
- Desarrolladores  
- Agentes IA especializados del repositorio  
- Revisores de calidad y arquitectura  

Todos están sujetos a la misma constitución. La IA no tiene privilegios para saltarse gates.

---

## 9. Criterios de aceptación de este capítulo (H1)

Este preface se considerará listo para Passed a Approved cuando el lector humano confirme que:

- [ ] Queda claro que el handbook es constitución, no tutorial.
- [ ] Queda clara la autoridad Draft vs Approved.
- [ ] Queda claro el doble entregable (producto + SDAF).
- [ ] Queda clara la regla de parar si no hay spec/ADR/aceptación.
- [ ] El índice en `README.md` refleja el mapa de capítulos acordado.

---

## 10. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-01 | Borrador inicial (sesión H1); Approved tras revisión humana |
