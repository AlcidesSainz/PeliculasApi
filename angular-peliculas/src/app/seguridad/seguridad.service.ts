import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpResponse } from '@angular/common/http';
import {
  CredencialesUsuarioDTO,
  RespuestaAutenticacionDTO,
  UsuarioDTO,
} from './seguridad';
import { Observable, tap } from 'rxjs';
import { PaginacionDTO } from '../compartidos/modelos/PaginacionDTO';
import { construirQueryParams } from '../compartidos/funciones/construirQueryParams';

@Injectable({
  providedIn: 'root',
})
export class SeguridadService {
  constructor() {}

  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + '/usuarios';
  private readonly llaveToken = 'token';
  private readonly llaveExpiracion = 'token-expiracion';

  obtenerUsuariosPaginado(
    paginacion: PaginacionDTO
  ): Observable<HttpResponse<UsuarioDTO[]>> {
    let queryParams = construirQueryParams(paginacion);
    return this.http.get<UsuarioDTO[]>(`${this.urlBase}/obtenerUsuarios`, {
      params: queryParams,
      observe: 'response',
    });
  }
  hacerAdmin(email: string) {
    return this.http.post(`${this.urlBase}/haceradmin`, { email });
  }
  removerAdmin(email: string) {
    return this.http.post(`${this.urlBase}/removeradmin`, { email });
  }

  registrar(
    credenciales: CredencialesUsuarioDTO
  ): Observable<RespuestaAutenticacionDTO> {
    return this.http
      .post<RespuestaAutenticacionDTO>(
        this.urlBase + '/registrar',
        credenciales
      )
      .pipe(
        tap((respuestaAutenticacion) =>
          this.guardarToken(respuestaAutenticacion)
        )
      );
  }

  login(
    credenciales: CredencialesUsuarioDTO
  ): Observable<RespuestaAutenticacionDTO> {
    return this.http
      .post<RespuestaAutenticacionDTO>(this.urlBase + '/login', credenciales)
      .pipe(
        tap((respuestaAutenticacion) =>
          this.guardarToken(respuestaAutenticacion)
        )
      );
  }

  obtenerCamposJWT(campo: string): string {
    const token = localStorage.getItem(this.llaveToken);
    if (!token) {
      return '';
    }
    var dataToken = JSON.parse(atob(token.split('.')[1]));
    return dataToken[campo];
  }

  guardarToken(respuestaAutenticacion: RespuestaAutenticacionDTO) {
    localStorage.setItem(this.llaveToken, respuestaAutenticacion.token);
    localStorage.setItem(
      this.llaveExpiracion,
      respuestaAutenticacion.expiracion.toString()
    );
  }

  estaLogueado(): boolean {
    const token = localStorage.getItem(this.llaveToken);

    if (!token) {
      return false;
    }
    const expiracion = localStorage.getItem(this.llaveExpiracion)!;
    const expiracionFecha = new Date(expiracion);

    if (expiracionFecha <= new Date()) {
      this.logout();
      return false;
    }
    return true;
  }
  logout() {
    localStorage.removeItem(this.llaveToken);
    localStorage.removeItem(this.llaveExpiracion);
    
  }
  obtenerRol(): string {
    const esAdmin = this.obtenerCamposJWT('esadmin');
    if (esAdmin) {
      return 'admin';
    } else {
      return '';
    }
  }
  obtenerToken(): string | null {
    return localStorage.getItem(this.llaveToken);
  }
}
