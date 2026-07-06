# PRD-001: LiqPagoEstandar — Generar Resumen de Pago Mensual Estándar para Personal de Casa de Familia

## Contexto y Problema

Un estudio contable requiere una aplicación que le genere un resumen de pago mensual estándar para el personal de casa de familias de sus clientes.

Personas:

- El contador envía mensualmente por correo electrónico el resumen de pago a sus clientes.
- El cliente puede tener 1 o más personal de casa de familia.

## Objetivos

- Generar el resumen de pago de cada personal de casa de familia.
- La aplicación hace el cálculo de los ítems a pagar y obtiene el total a pagar de cada personal.
- Cada cliente recibe por correo electrónico un archivo PDF detallado de los ítems y el total general a pagar a cada personal.

## Requerimientos Funcionales

### Acceso y generación

- **RF-01:** El sistema debe autenticar al usuario mediante usuario y contraseña antes de permitir el acceso a cualquier funcionalidad.
- **RF-02:** El sistema debe permitir al usuario autenticado seleccionar el mes y año a liquidar.
- **RF-03:** El sistema debe generar el resumen de pago para todos los personales correspondientes al mes seleccionado.

### Mantenimiento de tablas

- **RF-04:** El sistema debe permitir al usuario registrar, modificar y eliminar Clientes del estudio contable.
- **RF-05:** El sistema debe permitir al usuario registrar, modificar y eliminar Categorías con su valor hora con retiro y sin retiro.
- **RF-06:** El sistema debe permitir al usuario registrar, modificar y eliminar el Personal de cada cliente (DNI, cliente al que pertenece, fecha de ingreso, apellido, nombre, dirección, teléfono, categoría, con/sin retiro, provincia).
- **RF-07:** El sistema debe permitir al usuario registrar, modificar y eliminar las provincias de la tabla ZonaDesfavorable.
- **RF-08:** El sistema debe permitir al usuario registrar las Novedades del mes por personal.

### Modificación de novedades

- **RF-09:** El sistema debe permitir al usuario modificar la cantidad de horas trabajadas en días normales del mes para un personal (en la tabla de novedades).
- **RF-10:** El sistema debe permitir al usuario modificar las horas trabajadas en días feriados del mes para un personal (en la tabla de novedades).
- **RF-11:** El sistema debe permitir al usuario modificar las horas extras trabajadas en el mes para un personal (en la tabla de novedades).

### Cálculos

- **RF-12:** El sistema debe guardar, por categoría, el valor hora con retiro y el valor hora sin retiro en la tabla Categorías.
- **RF-13:** El sistema debe calcular el sueldo básico normal de un personal:
  - `Sueldo Básico Normal = Cantidad de horas mensuales × Valor Básico de la hora (según categoría y tipo de retiro)`
- **RF-14:** El sistema debe calcular el total de horas normales trabajadas de un personal:
  - `Total Horas Normales = Horas normales trabajadas × Valor Básico de la hora (según categoría y tipo de retiro)`
- **RF-15:** El sistema debe calcular la antigüedad de un personal como la cantidad de años completos entre la fecha de ingreso y la fecha actual.
- **RF-16:** El sistema debe calcular el ítem antigüedad de un personal:
  - `Antigüedad = (Sueldo Básico Normal / 100) × Años de antigüedad`
- **RF-17:** El sistema debe calcular el ítem zona desfavorable para el personal cuya provincia figure en la tabla ZonaDesfavorable:
  - `Zona Desfavorable = (Sueldo Básico Normal + Antigüedad) × 0,31`
- **RF-18:** El sistema debe calcular el ítem horas extras de un personal:
  - `Horas Extras = Valor hora base × 1,50 × Cantidad de horas extras`
- **RF-19:** El sistema debe calcular el ítem feriados de un personal:
  - `Feriados = Valor hora base × 2 × Cantidad de horas en feriados`
- **RF-20:** El sistema debe calcular el total a pagar de un personal como la sumatoria de:
  - Total horas normales + Total horas extras + Antigüedad + Feriados + Zona desfavorable

## Requerimientos No Funcionales

- **RNF-01:** El sistema debe generar el resumen de pago en menos de 120 s p95 bajo carga nominal.
- **RNF-02:** La API key del modelo no debe estar en el código; se lee de la variable de entorno `ANTHROPIC_API_KEY`.
- **RNF-03:** Las contraseñas deben almacenarse con hash seguro (bcrypt/argon2), nunca en texto plano; la sesión expira tras 24 h de inactividad.

## Criterios de Aceptación

- **AC-01 (RF-01):** Dado un usuario no autenticado,
  Cuando intenta acceder a cualquier pantalla de la aplicación,
  Entonces el sistema deniega el acceso y muestra la pantalla de inicio de sesión.

- **AC-02 (RF-02):** Dado un usuario autenticado en la pantalla principal,
  Cuando selecciona un mes y año,
  Entonces el sistema carga las novedades del período seleccionado y habilita la acción de generar el resumen.

- **AC-03 (RF-03):** Dado un usuario autenticado con un mes seleccionado y las novedades cargadas,
  Cuando ejecuta la generación del resumen de pago,
  Entonces el sistema calcula todos los ítems de cada personal y presenta el resumen mensual completo.

- **AC-04 (RF-04):** Dado que el usuario completa el formulario de alta o edición de un cliente sin ingresar correo electrónico,
  Cuando intenta guardar el registro,
  Entonces el sistema rechaza la operación y muestra un mensaje indicando que el correo electrónico es obligatorio.

- **AC-05 (RF-06):** Dado que el usuario completa el formulario de alta o edición de un personal sin seleccionar provincia,
  Cuando intenta guardar el registro,
  Entonces el sistema rechaza la operación y muestra un mensaje indicando que la provincia es obligatoria.

- **AC-06 (RF-06):** Dado que el usuario asigna una categoría a un personal indicando si es con o sin retiro,
  Cuando guarda el registro,
  Entonces el sistema asocia al personal con esa categoría y tipo de retiro, y queda disponible el valor hora correspondiente para los cálculos.

- **AC-07 (RF-09):** Dado un personal registrado en el sistema,
  Cuando el usuario ingresa la cantidad de horas normales trabajadas en el mes en la tabla de novedades y guarda,
  Entonces el sistema almacena el valor en la tabla de novedades para ese personal y ese mes.

- **AC-08 (RF-10):** Dado un personal registrado en el sistema,
  Cuando el usuario ingresa la cantidad de horas trabajadas en días feriados en la tabla de novedades y guarda,
  Entonces el sistema almacena el valor en la tabla de novedades para ese personal y ese mes.

- **AC-09 (RF-11):** Dado un personal registrado en el sistema,
  Cuando el usuario ingresa la cantidad de horas extras trabajadas en el mes en la tabla de novedades y guarda,
  Entonces el sistema almacena el valor en la tabla de novedades para ese personal y ese mes.

- **AC-10 (RF-13):** Dado un personal con categoría, tipo de retiro y cantidad de horas mensuales asignadas,
  Cuando el sistema calcula el sueldo básico normal,
  Entonces el resultado es igual a horas mensuales × valor hora de la categoría y tipo de retiro correspondiente.

- **AC-11 (RF-14):** Dado un personal con horas normales trabajadas registradas en novedades y su valor hora base,
  Cuando el sistema calcula el total de horas normales trabajadas,
  Entonces el resultado es igual a horas normales trabajadas × valor hora base de la categoría y tipo de retiro.

- **AC-12 (RF-15):** Dado un personal con fecha de ingreso registrada,
  Cuando el sistema calcula la antigüedad,
  Entonces el resultado es la cantidad de años completos transcurridos entre la fecha de ingreso y la fecha actual.

- **AC-13 (RF-16):** Dado un personal con sueldo básico normal y antigüedad en años calculados,
  Cuando el sistema calcula el ítem antigüedad,
  Entonces el resultado es `(Sueldo Básico Normal / 100) × años de antigüedad`.

- **AC-14 (RF-17):** Dado un personal cuya provincia figura en la tabla ZonaDesfavorable, con sueldo básico normal y antigüedad calculados,
  Cuando el sistema calcula el ítem zona desfavorable,
  Entonces el resultado es el 31 % de `(Sueldo Básico Normal + Antigüedad)`.

- **AC-15 (RF-18):** Dado un personal con horas extras registradas en novedades y su valor hora base,
  Cuando el sistema calcula el ítem horas extras,
  Entonces el resultado es `Valor hora base × 1,50 × cantidad de horas extras`.

- **AC-16 (RF-19):** Dado un personal con horas trabajadas en feriados registradas en novedades y su valor hora base,
  Cuando el sistema calcula el ítem feriados,
  Entonces el resultado es `Valor hora base × 2 × cantidad de horas en feriados`.

- **AC-17 (RF-20):** Dado un personal con todos los ítems calculados (horas normales, extras, antigüedad, feriados, zona desfavorable),
  Cuando el sistema calcula el total a pagar,
  Entonces el resultado es la sumatoria de: total horas normales + total horas extras + antigüedad + feriados + zona desfavorable.

## Fuera de Alcance

- El cálculo del ítem de vacaciones está fuera del alcance de la aplicación.
- El cálculo del ítem de licencias especiales está fuera del alcance de la aplicación.

## Riesgos y Dependencias

### Dependencias

- **D-01:** SQL Server — toda la operación de la aplicación depende de su disponibilidad.
- **D-02:** Servicio SMTP externo — el envío del PDF por correo electrónico (Objetivo 3) requiere disponibilidad del servidor de correo.
- **D-03:** API de IA (Anthropic) — la aplicación consume `ANTHROPIC_API_KEY` (RNF-02); una interrupción del servicio externo o el agotamiento del crédito impide las funciones que usan el modelo.

### Riesgos

- **R-01 — Cambio normativo en las fórmulas de liquidación:** Los porcentajes y multiplicadores están fijados por la regulación laboral vigente (31 % zona desfavorable, 1 % por año de antigüedad, ×1,50 horas extras, ×2 feriados). Un cambio de decreto o resolución requiere actualizar la lógica de cálculo en la aplicación antes del siguiente cierre mensual.

- **R-02 — Tablas maestras desactualizadas:** Los valores hora por categoría (RF-12) y la tabla ZonaDesfavorable (RF-07) se mantienen manualmente. Si el usuario no los actualiza ante un cambio de escala salarial o de lista de provincias, todos los cálculos del mes producirán resultados incorrectos sin que la aplicación lo detecte.

- **R-03 — Fallo en la entrega del correo electrónico:** El resumen mensual se distribuye únicamente por email (Objetivo 3). Si el servicio SMTP no está disponible al momento de generar el pago, los clientes no reciben el PDF y el estudio contable no tiene constancia de entrega.
