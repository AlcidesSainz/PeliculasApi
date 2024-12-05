import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ActorCreacionDTO } from './actores';

@Injectable({
  providedIn: 'root',
})
export class ActoresService {
  constructor() {}
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + '/actores';

  public crear(actor: ActorCreacionDTO) {
    const formData = this.construirFormData(actor);
    return this.http.post(this.urlBase, formData);
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
}
