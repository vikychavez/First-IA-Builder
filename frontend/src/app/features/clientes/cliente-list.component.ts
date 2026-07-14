import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { Cliente } from './cliente.model';
import { ClienteService } from './cliente.service';

@Component({
  selector: 'app-cliente-list',
  standalone: true,
  imports: [RouterLink, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './cliente-list.component.html'
})
export class ClienteListComponent implements OnInit {
  private readonly clienteService = inject(ClienteService);

  readonly clientes = signal<Cliente[]>([]);
  readonly columnas = ['nombre', 'email', 'telefono', 'acciones'];

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.clienteService.getAll(true).subscribe((clientes) => this.clientes.set(clientes));
  }

  darDeBaja(cliente: Cliente): void {
    if (!confirm(`¿Dar de baja a ${cliente.nombre}?`)) {
      return;
    }
    this.clienteService.baja(cliente.id).subscribe(() => this.cargar());
  }
}
