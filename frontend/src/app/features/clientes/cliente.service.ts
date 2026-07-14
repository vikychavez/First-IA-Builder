import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { Cliente, ClienteRequest, Sexo } from './cliente.model';

@Injectable({ providedIn: 'root' })
export class ClienteService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/clientes';

  getAll(soloActivos = true): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(this.baseUrl, { params: { soloActivos } });
  }

  calcularCuit(dni: string, sexo: Sexo): Observable<{ cuit: string }> {
    return this.http.get<{ cuit: string }>(`${this.baseUrl}/calcular-cuit`, { params: { dni, sexo } });
  }

  getById(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.baseUrl}/${id}`);
  }

  create(request: ClienteRequest): Observable<Cliente> {
    return this.http.post<Cliente>(this.baseUrl, request);
  }

  update(id: number, request: ClienteRequest): Observable<Cliente> {
    return this.http.put<Cliente>(`${this.baseUrl}/${id}`, request);
  }

  baja(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/baja`, {});
  }
}
