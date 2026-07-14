import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { ParametrosService } from './parametros.service';

@Component({
  selector: 'app-parametros-form',
  standalone: true,
  imports: [ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './parametros-form.component.html'
})
export class ParametrosFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly parametrosService = inject(ParametrosService);

  readonly guardado = signal(false);
  readonly error = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    porcentajeAntiguedad: [0, [Validators.required, Validators.min(0)]],
    porcentajeZonaDesfavorable: [0, [Validators.required, Validators.min(0)]],
    multiplicadorHorasExtras: [0, [Validators.required, Validators.min(0)]],
    multiplicadorFeriados: [0, [Validators.required, Validators.min(0)]]
  });

  constructor() {
    this.parametrosService.get().subscribe((parametros) => this.form.patchValue(parametros));
  }

  guardar(): void {
    if (this.form.invalid) {
      return;
    }

    this.error.set(null);
    this.guardado.set(false);

    this.parametrosService.update(this.form.getRawValue()).subscribe({
      next: () => this.guardado.set(true),
      error: () => this.error.set('No se pudieron guardar los parámetros.')
    });
  }
}
