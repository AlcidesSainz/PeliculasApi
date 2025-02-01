import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { PaginacionDTO } from '../../compartidos/modelos/PaginacionDTO';
import { ActoresService } from '../actores.service';
import { ActorDTO } from '../actores';
import { FiltroActores } from './filtroActores';
import { debounceTime } from 'rxjs';
import { ListadoActoresFiltroComponent } from '../listado-actores-filtro/listado-actores-filtro.component';

@Component({
  selector: 'app-filtro-actores',
  standalone: true,
  imports: [
    MatButtonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatPaginatorModule,
    ListadoActoresFiltroComponent,
  ],
  templateUrl: './filtro-actores.component.html',
  styleUrl: './filtro-actores.component.css',
})
export class FiltroActoresComponent implements OnInit {
  ngOnInit(): void {
    this.leerValoresUrl();
    this.buscarActores(this.form.value as FiltroActores);
    this.form.valueChanges.pipe(debounceTime(300)).subscribe((valores) => {
      this.buscarActores(valores as FiltroActores);
      this.escribirParametrosBusquedaEnUrl(valores as FiltroActores);
    });
  }
  actoresService = inject(ActoresService);
  paginacion: PaginacionDTO = { pagina: 1, recordsPorPagina: 12 };
  cantidadTotalRegistros!: number;

  buscarActores(valores: FiltroActores) {
    valores.pagina = this.paginacion.pagina;
    valores.recordsPorPagina = this.paginacion.recordsPorPagina;

    this.actoresService.filtrar(valores).subscribe((respuesta) => {
      this.actores = respuesta.body as ActorDTO[];
      const cabecera = respuesta.headers.get(
        'cantidad-total-registros'
      ) as string;
      this.cantidadTotalRegistros = parseInt(cabecera, 12);
    });
  }

  escribirParametrosBusquedaEnUrl(valores: FiltroActores) {
    let queryStrings = [];

    if (valores.nombre) {
      queryStrings.push(`nombre=${encodeURIComponent(valores.nombre)}`);
    }
    this.location.replaceState('actores/filtrar', queryStrings.join('&'));
  }
  leerValoresUrl() {
    this.activatedRoute.queryParams.subscribe((params: any) => {
      var objeto: any = {};

      if (params.nombre) {
        objeto.nombre = params.nombre;
      }
      this.form.patchValue(objeto);
    });
  }
  limpiar() {
    this.form.patchValue({
      nombre: '',
    });
  }

  actualizarPaginacion(datos: PageEvent) {
    this.paginacion = {
      pagina: datos.pageIndex + 1,
      recordsPorPagina: datos.pageSize,
    };
    this.buscarActores(this.form.value as FiltroActores);
  }
  private formBuilder = inject(FormBuilder);
  private location = inject(Location);
  private activatedRoute = inject(ActivatedRoute);
  form = this.formBuilder.group({
    nombre: '',
  });

  actores!: ActorDTO[];
}
