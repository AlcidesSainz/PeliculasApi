import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function primeraLetraMayuscula(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const valor = <string>control.value;
    if (!valor) return null;
    if (valor.length == 0) return null;
    const primeraLetra = valor[0];

    if (primeraLetra != primeraLetra.toLocaleUpperCase()) {
      return {
        primeraLetraMayuscula: {
          mensaje: 'La primera letra debe de ser mayuscula',
        },
      };
    }
    return null;
  };
}

export function fechaNoPuedeSerFutura(): ValidatorFn{
  return (control: AbstractControl): ValidationErrors | null => {
    const fechaEscogidaPorElUsuario = new Date(control.value)
    const fechaActual = new Date()

    if (fechaEscogidaPorElUsuario > fechaActual) {
      return {
        futuro: {
          mensaje: 'La fecha no puede ser del futuro'
        }
      };
    }
    return null;
  }
}
