import { inject, Injectable } from '@angular/core';
import { GeneroCreacionDTO, GeneroDTO } from './generos';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PaginacionDTO } from '../compartidos/modelos/PaginacionDTO';
import { construirQueryParams } from '../compartidos/funciones/construirQueryParams';
import { IServicioCRUD } from '../compartidos/interfaces/IServicioCrud';

@Injectable({
  providedIn: 'root',
})
export class GeneroService implements IServicioCRUD<GeneroDTO,GeneroCreacionDTO> {
  constructor() {}

  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + '/generos';

  public obtenerPaginado(
    paginacion: PaginacionDTO
  ): Observable<HttpResponse<GeneroDTO[]>> {
    let queryParams = construirQueryParams(paginacion);
    return this.http.get<GeneroDTO[]>(this.urlBase, {
      params: queryParams,
      observe: 'response',
    });
  }

  public crear(genero: GeneroCreacionDTO) {
    return this.http.post(this.urlBase, genero);
  }

  public obtenerPorId(id: number): Observable<GeneroDTO> {
    return this.http.get<GeneroDTO>(`${this.urlBase}/${id}`);
  }

  public obtenerTodoslosGeneros(): Observable<GeneroDTO[]> {
    return this.http.get<GeneroDTO[]>(this.urlBase);
  }

  public actualizar(id: number, genero: GeneroCreacionDTO) {
    return this.http.put(`${this.urlBase}/${id}`, genero);
  }

  public borrar(id: number) {
    return this.http.delete(`${this.urlBase}/${id}`)
  }
}
