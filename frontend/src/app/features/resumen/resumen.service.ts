import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';

import { EnviarResumenResultado, ResumenMensual } from './resumen.model';

@Injectable({ providedIn: 'root' })
export class ResumenService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/resumenes';

  get(anio: number, mes: number): Observable<ResumenMensual | null> {
    return this.http
      .get<ResumenMensual>(this.baseUrl, { params: { anio, mes } })
      .pipe(catchError(() => of(null)));
  }

  generar(anio: number, mes: number): Observable<ResumenMensual> {
    return this.http.post<ResumenMensual>(`${this.baseUrl}/generar`, { anio, mes });
  }

  enviar(anio: number, mes: number): Observable<EnviarResumenResultado> {
    return this.http.post<EnviarResumenResultado>(`${this.baseUrl}/enviar`, { anio, mes });
  }
}
