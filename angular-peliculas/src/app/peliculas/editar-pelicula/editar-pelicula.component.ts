import { Component, Input, numberAttribute } from '@angular/core';
import { PeliculaCreacionDTO, PeliculaDTO } from '../peliculas';
import { FormularioPeliculasComponent } from '../formulario-peliculas/formulario-peliculas.component';
import { SelectorMultipleDTO } from '../../compartidos/componentes/selector-multiple/SelectorMultipleModelo';
import { ActoreAutoCompleteDTO } from '../../actores/actores';

@Component({
  selector: 'app-editar-pelicula',
  standalone: true,
  imports: [FormularioPeliculasComponent],
  templateUrl: './editar-pelicula.component.html',
  styleUrl: './editar-pelicula.component.css',
})
export class EditarPeliculaComponent {
  @Input({ transform: numberAttribute })
  id!: number;

  pelicula: PeliculaDTO = {
    id: 1,
    titulo: 'El Padrino',
    trailer: 'ABC',
    fechaLanzamiento: new Date('1972-01-01'),
    poster:
      'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/5HlLUsmsv60cZVTzVns9ICZD6zU.jpg',
  };
  generosSeleccionados: SelectorMultipleDTO[] = [{ llave: 2, valor: 'Accion' }];

  generosNoSeleccionados: SelectorMultipleDTO[] = [
    { llave: 1, valor: 'Drama' },
    { llave: 3, valor: 'Comedia' },
    { llave: 4, valor: 'Terror' },
    { llave: 5, valor: 'Fantasia' },
    { llave: 6, valor: 'Thriller' },
  ];

  cinesSeleccionados: SelectorMultipleDTO[] = [{ llave: 3, valor: 'Cinemark' }];

  cinesNoSeleccionados: SelectorMultipleDTO[] = [
    { llave: 1, valor: 'Cinemax' },
    { llave: 2, valor: 'Multicines' },
    { llave: 4, valor: 'Supercines' },
  ];

  actoresSeleccionados: ActoreAutoCompleteDTO[] = [
    {
      id: 1,
      nombre: 'Leonardo DiCaprio',
      personajes: 'Jay Gatsby',
      foto: 'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/wo2hJpn04vbtmh0B9utCFdsQhxM.jpg',
    },
  ];
  guardarCambios(pelicula: PeliculaCreacionDTO) {
    console.log('Editando el actor', pelicula);
  }
}
