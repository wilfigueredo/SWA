import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { SWA } from "./models/swa";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { ChallengeService } from "./service/challenge.service";
import { Observable } from 'rxjs';
import { debounceTime, map, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.css']
})
export class ChallengeComponent implements OnInit {

  public swa: SWA[];
  public swaFilter: SWA[];
  errorMessage: string;
  page = 1;
  pageSize = 10;
  collectionSize: number;
  loading: boolean = false;
  especies: string[];
  public model: any;

  constructor(public service: ChallengeService, public toastr: ToastsManager, vcr: ViewContainerRef) 
  { 
    this.toastr.setRootViewContainerRef(vcr);
  }

  ngOnInit() {
    this.service.obterPersonagensPaginado(this.page)
      .subscribe(result => {        
        this.swa  = result;
        this.collectionSize = result[0].count;
      },
      error => this.errorMessage);

      this.service.ObterEspecies().subscribe(result => {
        this.especies = result;      
        this.loading = true;
      }, error => console.error(error));     
  }

  onSubmit() 
      {
        if(this.model == undefined || this.model == ""){          
          return this.swa;     
        }
        this.loading = false;
        this.service.ObterPersonagensPorEspecies(this.model).subscribe(result => {
          this.swa = result.slice(0, 10);          
          this.swaFilter = result;
          this.collectionSize = result[0].count;
          this.page = 1;
          this.loading = true;
        }, error => console.error(error));        
      }

      pageChange(){
        this.loading = false;
        if (this.model == undefined || this.model == "") {
          this.service.obterPersonagensPaginado(this.page)
          .subscribe(result => {
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
