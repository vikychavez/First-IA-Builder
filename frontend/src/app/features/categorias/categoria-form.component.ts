import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { CategoriaService } from './categoria.service';

@Component({
  selector: 'app-categoria-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './categoria-form.component.html'
})
export class CategoriaFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly categoriaService = inject(CategoriaService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly id = this.route.snapshot.paramMap.get('id');
  readonly editando = this.id !== null;
  readonly error = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    nombre: ['', Validators.required],
    valorHoraConRetiro: [0, [Validators.required, Validators.min(0.01)]],
    valorHoraSinRetiro: [0, [Validators.required, Validators.min(0.01)]]
  });

  constructor() {
    if (this.editando) {
      this.categoriaService.getById(Number(this.id)).subscribe((categoria) => {
        this.form.patchValue({
          nombre: categoria.nombre,
          valorHoraConRetiro: categoria.valorHoraConRetiro,
          valorHoraSinRetiro: categoria.valorHoraSinRetiro
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
      ? this.categoriaService.update(Number(this.id), request)
      : this.categoriaService.create(request);

    accion.subscribe({
      next: () => this.router.navigateByUrl('/categorias'),
      error: () => this.error.set('No se pudo guardar la categoría. Verificá los datos ingresados.')
    });
  }
}
