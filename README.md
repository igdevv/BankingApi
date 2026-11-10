# BankingApi — ASP.NET Core 10

API REST **educativa** para practicar operaciones bancarias y reglas de negocio con C#, Entity Framework Core y SQL Server.

> **Alcance:** proyecto de portafolio. No es un sistema bancario real ni está certificado, auditado o preparado para custodiar dinero.

## Funcionalidades presentes en el código

- Registro de usuarios con contraseñas almacenadas mediante BCrypt.
- Creación y consulta de cuentas.
- Depósitos, retiros y consulta de movimientos.
- Endpoint de transferencias entre cuentas.
- DTOs, servicios, validaciones y manejo centralizado de excepciones.
- Persistencia mediante EF Core y migraciones SQL Server.

## Estructura

`BankingApi/Controllers` expone HTTP; `Services` concentra casos de uso; `Entities` modela datos; `Data/AppDbContext.cs` configura persistencia; `Validators` contiene validaciones de solicitudes.

## Ejecutar localmente

Requiere .NET SDK 10 y SQL Server o LocalDB. Configura `ConnectionStrings__DefaultConnection` en el entorno o User Secrets; nunca publiques credenciales.

```powershell
dotnet restore BankingApi.slnx
dotnet build BankingApi.slnx
dotnet ef database update --project BankingApi
dotnet run --project BankingApi
```

Swagger está disponible en `/swagger` durante `Development`.

## Rutas principales

| Método | Ruta | Propósito |
| --- | --- | --- |
| POST | `/api/users` | Crear usuario |
| GET | `/api/users/{id}` | Consultar usuario |
| POST | `/api/accounts` | Crear cuenta |
| GET | `/api/accounts/{id}` | Consultar cuenta |
| POST | `/api/accounts/{id}/deposit` | Depositar |
| POST | `/api/accounts/{id}/withdraw` | Retirar |
| GET | `/api/accounts/{id}/transactions` | Consultar movimientos |
| POST | `/api/transfers` | Transferir |

## Limitaciones y siguientes pasos

No se ha validado una garantía de concurrencia, idempotencia ni integridad financiera bajo fallos. Faltan pruebas automatizadas de integración, autorización robusta, auditoría y controles operativos. **No utilizar con dinero ni datos reales.** La automatización de CI verifica la compilación, no el comportamiento bancario.

**Autor:** José Ignacio Suero.
