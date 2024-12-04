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
