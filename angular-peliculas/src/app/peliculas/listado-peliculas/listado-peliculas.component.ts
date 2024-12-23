import { CurrencyPipe, DatePipe,UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { ListadoGenericoComponent } from "../../compartidos/componentes/listado-generico/listado-generico.component";
import {MatButtonModule} from '@angular/material/button'
import { MatIconModule } from '@angular/material/icon';
import { RatingComponent } from "../../compartidos/componentes/rating/rating.component";
import { RouterLink } from '@angular/router';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { PeliculasService } from '../peliculas.service';
@Component({
  selector: 'app-listado-peliculas',
  standalone: true,
  imports: [DatePipe, UpperCasePipe, CurrencyPipe, ListadoGenericoComponent, MatButtonModule, MatIconModule, RouterLink,SweetAlert2Module],
  templateUrl: './listado-peliculas.component.html',
  styleUrl: './listado-peliculas.component.css',
})
export class ListadoPeliculasComponent  {

  peliculasService = inject(PeliculasService);

  @Input({required:true})
  //Arreglo de peliculas
  peliculas!: any[];

  @Output()
  borrado = new EventEmitter<void>();

  borrar(id: number) {
    this.peliculasService.borrar(id).subscribe(() => {
      this.borrado.emit();
    });
  }



}
