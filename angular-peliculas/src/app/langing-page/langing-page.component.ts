import { Component, inject, model, OnInit } from '@angular/core';
import { ListadoPeliculasComponent } from '../peliculas/listado-peliculas/listado-peliculas.component';
import { PeliculasService } from '../peliculas/peliculas.service';
import { AutorizadoComponent } from "../seguridad/autorizado/autorizado.component";

@Component({
  selector: 'app-langing-page',
  standalone: true,
  imports: [ListadoPeliculasComponent, AutorizadoComponent],
  templateUrl: './langing-page.component.html',
  styleUrl: './langing-page.component.css',
})
export class LangingPageComponent {
  peliculaService = inject(PeliculasService);
  peliculasEnCines!: any[];
  peliculasProximosEstrenos!: any[];
  peliculas!: any[];

  constructor() {
    this.cargarPeliculas();
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
