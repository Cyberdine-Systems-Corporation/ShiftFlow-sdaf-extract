# Guion — vídeo arquitectura y gobernanza (`mvp-0.1`)

| Campo | Valor |
|--------|--------|
| PBI | PBI-012 |
| Audiencia | Evaluador de máster en **desarrollo asistido por IA** |
| Registro | Decisiones y método, no inventario de tecnologías |
| Duración objetivo | **8–10 min** (handbook §4.5 orienta 8–15) |
| Palabras | ~1 430 (ritmo ~140 pal/min ≈ 10:10; recortar según §Notas para bajar de 10:00) |
| Visuales | [video-slides.md](video-slides.md) (láminas 1–17) |
| No cubrir | Demo clic a clic; nombres de clases; configuración |

Criterio de tono: cada tecnología se nombra **una vez** y siempre unida a la restricción que resuelve. El peso del relato está en **por qué** se decidió, **quién** decidió (humano o agente) y **qué evidencia** queda.

Las láminas llevan **más detalle que la locución** (identificadores de ADR y spec, semántica de las reglas, cifras del repositorio). Es deliberado: quien escucha sigue el argumento; quien pausa la imagen puede verificarlo.

| Bloque | Láminas |
|--------|---------|
| 1 — Qué se está evaluando | 1, 2 |
| 2 — La primera decisión fue de alcance | 3, 4 |
| 3 — Fronteras antes que componentes | 5, 6, 7 |
| 4 — Dónde ponemos la IA del producto | 8, 9, 10 |
| 5 — Especificación antes que generación | 11, 12 |
| 6 — Puertas de control y evidencia | 13, 14 |
| 7 — Qué enseñó el experimento | 15 |
| 8 — Camino y cierre | 16, 17 |

---

## Bloque 1 — Qué se está evaluando (0:00–0:50) · láminas 1–2

ShiftFlow es un sistema de planificación de turnos. Pero lo que traigo a evaluación no es solo la aplicación: son dos entregables que se sostienen mutuamente.

El primero es un producto demostrable en local, congelado en la etiqueta mvp-cero-punto-uno.

El segundo es el método con el que se construyó: un marco de desarrollo asistido por IA en el que las especificaciones gobiernan a los agentes, y no al revés. Lo llamo SDAF, Spec-Driven AI Development Framework.

En este vídeo hablo de decisiones de arquitectura y de gobierno del trabajo con IA. El recorrido funcional de la aplicación está en las slides de producto.

---

## Bloque 2 — La primera decisión fue de alcance (0:50–1:50) · láminas 3–4

La primera decisión arquitectónica de un proyecto asistido por IA no es técnica: es de alcance. Con un asistente que genera código muy rápido, el riesgo no es escribir poco, es escribir demasiado y sin control.

El dominio de partida era un documento funcional experto con reglas complejas: turnos pares e impares, bolsas mensuales de horas, cuotas nocturnas, validaciones entre meses. Todo eso es valioso y está guardado como conocimiento, pero implementarlo habría producido un sistema grande, frágil y sin demostrar.

Por eso el corte del MVP fija un techo explícito: un solo contexto de negocio, asignación manual y como máximo tres reglas duras. Esa frontera no vive en la cabeza de nadie: está escrita, aprobada y es la que frena al asistente cuando propone de más.

La lección de arquitectura es simple. Cuando la capacidad de producción deja de ser el cuello de botella, el cuello de botella pasa a ser la decisión. Y la decisión hay que documentarla.

---

## Bloque 3 — Fronteras antes que componentes (1:50–3:30) · láminas 5–7

La arquitectura de ShiftFlow se define por sus fronteras, no por su lista de piezas.

Primera frontera: una sola superficie de usuario. Existía la tentación de una aplicación web y otra móvil híbrida compartiendo componentes. Se descartó para este corte, con decisión registrada, porque duplicar cliente multiplica la superficie de error sin añadir nada a lo que hay que demostrar.

Segunda frontera: la lógica de negocio vive en el dominio y solo allí. La capa de aplicación orquesta casos de uso separando lo que modifica de lo que solo consulta. La infraestructura adapta persistencia e identidad. La interfaz no reimplementa ninguna regla: cuando el calendario muestra un rechazo, es porque el dominio ya lo evaluó y respondió. Esto importa especialmente con un asistente: es muy fácil que la IA resuelva un problema donde le resulta cómodo, normalmente en la pantalla, y erosione el modelo sin que nadie lo note.

Tercera frontera: un único contexto delimitado, planificación de personal. No hay microservicios. La autenticación es un subdominio de soporte, no un contexto propio.

Y una frontera de infraestructura que también es de producto: el sistema debe poder evaluarse en una máquina local, sin cuenta en la nube. Un orquestador local levanta base de datos, servicio y cliente con un comando. La nube puede venir después; no puede ser el peaje para probar el trabajo.

Un apunte de método antes de seguir: dos de estas decisiones se enmendaron sobre la marcha —la versión de plataforma y la estrategia de pruebas de integración—, y la enmienda quedó fechada dentro del propio documento de decisión. Un registro con historial vale más que uno que aparenta haber acertado a la primera.

---

## Bloque 4 — Dónde ponemos la IA dentro del producto (3:30–5:00) · láminas 8–10

Aquí está, para esta audiencia, la decisión más interesante.

El producto planifica turnos y valida reglas. Era tentador que un modelo generativo propusiera cuadrantes completos. Se decidió lo contrario, y quedó por escrito: en este corte, la inteligencia artificial **no escribe** en el cuadrante.

Lo que sí hace es explicar. Cuando el motor de reglas rechaza una asignación —por solape, por ausencia activa o por descanso mínimo insuficiente—, un componente de infraestructura devuelve una explicación en lenguaje natural de por qué se ha rechazado. Es un adaptador determinista detrás de un puerto del dominio, no un modelo obligatorio.

Esa decisión tiene tres consecuencias arquitectónicas. La autoridad sobre lo que es válido permanece en el dominio, que es auditable y testeable. La IA queda en el borde, como un adaptador sustituible: mañana puede haber un modelo de lenguaje real detrás del mismo puerto sin tocar el núcleo. Y el sistema nunca depende de que una respuesta generativa sea correcta para preservar una regla laboral.

Dicho de otro modo: la IA explica, el dominio decide. Esa asimetría es deliberada y es el patrón que defiendo para sistemas con reglas normativas.

---

## Bloque 5 — El método: especificación antes que generación (5:00–6:40) · láminas 11–12

Paso al segundo entregable, que es el que sitúa este trabajo en desarrollo asistido por IA.

SDAF organiza el proyecto como una jerarquía normativa. El conocimiento del experto es la fuente y es inmutable. Sobre él hay un handbook que actúa de constitución. De ahí bajan las especificaciones: producto, dominio, aplicación y aceptación. Las decisiones estructurales se registran como decisiones de arquitectura fechadas. Solo entonces se implementa, y los tests derivan de las especificaciones.

Sobre esa jerarquía trabajan agentes con rol acotado. Uno redacta especificaciones. Otro propone decisiones de arquitectura. Otro implementa dominio y casos de uso. Otro construye la interfaz. Otro revisa y prueba. Cada uno tiene un contrato escrito de lo que puede y no puede hacer, y un prompt versionado.

Ninguno de esos agentes puede aprobar una especificación por su cuenta. La aprobación es humana. El asistente propone; la arquitectura la firma una persona.

El relevo entre agentes tampoco ocurre en la conversación. Cada iteración deja un registro de trabajo con las especificaciones usadas, los archivos tocados, las pruebas ejecutadas, el estado y quién continúa. Si mañana se pierde el historial de chat, el proyecto sigue siendo reconstruible. Esa es la diferencia entre usar un asistente y tener un método.

---

## Bloque 6 — Puertas de control y evidencia (6:40–8:10) · láminas 13–14

El método se hace exigible con cuatro puertas.

La puerta cero es la más importante: antes de escribir código de producto tiene que existir especificación aprobada, criterios de aceptación, decisión de arquitectura si el cambio toca fronteras, elemento de backlog y registro de trabajo abierto. Si falta uno, se para. No se codifica un poco para ver qué sale. Esa regla es la que impide que la velocidad del asistente se convierta en deuda.

La puerta uno gobierna la ejecución: cambios en rebanada vertical y sin ampliar lo que está excluido.

La puerta dos condiciona la integración: pruebas de aceptación en verde, revisión con lista de comprobación y arranque local que sigue funcionando.

La puerta tres es el cierre del MVP: demo reproducible, presentación y etiqueta.

La evidencia de que esto se cumplió no es una afirmación mía. Está en el repositorio: decisiones de arquitectura fechadas, especificaciones con estado, registros de trabajo por iteración e informes de revisión asociados a cada integración. Un evaluador puede recorrer el camino desde una regla de negocio hasta la prueba que la verifica.

---

## Bloque 7 — Qué enseñó el experimento (8:10–9:10) · lámina 15

Tres aprendizajes que me parecen transferibles.

Primero: el valor del asistente crece con la calidad del contexto escrito, no con la longitud del prompt. Los tramos donde la especificación era precisa se resolvieron limpiamente; donde era ambigua, el agente inventó alcance y hubo que retroceder.

Segundo: la trazabilidad no es burocracia, es lo que permite cambiar de modelo, de herramienta o de sesión sin perder el proyecto. El registro de trabajo es el mecanismo de continuidad.

Tercero: hay que decidir explícitamente qué no delega uno. Aquí lo no delegado fue la aprobación de especificaciones, la validación de la demo y el criterio de alcance. El asistente redactó, propuso e implementó; no decidió qué es el producto.

También hay límites honestos: no hay pruebas automáticas de extremo a extremo sobre la interfaz, la cobertura de aceptación vive en la capa de servicio, y varias reglas avanzadas quedan documentadas pero no implementadas. Están registradas como tales, no escondidas.

---

## Bloque 8 — Camino recorrido y cierre (9:10–9:50) · láminas 16–17

En tres semanas el proyecto fue de un repositorio gobernado sin código de producto, a maestros y acceso, luego a calendario con reglas y ausencias, y finalmente a pulido, explicación de reglas, arranque en frío verificado y etiqueta de versión.

Lo que queda fuera está escrito: optimización automática de cuadrantes, generación de turnos por IA, aplicación nativa, colaboración en tiempo real, informes avanzados y despliegue en la nube como vía de evaluación.

Para ver el producto funcionando —organizaciones, calendario, un rechazo de regla explicado— están las slides y el runbook local.

Cierro con la idea que estructura todo el trabajo: en desarrollo asistido por IA, la especificación es la arquitectura ejecutable, y la trazabilidad es lo que convierte una sesión de asistente en un sistema de ingeniería.

---

## Notas de locución

- Primera mención: «eme-uve-pe cero punto uno»; después «mvp-cero-punto-uno».
- Evitar siglas habladas: decir «rebanada vertical», «puerta cero», «registro de trabajo», «decisión de arquitectura registrada». Las siglas viven en las láminas.
- No enumerar tecnologías seguidas. Cada una aparece unida a su restricción.
- Ritmo: pausa de medio segundo tras cada frase-tesis (final de bloques 2, 4, 5 y 8).
- No mostrar la interfaz de la aplicación (opcional: 10–15 s del panel del orquestador local en el bloque 3).
- Láminas que **no** se narran y se dejan leer (2–3 s extra en pantalla): 2 (inventario de evidencia), 6 (grafo de dependencias), 9 (semántica de HR-01…03) y 14 (cadena de trazabilidad). No leerlas en voz alta.
- Si hay que recortar, en este orden: apunte de enmiendas del bloque 3 → bloque 7 (aprendizajes) → resto del detalle del bloque 3 → lista de exclusiones del 8.

## Checklist de ensayo (Fase 0.3)

- [ ] Una pasada cronometrada entre 8:00 y 10:00
- [ ] Suena a decisiones y trade-offs, no a listado de stack
- [ ] Queda claro qué hizo la IA y qué se reservó el humano
- [ ] Se nombra evidencia verificable en el repositorio
- [ ] Cierre con puente explícito a slides + runbook
