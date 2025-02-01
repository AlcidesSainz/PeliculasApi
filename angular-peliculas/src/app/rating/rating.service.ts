import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class RatingService {
  constructor() {}

  private urlBase = environment.apiUrl + '/ratings';
  private http = inject(HttpClient);

  puntuar(peliculaId: number, puntuacion: number) {
    return this.http.post(this.urlBase, { peliculaId, puntuacion });
  }
  puntuarActor(actorId: number, puntuacion: number) {
    return this.http.post(this.urlBase+'/actor', { actorId, puntuacion });
  }
}
