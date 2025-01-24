import {
  Component,
  EventEmitter,
  inject,
  Input,
  numberAttribute,
  OnInit,
  Output,
} from '@angular/core';
import { PeliculaDTO } from '../peliculas';
import { PeliculasService } from '../peliculas.service';
import { CargandoComponent } from '../../compartidos/componentes/cargando/cargando.component';
import { MatChipsModule } from '@angular/material/chips';
import { Router, RouterLink } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Coordenada } from '../../compartidos/componentes/mapa/Coordenada';
import { MapaComponent } from '../../compartidos/componentes/mapa/mapa.component';
import { RatingService } from '../../rating/rating.service';
import { SeguridadService } from '../../seguridad/seguridad.service';
import Swal from 'sweetalert2';
import { RatingComponent } from "../../compartidos/componentes/rating/rating.component";
import { AutorizadoComponent } from "../../seguridad/autorizado/autorizado.component";
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { MatButtonModule } from '@angular/material/button';
import { FechaEspanolPipe } from '../../pipes/fecha-espanol.pipe';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-detalle-pelicula',
  standalone: true,
  imports: [
    CargandoComponent,
    MatChipsModule,
    RouterLink,
    MapaComponent,
    RatingComponent,
    AutorizadoComponent,
    SweetAlert2Module,
    MatButtonModule,
    FechaEspanolPipe,
    MatIconModule,
  ],
  templateUrl: './detalle-pelicula.component.html',
  styleUrl: './detalle-pelicula.component.css',
})
export class DetallePeliculaComponent implements OnInit {
  @Input({ transform: numberAttribute })
  id!: number;
  ratingService = inject(RatingService);
  peliculasService = inject(PeliculasService);
  seguridadService = inject(SeguridadService);
  pelicula!: PeliculaDTO;
  sanitizer = inject(DomSanitizer);
  trailerURL!: SafeResourceUrl;
  coordenadas: Coordenada[] = [];
  peliculaDescargarNombre!:string

  ngOnInit(): void {
    this.peliculasService.obtenerPorId(this.id).subscribe((pelicula) => {
      pelicula.fechaLanzamiento = new Date(pelicula.fechaLanzamiento);
      this.pelicula = pelicula;
      this.trailerURL = this.generarURLYoutubeEmbed(pelicula.trailer);
      this.peliculaDescargarNombre = `${this.pelicula.titulo
        .replace(/[:!?,.()]/g, '') // Elimina caracteres especiales, incluidos los paréntesis
        .replace(/ /g, '-') // Reemplaza los espacios con guiones
        .toLowerCase()}-${this.pelicula.fechaLanzamiento.getFullYear()}`;

      this.coordenadas = pelicula.cines!.map((cine) => {
        return <Coordenada>{
          latitud: cine.latitud,
          longitud: cine.longitud,
          text: cine.nombre,
        };
      });
    });
  }

  generarURLYoutubeEmbed(url: string): SafeResourceUrl | string {
    if (!url) {
      return '';
    }
    var videoId = url.split('v=')[1];
    var posicionAmpersand = videoId.indexOf('&');
    if (posicionAmpersand !== -1) {
      videoId = videoId.substring(0, posicionAmpersand);
    }
    return this.sanitizer.bypassSecurityTrustResourceUrl(
      `https://www.youtube.com/embed/${videoId}`
    );
  }

  puntuar(puntuacion: number) {
    if (!this.seguridadService.estaLogueado()) {
      Swal.fire('Error', 'Debes iniciar sesión para puntuar', 'error');
      return;
    }
    this.ratingService.puntuar(this.id, puntuacion).subscribe(() => {
      Swal.fire('Éxito', 'Gracias por tu puntuación', 'success');
    });
  }

  @Output()
  borrado = new EventEmitter<void>();
  router = inject(Router);
  borrar(id: number) {
    this.peliculasService.borrar(id).subscribe(() => {
      this.borrado.emit();
      this.router.navigate(['/']);
    });
  }
}
