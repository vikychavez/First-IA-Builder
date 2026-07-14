import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { PROVINCIAS } from '../../core/provincias';
import { ZonaService } from './zona.service';

@Component({
  selector: 'app-zona-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatSelectModule, MatButtonModule],
  templateUrl: './zona-form.component.html'
})
export class ZonaFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly zonaService = inject(ZonaService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly provincias = PROVINCIAS;
  private readonly id = this.route.snapshot.paramMap.get('id');
  readonly editando = this.id !== null;
  readonly error = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    provincia: ['', Validators.required]
  });

  constructor() {
    if (this.editando) {
      this.zonaService.getAll(false).subscribe((zonas) => {
        const zona = zonas.find((z) => z.id === Number(this.id));
        if (zona) {
          this.form.patchValue({ provincia: zona.provincia });
        }
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
      ? this.zonaService.update(Number(this.id), request)
      : this.zonaService.create(request);

    accion.subscribe({
      next: () => this.router.navigateByUrl('/zonas-desfavorables'),
      error: (err) =>
        this.error.set(err.error?.mensaje ?? 'No se pudo guardar la zona desfavorable.')
    });
  }
}
