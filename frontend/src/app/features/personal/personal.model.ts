export type TipoRetiro = 'ConRetiro' | 'SinRetiro';

export interface Personal {
  id: number;
  dni: string;
  clienteId: number;
  clienteNombre: string;
  fechaIngreso: string;
  apellido: string;
  nombre: string;
  direccion: string;
  telefono: string;
  categoriaId: number;
  categoriaNombre: string;
  tipoRetiro: TipoRetiro;
  provincia: string;
  horasMensualesPactadas: number;
  valorHoraBase: number;
  activo: boolean;
}

export interface PersonalRequest {
  dni: string;
  clienteId: number;
  fechaIngreso: string;
  apellido: string;
  nombre: string;
  direccion: string;
  telefono: string;
  categoriaId: number;
  tipoRetiro: TipoRetiro;
  provincia: string;
  horasMensualesPactadas: number;
}
