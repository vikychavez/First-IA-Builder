import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet, Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { AuthService } from '../core/auth/auth.service';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    RouterOutlet,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss'
})
export class ShellComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly navItems = [
    { path: 'resumen', label: 'Resumen de Pago' },
    { path: 'novedades', label: 'Novedades' },
    { path: 'clientes', label: 'Clientes' },
    { path: 'categorias', label: 'Categorías' },
    { path: 'personal', label: 'Personal' },
    { path: 'zonas-desfavorables', label: 'Zonas Desfavorables' },
    { path: 'parametros', label: 'Parámetros' }
  ];

  cerrarSesion(): void {
    this.authService.logout().subscribe(() => this.router.navigateByUrl('/login'));
  }
}
