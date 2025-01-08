import {
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { SeguridadService } from './seguridad.service';
import { inject } from '@angular/core';

export const authInterceptor: HttpInterceptorFn = (
  requ: HttpRequest<any>,
  next: HttpHandlerFn
) => {
  const seguridadService = inject(SeguridadService);
  const token = seguridadService.obtenerToken();

  if (token) {
    requ = requ.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
  return next(requ);
};
