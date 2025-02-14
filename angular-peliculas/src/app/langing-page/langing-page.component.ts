import { Component, inject, model, OnInit } from '@angular/core';
import { ListadoPeliculasComponent } from '../peliculas/listado-peliculas/listado-peliculas.component';
import { PeliculasService } from '../peliculas/peliculas.service';
import { CarruselComponent } from '../compartidos/componentes/carrusel/carrusel.component';
import { ActoresService } from '../actores/actores.service';
import { ListadoActoresComponent } from "../actores/listado-actores/listado-actores.component";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-langing-page',
  standalone: true,
  imports: [ListadoPeliculasComponent,  CarruselComponent, ListadoActoresComponent,RouterLink],
  templateUrl: './langing-page.component.html',
  styleUrl: './langing-page.component.css',
})
export class LangingPageComponent {
  peliculaService = inject(PeliculasService);
  peliculasEnCines!: any[];
  peliculasProximosEstrenos!: any[];
  peliculas!: any[];

  actoresService = inject(ActoresService);
  actoresTendencia!: any[];
  constructor() {
    this.cargarPeliculas();
    this.actoresService.obtenerLandingPage().subscribe((modelo) => {
      this.actoresTendencia = this.randomizeArray(modelo.enTendencia);
    });
  }
  randomizeArray(enTendencia: import("../actores/actores").ActorDTO[]): any[] {
    return enTendencia.sort(() => Math.random() - 0.5);
  }

  peliculasBorrado() {
    this.cargarPeliculas();
  }

  cargarPeliculas() {
    this.peliculaService.obtenerLandingPage().subscribe((modelo) => {
      this.peliculasEnCines = modelo.enCines;
      this.peliculasProximosEstrenos = modelo.proximosEstrenos;
      this.peliculas = modelo.todasPeliculas;
    });
  }
}
