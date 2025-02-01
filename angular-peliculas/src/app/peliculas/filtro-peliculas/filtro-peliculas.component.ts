import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ListadoPeliculasComponent } from '../listado-peliculas/listado-peliculas.component';
import { FiltroPeliculas } from './filtroPeliculas';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { GeneroDTO } from '../../generos/generos';
import { PeliculaDTO } from '../peliculas';
import { PeliculasService } from '../peliculas.service';
import { GeneroService } from '../../generos/genero.service';
import { PaginacionDTO } from '../../compartidos/modelos/PaginacionDTO';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-filtro-peliculas',
  standalone: true,
  imports: [
    MatButtonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    ListadoPeliculasComponent,
    MatPaginatorModule,
  ],
  templateUrl: './filtro-peliculas.component.html',
  styleUrl: './filtro-peliculas.component.css',
})
export class FiltroPeliculasComponent implements OnInit {
  generosService = inject(GeneroService);
  peliculasService = inject(PeliculasService);
  paginacion: PaginacionDTO = { pagina: 1, recordsPorPagina: 12 };
  cantidadTotalRegistros!: number;
  ngOnInit(): void {
    this.generosService.obtenerTodoslosGeneros().subscribe((generos) => {
      this.generos = generos;

      this.leerValoresUrl();
      this.buscarPelicuas(this.form.value as FiltroPeliculas);
      this.form.valueChanges
        .pipe(debounceTime(300))
        .subscribe((valores) => {
        this.buscarPelicuas(valores as FiltroPeliculas);
        this.escribirParametrosBusquedaEnUrl(valores as FiltroPeliculas);
      });
    });
  }

  buscarPelicuas(valores: FiltroPeliculas) {
    valores.pagina = this.paginacion.pagina;
    valores.recordsPorPagina = this.paginacion.recordsPorPagina;

    this.peliculasService.filtrar(valores).subscribe((respuesta) => {
      this.peliculas = respuesta.body as PeliculaDTO[];
      const cabecera = respuesta.headers.get(
        'cantidad-total-registros'
      ) as string;
      this.cantidadTotalRegistros = parseInt(cabecera, 10);
    });
  }

  escribirParametrosBusquedaEnUrl(valores: FiltroPeliculas) {
    let queryStrings = [];

    if (valores.titulo) {
      queryStrings.push(`titulo=${encodeURIComponent(valores.titulo)}`);
    }
    if (valores.generoId != 0) {
      queryStrings.push(`genero=${valores.generoId}`);
    }
    if (valores.proximosEstrenos) {
      queryStrings.push(`proximosEstrenos=${valores.proximosEstrenos}`);
    }
    if (valores.enCines) {
      queryStrings.push(`enCines=${valores.enCines}`);
    }

    this.location.replaceState('peliculas/filtrar', queryStrings.join('&'));
  }
  leerValoresUrl() {
    this.activatedRoute.queryParams.subscribe((params: any) => {
      var objeto: any = {};

      if (params.titulo) {
        objeto.titulo = params.titulo;
      }
      if (params.generoId) {
        objeto.generoId = Number(params.generoId);
      }
      if (params.enCines) {
        objeto.enCines = params.enCines;
      }
      if (params.proximosEstrenos) {
        objeto.proximosEstrenos = params.proximosEstrenos;
      }
      this.form.patchValue(objeto);
    });
  }
  limpiar() {
    this.form.patchValue({
      titulo: '',
      generoId: 0,
      proximosEstrenos: false,
      enCines: false,
    });
  }

  actualizarPaginacion(datos: PageEvent) {
    this.paginacion = {pagina:datos.pageIndex + 1, recordsPorPagina: datos.pageSize};
    this.buscarPelicuas(this.form.value as FiltroPeliculas);
  }
  private formBuilder = inject(FormBuilder);
  private location = inject(Location);
  private activatedRoute = inject(ActivatedRoute);
  form = this.formBuilder.group({
    titulo: '',
    generoId: 0,
    proximosEstrenos: false,
    enCines: false,
  });
  //Lista de generos
  generos!: GeneroDTO[];

  peliculas!: PeliculaDTO[];
}
