import { ActoreAutoCompleteDTO } from '../actores/actores';
import { CineDTO } from '../cines/cines';
import { GeneroDTO } from '../generos/generos';

export interface PeliculaDTO {
  id: number;
  titulo: string;
  fechaLanzamiento: Date;
  trailer: string;
  poster?: string;
}
export interface PeliculaCreacionDTO {
  titulo: string;
  fechaLanzamiento: Date;
  trailer: string;
  poster?: File;
  generosIds: number[];
  cinesIds: number[];
  actores?: ActoreAutoCompleteDTO[];
}

export interface PeliculasPostGetDTO {
  generos: GeneroDTO[];
  cine: CineDTO[];
}
export interface LandingPagePeliculasDTO {
  enCines: PeliculaDTO[];
  proximosEstrenos: PeliculaDTO[];
  todasPeliculas: PeliculaDTO[];
}

export interface PeliculaPutGetDTO{
  pelicula: PeliculaDTO
  generosSeleccionados: GeneroDTO[]
  generosNoSeleccionados: GeneroDTO[]
  cinesSeleccionados: CineDTO[]
  cinesNoSeleccionados: CineDTO[]
  actores:ActoreAutoCompleteDTO[]
}
