import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.Register(this.model).subscribe(response => {
      console.log(response);
      this.CancelRegisterMethod();
    },
      error => { console.log(error) });
  }

  CancelRegisterMethod() {
    this.cancelRegister.emit(false);
  }

}
