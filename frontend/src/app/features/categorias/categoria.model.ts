export interface Categoria {
  id: number;
  nombre: string;
  valorHoraConRetiro: number;
  valorHoraSinRetiro: number;
  activo: boolean;
}

export interface CategoriaRequest {
  nombre: string;
  valorHoraConRetiro: number;
  valorHoraSinRetiro: number;
}
