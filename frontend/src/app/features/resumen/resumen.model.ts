export interface ResumenPersonalDetalle {
  personalId: number;
  clienteId: number;
  clienteNombre: string;
  personalNombreCompleto: string;
  dni: string;
  categoriaNombre: string;
  valorHora: number;
  sueldoBasicoNormal: number;
  totalHorasNormales: number;
  itemHorasExtras: number;
  aniosAntiguedad: number;
  itemAntiguedad: number;
  itemFeriados: number;
  itemZonaDesfavorable: number;
  totalAPagar: number;
  tienePdf: boolean;
}

export interface ResumenMensual {
  anio: number;
  mes: number;
  estado: 'Generado' | 'Enviado';
  fechaGeneracion: string;
  fechaEnvio: string | null;
  detalles: ResumenPersonalDetalle[];
}

export interface EnviarResumenResultado {
  clientesEnviados: string[];
  clientesSinEmail: string[];
  clientesConError: string[];
}
