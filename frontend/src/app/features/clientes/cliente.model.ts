export type Sexo = 'M' | 'F';

export interface Cliente {
  id: number;
  dni: string;
  cuit: string;
  sexo: Sexo;
  apellido: string;
  nombre: string;
  fechaNacimiento: string;
  email: string | null;
  telefono: string | null;
  direccion: string | null;
  activo: boolean;
}

export interface ClienteRequest {
  dni: string;
  sexo: Sexo;
  apellido: string;
  nombre: string;
  fechaNacimiento: string;
  email: string;
  telefono: string | null;
  direccion: string | null;
}
