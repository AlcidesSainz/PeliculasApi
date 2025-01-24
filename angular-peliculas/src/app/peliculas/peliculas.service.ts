import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import {
  LandingPagePeliculasDTO,
  PeliculaCreacionDTO,
  PeliculaDTO,
  PeliculaPutGetDTO,
  PeliculasPostGetDTO,
} from './peliculas';

@Injectable({
  providedIn: 'root',
})
export class PeliculasService {
  constructor() {}

  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + '/peliculas';

  public obtenerLandingPage(): Observable<LandingPagePeliculasDTO> {
    return this.http.get<LandingPagePeliculasDTO>(`${this.urlBase}/landing`);
  }

  public obtenerPorId(id: number): Observable<PeliculaDTO> {
    return this.http.get<PeliculaDTO>(`${this.urlBase}/${id}`);
  }

  public filtrar(valores: any): Observable<HttpResponse<PeliculaDTO[]>> {
    const params = new HttpParams({ fromObject: valores });
    return this.http.get<PeliculaDTO[]>(`${this.urlBase}/filtrar`, {
      params,
      observe: 'response',
    });
  }
  public crearGet(): Observable<PeliculasPostGetDTO> {
    return this.http.get<PeliculasPostGetDTO>(`${this.urlBase}/postget`);
  }

  public crear(pelicula: PeliculaCreacionDTO): Observable<PeliculaDTO> {
    const formData = this.construirFormData(pelicula);
    return this.http.post<PeliculaDTO>(this.urlBase, formData);
  }

  public actualizarGet(id: number): Observable<PeliculaPutGetDTO> {
    return this.http.get<PeliculaPutGetDTO>(`${this.urlBase}/putget/${id}`);
  }
  public actualizar(id: number, pelicula: PeliculaCreacionDTO) {
    const formData = this.construirFormData(pelicula);
    return this.http.put(`${this.urlBase}/${id}`, formData);
  }

  private construirFormData(pelicula: PeliculaCreacionDTO): FormData {
    const formData = new FormData();

    formData.append('titulo', pelicula.titulo);
    formData.append(
      'fechaLanzamiento',
      pelicula.fechaLanzamiento.toISOString().split('T')[0]
    );

    if (pelicula.poster) {
      formData.append('poster', pelicula.poster);
    }
    if (pelicula.sinopsis) {
      formData.append('sinopsis', pelicula.sinopsis);
    }
    if (pelicula.trailer) {
      formData.append('trailer', pelicula.trailer);
    }
    formData.append('generosIds', JSON.stringify(pelicula.generosIds));
    formData.append('cinesIds', JSON.stringify(pelicula.cinesIds));
    formData.append('actores', JSON.stringify(pelicula.actores));

    return formData;
  }
  borrar(id: number): Observable<any> {
    return this.http.delete(`${this.urlBase}/${id}`);
  }
}
