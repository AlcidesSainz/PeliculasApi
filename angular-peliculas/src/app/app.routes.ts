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
import { DetallePeliculaComponent } from './peliculas/detalle-pelicula/detalle-pelicula.component';
import { esAdminGuard } from './compartidos/guards/es-admin.guard';
import { LoginComponent } from './seguridad/login/login.component';
import { RegistroComponent } from './seguridad/registro/registro.component';

export const routes: Routes = [
  { path: '', component: LangingPageComponent },
  {
    path: 'generos',
    component: IndiceGenerosComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'generos/crear',
    component: CrearGenerosComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'generos/editar/:id',
    component: EditarGeneroComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'actores',
    component: IndiceActoresComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'actores/crear',
    component: CrearActorComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'actores/editar/:id',
    component: EditarActorComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'cines',
    component: IndiceCinesComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'cines/crear',
    component: CrearCineComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'cines/editar/:id',
    component: EditarCineComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'peliculas/crear',
    component: CrearPeliculasComponent,
    canActivate: [esAdminGuard],
  },
  {
    path: 'peliculas/editar/:id',
    component: EditarPeliculaComponent,
    canActivate: [esAdminGuard],
  },

  { path: 'peliculas/filtrar', component: FiltroPeliculasComponent },
  { path: 'peliculas/:id', component: DetallePeliculaComponent },

  { path: 'login', component: LoginComponent },
  { path: 'registrar', component: RegistroComponent },
  //Este siempre debe de ir al final ya que es usado para cuando no se encuentra ninguna ruta de las que ya estan configuradas
  { path: '**', redirectTo: '' },
];
