import { Observable } from "rxjs";
import { PaginacionDTO } from "../modelos/PaginacionDTO";
import { HttpResponse } from "@angular/common/http";

export interface IServicioCRUD<TDTO, TCreacionDTO> {
  obtenerPaginado(paginacion: PaginacionDTO): Observable<HttpResponse<TDTO[]>>;
  crear(entidad: TCreacionDTO): Observable<any>;
  obtenerPorId(id: number): Observable<TDTO>;
  actualizar(id: number, entidad: TCreacionDTO): Observable<any>;
  borrar(id: number):Observable<any>;
}