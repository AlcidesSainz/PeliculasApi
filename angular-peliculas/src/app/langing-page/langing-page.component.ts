import { Component, OnInit } from '@angular/core';
import { ListadoPeliculasComponent } from "../peliculas/listado-peliculas/listado-peliculas.component";

@Component({
  selector: 'app-langing-page',
  standalone: true,
  imports: [ListadoPeliculasComponent],
  templateUrl: './langing-page.component.html',
  styleUrl: './langing-page.component.css',
})
export class LangingPageComponent implements OnInit{
  ngOnInit(): void {
    setTimeout(() => {
      this.peliculasEnCines = [
        {
          titulo: 'El Padrino',
          director: 'Francis Ford Coppola',
          fechaLanzamiento: new Date('1972'),
          presupuesto: 6000000,
          poster:
            'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/5HlLUsmsv60cZVTzVns9ICZD6zU.jpg',
        },
        {
          titulo: 'La sustancia',
          director: 'Coralie Fargeat',
          fechaLanzamiento: new Date('2024'),
          presupuesto: 17500000.0,
          poster:
            'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/w1PiIqM89r4AM7CiMEP4VLCEFUn.jpg',
        },
        {
          titulo: 'Look Back',
          director: 'Kiyotaka Oshiyama',
          fechaLanzamiento: new Date('2024'),
          presupuesto: 1000000,
          poster:
            'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/yGPEQdlYbAINCK7DbhhEY0UUycS.jpg',
        },
        {
          titulo: 'Pedro Páramo',
          director: 'Rodrigo Prieto',
          fechaLanzamiento: new Date('2024'),
          presupuesto: 1000000,
          poster:
            'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/AeeqpYGfMr4dpepzrwfYTo22aDy.jpg',
        },
      ];
      this.peliculasProximosEstrenos = [
        {
          titulo: 'La Cenicienta',
          director: 'Hamilton Luske',
          fechaLanzamiento: new Date('1950'),
          presupuesto: 2900000.0,
          poster:
            'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/5kqNkGE5PFt4EOloc90LMUoXqxb.jpg',
        },
        {
          titulo: 'Enredados',
          director: 'Byron Howard',
          fechaLanzamiento: new Date('2010'),
          presupuesto: 260000000.0,
          poster:
            'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/bFUreK1CxkUwk4y6W2Qoo88l0UF.jpg',
        },
        {
          titulo: 'Los Increíbles',
          director: 'Brad Bird',
          fechaLanzamiento: new Date('2004'),
          presupuesto: 92000000.0,
          poster:
            'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/al1jusd4T7JPatZlj4BuYkDDOzr.jpg',
        },
        {
          titulo: 'La vida es bella',
          director: 'Roberto Benigni',
          fechaLanzamiento: new Date('1997'),
          presupuesto: 20000000.0,
          poster:
            'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/aZ7MFlKPfB02Lr9NwZQ4vsYRgcy.jpg',
        },
      ];
    }, 100);
  }
  peliculasEnCines!: any[];
  peliculasProximosEstrenos!: any[];
}
