import { Component, Input, numberAttribute } from '@angular/core';
import { CineDTO } from '../cines';
import { FormularioCinesComponent } from '../formulario-cines/formulario-cines.component';

@Component({
  selector: 'app-editar-cine',
  standalone: true,
  imports: [FormularioCinesComponent],
  templateUrl: './editar-cine.component.html',
  styleUrl: './editar-cine.component.css',
})
export class EditarCineComponent {
  @Input({ transform: numberAttribute })
  id!: number;

  cine: CineDTO = {
    id: 1,
    nombre: 'Cinemax',
    latitud: -0.1745559665308978,
    longitud: -78.49288823311194,
  };

  guardarCambios() {
    console.log('editando cine', this.cine);
  }
}
