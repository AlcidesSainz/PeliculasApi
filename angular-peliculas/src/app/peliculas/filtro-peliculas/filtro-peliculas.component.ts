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
  ],
  templateUrl: './filtro-peliculas.component.html',
  styleUrl: './filtro-peliculas.component.css',
})
export class FiltroPeliculasComponent implements OnInit {
  ngOnInit(): void {
    this.leerValoresUrl();
    this.buscarPelicuas(this.form.value as FiltroPeliculas);

    this.form.valueChanges.subscribe((valores) => {
      this.peliculas = this.peliculasOriginal;
      this.buscarPelicuas(valores as FiltroPeliculas);
      this.escribirParametrosBusquedaEnUrl(valores as FiltroPeliculas);
    });
  }

  buscarPelicuas(valores: FiltroPeliculas) {
    if (valores.titulo) {
      this.peliculas = this.peliculas.filter(
        (pelicula) => pelicula.titulo.indexOf(valores.titulo) !== -1
      );
    }
    if (valores.generoId !== 0) {
      this.peliculas = this.peliculas.filter(
        (pelicula) => pelicula.generos.indexOf(valores.generoId) !== -1
      );
    }
    if (valores.enCines) {
      this.peliculas = this.peliculas.filter((pelicula) => pelicula.enCines);
    }
    if (valores.proximosEstrenos) {
      this.peliculas = this.peliculas.filter(
        (pelicula) => pelicula.proximosEstrenos
      );
    }
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
  generos = [
    { id: 1, nombre: 'Drama' },
    { id: 2, nombre: 'Accion' },
    { id: 3, nombre: 'Comedia' },
    { id: 4, nombre: 'Terror' },
    { id: 5, nombre: 'Fantasia' },
    { id: 6, nombre: 'Thriller' },
  ];
  //Lista de peliculas
  peliculasOriginal = [
    {
      titulo: 'El Padrino',
      director: 'Francis Ford Coppola',
      fechaLanzamiento: new Date('1972'),
      presupuesto: 6000000,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/5HlLUsmsv60cZVTzVns9ICZD6zU.jpg',
      generos: [1],
      enCines: true,
      proximosEstrenos: false,
    },
    {
      titulo: 'La sustancia',
      director: 'Coralie Fargeat',
      fechaLanzamiento: new Date('2024'),
      presupuesto: 17500000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/w1PiIqM89r4AM7CiMEP4VLCEFUn.jpg',
      generos: [4],
      enCines: true,
      proximosEstrenos: false,
    },
    {
      titulo: 'Look Back',
      director: 'Kiyotaka Oshiyama',
      fechaLanzamiento: new Date('2024'),
      presupuesto: 1000000,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/yGPEQdlYbAINCK7DbhhEY0UUycS.jpg',
      generos: [1],
      enCines: true,
      proximosEstrenos: false,
    },
    {
      titulo: 'Pedro Páramo',
      director: 'Rodrigo Prieto',
      fechaLanzamiento: new Date('2024'),
      presupuesto: 1000000,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/AeeqpYGfMr4dpepzrwfYTo22aDy.jpg',
      generos: [1],
      enCines: false,
      proximosEstrenos: true,
    },
    {
      titulo: 'La Cenicienta',
      director: 'Hamilton Luske',
      fechaLanzamiento: new Date('1950'),
      presupuesto: 2900000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/5kqNkGE5PFt4EOloc90LMUoXqxb.jpg',
      generos: [5],
      enCines: false,
      proximosEstrenos: false,
    },
    {
      titulo: 'Enredados',
      director: 'Byron Howard',
      fechaLanzamiento: new Date('2010'),
      presupuesto: 260000000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/bFUreK1CxkUwk4y6W2Qoo88l0UF.jpg',
      generos: [5],
      enCines: true,
      proximosEstrenos: true,
    },
    {
      titulo: 'Los Increíbles',
      director: 'Brad Bird',
      fechaLanzamiento: new Date('2004'),
      presupuesto: 92000000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/al1jusd4T7JPatZlj4BuYkDDOzr.jpg',
      generos: [2],
      enCines: true,
      proximosEstrenos: true,
    },
    {
      titulo: 'La vida es bella',
      director: 'Roberto Benigni',
      fechaLanzamiento: new Date('1997'),
      presupuesto: 20000000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/aZ7MFlKPfB02Lr9NwZQ4vsYRgcy.jpg',
      generos: [1],
      enCines: false,
      proximosEstrenos: true,
    },
    {
      titulo: 'Inception',
      director: 'Christopher Nolan',
      fechaLanzamiento: new Date('2010 '),
      presupuesto: 160000000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/tXQvtRWfkUUnWJAn2tN3jERIUG.jpg',
      generos: [6],
      enCines: true,
      proximosEstrenos: true,
    },
    {
      titulo: 'Shutter Island',
      director: 'Martin Scorsese',
      fechaLanzamiento: new Date('2010'),
      presupuesto: 80000000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/4GDy0PHYX3VRXUtwK5ysFbg3kEx.jpg',
      generos: [6],
      enCines: false,
      proximosEstrenos: false,
    },
    {
      titulo: 'The Wolf of Wall Street',
      director: 'Martin Scorsese',
      fechaLanzamiento: new Date('2013'),
      presupuesto: 100000000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/34m2tygAYBGqA9MXKhRDtzYd4MR.jpg',
      generos: [1],
      enCines: false,
      proximosEstrenos: true,
    },
    {
      titulo: 'The Great Gatsby',
      director: 'Baz Luhrmann',
      fechaLanzamiento: new Date('2013'),
      presupuesto: 105000000.0,
      poster:
        'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/tyxfCBQv6Ap74jcu3xd7aBiaa29.jpg',
      generos: [1],
      enCines: true,
      proximosEstrenos: false,
    },
  ];
  peliculas = this.peliculasOriginal;
}
