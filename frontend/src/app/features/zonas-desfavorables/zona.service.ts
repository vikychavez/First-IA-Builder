import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ZonaDesfavorable, ZonaDesfavorableRequest } from './zona.model';

@Injectable({ providedIn: 'root' })
export class ZonaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/zonas-desfavorables';

  getAll(soloActivas = true): Observable<ZonaDesfavorable[]> {
    return this.http.get<ZonaDesfavorable[]>(this.baseUrl, { params: { soloActivas } });
  }

  create(request: ZonaDesfavorableRequest): Observable<ZonaDesfavorable> {
    return this.http.post<ZonaDesfavorable>(this.baseUrl, request);
  }

  update(id: number, request: ZonaDesfavorableRequest): Observable<ZonaDesfavorable> {
    return this.http.put<ZonaDesfavorable>(`${this.baseUrl}/${id}`, request);
  }

  baja(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/baja`, {});
  }
}
