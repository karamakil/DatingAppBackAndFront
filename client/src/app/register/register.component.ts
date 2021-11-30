import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { error } from 'protractor';
import { AccountService } from '../_Services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  // @Input() usersFromHomeComponents: any;.
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  maxDate: Date;
  validationErrors: string[] = [];
  public reactiveForm: FormGroup;

  constructor(private accountService: AccountService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router) {

  }

  ngOnInit(): void {
    this.InitializeForm();
    this.maxDate = new Date()
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18)
  }

  InitializeForm() {
    //Using FormBuilder
    this.reactiveForm = this.fb.group({
      gender: ['male'],
      userName: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.MatchValuesValidator("password")]]
    })
    // to initialize the form inputs with usage of formBuilder
    // this.reactiveForm = new FormGroup({
    //   userName: new FormControl('', Validators.required),
    //   password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', [Validators.required, this.MatchValuesValidator("password")])
    // })
  }

  MatchValuesValidator(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
  }


  register() {
    this.accountService.Register(this.reactiveForm.value).subscribe(response => {
      console.log(response);
      this.router.navigateByUrl("/members");
      // this.CancelRegisterMethod();
    },
      error => {
        console.log(error);
        this.validationErrors = error;
      });
  }

  CancelRegisterMethod() {
    this.cancelRegister.emit(false);
  }

  // ngAfterViewInit(){
  //   console.log("ngAfterViewInit")
  // }

}
