import { PeliculaDTO } from "../peliculas/peliculas";

//DTO para lectura
export interface ActorDTO {
  id: number;
  nombre: string;
  biografia: string;
  fechaNacimiento: Date;
  foto?: string;
  votoUsuario: number;
  votoPromedio: number;
  primeraPelicula: string;
  ultimaPelicula: string;
  primeraPeliculaId: number;
  ultimaPeliculaId: number;
  peliculas: PeliculaDTO[];
}
//DTO para la creacion
export interface ActorCreacionDTO {
  nombre: string;
  biografia:string;
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
export interface DirectoresAutoCompleteDTO {
  id: number;
  nombre: string;
  foto: string;
}
export interface LandingPageActoresDTO {
  enTendencia: ActorDTO[];
}
