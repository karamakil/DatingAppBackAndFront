import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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

  constructor(private accountService: AccountService,private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.Register(this.model).subscribe(response => {
      console.log(response);
      this.CancelRegisterMethod();
    },
      error => { console.log(error);
        this.toastr.error(error);
       });
  }

  CancelRegisterMethod() {
    this.cancelRegister.emit(false);
  }

}
