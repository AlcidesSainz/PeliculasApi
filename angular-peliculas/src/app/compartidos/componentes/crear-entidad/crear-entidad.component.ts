import {
  AfterViewInit,
  Component,
  ComponentRef,
  inject,
  Input,
  ViewChild,
  viewChild,
  ViewContainerRef,
} from '@angular/core';
import { SERVICIO_CRUD_TOKEN } from '../../proveedores/proveedores';
import { IServicioCRUD } from '../../interfaces/IServicioCrud';
import { Router } from '@angular/router';
import { extraerErrores } from '../../funciones/extraerErrores';
import { MostrarErroresComponent } from '../mostrar-errores/mostrar-errores.component';

@Component({
  selector: 'app-crear-entidad',
  standalone: true,
  imports: [MostrarErroresComponent],
  templateUrl: './crear-entidad.component.html',
  styleUrl: './crear-entidad.component.css',
})
export class CrearEntidadComponent<TDTO, TCreacionDTO>
  implements AfterViewInit
{
  ngAfterViewInit(): void {
    this.componentRef = this.contenedorFormulario.createComponent(
      this.formulario
    );
    this.componentRef.instance.posteoFormulario.subscribe((entidad: any) => {
      this.guardarCambios(entidad);
    });
  }
  @Input({ required: true })
  titulo!: string;

  @Input({ required: true })
  rutaCancelar!: string;

  @Input({ required: true })
  formulario: any;

  errores: string[] = [];

  servicioCrud = inject(SERVICIO_CRUD_TOKEN) as IServicioCRUD<
    TDTO,
    TCreacionDTO
  >;

  @ViewChild('contenedorFormulario', { read: ViewContainerRef })
  contenedorFormulario!: ViewContainerRef;
  private router = inject(Router);

  private componentRef!: ComponentRef<any>;
  guardarCambios(entidad: TCreacionDTO) {
    this.servicioCrud.crear(entidad).subscribe({
      next: () => {
        this.router.navigate([this.rutaCancelar]);
      },
      error: (err) => {
        const errores = extraerErrores(err);
        this.errores = errores;
      },
    });
  }
}
