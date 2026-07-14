import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { Novedad, NovedadUpsertRequest } from './novedad.model';

@Injectable({ providedIn: 'root' })
export class NovedadService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/novedades';

  getByPeriodo(anio: number, mes: number): Observable<Novedad[]> {
    return this.http.get<Novedad[]>(this.baseUrl, { params: { anio, mes } });
  }

  upsert(request: NovedadUpsertRequest): Observable<Novedad> {
    return this.http.put<Novedad>(this.baseUrl, request);
  }
}
