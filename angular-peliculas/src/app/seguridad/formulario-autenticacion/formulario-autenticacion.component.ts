import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CredencialesUsuarioDTO } from '../seguridad';
import { MostrarErroresComponent } from '../../compartidos/componentes/mostrar-errores/mostrar-errores.component';
import { RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-formulario-autenticacion',
  standalone: true,
  imports: [MostrarErroresComponent,ReactiveFormsModule,RouterLink,MatFormFieldModule,MatButtonModule,MatInputModule],
  templateUrl: './formulario-autenticacion.component.html',
  styleUrl: './formulario-autenticacion.component.css',
})
export class FormularioAutenticacionComponent {
  private formBuilder = inject(FormBuilder);

  form = this.formBuilder.group({
    email: ['', { validators: [Validators.required, Validators.email] }],
    password: ['', { validators: [Validators.required] }],
  });

  @Input({ required: true })
  titulo!: string;

  @Input({ required: true })
  errores: string[] = [];

  @Output()
  posteoFormulario = new EventEmitter<CredencialesUsuarioDTO>();

  obtenerErrorEmail(): string {
    let campo = this.form.controls.email;

    if (campo.hasError('required')) {
      return 'El email es requerido';
    }
    if (campo.hasError('email')) {
      return 'El email no es válido';
    }
    return '';
  }
  obtenerErrorPassword(): string {
    let campo = this.form.controls.password;

    if (campo.hasError('required')) {
      return 'El email es requerido';
    }
    return '';
  }

  guardarCambios(): void {
    if (!this.form.valid) {
      return;
    }
    const credenciales = this.form.value as CredencialesUsuarioDTO;
    this.posteoFormulario.emit(credenciales);
  }
}
