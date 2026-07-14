import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { ZonaDesfavorable } from './zona.model';
import { ZonaService } from './zona.service';

@Component({
  selector: 'app-zona-list',
  standalone: true,
  imports: [RouterLink, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './zona-list.component.html'
})
export class ZonaListComponent implements OnInit {
  private readonly zonaService = inject(ZonaService);

  readonly zonas = signal<ZonaDesfavorable[]>([]);
  readonly columnas = ['provincia', 'acciones'];

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.zonaService.getAll(true).subscribe((zonas) => this.zonas.set(zonas));
  }

  darDeBaja(zona: ZonaDesfavorable): void {
    if (!confirm(`¿Dar de baja la zona desfavorable de ${zona.provincia}?`)) {
      return;
    }
    this.zonaService.baja(zona.id).subscribe(() => this.cargar());
  }
}
