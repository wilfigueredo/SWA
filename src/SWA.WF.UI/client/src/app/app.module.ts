import { BrowserModule, Title } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { rootRouterConfig } from './app.routes';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
registerLocaleData(localePt);

// 3s components
import { ToastModule, ToastOptions } from 'ng2-toastr/ng2-toastr';
import { CustomFormsModule } from 'ng2-validation'

// bootstrap
import { AlertModule } from 'ngx-bootstrap';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { NgbPaginationModule, NgbAlertModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

// shared
import { AcessoNegadoComponent } from './shared/acesso-negado/acesso-negado.component';
import { NaoEncontradoComponent } from './shared/nao-encontrado/nao-encontrado.component';

// components
import { HomeComponent } from './home/home.component';
import { RegistrarComponent } from './usuario/registrar/registrar.component';
import { LoginComponent } from './usuario/login/login.component';
import { AppComponent } from './app.component';
import { ChallengeComponent } from './challenge/challenge.component';

// services
import { UsuarioService } from "./services/Usuario.service";
import { ErrorInterceptor } from './services/error.handler.service';
import { ChallengeService } from "./challenge/service/challenge.service";

// modules
import { SharedModule } from "./shared/shared.module";


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    RegistrarComponent,
    LoginComponent,
    AcessoNegadoComponent,
    NaoEncontradoComponent,
    ChallengeComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    NgbModule.forRoot(),
    NgbPaginationModule,
    NgbAlertModule,
    NgbTypeaheadModule.forRoot(),
    HttpClientModule,    
    SharedModule,
    ReactiveFormsModule,
    ToastModule.forRoot(),
    AlertModule.forRoot(),
    CollapseModule.forRoot(),    
    RouterModule.forRoot(rootRouterConfig, { useHash: false })
  ],
  providers: [
    Title,   
    UsuarioService,
    {
        provide: HTTP_INTERCEPTORS,
        useClass: ErrorInterceptor,
        multi: true
      },
      ChallengeService,
    {
        provide: HTTP_INTERCEPTORS,
        useClass: ErrorInterceptor,
        multi: true
      }    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

