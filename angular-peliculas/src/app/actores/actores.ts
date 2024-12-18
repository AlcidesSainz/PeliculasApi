//DTO para lectura
export interface ActorDTO {
  id: number;
  nombre: string;
  fechaNacimiento: Date;
  foto?: string;
}
//DTO para la creacion
export interface ActorCreacionDTO {
  nombre: string;
  fechaNacimiento: Date;
  foto?: File;
}
//DTO para el autocomplete de los actores
export interface ActoreAutoCompleteDTO {
  id: number;
  nombre: string;
  personaje: string;
  foto: string;
}
