export interface Usuario {
  id: number;
  nombreUsuario: string;
}

export interface LoginRequest {
  nombreUsuario: string;
  password: string;
}
