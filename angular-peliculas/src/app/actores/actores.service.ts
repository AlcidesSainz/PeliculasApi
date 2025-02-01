import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ActorCreacionDTO, ActorDTO, ActoreAutoCompleteDTO, DirectoresAutoCompleteDTO, LandingPageActoresDTO } from './actores';
import { PaginacionDTO } from '../compartidos/modelos/PaginacionDTO';
import { Observable } from 'rxjs';
import { construirQueryParams } from '../compartidos/funciones/construirQueryParams';
import { IServicioCRUD } from '../compartidos/interfaces/IServicioCrud';
import { PeliculaDirectorResponseDTO, PeliculaDTO } from '../peliculas/peliculas';

@Injectable({
  providedIn: 'root',
})
export class ActoresService
  implements IServicioCRUD<ActorDTO, ActorCreacionDTO>
{
  constructor() {}
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + '/actores';

  public obtenerLandingPage(): Observable<LandingPageActoresDTO> {
    return this.http.get<LandingPageActoresDTO>(`${this.urlBase}/landing`);
  }

  public crear(actor: ActorCreacionDTO) {
    const formData = this.construirFormData(actor);
    return this.http.post(this.urlBase, formData);
  }
  public obtenerPaginado(
    paginacion: PaginacionDTO
  ): Observable<HttpResponse<ActorDTO[]>> {
    let queryParams = construirQueryParams(paginacion);
    return this.http.get<ActorDTO[]>(this.urlBase, {
      params: queryParams,
      observe: 'response',
    });
  }

  private construirFormData(actor: ActorCreacionDTO): FormData {
    const formData = new FormData();

    formData.append('nombre', actor.nombre);

    formData.append('biografia', actor.biografia);
    /*toIsoString es para convertirlo a formato de fecha y hora 2024-01-25T15:18:20 y al utilizar 
    el split en la T y seleccionar el 0 para quedarse con la primera parte, o sea lo que esta antes de la T*/
    formData.append(
      'fechaNacimiento',
      actor.fechaNacimiento.toISOString().split('T')[0]
    );

    if (actor.foto) {
      formData.append('foto', actor.foto);
    }
    return formData;
  }
  public obtenerPorId(id: number): Observable<ActorDTO> {
    return this.http.get<ActorDTO>(`${this.urlBase}/${id}`);
  }
  public obtenerPeliculasPorActor(id: number): Observable<PeliculaDTO[]> {
    return this.http.get<PeliculaDTO[]>(`${this.urlBase}/${id}/peliculas`);
  }
  public obtenerPorNombre(nombre: string): Observable<ActoreAutoCompleteDTO[]> {
    return this.http.get<ActoreAutoCompleteDTO[]>(`${this.urlBase}/${nombre}`);
  }
  public obtenerPorNombreDirector(
    nombre: string
  ): Observable<DirectoresAutoCompleteDTO[]> {
    return this.http.get<DirectoresAutoCompleteDTO[]>(
      `${this.urlBase}/${nombre}`
    );
  }
  public actualizar(id: number, actor: ActorCreacionDTO) {
    const formData = this.construirFormData(actor);
    return this.http.put(`${this.urlBase}/${id}`, formData);
  }
  public borrar(id: number) {
    return this.http.delete(`${this.urlBase}/${id}`);
  }
    public filtrar(valores: any): Observable<HttpResponse<ActorDTO[]>> {
      const params = new HttpParams({ fromObject: valores });
      return this.http.get<ActorDTO[]>(`${this.urlBase}/filtrar`, {
        params,
        observe: 'response',
      });
    }
}
