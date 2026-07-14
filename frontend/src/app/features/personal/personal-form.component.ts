import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';

import { PROVINCIAS } from '../../core/provincias';
import { Categoria } from '../categorias/categoria.model';
import { CategoriaService } from '../categorias/categoria.service';
import { PersonalService } from './personal.service';

@Component({
  selector: 'app-personal-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatButtonModule
  ],
  templateUrl: './personal-form.component.html'
})
export class PersonalFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly categoriaService = inject(CategoriaService);
  private readonly personalService = inject(PersonalService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly provincias = PROVINCIAS;
  readonly categorias = signal<Categoria[]>([]);
  readonly error = signal<string | null>(null);

  readonly clienteId = Number(this.route.snapshot.paramMap.get('clienteId'));
  private readonly id = this.route.snapshot.paramMap.get('id');
  readonly editando = this.id !== null;

  readonly form = this.fb.nonNullable.group({
    dni: ['', Validators.required],
    fechaIngreso: ['', Validators.required],
    apellido: ['', Validators.required],
    nombre: ['', Validators.required],
    direccion: ['', Validators.required],
    telefono: ['', Validators.required],
    categoriaId: [0, [Validators.required, Validators.min(1)]],
    tipoRetiro: ['SinRetiro' as 'ConRetiro' | 'SinRetiro', Validators.required],
    provincia: ['', Validators.required],
    horasMensualesPactadas: [0, [Validators.required, Validators.min(0.01)]]
  });

  constructor() {
    this.categoriaService.getAll(true).subscribe((categorias) => this.categorias.set(categorias));

    if (this.editando) {
      this.personalService.getById(Number(this.id)).subscribe((personal) => {
        this.form.patchValue({
          dni: personal.dni,
          fechaIngreso: personal.fechaIngreso,
          apellido: personal.apellido,
          nombre: personal.nombre,
          direccion: personal.direccion,
          telefono: personal.telefono,
          categoriaId: personal.categoriaId,
          tipoRetiro: personal.tipoRetiro,
          provincia: personal.provincia,
          horasMensualesPactadas: personal.horasMensualesPactadas
        });
      });
    }
  }

  guardar(): void {
    if (this.form.invalid) {
      return;
    }

    this.error.set(null);
    const request = { ...this.form.getRawValue(), clienteId: this.clienteId };
    const accion = this.editando
      ? this.personalService.update(Number(this.id), request)
      : this.personalService.create(request);

    accion.subscribe({
      next: () => this.router.navigateByUrl(`/clientes/${this.clienteId}/personal`),
      error: (err) =>
        this.error.set(err.error?.mensaje ?? 'No se pudo guardar el personal. Verificá los datos ingresados.')
    });
  }
}
