# LiqPagoEstandar

Aplicación que genera un resumen de pago mensual estándar para el personal de casa de familia de los clientes de un estudio contable.

## Referencias

- @PRD.md

## Stack

- Angular 17.3.17 + SQL Server 2022
- API en C# para consultas y escritura a la Base de Datos

## Cómo correr

- Frontend: `ng serve`
- Backend/API: `dotnet run`
- Migrations (EF Core): `dotnet ef database update`
- Tests backend: `dotnet test`
- Tests frontend: `ng test`
- Variables de entorno: copiar `.env.example` a `.env` con `ANTHROPIC_API_KEY`

## Qué NO hacer

- NO hardcodear la API key: va en `.env` como `ANTHROPIC_API_KEY`.
- NO agregar features fuera del PRD.
