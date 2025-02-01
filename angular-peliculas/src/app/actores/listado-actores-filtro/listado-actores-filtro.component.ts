import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ActoresService } from '../actores.service';
import { ListadoGenericoComponent } from "../../compartidos/componentes/listado-generico/listado-generico.component";
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { AutorizadoComponent } from '../../seguridad/autorizado/autorizado.component';

@Component({
  selector: 'app-listado-actores-filtro',
  standalone: true,
  imports: [
    ListadoGenericoComponent,
    MatButtonModule,
    MatIconModule,
    RouterLink,
    SweetAlert2Module,
    AutorizadoComponent,
  ],
  templateUrl: './listado-actores-filtro.component.html',
  styleUrl: './listado-actores-filtro.component.css',
})
export class ListadoActoresFiltroComponent {
  actoresService = inject(ActoresService);
  @Input({ required: true })
  actores!: any[];

  @Output()
  borrado = new EventEmitter<void>();

  borrar(id: number) {
    this.actoresService.borrar(id).subscribe(() => {
      this.borrado.emit();
    });
  }
}
