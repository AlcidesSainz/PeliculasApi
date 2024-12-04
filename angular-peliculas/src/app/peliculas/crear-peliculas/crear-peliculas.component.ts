import { Component } from '@angular/core';
import { PeliculaCreacionDTO } from '../peliculas';
import { FormularioPeliculasComponent } from '../formulario-peliculas/formulario-peliculas.component';
import { SelectorMultipleDTO } from '../../compartidos/componentes/selector-multiple/SelectorMultipleModelo';
import { ActoreAutoCompleteDTO } from '../../actores/actores';

@Component({
  selector: 'app-crear-peliculas',
  standalone: true,
  imports: [FormularioPeliculasComponent],
  templateUrl: './crear-peliculas.component.html',
  styleUrl: './crear-peliculas.component.css',
})
export class CrearPeliculasComponent {
  generosSeleccionados: SelectorMultipleDTO[] = [];

  generosNoSeleccionados: SelectorMultipleDTO[] = [
    { llave: 1, valor: 'Drama' },
    { llave: 2, valor: 'Accion' },
    { llave: 3, valor: 'Comedia' },
    { llave: 4, valor: 'Terror' },
    { llave: 5, valor: 'Fantasia' },
    { llave: 6, valor: 'Thriller' },
  ];
  cinesSeleccionados: SelectorMultipleDTO[] = [];

  cinesNoSeleccionados: SelectorMultipleDTO[] = [
    { llave: 1, valor: 'Cinemax' },
    { llave: 2, valor: 'Multicines' },
    { llave: 3, valor: 'Cinemark' },
    { llave: 4, valor: 'Supercines' },
  ];
  actoresSeleccionados: ActoreAutoCompleteDTO[] = [];
  guardarcambios(pelicula: PeliculaCreacionDTO) {
    console.log('Creando pelicula: ', pelicula);
  }
}
