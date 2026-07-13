# PRD-001: LiqPagoEstandar — Generar Resumen de Pago Mensual Estándar para Personal de Casa de Familia

## Contexto y Problema

Un estudio contable requiere una aplicación que le genere un resumen de pago mensual estándar para el personal de casa de familias de sus clientes.

Personas:

- El contador envía mensualmente por correo electrónico el resumen de pago a sus clientes.
- El cliente puede tener 1 o más personal de casa de familia.
- Todos los usuarios que se autentican en el sistema son personal interno del estudio contable, con el mismo nivel de acceso sobre todos los clientes (no hay asignación de clientes por usuario).

## Objetivos

- Generar el resumen de pago de cada personal de casa de familia.
- La aplicación hace el cálculo de los ítems a pagar y obtiene el total a pagar de cada personal.
- Cada cliente recibe por correo electrónico un archivo PDF detallado de los ítems y el total general a pagar a cada personal.

## Requerimientos Funcionales

### Acceso y generación

- **RF-01:** El sistema debe autenticar al usuario mediante usuario y contraseña antes de permitir el acceso a cualquier funcionalidad.
- **RF-02:** El sistema debe permitir al usuario autenticado seleccionar el mes y año a liquidar.
- **RF-03:** El sistema debe generar el resumen de pago para todos los personales activos correspondientes al mes seleccionado.

### Mantenimiento de Clientes

- **RF-04:** El sistema debe permitir al usuario registrar un Cliente del estudio contable.
- **RF-05:** El sistema debe permitir al usuario modificar los datos de un Cliente.
- **RF-06:** El sistema debe permitir al usuario dar de baja (lógica) a un Cliente, marcándolo inactivo sin eliminar sus datos.

### Mantenimiento de Categorías

- **RF-07:** El sistema debe permitir al usuario registrar una Categoría con su nombre, valor hora con retiro y valor hora sin retiro.
- **RF-08:** El sistema debe permitir al usuario modificar los datos de una Categoría.
- **RF-09:** El sistema debe permitir al usuario dar de baja (lógica) a una Categoría, marcándola inactiva sin eliminar sus datos.

### Mantenimiento de Personal

- **RF-10:** El sistema debe permitir al usuario registrar el Personal de un cliente (DNI, cliente al que pertenece, fecha de ingreso, apellido, nombre, dirección, teléfono, categoría, con/sin retiro, provincia).
- **RF-11:** El sistema debe permitir al usuario modificar los datos de un Personal.
- **RF-12:** El sistema debe permitir al usuario dar de baja (lógica) a un Personal, marcándolo inactivo sin eliminar sus datos.

### Mantenimiento de ZonaDesfavorable

- **RF-13:** El sistema debe permitir al usuario registrar una provincia en la tabla ZonaDesfavorable.
- **RF-14:** El sistema debe permitir al usuario modificar una provincia registrada en la tabla ZonaDesfavorable.
- **RF-15:** El sistema debe permitir al usuario dar de baja (lógica) a una provincia de la tabla ZonaDesfavorable.

### Novedades

- **RF-16:** El sistema debe permitir al usuario registrar las Novedades del mes (horas normales, horas en feriados, horas extras) por personal.
- **RF-17:** El sistema debe permitir al usuario modificar la cantidad de horas trabajadas en días normales del mes para un personal (en la tabla de novedades).
- **RF-18:** El sistema debe permitir al usuario modificar las horas trabajadas en días feriados del mes para un personal (en la tabla de novedades).
- **RF-19:** El sistema debe permitir al usuario modificar las horas extras trabajadas en el mes para un personal (en la tabla de novedades).

### Cálculos

- **RF-20:** El sistema debe calcular el sueldo básico normal de un personal:
  - `Sueldo Básico Normal = Cantidad de horas mensuales × Valor Básico de la hora (según categoría y tipo de retiro)`
- **RF-21:** El sistema debe calcular el total de horas normales trabajadas de un personal:
  - `Total Horas Normales = Horas normales trabajadas × Valor Básico de la hora (según categoría y tipo de retiro)`
- **RF-22:** El sistema debe calcular la antigüedad de un personal como la cantidad de años completos entre la fecha de ingreso y la fecha actual.
- **RF-23:** El sistema debe calcular el ítem antigüedad de un personal:
  - `Antigüedad = (Sueldo Básico Normal / 100) × Años de antigüedad`
- **RF-24:** El sistema debe calcular el ítem zona desfavorable para el personal cuya provincia figure activa en la tabla ZonaDesfavorable:
  - `Zona Desfavorable = (Sueldo Básico Normal + Antigüedad) × 0,31`
- **RF-25:** El sistema debe calcular el ítem horas extras de un personal:
  - `Horas Extras = Valor hora base × 1,50 × Cantidad de horas extras`
- **RF-26:** El sistema debe calcular el ítem feriados de un personal:
  - `Feriados = Valor hora base × 2 × Cantidad de horas en feriados`
- **RF-27:** El sistema debe calcular el total a pagar de un personal como la sumatoria de:
  - Total horas normales + Total horas extras + Antigüedad + Feriados + Zona desfavorable
- **RF-28:** El sistema debe permitir al usuario modificar el porcentaje de zona desfavorable, el porcentaje de antigüedad, el multiplicador de horas extras y el multiplicador de feriados utilizados en RF-23 a RF-26, sin requerir un cambio de código.

### Distribución

- **RF-29:** El sistema debe generar un archivo PDF por personal con el detalle de cada ítem calculado (horas normales, horas extras, antigüedad, feriados, zona desfavorable) y el total a pagar.
- **RF-30:** El sistema debe permitir al usuario ejecutar una acción de envío que distribuya por correo electrónico, a cada Cliente con correo electrónico registrado, los PDF generados de su Personal a cargo para el período liquidado.
- **RF-31:** El sistema debe conservar disponible el resumen de pago generado (RF-03) hasta que el usuario ejecute su envío (RF-30) o hasta que se genere un nuevo resumen para el mismo período.

## Requerimientos No Funcionales

- **RNF-01:** El sistema debe calcular y presentar el resumen de pago (RF-03) en menos de 120 s p95 bajo carga nominal, para el conjunto de personal de todos los clientes del mes seleccionado.
- **RNF-02:** Las contraseñas deben almacenarse con hash seguro (bcrypt/argon2), nunca en texto plano; la sesión expira tras 24 h de inactividad.

## Criterios de Aceptación

- **AC-01 (RF-01):** Dado un usuario no autenticado,
  Cuando intenta acceder a cualquier pantalla de la aplicación,
  Entonces el sistema deniega el acceso y muestra la pantalla de inicio de sesión.

- **AC-02 (RF-02):** Dado un usuario autenticado en la pantalla principal,
  Cuando selecciona un mes y año,
  Entonces el sistema carga las novedades del período seleccionado y habilita la acción de generar el resumen.

- **AC-03 (RF-03):** Dado un usuario autenticado con un mes seleccionado y las novedades cargadas,
  Cuando ejecuta la generación del resumen de pago,
  Entonces el sistema calcula todos los ítems de cada personal activo y presenta el resumen mensual completo.

- **AC-04 (RF-03):** Dado un Personal dado de baja (inactivo),
  Cuando el usuario genera el resumen de pago del mes,
  Entonces el sistema no lo incluye en el resumen generado.

- **AC-05 (RF-04):** Dado un usuario que completa el formulario de alta de un Cliente con nombre y correo electrónico,
  Cuando guarda el registro,
  Entonces el sistema crea el Cliente y lo muestra en el listado de clientes activos.

- **AC-06 (RF-04, RF-05):** Dado que el usuario completa el formulario de alta o edición de un cliente sin ingresar correo electrónico,
  Cuando intenta guardar el registro,
  Entonces el sistema rechaza la operación y muestra un mensaje indicando que el correo electrónico es obligatorio.

- **AC-07 (RF-05):** Dado un Cliente existente,
  Cuando el usuario modifica sus datos y guarda,
  Entonces el sistema actualiza el registro y refleja los cambios en el listado.

- **AC-08 (RF-06):** Dado un Cliente registrado,
  Cuando el usuario ejecuta la baja,
  Entonces el sistema lo marca como inactivo, deja de listarlo en las pantallas de selección de clientes activos, y conserva sus datos históricos.

- **AC-09 (RF-07):** Dado un usuario que completa el formulario de alta de una Categoría con nombre, valor hora con retiro y valor hora sin retiro,
  Cuando guarda el registro,
  Entonces el sistema crea la Categoría y la muestra en el listado.

- **AC-10 (RF-07, RF-08):** Dado que el usuario completa el formulario de alta o edición de una Categoría sin ingresar el valor hora con retiro o el valor hora sin retiro,
  Cuando intenta guardar el registro,
  Entonces el sistema rechaza la operación y muestra un mensaje indicando que ambos valores son obligatorios.

- **AC-11 (RF-08):** Dado una Categoría existente,
  Cuando el usuario modifica su valor hora y guarda,
  Entonces el sistema actualiza el registro.

- **AC-12 (RF-09):** Dado una Categoría registrada,
  Cuando el usuario ejecuta la baja,
  Entonces el sistema la marca como inactiva y deja de ofrecerla como opción al registrar o modificar Personal.

- **AC-13 (RF-10):** Dado un usuario que completa el formulario de alta de Personal con todos los datos obligatorios (DNI, cliente, fecha de ingreso, apellido, nombre, dirección, teléfono, categoría, con/sin retiro, provincia),
  Cuando guarda el registro,
  Entonces el sistema crea el registro y lo muestra en el listado de personal del cliente correspondiente.

- **AC-14 (RF-10, RF-11):** Dado que el usuario completa el formulario de alta o edición de un personal sin seleccionar provincia,
  Cuando intenta guardar el registro,
  Entonces el sistema rechaza la operación y muestra un mensaje indicando que la provincia es obligatoria.

- **AC-15 (RF-10, RF-11):** Dado que el usuario completa el formulario de alta o edición de un personal sin ingresar DNI, o con un DNI que ya existe en otro personal activo,
  Cuando intenta guardar el registro,
  Entonces el sistema rechaza la operación y muestra un mensaje indicando que el DNI es obligatorio y debe ser único entre los personales activos.

- **AC-16 (RF-10, RF-12):** Dado un Personal dado de baja con un DNI determinado,
  Cuando el usuario registra un nuevo Personal con ese mismo DNI,
  Entonces el sistema permite el alta y conserva el registro anterior — incluida su fecha de ingreso original — como historial independiente y distinguible del nuevo registro.

- **AC-17 (RF-10, RF-11):** Dado que el usuario asigna una categoría a un personal indicando si es con o sin retiro,
  Cuando guarda el registro,
  Entonces el sistema asocia al personal con esa categoría y tipo de retiro, y queda disponible el valor hora correspondiente para los cálculos.

- **AC-18 (RF-11):** Dado un Personal existente,
  Cuando el usuario modifica sus datos y guarda,
  Entonces el sistema actualiza el registro.

- **AC-19 (RF-12):** Dado un Personal registrado,
  Cuando el usuario ejecuta la baja,
  Entonces el sistema lo marca como inactivo y deja de incluirlo en la generación de futuros resúmenes de pago (ver AC-04).

- **AC-20 (RF-13):** Dado un usuario que ingresa el nombre de una provincia no registrada previamente en ZonaDesfavorable,
  Cuando guarda el registro,
  Entonces el sistema la agrega a la tabla ZonaDesfavorable.

- **AC-21 (RF-13):** Dado que el usuario intenta registrar una provincia sin nombre, o una provincia ya existente y activa en la tabla ZonaDesfavorable,
  Cuando intenta guardar,
  Entonces el sistema rechaza la operación y muestra un mensaje indicando el motivo.

- **AC-22 (RF-14):** Dado una provincia registrada en ZonaDesfavorable,
  Cuando el usuario modifica su nombre y guarda,
  Entonces el sistema actualiza el registro.

- **AC-23 (RF-15):** Dado una provincia registrada en ZonaDesfavorable,
  Cuando el usuario ejecuta la baja,
  Entonces el sistema la marca como inactiva y deja de aplicar el ítem zona desfavorable a los personales de esa provincia en los cálculos posteriores.

- **AC-24 (RF-16):** Dado un Personal activo,
  Cuando el usuario registra sus novedades del mes (horas normales, horas en feriados, horas extras),
  Entonces el sistema las almacena asociadas a ese personal y ese mes.

- **AC-25 (RF-17):** Dado un personal registrado en el sistema,
  Cuando el usuario ingresa la cantidad de horas normales trabajadas en el mes en la tabla de novedades y guarda,
  Entonces el sistema almacena el valor en la tabla de novedades para ese personal y ese mes.

- **AC-26 (RF-18):** Dado un personal registrado en el sistema,
  Cuando el usuario ingresa la cantidad de horas trabajadas en días feriados en la tabla de novedades y guarda,
  Entonces el sistema almacena el valor en la tabla de novedades para ese personal y ese mes.

- **AC-27 (RF-19):** Dado un personal registrado en el sistema,
  Cuando el usuario ingresa la cantidad de horas extras trabajadas en el mes en la tabla de novedades y guarda,
  Entonces el sistema almacena el valor en la tabla de novedades para ese personal y ese mes.

- **AC-28 (RF-20):** Dado un personal con categoría, tipo de retiro y cantidad de horas mensuales asignadas,
  Cuando el sistema calcula el sueldo básico normal,
  Entonces el resultado es igual a horas mensuales × valor hora de la categoría y tipo de retiro correspondiente.
  Ejemplo: Si el personal tiene 36 horas mensuales, su valor hora de categoría y tipo de retiro es 10.000. El resultado del sueldo básico normal es 36x10.000=360.000

- **AC-29 (RF-21):** Dado un personal con horas normales trabajadas registradas en novedades y su valor hora base,
  Cuando el sistema calcula el total de horas normales trabajadas,
  Entonces el resultado es igual a horas normales trabajadas × valor hora base de la categoría y tipo de retiro.
  Ejemplo: Si el personal tiene 36 horas normales trabajadas y su valor hora de categoría y tipo de retiro es 10.000. El resultado de horas normales trabajadas es 36x10.000=360.000

- **AC-30 (RF-22):** Dado un personal con fecha de ingreso registrada,
  Cuando el sistema calcula la antigüedad,
  Entonces el resultado es la cantidad de años completos transcurridos entre la fecha de ingreso y la fecha actual.
  Ejemplo: Si el personal tiene fecha de ingreso 22/05/2022, y si la fecha actual es 13/07/2026, la antiguedad es de 4 años.
  Ejemplo: Si el personal tiene fecha de ingreso 20/12/2025, y si la fecha actual es 13/07/2026, la antiguedad es de 0 años.

- **AC-31 (RF-23):** Dado un personal con sueldo básico normal y antigüedad en años calculados,
  Cuando el sistema calcula el ítem antigüedad,
  Entonces el resultado es `(Sueldo Básico Normal / 100) × años de antigüedad`.
  Ejemplo: Si el personal tiene sueldo básico normal=360.000 y su antiguedad en años es de 4.
  Su antiguedad es de (360.000/100) x 4 años=14.400
  Ejemplo: Si el personal tiene sueldo básico normal=360.000 y su antiguedad en años es de 0.
  Su antiguedad es de (360.000/100) x 0 años=0

- **AC-32 (RF-24):** Dado un personal cuya provincia figura activa en la tabla ZonaDesfavorable, con sueldo básico normal y antigüedad calculados,
  Cuando el sistema calcula el ítem zona desfavorable,
  Entonces el resultado es el 31 % de `(Sueldo Básico Normal + Antigüedad)`.
  Ejemplo: el personal tiene asignada una provincia que existe activa en la tabla ZonaDesfavorable y su antigüedad es de 4 años, su sueldo básico normal es de 360.000, el resultado del ítem de zona desfavorable es (360.000+14400)*0,31=116.064

- **AC-33 (RF-25):** Dado un personal con horas extras registradas en novedades y su valor hora base,
  Cuando el sistema calcula el ítem horas extras,
  Entonces el resultado es `Valor hora base × 1,50 × cantidad de horas extras`.
  Ejemplo: Si el personal tiene un valor hora base de 10.000 y tiene un total de 8 horas extras, el resultado del item Horas Extras es 10.000 x 1,50 x 8=120.000

- **AC-34 (RF-26):** Dado un personal con horas trabajadas en feriados registradas en novedades y su valor hora base,
  Cuando el sistema calcula el ítem feriados,
  Entonces el resultado es `Valor hora base × 2 × cantidad de horas en feriados`.
  Ejemplo: Si el personal tiene un valor hora base de 10.000 y tiene un total de 3 horas trabajadas en días feriados, el resultado del item Feriado es 10.000 x 2 x 3=60.000

- **AC-35 (RF-27):** Dado un personal con todos los ítems calculados (horas normales, extras, antigüedad, feriados, zona desfavorable),
  Cuando el sistema calcula el total a pagar,
  Entonces el resultado es la sumatoria de: total horas normales + total horas extras + antigüedad + feriados + zona desfavorable.
  Ejemplo: Si el personal tiene total horas normales=360.000 y el item Feriados es 60.000, el item Horas Extras es 120.000, el item antigüedad es 14.400, el item zona desfavorable es 116.064. El resultado de total a pagar es 360.000 + 60.000 + 120.000 + 14.400 + 116.064=670.464

- **AC-36 (RF-28):** Dado un usuario autenticado en la pantalla de parámetros regulatorios,
  Cuando modifica el porcentaje de zona desfavorable, el porcentaje de antigüedad, el multiplicador de horas extras o el multiplicador de feriados, y guarda,
  Entonces el sistema utiliza los nuevos valores en los cálculos de los resúmenes generados a partir de ese momento, sin requerir un despliegue de código.

- **AC-37 (RF-29):** Dado un personal con todos los ítems calculados (horas normales, extras, antigüedad, feriados, zona desfavorable) y el total a pagar,
  Cuando el sistema genera su PDF,
  Entonces el archivo contiene cada ítem individual con su valor y el total a pagar, coincidiendo con los valores calculados.

- **AC-38 (RF-29):** Dado un personal con su PDF generado,
  Cuando el sistema nombra el archivo,
  Entonces el nombre incluye el nombre del personal, el mes y el año liquidados.
  Ejemplo: El cliente José Fernandez tiene a su cargo 2 personas; al generar el resumen se crean 2 archivos PDF, cada uno con el nombre del personal + mes + año.

- **AC-39 (RF-30):** Dado un Cliente con correo electrónico registrado y al menos un Personal activo con su resumen generado,
  Cuando el usuario ejecuta la acción de enviar los resúmenes del período,
  Entonces el sistema envía un correo electrónico a ese Cliente adjuntando el PDF de cada uno de sus Personal a cargo.

- **AC-40 (RF-30):** Dado un Cliente sin correo electrónico registrado,
  Cuando el usuario ejecuta la acción de enviar los resúmenes del período,
  Entonces el sistema excluye a ese Cliente del envío y muestra un mensaje indicando qué clientes no recibieron el correo por falta de email.

- **AC-41 (RF-31):** Dado un resumen de pago generado para un período,
  Cuando el usuario no ha ejecutado todavía la acción de envío (RF-30),
  Entonces el sistema mantiene disponible ese resumen para poder enviarlo, sin necesidad de volver a generarlo.

## Fuera de Alcance

- El cálculo del ítem de vacaciones está fuera del alcance de la aplicación.
- El cálculo del ítem de licencias especiales está fuera del alcance de la aplicación.
- El control de acceso diferenciado por cliente está fuera de alcance: todos los usuarios internos autenticados del estudio contable pueden ver y operar sobre todos los clientes.
- El reintento automático o la reprogramación del envío de correo ante un fallo del servicio SMTP están fuera de alcance; el envío es una acción manual (RF-30) que el usuario puede reintentar manualmente.
- El historial o auditoría de correos enviados está fuera de alcance de esta versión.
- La edición o el reenvío de un resumen de un período ya generado anteriormente están fuera de alcance.

## Riesgos y Dependencias

### Dependencias

- **D-01:** SQL Server — toda la operación de la aplicación depende de su disponibilidad.
- **D-02:** Servicio SMTP externo — el envío del PDF por correo electrónico (RF-30) requiere disponibilidad del servidor de correo.

### Riesgos

- **R-01 — Cambio normativo en las fórmulas de liquidación:** Los porcentajes y multiplicadores (31 % zona desfavorable, 1 % por año de antigüedad, ×1,50 horas extras, ×2 feriados) están fijados por la regulación laboral vigente. RF-28 permite actualizarlos sin cambio de código; el riesgo remanente es que el usuario responsable no actualice el parámetro a tiempo ante un cambio de decreto o resolución, antes del siguiente cierre mensual.

- **R-02 — Tablas maestras desactualizadas:** Los valores hora por categoría (RF-07), la tabla ZonaDesfavorable (RF-13) y los parámetros regulatorios (RF-28) se mantienen manualmente. Si el usuario no los actualiza ante un cambio de escala salarial, de lista de provincias o de normativa, todos los cálculos del mes producirán resultados incorrectos sin que la aplicación lo detecte.

- **R-03 — Fallo en la entrega del correo electrónico:** El resumen mensual se distribuye por email mediante una acción manual (RF-30). Si el servicio SMTP no está disponible al momento del envío, los clientes no reciben el PDF; el usuario ve el resultado del envío en el momento (AC-39/AC-40) pero no hay reintento automático (ver Fuera de Alcance).
