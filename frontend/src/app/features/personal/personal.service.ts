import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { Personal, PersonalRequest } from './personal.model';

@Injectable({ providedIn: 'root' })
export class PersonalService {
  private readonly http = inject(HttpClient);

  getByCliente(clienteId: number, soloActivos = true): Observable<Personal[]> {
    return this.http.get<Personal[]>(`/api/clientes/${clienteId}/personal`, { params: { soloActivos } });
  }

  getById(id: number): Observable<Personal> {
    return this.http.get<Personal>(`/api/personal/${id}`);
  }

  create(request: PersonalRequest): Observable<Personal> {
    return this.http.post<Personal>('/api/personal', request);
  }

  update(id: number, request: PersonalRequest): Observable<Personal> {
    return this.http.put<Personal>(`/api/personal/${id}`, request);
  }

  baja(id: number): Observable<void> {
    return this.http.post<void>(`/api/personal/${id}/baja`, {});
  }
}
