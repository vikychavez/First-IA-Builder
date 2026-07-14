import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { ClienteService } from './cliente.service';
import { Sexo } from './cliente.model';

@Component({
  selector: 'app-cliente-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule
  ],
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
  readonly cuitPreview = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    dni: ['', [Validators.required, Validators.pattern(/^\d{1,8}$/)]],
    sexo: ['' as Sexo | '', Validators.required],
    apellido: ['', Validators.required],
    nombre: ['', Validators.required],
    fechaNacimiento: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    telefono: [''],
    direccion: ['']
  });

  constructor() {
    this.form.get('dni')!.valueChanges.subscribe(() => this.actualizarCuitPreview());
    this.form.get('sexo')!.valueChanges.subscribe(() => this.actualizarCuitPreview());

    if (this.editando) {
      this.clienteService.getById(Number(this.id)).subscribe((cliente) => {
        this.form.patchValue({
          dni: cliente.dni,
          sexo: cliente.sexo,
          apellido: cliente.apellido,
          nombre: cliente.nombre,
          fechaNacimiento: cliente.fechaNacimiento,
          email: cliente.email ?? '',
          telefono: cliente.telefono ?? '',
          direccion: cliente.direccion ?? ''
        });
        this.cuitPreview.set(cliente.cuit);
      });
    }
  }

  private actualizarCuitPreview(): void {
    const dni = this.form.get('dni')!.value;
    const sexo = this.form.get('sexo')!.value as Sexo | '';

    if (!sexo || !/^\d{1,8}$/.test(dni)) {
      this.cuitPreview.set(null);
      return;
    }

    this.clienteService.calcularCuit(dni, sexo).subscribe({
      next: ({ cuit }) => this.cuitPreview.set(cuit),
      error: () => this.cuitPreview.set(null)
    });
  }

  guardar(): void {
    if (this.form.invalid) {
      return;
    }

    this.error.set(null);
    const request = { ...this.form.getRawValue(), sexo: this.form.getRawValue().sexo as Sexo };
    const accion = this.editando
      ? this.clienteService.update(Number(this.id), request)
      : this.clienteService.create(request);

    accion.subscribe({
      next: () => this.router.navigateByUrl('/clientes'),
      error: (err) =>
        this.error.set(err.error?.mensaje ?? 'No se pudo guardar el cliente. Verificá los datos ingresados.')
    });
  }
}
