import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { Novedad } from './novedad.model';
import { NovedadService } from './novedad.service';

@Component({
  selector: 'app-novedades',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink,
    MatTableModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './novedades.component.html'
})
export class NovedadesComponent implements OnInit {
  private readonly novedadService = inject(NovedadService);

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

  anio = new Date().getFullYear();
  mes = new Date().getMonth() + 1;

  novedades: Novedad[] = [];
  readonly columnas = ['clienteNombre', 'apellidoNombre', 'horasNormales', 'horasFeriado', 'horasExtra', 'acciones'];
  readonly cargado = signal(false);
  readonly guardandoId = signal<number | null>(null);

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.novedadService.getByPeriodo(this.anio, this.mes).subscribe((novedades) => {
      this.novedades = novedades;
      this.cargado.set(true);
    });
  }

  guardar(novedad: Novedad): void {
    this.guardandoId.set(novedad.personalId);
    this.novedadService
      .upsert({
        personalId: novedad.personalId,
        anio: this.anio,
        mes: this.mes,
        horasNormales: novedad.horasNormales,
        horasFeriado: novedad.horasFeriado,
        horasExtra: novedad.horasExtra
      })
      .subscribe({
        next: () => this.guardandoId.set(null),
        error: () => this.guardandoId.set(null)
      });
  }
}
