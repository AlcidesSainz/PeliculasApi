import { Component, inject, Input } from '@angular/core';
import { SERVICIO_CRUD_TOKEN } from '../../proveedores/proveedores';
import { PaginacionDTO } from '../../modelos/PaginacionDTO';
import { HttpResponse } from '@angular/common/http';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ListadoGenericoComponent } from '../listado-generico/listado-generico.component';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { IServicioCRUD } from '../../interfaces/IServicioCrud';

@Component({
  selector: 'app-indice-entidad',
  standalone: true,
  imports: [
    ListadoGenericoComponent,
    RouterLink,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    SweetAlert2Module,
  ],
  templateUrl: './indice-entidad.component.html',
  styleUrl: './indice-entidad.component.css',
})
export class IndiceEntidadComponent<TDTO, TCreacionDTO> {
  @Input({ required: true })
  titulo!: string;

  @Input({ required: true })
  rutaCrear!: string;

  @Input({ required: true })
  rutaEditar!: string;

  @Input()
  columnasMostrar = ['id', 'nombre', 'acciones'];

  servicioCrud = inject(SERVICIO_CRUD_TOKEN) as IServicioCRUD<
    TDTO,
    TCreacionDTO
  >;

  paginacion: PaginacionDTO = { pagina: 1, recordsPorPagina: 5 };

  entidades!: TDTO[];

  cantidadRegistros!: number;

  constructor() {
    this.cargarRegistros();
  }

  actualizarPaginacion(datos: PageEvent) {
    this.paginacion = {
      pagina: datos.pageIndex + 1,
      recordsPorPagina: datos.pageSize,
    };
    this.cargarRegistros();
  }
  cargarRegistros() {
    this.servicioCrud
      .obtenerPaginado(this.paginacion)
      .subscribe((respuesta: HttpResponse<TDTO[]>) => {
        this.entidades = respuesta.body as TDTO[];
        const cabecera = respuesta.headers.get(
          'cantidad-total-registros'
        ) as string;
        this.cantidadRegistros = parseInt(cabecera, 10);
      });
  }

  borrar(id: number) {
    this.servicioCrud.borrar(id).subscribe(() => {
      this.paginacion = { pagina: 1, recordsPorPagina: 5 };
      this.cargarRegistros();
    });
  }
  primeraLetraMayuscula(valor: string) {
    if (!valor) return valor;
    return valor.charAt(0).toUpperCase() + valor.slice(1);
  }
}
