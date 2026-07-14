import { Component, OnInit, inject, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { Categoria } from './categoria.model';
import { CategoriaService } from './categoria.service';

@Component({
  selector: 'app-categoria-list',
  standalone: true,
  imports: [RouterLink, DecimalPipe, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './categoria-list.component.html'
})
export class CategoriaListComponent implements OnInit {
  private readonly categoriaService = inject(CategoriaService);

  readonly categorias = signal<Categoria[]>([]);
  readonly columnas = ['nombre', 'valorHoraConRetiro', 'valorHoraSinRetiro', 'acciones'];

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.categoriaService.getAll(true).subscribe((categorias) => this.categorias.set(categorias));
  }

  darDeBaja(categoria: Categoria): void {
    if (!confirm(`¿Dar de baja la categoría ${categoria.nombre}?`)) {
      return;
    }
    this.categoriaService.baja(categoria.id).subscribe(() => this.cargar());
  }
}
