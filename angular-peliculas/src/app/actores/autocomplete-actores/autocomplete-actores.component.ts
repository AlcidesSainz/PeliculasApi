import { Component, Input, ViewChild } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTable, MatTableModule } from '@angular/material/table';
import { ActoreAutoCompleteDTO } from '../actores';
import {
  CdkDragDrop,
  DragDropModule,
  moveItemInArray,
} from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-autocomplete-actores',
  standalone: true,
  imports: [
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatIconModule,
    FormsModule,
    MatTableModule,
    MatInputModule,
    DragDropModule,
  ],
  templateUrl: './autocomplete-actores.component.html',
  styleUrl: './autocomplete-actores.component.css',
})
export class AutocompleteActoresComponent {
  control = new FormControl();

  actores: ActoreAutoCompleteDTO[] = [
    {
      id: 1,
      nombre: 'Leonardo DiCaprio',
      personajes: '',
      foto: 'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/wo2hJpn04vbtmh0B9utCFdsQhxM.jpg',
    },
    {
      id: 2,
      nombre: 'Tobey Maguire',
      personajes: '',
      foto: 'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/ncF4HivY2W6SQW5dEP3N3I4mfT0.jpg',
    },
    {
      id: 3,
      nombre: 'Gary Oldman',
      personajes: '',
      foto: 'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/2v9FVVBUrrkW2m3QOcYkuhq9A6o.jpg',
    },
    {
      id: 4,
      nombre: 'Pedro Pascal',
      personajes: '',
      foto: 'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/9VYK7oxcqhjd5LAH6ZFJ3XzOlID.jpg',
    },
    {
      id: 5,
      nombre: 'Ana de Armas',
      personajes: '',
      foto: 'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/3vxvsmYLTf4jnr163SUlBIw51ee.jpg',
    },
    {
      id: 6,
      nombre: 'Robert Pattinson',
      personajes: '',
      foto: 'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/8A4PS5iG7GWEAVFftyqMZKl3qcr.jpg',
    },
    {
      id: 7,
      nombre: 'Margaret Qualley',
      personajes: '',
      foto: 'https://media.themoviedb.org/t/p/w300_and_h450_bestv2/jStNyMj3acpLuH48awLVLqqlyaV.jpg',
    },
  ];

  @Input({required:true})
  actoresSeleccionados: ActoreAutoCompleteDTO[] = [];

  columnasMostrar = ['imagen', 'nombre', 'personaje', 'acciones'];

  @ViewChild(MatTable) table!: MatTable<ActoreAutoCompleteDTO>;

  actorSeleccionado(event: MatAutocompleteSelectedEvent) {
    this.actoresSeleccionados.push(event.option.value);
    this.control.patchValue('');

    if (this.table != undefined) {
      this.table.renderRows();
    }
  }

  finalizarArrastre(event: CdkDragDrop<any[]>) {
    const indicePrevio = this.actoresSeleccionados.findIndex(
      (actor) => actor === event.item.data
    );
    moveItemInArray(
      this.actoresSeleccionados,
      indicePrevio,
      event.currentIndex
    );
    this.table.renderRows();
  }

  eliminar(actor: ActoreAutoCompleteDTO) {
    const indice = this.actoresSeleccionados.findIndex(
      (a: ActoreAutoCompleteDTO) => a.id === actor.id
    );
    this.actoresSeleccionados.splice(indice, 1);
    this.table.renderRows();
  }
}
