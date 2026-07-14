import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { Cliente } from '../clientes/cliente.model';
import { ClienteService } from '../clientes/cliente.service';
import { Personal } from './personal.model';
import { PersonalService } from './personal.service';

@Component({
  selector: 'app-personal-list',
  standalone: true,
  imports: [RouterLink, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './personal-list.component.html'
})
export class PersonalListComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly clienteService = inject(ClienteService);
  private readonly personalService = inject(PersonalService);

  readonly clienteId = Number(this.route.snapshot.paramMap.get('clienteId'));
  readonly cliente = signal<Cliente | null>(null);
  readonly personal = signal<Personal[]>([]);
  readonly columnas = ['dni', 'apellidoNombre', 'categoria', 'provincia', 'acciones'];

  ngOnInit(): void {
    this.clienteService.getById(this.clienteId).subscribe((cliente) => this.cliente.set(cliente));
    this.cargar();
  }

  cargar(): void {
    this.personalService.getByCliente(this.clienteId, true).subscribe((personal) => this.personal.set(personal));
  }

  darDeBaja(persona: Personal): void {
    if (!confirm(`¿Dar de baja a ${persona.apellido}, ${persona.nombre}?`)) {
      return;
    }
    this.personalService.baja(persona.id).subscribe(() => this.cargar());
  }
}
