export function extraerErrores(obj: any): string[] {
  const err = obj.error.errors;
  let mensasjeDeError: string[] = [];

  for (let llave in err) {
    let campo = llave;
    const mensajeConCampos = err[llave].map(
      (mensaje: string) => `${campo}:${mensaje}`
    );
    mensasjeDeError = mensasjeDeError.concat(mensajeConCampos);
  }
  return mensasjeDeError;
}
export function extrarErroresIdentity(obj: any): string[]{
  let mensajesDeError: string[] = [];

  for (let i = 0; i < obj.error.length; i++){
    const elementos = obj.error[i]
    mensajesDeError.push(elementos.description)
  }
  return mensajesDeError;
}