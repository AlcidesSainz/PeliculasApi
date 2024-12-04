import { HttpParams } from '@angular/common/http';

export function construirQueryParams(obj: any): HttpParams {
  let queryParams = new HttpParams();

  for (let propiedadd in obj) {
    if (obj.hasOwnProperty(propiedadd)) {
      queryParams = queryParams.append(propiedadd, obj[propiedadd]);
    }
  }
  return queryParams;
}
