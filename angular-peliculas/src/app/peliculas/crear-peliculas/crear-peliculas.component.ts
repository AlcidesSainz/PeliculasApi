import { Component, inject, model } from '@angular/core';
import { PeliculaCreacionDTO } from '../peliculas';
import { FormularioPeliculasComponent } from '../formulario-peliculas/formulario-peliculas.component';
import { SelectorMultipleDTO } from '../../compartidos/componentes/selector-multiple/SelectorMultipleModelo';
import { ActoreAutoCompleteDTO } from '../../actores/actores';
import { PeliculasService } from '../peliculas.service';
import { SelectorMultipleComponent } from '../../compartidos/componentes/selector-multiple/selector-multiple.component';
import { Router } from '@angular/router';
import { extraerErrores } from '../../compartidos/funciones/extraerErrores';
import { MostrarErroresComponent } from "../../compartidos/componentes/mostrar-errores/mostrar-errores.component";
import { CargandoComponent } from "../../compartidos/componentes/cargando/cargando.component";

@Component({
  selector: 'app-crear-peliculas',
  standalone: true,
  imports: [FormularioPeliculasComponent, MostrarErroresComponent, CargandoComponent],
  templateUrl: './crear-peliculas.component.html',
  styleUrl: './crear-peliculas.component.css',
})
export class CrearPeliculasComponent {
  generosSeleccionados: SelectorMultipleDTO[] = [];
  generosNoSeleccionados: SelectorMultipleDTO[] = [];
  cinesSeleccionados: SelectorMultipleDTO[] = [];
  cinesNoSeleccionados: SelectorMultipleDTO[] = [];
  actoresSeleccionados: ActoreAutoCompleteDTO[] = [];

  peliculaService = inject(PeliculasService);
  router = inject(Router);

  errores: string[] = [];
  constructor() {
    this.peliculaService.crearGet().subscribe((modelo) => {
      console.log('Modelo recibido:', modelo);
      this.generosNoSeleccionados = modelo.generos.map((genero) => {
        return <SelectorMultipleDTO>{ llave: genero.id, valor: genero.nombre };
      });

      this.cinesNoSeleccionados = modelo.cine.map(cine => {
        return <SelectorMultipleDTO>{llave: cine.id,valor: cine.nombre};
      });
    });
  }
  guardarcambios(pelicula: PeliculaCreacionDTO) {
    this.peliculaService.crear(pelicula).subscribe({
      next: (pelicula) => {
        console.log(pelicula);
        this.router.navigate(['/']);
      },
      error: (err) => {
        const errores = extraerErrores(err);
        this.errores = errores;
      },
    });
  }
}
