import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Observable } from "rxjs/Observable";

import { BaseService } from "../../services/base.service";

import { SWA } from "../models/swa";

@Injectable()
export class ChallengeService extends BaseService {

  constructor(private http: HttpClient) { super(); }  

  obterPersonagensPaginado(page: number): Observable<SWA[]> {    
      return this.http
          .get<SWA[]>(this.UrlServiceV1 + "challenge/" + page,super.ObterAuthHeaderJson())
          .catch(super.serviceError);
  }
  
  ObterEspecies(): Observable<string[]>{    
    return this.http
    .get<string[]>(this.UrlServiceV1 + 'challenge/getEspecies',super.ObterAuthHeaderJson())
    .catch(super.serviceError);
  }

  ObterPersonagensPorEspecies(filter: string): Observable<SWA[]>{    
    return this.http
    .get<string[]>(this.UrlServiceV1 + 'challenge/findByEspecies/' + filter,super.ObterAuthHeaderJson())
    .catch(super.serviceError);
  }
}