# ShiftFlow Engineering Handbook

**Versión:** 0.1 (Draft)  
**Estado:** Draft  
**Fecha:** Julio 2026  
**Autor:** Equipo de Ingeniería ShiftFlow  
**Clasificación:** Documento Fundacional

---

# Capítulo 1 — Product Charter

> *"Este documento define el propósito, la visión, los principios y el alcance estratégico de ShiftFlow. Constituye el punto de partida de todas las decisiones técnicas, funcionales y organizativas del proyecto."*

---

# 1. Introducción

## 1.1 Propósito

El presente documento establece los principios fundacionales del proyecto **ShiftFlow**.

Su finalidad es definir de forma inequívoca:

- Qué problema pretende resolver el producto.
- Qué objetivos persigue.
- Cuáles son los principios que deben guiar todas las decisiones.
- Qué alcance tendrá el MVP.
- Qué restricciones condicionan el proyecto.
- Qué criterios determinarán el éxito del producto.

Este documento constituye la referencia principal para:

- Product Owners
- Arquitectos
- Desarrolladores
- Agentes IA
- Revisores
- Stakeholders

Ninguna decisión arquitectónica, funcional o técnica deberá contradecir este documento sin una revisión formal.

---

# 2. Declaración de Misión

## Misión

Desarrollar una plataforma inteligente de planificación y gestión de turnos que permita a cualquier organización optimizar la asignación de personal mediante reglas configurables, automatización e inteligencia artificial, reduciendo la complejidad operativa y mejorando la eficiencia de los equipos.

---

# 3. Declaración de Visión

## Visión

Convertirse en la plataforma de referencia para la planificación inteligente de recursos humanos en organizaciones que trabajan mediante turnos, proporcionando una experiencia moderna, configurable y asistida por IA.

ShiftFlow no pretende ser únicamente un gestor de cuadrantes.

Su objetivo es convertirse en un asistente inteligente capaz de colaborar con los responsables de planificación.

---

# 4. Problema que resuelve

Actualmente muchas organizaciones realizan la planificación de turnos mediante:

- Excel
- Aplicaciones heredadas
- Soluciones rígidas
- Procesos manuales

Esto genera:

- Errores.
- Conflictos.
- Incumplimientos normativos.
- Baja productividad.
- Dificultad para introducir cambios.

ShiftFlow nace para resolver estos problemas.

---

# 5. Oportunidad

Cada organización posee reglas distintas.

Por ejemplo:

### Hospitales

- Guardias.
- Festivos.
- Especialidades.
- Disponibilidad.

### Bomberos

- Retenes.
- Refuerzos.
- Guardias localizadas.

### Policía

- Servicios especiales.
- Rotaciones.
- Disponibilidad.

Actualmente la mayoría de soluciones requieren desarrollos específicos.

ShiftFlow pretende ofrecer una plataforma configurable donde las reglas sean datos y no código.

---

# 6. Objetivos Estratégicos

El producto deberá:

- Simplificar la planificación.
- Reducir errores.
- Disminuir el tiempo necesario para generar cuadrantes.
- Facilitar modificaciones.
- Permitir colaboración.
- Mejorar la trazabilidad.
- Incorporar IA de forma progresiva.

---

# 7. Principios del Producto

Todo el desarrollo deberá respetar los siguientes principios.

## 7.1 Configuración sobre programación

Las reglas deberán configurarse.

No programarse.

---

## 7.2 IA como asistente

La IA nunca sustituirá al responsable.

Lo asistirá.

---

## 7.3 Arquitectura antes que funcionalidades

Nunca se añadirá una funcionalidad comprometiendo la arquitectura.

---

## 7.4 El dominio manda

Las decisiones de negocio prevalecen sobre las decisiones técnicas.

---

## 7.5 Todo debe ser trazable

Cada decisión deberá poder reconstruirse.

---

## 7.6 La documentación es código

La documentación forma parte del producto.

No es un entregable posterior.

---

## 7.7 Especificación antes que implementación

Nunca se implementará una funcionalidad sin una especificación aprobada.

---

## 7.8 Calidad sobre velocidad

La IA acelera el desarrollo.

No elimina la necesidad de calidad.

---

# 8. Público objetivo

El producto está orientado inicialmente a:

- Hospitales.
- Policía.
- Bomberos.
- Protección Civil.
- Seguridad Privada.

Posteriormente podrá extenderse a:

- Transporte.
- Industria.
- Logística.
- Hoteles.
- Call Centers.
- Aeropuertos.

---

# 9. Propuesta de Valor

ShiftFlow ofrece:

- Configuración flexible.
- Arquitectura moderna.
- IA integrada.
- Escalabilidad.
- Trazabilidad.
- Plataforma multiplataforma.

---

# 10. Objetivos del MVP

El MVP deberá permitir:

- Gestionar organizaciones.
- Gestionar departamentos.
- Gestionar empleados.
- Gestionar tipos de turno.
- Crear calendarios.
- Asignar turnos manualmente.
- Gestionar vacaciones.
- Gestionar usuarios.
- Gestionar roles.
- Mostrar un calendario mensual.
- Exponer una API REST.
- Disponer de cliente Blazor Web App.
- Disponer de cliente .NET MAUI Blazor Hybrid.
- Mantener documentación técnica completa.

---

# 11. Fuera del Alcance

No formarán parte del MVP:

- Optimización automática de cuadrantes.
- Planificación mediante IA.
- Machine Learning.
- Predicción de demanda.
- Integración con ERP.
- Integración con Active Directory.
- Informes avanzados.
- Multitenancy avanzado.
- Funcionamiento offline.
- Aplicaciones móviles nativas independientes.

---

# 12. Factores Críticos de Éxito

El proyecto se considerará exitoso si:

- La arquitectura soporta la evolución futura.
- El dominio representa correctamente el negocio.
- La plataforma es fácilmente ampliable.
- Los agentes IA pueden trabajar de forma coordinada.
- La documentación permite reproducir el desarrollo.
- El código mantiene una alta calidad.

---

# 13. Stakeholders

## Internos

- Product Owner.
- Arquitecto.
- Desarrolladores.
- QA.
- DevOps.

## Externos

- Clientes.
- Responsables de planificación.
- Administradores.
- Empleados.

---

# 14. Restricciones

## Tecnológicas

- .NET 10.
- ASP.NET Core.
- Blazor Web App.
- .NET MAUI Blazor Hybrid.
- Entity Framework Core.
- PostgreSQL.
- SignalR.
- Redis.
- Docker.
- .NET Aspire.

## Arquitectónicas

- Domain-Driven Design (DDD).
- CQRS.
- Clean Architecture.
- Vertical Slice Architecture.

## Metodológicas

- Spec-Driven AI Development Framework (SDAF).

---

# 15. Riesgos Iniciales

Riesgos identificados:

- Complejidad del dominio.
- Crecimiento excesivo del alcance del MVP.
- Dependencia excesiva de la IA.
- Especificaciones incompletas.
- Reglas de negocio ambiguas.
- Cambios frecuentes de requisitos.
- Subestimación del esfuerzo de integración.

---

# 16. Criterios de Éxito del MVP

El MVP se considerará finalizado cuando:

- Todas las funcionalidades definidas estén implementadas.
- Las pruebas críticas sean satisfactorias.
- La documentación esté actualizada.
- La arquitectura permanezca limpia y mantenible.
- Exista trazabilidad completa entre especificaciones, código y pruebas.
- Los agentes puedan continuar desarrollando el producto sin pérdida de contexto.

---

# 17. Principios de Ingeniería

ShiftFlow se desarrollará siguiendo los siguientes principios:

- Architecture First.
- Specification First.
- AI Assisted.
- Human Supervised.
- Documentation Driven.
- Domain Centric.
- Test First (cuando sea viable).
- Automation First.
- Simplicity over Cleverness.
- Evolutionary Architecture.

---

# 18. Relación con el SDAF

Este documento constituye el nivel superior del **Spec-Driven AI Development Framework (SDAF)**.

La jerarquía normativa será:

```text
Engineering Handbook
        │
        ▼
Product Vision
        │
        ▼
Engineering Principles
        │
        ▼
Specifications
        │
        ▼
Architecture
        │
        ▼
Implementation
        │
        ▼
Testing
        │
        ▼
Release
```

Toda decisión técnica deberá poder justificarse remontándose hasta este capítulo.

---

# 19. Historial de Versiones

| Versión | Fecha | Autor | Descripción |
|----------|--------|--------|-------------|
| 0.1 | Julio 2026 | Equipo de Ingeniería ShiftFlow | Primera versión del Product Charter. |

---

# 20. Aprobación

Este documento deberá ser revisado y aprobado antes de iniciar el desarrollo de cualquier especificación funcional, arquitectura o componente de software.

Su modificación requerirá una revisión formal y deberá quedar registrada en el historial de cambios del **ShiftFlow Engineering Handbook**.