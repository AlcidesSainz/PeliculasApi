import { Component, inject } from '@angular/core';
import { ActoresService } from '../actores.service';

@Component({
  selector: 'app-listado-actores',
  standalone: true,
  imports: [],
  templateUrl: './listado-actores.component.html',
  styleUrl: './listado-actores.component.css'
})
export class ListadoActoresComponent {

  actoresService = inject(ActoresService);
  
}
