import { Component, inject } from '@angular/core';
import { PeliculasService } from '../../../peliculas/peliculas.service';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-carrusel',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './carrusel.component.html',
  styleUrl: './carrusel.component.css',
})
export class CarruselComponent {
  peliculaService = inject(PeliculasService);

  peliculasProximosEstrenos!: any[];

  constructor(private sanitizer: DomSanitizer) {
    this.cargarPeliculas();
  }

  peliculasBorrado() {
    this.cargarPeliculas();
  }

  cargarPeliculas() {
    this.peliculaService.obtenerLandingPage().subscribe((modelo) => {
      this.peliculasProximosEstrenos = modelo.proximosEstrenos.map(
        (pelicula: any) => {
          return {
            id: pelicula.id,
            titulo: pelicula.titulo,
            poster: pelicula.poster,
            trailer: pelicula.trailer,
          };
        }
      );
      console.log(this.peliculasProximosEstrenos); // Verifica los datos aquí
    });
  }
  sanitizarUrl(url: string): SafeResourceUrl {
    const sanitizedUrl = url.includes('?')
      ? `${url}&rel=0&showinfo=0`
      : `${url}?rel=0&showinfo=0`;
    if (sanitizedUrl.includes('watch?v=')) {
      url = url.replace('watch?v=', 'embed/');
    }

    // Agregar parámetros personalizados
    url += '?controls=1&rel=0&autohide=1&modestbranding=1';

    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
}
