import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';

import { EnviarResumenResultado, ResumenMensual, ResumenPersonalDetalle } from './resumen.model';
import { ResumenService } from './resumen.service';

@Component({
  selector: 'app-resumen',
  standalone: true,
  imports: [FormsModule, DatePipe, DecimalPipe, MatFormFieldModule, MatSelectModule, MatButtonModule, MatTableModule],
  templateUrl: './resumen.component.html'
})
export class ResumenComponent implements OnInit {
  private readonly resumenService = inject(ResumenService);
  private readonly route = inject(ActivatedRoute);

  readonly anios: number[] = Array.from({ length: 6 }, (_, i) => new Date().getFullYear() - 2 + i);
  readonly meses = [
    { valor: 1, nombre: 'Enero' },
    { valor: 2, nombre: 'Febrero' },
    { valor: 3, nombre: 'Marzo' },
    { valor: 4, nombre: 'Abril' },
    { valor: 5, nombre: 'Mayo' },
    { valor: 6, nombre: 'Junio' },
    { valor: 7, nombre: 'Julio' },
    { valor: 8, nombre: 'Agosto' },
    { valor: 9, nombre: 'Septiembre' },
    { valor: 10, nombre: 'Octubre' },
    { valor: 11, nombre: 'Noviembre' },
    { valor: 12, nombre: 'Diciembre' }
  ];

  anio: number;
  mes: number;

  readonly resumen = signal<ResumenMensual | null>(null);
  readonly cargando = signal(false);
  readonly enviando = signal(false);
  readonly error = signal<string | null>(null);
  readonly resultadoEnvio = signal<EnviarResumenResultado | null>(null);
  readonly columnas = [
    'cliente',
    'personal',
    'categoria',
    'valorHora',
    'totalHorasNormales',
    'itemHorasExtras',
    'itemAntiguedad',
    'itemFeriados',
    'itemZonaDesfavorable',
    'totalAPagar',
    'pdf'
  ];

  readonly detallesOrdenados = computed<ResumenPersonalDetalle[]>(() =>
    [...(this.resumen()?.detalles ?? [])].sort(
      (a, b) => a.clienteNombre.localeCompare(b.clienteNombre) || a.personalNombreCompleto.localeCompare(b.personalNombreCompleto)
    )
  );

  readonly totalGeneral = computed(() =>
    this.detallesOrdenados().reduce((acc, d) => acc + d.totalAPagar, 0)
  );

  constructor() {
    const queryParams = this.route.snapshot.queryParamMap;
    this.anio = Number(queryParams.get('anio')) || new Date().getFullYear();
    this.mes = Number(queryParams.get('mes')) || new Date().getMonth() + 1;
  }

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.error.set(null);
    this.resultadoEnvio.set(null);
    this.resumenService.get(this.anio, this.mes).subscribe((resumen) => this.resumen.set(resumen));
  }

  pdfUrl(personalId: number): string {
    return `/api/resumenes/${this.anio}/${this.mes}/personal/${personalId}/pdf`;
  }

  generar(): void {
    this.cargando.set(true);
    this.error.set(null);
    this.resumenService.generar(this.anio, this.mes).subscribe({
      next: (resumen) => {
        this.resumen.set(resumen);
        this.cargando.set(false);
      },
      error: () => {
        this.error.set('No se pudo generar el resumen.');
        this.cargando.set(false);
      }
    });
  }

  enviar(): void {
    this.enviando.set(true);
    this.error.set(null);
    this.resultadoEnvio.set(null);
    this.resumenService.enviar(this.anio, this.mes).subscribe({
      next: (resultado) => {
        this.resultadoEnvio.set(resultado);
        this.enviando.set(false);
        this.cargar();
      },
      error: (err) => {
        this.error.set(err.error?.mensaje ?? 'No se pudo enviar el resumen.');
        this.enviando.set(false);
      }
    });
  }
}
