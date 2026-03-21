# Template Clean Architecture (.NET)

Plantilla base para proyectos .NET estructurados con Clean Architecture, diseñada para acelerar la creación de aplicaciones con buena separación de responsabilidades y fácil mantenimiento.

## 🧱 ¿Qué incluye este template?

Este template sigue los principios de **Clean Architecture**, distribuyendo la solución en capas claramente definidas:

- **Domain** – Entidades, Value Objects, reglas de negocio.
- **Application** – Casos de uso, interfaces, lógica de aplicación.
- **Infrastructure** – Acceso a datos, implementación de repositorios y servicios externos.
- **Api / Web** – Punto de entrada (API REST), configuración y endpoints.

> Esta estructura te permite escalar tu aplicación de forma limpia y testeable.

---

## 🚀 Cómo usar esta plantilla

### Opción 1 — Usar como repositorio base (GitHub Template)

1. Haz clic en **Use this template** en la página del repositorio.
2. Crea un nuevo repositorio a partir de esta plantilla.
3. Clona tu nuevo repo y comienza a trabajar.

Ejemplo:
```bash
git clone https://github.com/tu-usuario/mi-proyecto.git
cd mi-proyecto
