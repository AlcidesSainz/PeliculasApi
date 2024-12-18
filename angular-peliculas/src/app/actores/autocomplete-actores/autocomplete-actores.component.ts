import { Component, inject, Input, OnInit, ViewChild } from '@angular/core';
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
import { ActoresService } from '../actores.service';

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
export class AutocompleteActoresComponent implements OnInit {
  ngOnInit(): void {
    this.control.valueChanges.subscribe(
      valor=>{
        if (typeof valor === 'string' && valor) {
          this.actorService.obtenerPorNombre(valor).subscribe((actores) => {
            this.actores = actores
          })
        } 
      }
    )
  }
  control = new FormControl();

  actores: ActoreAutoCompleteDTO[] = [];

  actorService = inject(ActoresService)
  
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
