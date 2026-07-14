import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ParametrosLiquidacion } from './parametros.model';

@Injectable({ providedIn: 'root' })
export class ParametrosService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/parametros-liquidacion';

  get(): Observable<ParametrosLiquidacion> {
    return this.http.get<ParametrosLiquidacion>(this.baseUrl);
  }

  update(request: ParametrosLiquidacion): Observable<ParametrosLiquidacion> {
    return this.http.put<ParametrosLiquidacion>(this.baseUrl, request);
  }
}
