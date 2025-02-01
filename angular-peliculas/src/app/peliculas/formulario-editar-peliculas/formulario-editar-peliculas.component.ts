import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import moment from 'moment';
import {
  ActoreAutoCompleteDTO,
  DirectoresAutoCompleteDTO,
} from '../../actores/actores';
import { AutocompleteActoresComponent } from '../../actores/autocomplete-actores/autocomplete-actores.component';
import { AutocompleteDirectoresComponent } from '../../actores/autocomplete-directores/autocomplete-directores.component';
import { InputImgComponent } from '../../compartidos/componentes/input-img/input-img.component';
import { SelectorMultipleComponent } from '../../compartidos/componentes/selector-multiple/selector-multiple.component';
import { SelectorMultipleDTO } from '../../compartidos/componentes/selector-multiple/SelectorMultipleModelo';
import { PeliculaDTO, PeliculaCreacionDTO } from '../peliculas';
import { AutocompleteEditarDirectoresComponent } from "../../actores/autocomplete-editar-directores/autocomplete-editar-directores.component";

@Component({
  selector: 'app-formulario-editar-peliculas',
  standalone: true,
  imports: [
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    RouterLink,
    MatDatepickerModule,
    InputImgComponent,
    SelectorMultipleComponent,
    AutocompleteActoresComponent,
    AutocompleteEditarDirectoresComponent
],
  templateUrl: './formulario-editar-peliculas.component.html',
  styleUrl: './formulario-editar-peliculas.component.css',
})
export class FormularioEditarPeliculasComponent implements OnInit {
  ngOnInit(): void {
    if (this.modelo !== undefined) {
      this.form.patchValue(this.modelo);
    }
  }
  @Input({ required: true })
  generosNoSeleccionados!: SelectorMultipleDTO[];
  @Input({ required: true })
  generosSeleccionados!: SelectorMultipleDTO[];
  @Input({ required: true })
  cinesSeleccionados!: SelectorMultipleDTO[];
  @Input({ required: true })
  cinesNoSeleccionados!: SelectorMultipleDTO[];
  @Input({ required: true })
  actoresSeleccionados!: ActoreAutoCompleteDTO[];
  @Input({ required: true })
  directoresSeleccionados!: DirectoresAutoCompleteDTO[];
  @Input()
  modelo?: PeliculaDTO;
  @Output()
  posteoFormulario = new EventEmitter<PeliculaCreacionDTO>();

  private formBuilder = inject(FormBuilder);
  form = this.formBuilder.group({
    titulo: ['', { validators: [Validators.required] }],
    fechaLanzamiento: new FormControl<Date | null>(null, {
      validators: [Validators.required],
    }),
    sinopsis: '',
    trailer: '',
    poster: new FormControl<File | string | null>(null),
  });

  archivoSeleccionado(file: File) {
    this.form.controls.poster.setValue(file);
  }
  guardarCambios() {
    if (!this.form.valid) {
      return;
    }
    console.log(this.form.value);
    const pelicula = this.form.value as PeliculaCreacionDTO;
    pelicula.fechaLanzamiento = moment(pelicula.fechaLanzamiento).toDate();

    const generosIds = this.generosSeleccionados.map((val) => val.llave);
    pelicula.generosIds = generosIds;

    const cinesIds = this.cinesSeleccionados.map((val) => val.llave);
    pelicula.cinesIds = cinesIds;

    pelicula.actores = this.actoresSeleccionados;
    pelicula.directores = this.directoresSeleccionados.map((director) => ({
      ActorId: director.id, // Asumiendo que director.llave es el ID del actor/director
      PeliculaId: 0, // Si es creación, id será 0 (ajusta según tu lógica)
    }));
    this.posteoFormulario.emit(pelicula);
  }
  obtenerErrorCampoTitulo(): string {
    let campo = this.form.controls.titulo;
    if (campo.hasError('required')) {
      return 'El campo nombre es requerido';
    }
    return '';
  }
  obtenerErrorCampoFechaLanzamiento(): string {
    let campo = this.form.controls.fechaLanzamiento;
    if (campo.hasError('required')) {
      return 'El campo fecha de lanzamiento es requerido';
    }
    return '';
  }
}
