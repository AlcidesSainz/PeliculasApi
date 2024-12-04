import { Routes } from '@angular/router';
import { LangingPageComponent } from './langing-page/langing-page.component';
import { IndiceGenerosComponent } from './generos/indice-generos/indice-generos.component';
import { CrearGenerosComponent } from './generos/crear-generos/crear-generos.component';
import { IndiceActoresComponent } from './actores/indice-actores/indice-actores.component';
import { CrearActorComponent } from './actores/crear-actor/crear-actor.component';
import { IndiceCinesComponent } from './cines/indice-cines/indice-cines.component';
import { CrearCineComponent } from './cines/crear-cine/crear-cine.component';
import { CrearPeliculasComponent } from './peliculas/crear-peliculas/crear-peliculas.component';
import { EditarGeneroComponent } from './generos/editar-genero/editar-genero.component';
import { EditarActorComponent } from './actores/editar-actor/editar-actor.component';
import { EditarPeliculaComponent } from './peliculas/editar-pelicula/editar-pelicula.component';
import { EditarCineComponent } from './cines/editar-cine/editar-cine.component';
import { FiltroPeliculasComponent } from './peliculas/filtro-peliculas/filtro-peliculas.component';

export const routes: Routes = [
  { path: '', component: LangingPageComponent },

  { path: 'generos', component: IndiceGenerosComponent },
  { path: 'generos/crear', component: CrearGenerosComponent },
  { path: 'generos/editar/:id', component: EditarGeneroComponent },

  { path: 'actores', component: IndiceActoresComponent },
  { path: 'actores/crear', component: CrearActorComponent },
  { path: 'actores/editar/:id', component: EditarActorComponent },

  { path: 'cines', component: IndiceCinesComponent },
  { path: 'cines/crear', component: CrearCineComponent },
  { path: 'cines/editar/:id', component: EditarCineComponent },

  { path: 'peliculas/crear', component: CrearPeliculasComponent },
  { path: 'peliculas/editar/:id', component: EditarPeliculaComponent },

  { path: 'peliculas/filtrar', component: FiltroPeliculasComponent },

  //Este siempre debe de ir al final ya que es usado para cuando no se encuentra ninguna ruta de las que ya estan configuradas
  { path: '**', redirectTo: '' },
];
