import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { debounceTime, map, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.css']
})
export class ChallengeComponent{
  public swa: SWA[];
  public swaFilter: SWA[];
  page = 1;
  pageSize = 10;
  collectionSize: number;
  call: HttpClient;
  baseUrl: string;
  loading: boolean = false;  
  especies: string[];
  public model: any;
  

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {    
    this.call = http;
    this.baseUrl = baseUrl;
    http.get<SWA[]>(baseUrl + 'Challenge?page=' + this.page).subscribe(result => {
      this.swa = result;
      this.collectionSize = result[0].count;      
    }, error => console.error(error));    
    http.get<string[]>(baseUrl + 'Challenge/GetEspecies').subscribe(result => {
      this.especies = result;      
      this.loading = true;
    }, error => console.error(error));

  }

  onSubmit() {
    this.loading = false;
    this.call.get<SWA[]>(this.baseUrl + 'Challenge/FindByEspecies?filter=' + this.model).subscribe(result => {
      this.swa = result.slice(0, 10);
      this.swaFilter = result;
      this.collectionSize = result[0].count;
      this.page = 1;
      this.loading = true;
    }, error => console.error(error));  
    
  }

  public loadData() {
    this.loading = false;
    if (this.model == undefined || this.model == "") {
        this.call.get<SWA[]>(this.baseUrl + 'Challenge?page=' + this.page).subscribe(result => {
        this.swa = result;
        this.loading = true;
      }, error => console.error(error));
    } else {
      if (this.page == 1) {
        this.swa = this.swaFilter.slice(0, this.page * 10)
      } else {
        this.swa = this.swaFilter.slice((this.page*10) - 10, this.page * 10)
      }
      this.loading = true;
    }
  }

  

  search = (text$: Observable<string>) =>    
    text$.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      map(term => term.length < 2 ? []
        : this.especies.filter(v => v.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10))
    )  
}

interface SWA {
  nomePersonagem: string;
  planetaNatal: string;
  count: number;
}

