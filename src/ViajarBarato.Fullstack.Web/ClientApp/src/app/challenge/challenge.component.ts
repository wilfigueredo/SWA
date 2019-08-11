import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { getBaseUrl } from '../../main';

@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.css']
})
export class ChallengeComponent{
  public swa: WeatherForecast[];
  page = 1;
  pageSize = 10;
  collectionSize: number;
  call: HttpClient;
  baseUrl: string;
  loading: boolean = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {    
    this.call = http;
    this.baseUrl = baseUrl;
    http.get<WeatherForecast[]>(baseUrl + 'Challenge?page=' + this.page).subscribe(result => {
      this.swa = result;
      this.collectionSize = result[0].count;
      this.loading = true;
    }, error => console.error(error));   

  }

  public loadData() {
    this.loading = false;
    this.call.get<WeatherForecast[]>(this.baseUrl + 'Challenge?page=' + this.page).subscribe(result => {
      this.swa = result;
      this.loading = true;      
    }, error => console.error(error));  
  }
}

interface WeatherForecast {
  nomePersonagem: string;
  planetaNatal: string;
  count: number;
}
