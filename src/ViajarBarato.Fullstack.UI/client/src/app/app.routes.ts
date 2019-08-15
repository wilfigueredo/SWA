import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { ChallengeComponent } from './challenge/challenge.component';
import { AcessoNegadoComponent } from './shared/acesso-negado/acesso-negado.component';
import { RegistrarComponent } from "./usuario/registrar/registrar.component";
import { LoginComponent } from './usuario/login/login.component';
import { NaoEncontradoComponent } from './shared/nao-encontrado/nao-encontrado.component';

export const rootRouterConfig: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    { path: 'challenge', component: ChallengeComponent },
    { path: 'registrar', component: RegistrarComponent },
    { path: 'entrar', component: LoginComponent },
    { path: 'acesso-negado', component: AcessoNegadoComponent },
    {path: 'nao-encontrado', component: NaoEncontradoComponent },    
];