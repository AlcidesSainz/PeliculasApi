import { Component, Input, numberAttribute } from '@angular/core';
import { transform } from 'typescript';
import { ActorCreacionDTO, ActorDTO } from '../actores';
import { FormularioActoresComponent } from "../formulario-actores/formulario-actores.component";

@Component({
  selector: 'app-editar-actor',
  standalone: true,
  imports: [FormularioActoresComponent],
  templateUrl: './editar-actor.component.html',
  styleUrl: './editar-actor.component.css',
})
export class EditarActorComponent {
  @Input({ transform: numberAttribute })
  id!: number;

  actor: ActorDTO = {
    id: 1,
    nombre: 'TOm Holland',
    fechaNacimiento: new Date('1991-01-25'),
    foto: 'https://upload.wikimedia.org/wikipedia/commons/thumb/3/36/Tom_Holland_at_KCA_2022.jpg/330px-Tom_Holland_at_KCA_2022.jpg',
  };
  guardarCambios(actor: ActorCreacionDTO) {
    console.log('Editando el actor', actor);
  }
}
