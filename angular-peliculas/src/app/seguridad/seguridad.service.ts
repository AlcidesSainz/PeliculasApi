import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SeguridadService {

  estaLogueado(): boolean {
    return true;
  }
  obtenerRol():string{
    return "";
  }
}
