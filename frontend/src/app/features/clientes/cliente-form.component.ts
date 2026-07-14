import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { ClienteService } from './cliente.service';

@Component({
  selector: 'app-cliente-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './cliente-form.component.html'
})
export class ClienteFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly clienteService = inject(ClienteService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly id = this.route.snapshot.paramMap.get('id');
  readonly editando = this.id !== null;
  readonly error = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    nombre: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    telefono: [''],
    direccion: ['']
  });

  constructor() {
    if (this.editando) {
      this.clienteService.getById(Number(this.id)).subscribe((cliente) => {
        this.form.patchValue({
          nombre: cliente.nombre,
          email: cliente.email ?? '',
          telefono: cliente.telefono ?? '',
          direccion: cliente.direccion ?? ''
        });
      });
    }
  }

  guardar(): void {
    if (this.form.invalid) {
      return;
    }

    this.error.set(null);
    const request = this.form.getRawValue();
    const accion = this.editando
      ? this.clienteService.update(Number(this.id), request)
      : this.clienteService.create(request);

    accion.subscribe({
      next: () => this.router.navigateByUrl('/clientes'),
      error: () => this.error.set('No se pudo guardar el cliente. Verificá los datos ingresados.')
    });
  }
}
