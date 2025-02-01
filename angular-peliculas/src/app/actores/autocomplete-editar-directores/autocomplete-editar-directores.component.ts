import { DragDropModule, CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, inject, Input, OnInit, ViewChild } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormControl } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule, MatTable } from '@angular/material/table';
import { DirectoresAutoCompleteDTO } from '../actores';
import { ActoresService } from '../actores.service';

@Component({
  selector: 'app-autocomplete-editar-directores',
  standalone: true,
  imports: [MatAutocompleteModule,
      ReactiveFormsModule,
      MatFormFieldModule,
      MatIconModule,
      FormsModule,
      MatTableModule,
      MatInputModule,
      DragDropModule,],
  templateUrl: './autocomplete-editar-directores.component.html',
  styleUrl: './autocomplete-editar-directores.component.css'
})
export class AutocompleteEditarDirectoresComponent implements OnInit {
  ngOnInit(): void {
    this.control.valueChanges.subscribe((valor) => {
      if (typeof valor === 'string' && valor) {
        this.actorService.obtenerPorNombre(valor).subscribe((directores) => {
          this.directores = directores;
        });
      }
    });
  }

  control = new FormControl();

  directores: DirectoresAutoCompleteDTO[] = [];

  actorService = inject(ActoresService);

  @Input({ required: true })
  directoresSeleccionados: DirectoresAutoCompleteDTO[] = [];

  columnasMostrar = ['imagen', 'nombre', 'acciones'];

  @ViewChild(MatTable) table!: MatTable<DirectoresAutoCompleteDTO>; // Usar DirectoresAutoCompleteDTO

  directorSeleccionado(event: MatAutocompleteSelectedEvent) {
    const directorSeleccionado: DirectoresAutoCompleteDTO = event.option.value;
    this.directoresSeleccionados.push(directorSeleccionado);
    this.control.patchValue('');

    if (this.table != undefined) {
      this.table.renderRows();
    }
  }

  finalizarArrastre(event: CdkDragDrop<any[]>) {
    const indicePrevio = this.directoresSeleccionados.findIndex(
      (director) => director === event.item.data
    );
    moveItemInArray(
      this.directoresSeleccionados,
      indicePrevio,
      event.currentIndex
    );
    this.table.renderRows();
  }

  eliminar(director: DirectoresAutoCompleteDTO) {
    const indice = this.directoresSeleccionados.findIndex(
      (d: DirectoresAutoCompleteDTO) => d.id === director.id
    );
    this.directoresSeleccionados.splice(indice, 1);
    this.table.renderRows();
  }
}
