export interface Novedad {
  personalId: number;
  dni: string;
  clienteNombre: string;
  apellidoNombre: string;
  horasNormales: number;
  horasFeriado: number;
  horasExtra: number;
}

export interface NovedadUpsertRequest {
  personalId: number;
  anio: number;
  mes: number;
  horasNormales: number;
  horasFeriado: number;
  horasExtra: number;
}
