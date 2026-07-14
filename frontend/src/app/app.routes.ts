import { Routes } from '@angular/router';

import { authGuard } from './core/auth/auth.guard';
import { LoginComponent } from './features/login/login.component';
import { ShellComponent } from './layout/shell.component';
import { ClienteListComponent } from './features/clientes/cliente-list.component';
import { ClienteFormComponent } from './features/clientes/cliente-form.component';
import { CategoriaListComponent } from './features/categorias/categoria-list.component';
import { CategoriaFormComponent } from './features/categorias/categoria-form.component';
import { PersonalListComponent } from './features/personal/personal-list.component';
import { PersonalFormComponent } from './features/personal/personal-form.component';
import { ZonaListComponent } from './features/zonas-desfavorables/zona-list.component';
import { ZonaFormComponent } from './features/zonas-desfavorables/zona-form.component';
import { ParametrosFormComponent } from './features/parametros/parametros-form.component';
import { NovedadesComponent } from './features/novedades/novedades.component';
import { ResumenComponent } from './features/resumen/resumen.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: ShellComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'clientes', pathMatch: 'full' },
      { path: 'clientes', component: ClienteListComponent },
      { path: 'clientes/nuevo', component: ClienteFormComponent },
      { path: 'clientes/:id/editar', component: ClienteFormComponent },
      { path: 'clientes/:clienteId/personal', component: PersonalListComponent },
      { path: 'clientes/:clienteId/personal/nuevo', component: PersonalFormComponent },
      { path: 'clientes/:clienteId/personal/:id/editar', component: PersonalFormComponent },
      { path: 'categorias', component: CategoriaListComponent },
      { path: 'categorias/nuevo', component: CategoriaFormComponent },
      { path: 'categorias/:id/editar', component: CategoriaFormComponent },
      { path: 'zonas-desfavorables', component: ZonaListComponent },
      { path: 'zonas-desfavorables/nuevo', component: ZonaFormComponent },
      { path: 'zonas-desfavorables/:id/editar', component: ZonaFormComponent },
      { path: 'parametros', component: ParametrosFormComponent },
      { path: 'novedades', component: NovedadesComponent },
      { path: 'resumen', component: ResumenComponent }
    ]
  }
];
