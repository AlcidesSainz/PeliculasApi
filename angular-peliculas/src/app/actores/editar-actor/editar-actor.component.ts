import {
  Component,
  inject,
  Input,
  numberAttribute,
  OnInit,
} from '@angular/core';
import { transform } from 'typescript';
import { ActorCreacionDTO, ActorDTO } from '../actores';
import { FormularioActoresComponent } from '../formulario-actores/formulario-actores.component';
import { ActoresService } from '../actores.service';
import { Router } from '@angular/router';
import { extraerErrores } from '../../compartidos/funciones/extraerErrores';
import { MostrarErroresComponent } from '../../compartidos/componentes/mostrar-errores/mostrar-errores.component';

@Component({
  selector: 'app-editar-actor',
  standalone: true,
  imports: [FormularioActoresComponent, MostrarErroresComponent],
  templateUrl: './editar-actor.component.html',
  styleUrl: './editar-actor.component.css',
})
export class EditarActorComponent implements OnInit {
  ngOnInit(): void {
    this.actorService.obtenerPorId(this.id).subscribe((actor) => {
      this.actor = actor;
    });
  }
  @Input({ transform: numberAttribute })
  id!: number;
  actor?: ActorDTO;
  actorService = inject(ActoresService);
  errores: string[] = [];
  router = inject(Router);

  guardarCambios(actor: ActorCreacionDTO) {
    this.actorService.actualizar(this.id, actor).subscribe({
      next: () => {
        this.router.navigate(['/actores']);
      },
      error: (err) => {
        const errores = extraerErrores(err);
        this.errores = errores;
      },
    });
  }
}
