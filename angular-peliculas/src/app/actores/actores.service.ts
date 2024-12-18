import { HttpClient, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ActorCreacionDTO, ActorDTO, ActoreAutoCompleteDTO } from './actores';
import { PaginacionDTO } from '../compartidos/modelos/PaginacionDTO';
import { Observable } from 'rxjs';
import { construirQueryParams } from '../compartidos/funciones/construirQueryParams';
import { IServicioCRUD } from '../compartidos/interfaces/IServicioCrud';

@Injectable({
  providedIn: 'root',
})
export class ActoresService
  implements IServicioCRUD<ActorDTO, ActorCreacionDTO>
{
  constructor() {}
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + '/actores';

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

  public obtenerPorNombre(nombre: string): Observable<ActoreAutoCompleteDTO[]> {
    return this.http.get<ActoreAutoCompleteDTO[]>(`${this.urlBase}/${nombre}`);
  }

  public actualizar(id: number, actor: ActorCreacionDTO) {
    const formData = this.construirFormData(actor);
    return this.http.put(`${this.urlBase}/${id}`, formData);
  }
  public borrar(id: number) {
    return this.http.delete(`${this.urlBase}/${id}`);
  }
}
