import { Component, OnInit, AfterViewInit, OnDestroy, ViewChildren, ElementRef, ViewContainerRef } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormControl, FormArray, Validators, FormControlName } from '@angular/forms';

import { Router } from "@angular/router";

import { GenericValidator } from "../../common/validation/generic-form-validator";
import { ToastsManager, Toast } from 'ng2-toastr/ng2-toastr';
import { CustomValidators, CustomFormsModule } from 'ng2-validation'
import { UUID } from 'angular2-uuid';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/observable/merge';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { UsuarioService } from "../../services/Usuario.service";
import { Usuario } from "../models/Usuario";
import { StringUtils } from "../../common/data-type-utils/string-utils";

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html'
})

export class RegistrarComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  public errors: any[] = [];
  registrarForm: FormGroup;
  usuario: Usuario;
  loading: boolean = false;

  constructor(private fb: FormBuilder,
    private UsuarioService: UsuarioService,
    private router: Router,
    private toastr: ToastsManager,
    vcr: ViewContainerRef) {
    
    this.toastr.setRootViewContainerRef(vcr);

    this.validationMessages = {
      nome: {
        required: 'O Nome é requerido.',
        minlength: 'O Nome precisa ter no mínimo 2 caracteres',
        maxlength: 'O Nome precisa ter no máximo 150 caracteres'
      },
      cpf: {
        required: 'Informe o CPF',
        rangeLength: 'CPF deve conter 11 caracteres'
      },
      email: {
        required: 'Informe o e-mail',
        email: 'Email invalido'
      },
      senha: {
        required: 'Informe a senha',
        minlength: 'A senha deve possuir no mínimo 6 caracteres'
      },
      senhaConfirmacao: {
        required: 'Informe a senha novamente',
        minlength: 'A senha deve possuir no mínimo 6 caracteres',
        equalTo: 'As senhas não conferem'
      }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
    this.usuario = new Usuario();
  }

  displayMessage: { [key: string]: string } = {};
  private validationMessages: { [key: string]: { [key: string]: string } };
  private genericValidator: GenericValidator;

  ngOnInit(): void {
    let senha = new FormControl('', [Validators.required, Validators.minLength(6)]);
    let senhaConfirmacao = new FormControl('', [Validators.required, Validators.minLength(6), CustomValidators.equalTo(senha)]);

    this.registrarForm = this.fb.group({
      nome: ['', [Validators.required,
      Validators.minLength(2),
      Validators.maxLength(150)]],      
      email: ['', [Validators.required,
      CustomValidators.email]],
      senha: senha,
      senhaConfirmacao: senhaConfirmacao
    });
    this.loading = true;
  }

  ngAfterViewInit(): void {
    let controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => Observable.fromEvent(formControl.nativeElement, 'blur'));

    Observable.merge(...controlBlurs).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.registrarForm);
    });
  }

  adicionarUsuario() {
    if (this.registrarForm.dirty && this.registrarForm.valid) {
      this.loading = false;
      let p = Object.assign({}, this.usuario, this.registrarForm.value);
      p.id = UUID.UUID();

      this.UsuarioService.registrarUsuario(p)
        .subscribe(
        result => { this.onSaveComplete(result) },
        fail => { this.onError(fail) }
        );
    }
  }

  onSaveComplete(response: any): void {
    this.registrarForm.reset();
    this.errors = [];

    localStorage.setItem('vb.token', response.access_token);
    localStorage.setItem('vb.user', JSON.stringify(response.user));

    this.toastr.success('Registro realizado com Sucesso!', 'Bem vindo!!!', { dismiss: 'controlled' })
      .then((toast: Toast) => {
        setTimeout(() => {
          this.toastr.dismissToast(toast);
          this.router.navigate(['/home']);
        }, 3500);
      });
  }

  onError(fail: any) {
    this.toastr.error('Ocorreu um erro!', 'Opa :(')
    this.errors = fail.error.errors;
  }

  ngOnDestroy(): void {
    //throw new Error('Method not implemented.');
  }
}
