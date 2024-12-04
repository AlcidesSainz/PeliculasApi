import { inject, Injectable } from '@angular/core';
import { GeneroCreacionDTO, GeneroDTO } from './generos';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PaginacionDTO } from '../compartidos/modelos/PaginacionDTO';
import { construirQueryParams } from '../compartidos/funciones/construirQueryParams';

@Injectable({
  providedIn: 'root',
})
export class GeneroService {
  constructor() {}

  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + '/generos';

  public obtenerPaginado(paginacion: PaginacionDTO): Observable<HttpResponse<GeneroDTO[]>>
  {
    let queryParams = construirQueryParams(paginacion);
    return this.http.get<GeneroDTO[]>(this.urlBase, {
      params: queryParams,
      observe: 'response',
    });
  }

  public crear(genero: GeneroCreacionDTO) {
    return this.http.post(this.urlBase, genero);
  }
}