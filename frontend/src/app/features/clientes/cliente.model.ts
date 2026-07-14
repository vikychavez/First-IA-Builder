export interface Cliente {
  id: number;
  nombre: string;
  email: string | null;
  telefono: string | null;
  direccion: string | null;
  activo: boolean;
}

export interface ClienteRequest {
  nombre: string;
  email: string;
  telefono: string | null;
  direccion: string | null;
}
