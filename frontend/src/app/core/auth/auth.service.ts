import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { LoginRequest, Usuario } from './usuario.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/auth';

  private readonly usuarioActual = signal<Usuario | null>(null);
  readonly estaAutenticado = computed(() => this.usuarioActual() !== null);

  login(credenciales: LoginRequest): Observable<Usuario> {
    return this.http
      .post<Usuario>(`${this.baseUrl}/login`, credenciales)
      .pipe(tap((usuario) => this.usuarioActual.set(usuario)));
  }

  logout(): Observable<void> {
    return this.http
      .post<void>(`${this.baseUrl}/logout`, {})
      .pipe(tap(() => this.usuarioActual.set(null)));
  }

  cargarSesionActual(): Observable<Usuario> {
    return this.http
      .get<Usuario>(`${this.baseUrl}/me`)
      .pipe(tap((usuario) => this.usuarioActual.set(usuario)));
  }
}
