import { Component, EventEmitter, inject, Input, numberAttribute, OnInit, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, Router } from '@angular/router';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import Swal from 'sweetalert2';
import { CargandoComponent } from '../../compartidos/componentes/cargando/cargando.component';
import { RatingComponent } from '../../compartidos/componentes/rating/rating.component';
import { PeliculaDTO } from '../../peliculas/peliculas';
import { FechaEspanolPipe } from '../../pipes/fecha-espanol.pipe';
import { RatingService } from '../../rating/rating.service';
import { AutorizadoComponent } from '../../seguridad/autorizado/autorizado.component';
import { SeguridadService } from '../../seguridad/seguridad.service';
import { ActoresService } from '../actores.service';
import { ActorDTO } from '../actores';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-detalle-actor',
  standalone: true,
  imports: [
    CargandoComponent,
    MatChipsModule,
    RouterLink,
    RatingComponent,
    AutorizadoComponent,
    SweetAlert2Module,
    MatButtonModule,
    FechaEspanolPipe,
    MatIconModule,
    CommonModule,
  ],
  templateUrl: './detalle-actor.component.html',
  styleUrl: './detalle-actor.component.css',
  providers: [DatePipe],
})
export class DetalleActorComponent implements OnInit {
  @Input({ transform: numberAttribute })
  id!: number;
  ratingService = inject(RatingService);
  actoresService = inject(ActoresService);
  seguridadService = inject(SeguridadService);
  actor!: ActorDTO;
  peliculas: PeliculaDTO[] = [];

  ngOnInit(): void {
    this.actoresService.obtenerPorId(this.id).subscribe((actor) => {
      actor.fechaNacimiento = new Date(actor.fechaNacimiento);
      this.actor = actor;
    });
    // Obtener películas del actor
    this.actoresService
      .obtenerPeliculasPorActor(this.id)
      .subscribe((peliculas) => {
        this.peliculas = peliculas;
      });
  }

  puntuar(puntuacion: number) {
    if (!this.seguridadService.estaLogueado()) {
      Swal.fire('Error', 'Debes iniciar sesión para puntuar', 'error');
      return;
    }
    this.ratingService.puntuarActor(this.id, puntuacion).subscribe(() => {
      Swal.fire('Éxito', 'Gracias por tu puntuación', 'success');
    });
  }

  @Output()
  borrado = new EventEmitter<void>();
  router = inject(Router);
  borrar(id: number) {
    this.actoresService.borrar(id).subscribe(() => {
      this.borrado.emit();
      this.router.navigate(['/']);
    });
  }

  currentIndex = 0; // Índice actual del carrusel
  maxIndex = 5;

  next() {
    if (this.currentIndex < this.maxIndex) {
      this.currentIndex++;
    } else {
      this.currentIndex = 0;
    }
  }

  prev() {
    if (this.currentIndex > 0) {
      this.currentIndex--;
    } else {
      this.currentIndex = this.peliculas.length - 5;
    }
  }
}
