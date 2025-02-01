import { ActorDTO, ActoreAutoCompleteDTO, DirectoresAutoCompleteDTO } from '../actores/actores';
import { CineDTO } from '../cines/cines';
import { GeneroDTO } from '../generos/generos';

export interface PeliculaDTO {
  id: number;
  titulo: string;
  fechaLanzamiento: Date;
  trailer: string;
  poster?: string;
  sinopsis: string;
  generos: GeneroDTO[];
  cines: CineDTO[];
  actores: ActoreAutoCompleteDTO[];
  directores: PeliculaDirectorResponseDTO[];
  votoUsuario: number;
  votoPromedio: number;
}
export interface PeliculaDirectorResponseDTO {
  actorId: number;
  peliculaId: number;
  actor: ActorDTO; // Contiene el nombre del director (que es un actor)
}
export interface PeliculaCreacionDTO {
  titulo: string;
  fechaLanzamiento: Date;
  trailer: string;
  poster?: File;
  sinopsis: string;
  generosIds: number[];
  cinesIds: number[];
  actores?: ActoreAutoCompleteDTO[];
  directores?: { ActorId: number; PeliculaId: number }[];
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

export interface PeliculaPutGetDTO {
  pelicula: PeliculaDTO;
  generosSeleccionados: GeneroDTO[];
  generosNoSeleccionados: GeneroDTO[];
  cinesSeleccionados: CineDTO[];
  cinesNoSeleccionados: CineDTO[];
  actores: ActoreAutoCompleteDTO[];
  directores: DirectoresAutoCompleteDTO[];
}
