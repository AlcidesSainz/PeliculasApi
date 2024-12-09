import {
  Component,
  ComponentRef,
  inject,
  Input,
  OnInit,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { IServicioCRUD } from '../../interfaces/IServicioCrud';
import { Router } from '@angular/router';
import { extraerErrores } from '../../funciones/extraerErrores';
import { SERVICIO_CRUD_TOKEN } from '../../proveedores/proveedores';
import { MostrarErroresComponent } from '../mostrar-errores/mostrar-errores.component';

@Component({
  selector: 'app-editar-entidad',
  standalone: true,
  imports: [MostrarErroresComponent],
  templateUrl: './editar-entidad.component.html',
  styleUrl: './editar-entidad.component.css',
})
export class EditarEntidadComponent<TDTO, TCreacionDTO> implements OnInit {
  ngOnInit(): void {
    this.servicioCrud.obtenerPorId(this.id).subscribe((entidad) => {
      this.cargarComponente(entidad);
    });
  }
  cargarComponente(entidad: any) {
    if (this.contenedorFormulario) {
      this.componentRef = this.contenedorFormulario.createComponent(
        this.formulario
      );
      this.componentRef.instance.modelo = entidad;
      this.componentRef.instance.posteoFormulario.subscribe((entidad: any) => {
        this.guardarCambios(entidad);
      });
      this.cargando = false;
    }
  }
  @Input()
  id!: number;

  @Input({ required: true })
  titulo!: string;

  @Input({ required: true })
  rutaCancelar!: string;

  @Input({ required: true })
  formulario: any;

  errores: string[] = [];
  cargando = true;
  servicioCrud = inject(SERVICIO_CRUD_TOKEN) as IServicioCRUD<
    TDTO,
    TCreacionDTO
  >;

  @ViewChild('contenedorFormulario', { read: ViewContainerRef })
  contenedorFormulario!: ViewContainerRef;
  private router = inject(Router);

  private componentRef!: ComponentRef<any>;
  guardarCambios(entidad: TCreacionDTO) {
    this.servicioCrud.actualizar(this.id, entidad).subscribe({
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
