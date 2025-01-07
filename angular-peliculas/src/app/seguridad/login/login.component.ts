import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SeguridadService } from '../seguridad.service';
import { CredencialesUsuarioDTO } from '../seguridad';
import { extrarErroresIdentity } from '../../compartidos/funciones/extraerErrores';
import { FormularioAutenticacionComponent } from '../formulario-autenticacion/formulario-autenticacion.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormularioAutenticacionComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  seguridadService = inject(SeguridadService);
  router = inject(Router);
  errores: string[] = [];

  loguear(credenciales: CredencialesUsuarioDTO) {
    this.seguridadService.login(credenciales).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        const errores = extrarErroresIdentity(err);
        this.errores = errores;
      },
    });
  }
}
