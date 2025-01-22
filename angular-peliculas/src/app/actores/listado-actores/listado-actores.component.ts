import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ActoresService } from '../actores.service';
import { ListadoGenericoComponent } from '../../compartidos/componentes/listado-generico/listado-generico.component';
import { RouterLink } from '@angular/router';
import { AutorizadoComponent } from '../../seguridad/autorizado/autorizado.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-listado-actores',
  standalone: true,
  imports: [
    ListadoGenericoComponent,
    RouterLink,
    AutorizadoComponent,
    SweetAlert2Module,
    MatButtonModule,
    MatIconModule,
    CommonModule
  ],
  templateUrl: './listado-actores.component.html',
  styleUrl: './listado-actores.component.css',
})
export class ListadoActoresComponent {
  actoresService = inject(ActoresService);
  @Input({ required: true })
  actores!: any[];

  @Output()
  borrado = new EventEmitter<void>();

  currentIndex = 0; // Índice actual del carrusel
  maxIndex = 5;
  cardWidth = 240; // Ancho de cada tarjeta (200px + márgenes)

  borrar(id: number) {
    this.actoresService.borrar(id).subscribe(() => {
      this.borrado.emit();
    });
  }

  next() {
    if (this.currentIndex < this.maxIndex) {
      this.currentIndex++;
    } else {
      this.currentIndex = 0
    }
  }

  prev() {
    if (this.currentIndex > 0) {
      this.currentIndex--;
    } else {
      this.currentIndex = this.actores.length-5
    }
  }

}

