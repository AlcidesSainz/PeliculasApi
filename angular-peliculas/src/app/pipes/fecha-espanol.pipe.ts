import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'fechaEspanol',
  standalone: true,
})
export class FechaEspanolPipe implements PipeTransform {
  transform(value: Date | string): string {
    if (!value) return '';

    const fecha = new Date(value);
    const opciones: Intl.DateTimeFormatOptions = {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    };

    return new Intl.DateTimeFormat('es-ES', opciones).format(fecha);
  }
}
