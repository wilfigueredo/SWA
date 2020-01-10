import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

import { Observable } from "rxjs/Observable";

import { Usuario } from "../usuario/models/Usuario";
import { BaseService } from "./base.service";


@Injectable()
export class UsuarioService extends BaseService {

  constructor(private http: HttpClient){ super() }    

    registrarUsuario(usuario: Usuario) : Observable<Usuario> {

        let response = this.http
            .post(this.UrlServiceV1 + "nova-conta", usuario, super.ObterHeaderJson())
            .map(super.extractData)
            .catch(super.serviceError)

        return response;
    }
    
    login(usuario: Usuario) : Observable<Usuario> {

        let response = this.http
            .post(this.UrlServiceV1 + "conta", usuario, super.ObterHeaderJson())
            .map(super.extractData)
            .catch(super.serviceError)

        return response;
    }
}
