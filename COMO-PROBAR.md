# Cómo probar LiqPagoEstandar

Guía consolidada de todo lo que fuimos viendo en la conversación, para no tener que scrollear en el chat.

Todos los comandos van en **PowerShell nativo de Windows** (no en WSL), porque SQL Server usa autenticación de Windows que no cruza bien la frontera WSL↔Windows.

El proyecto entero se movió de `C:\Users\Viky_Chavez\IAFirstBuilders` a **`D:\IAFirstBuilders`** (por espacio en disco). Todas las rutas de acá abajo ya están actualizadas a D:.

---

## 1. Aplicar la migración a la base de datos

```powershell
cd D:\IAFirstBuilders\backend
dotnet tool install --global dotnet-ef --version 8.0.11   # solo la primera vez
dotnet ef database update --project src/LiqPagoEstandar.Data --startup-project src/LiqPagoEstandar.Api
```

**Estado: ✅ ya lo corriste (cuando el proyecto estaba en C:) y terminó con "Done". No hace falta repetirlo — la base de datos ya existe en tu SQL Server, independientemente de en qué disco esté el código.**

---

## 2. Reconstruir el backend en la nueva ubicación

Por las dudas, ya que `bin`/`obj` tenían referencias a la ruta vieja en C::

```powershell
cd D:\IAFirstBuilders\backend
Get-ChildItem -Recurse -Directory -Include bin,obj | Remove-Item -Recurse -Force
dotnet build
```

---

## 3. Levantar el backend

```powershell
cd D:\IAFirstBuilders\backend\src\LiqPagoEstandar.Api
dotnet run
```

Debería mostrar `Now listening on: https://localhost:7042` y abrir Swagger (`/swagger`) — si no abre solo, entrá manualmente a `https://localhost:7042/swagger`.

---

## 4. Levantar el frontend

```powershell
cd D:\IAFirstBuilders\frontend
ng serve
```

Esperá a ver `Application bundle generation complete` y `Local: http://localhost:4200/`.

**Estado: ✅ `npm install` ya se hizo con éxito en D: (usando `--cache "D:\npm-cache"` y `$env:TEMP`/`$env:TMP` apuntando a D: para evitar el error ENOSPC de C:).**

---

## 5. Login

Abrí `http://localhost:4200` en el navegador.

- Usuario: `admin`
- Contraseña: `Admin123!`

---

## 6. Recorrido del flujo completo (golden path)

1. **Categorías** → crear una (ej. "Niñera", valor hora con/sin retiro).
2. **Clientes** → crear uno con email real o de prueba.
3. Desde el listado de Clientes, ícono de personas → **Personal** → crear uno, asignarle la categoría creada.
4. **Novedades** → seleccionar mes/año actual, cargar horas normales/extras/feriado para ese personal, guardar.
5. **Resumen** → mismo mes/año → "Generar Resumen" → revisar el detalle calculado, descargar el PDF de esa fila.
6. **Parámetros** → cambiar un valor (ej. multiplicador de feriados) y regenerar el resumen → confirmar que el resultado cambia (valida RF-28).
7. Dar de baja el Personal → regenerar el resumen del mismo período → confirmar que ya no aparece (valida AC-04).
8. **Enviar por Email** → necesita SMTP configurado (ver punto 7 abajo).

---

## 7. Para probar el envío de email

Configurá `D:\IAFirstBuilders\backend\src\LiqPagoEstandar.Api\appsettings.Development.json`:

```json
"Smtp": {
  "Host": "smtp.ethereal.email",
  "Port": 587,
  "User": "tu_usuario",
  "Password": "tu_password",
  "From": "no-reply@liqpago.local",
  "UseSsl": true
}
```

Para pruebas rápidas sin cuenta real: [Ethereal Email](https://ethereal.email/) o Mailtrap dan credenciales SMTP de prueba gratis en un click.

---

## Tests automatizados

```powershell
cd D:\IAFirstBuilders\backend
dotnet test     # 14 tests de Core, ya verificados en verde
```

```powershell
cd D:\IAFirstBuilders\frontend
ng test         # necesita Chrome instalado localmente
```
