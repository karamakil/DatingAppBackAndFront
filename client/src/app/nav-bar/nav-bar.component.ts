import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';
import { Observable, observable } from 'rxjs';
import { User } from '../_Models/User';
import { AccountService } from '../_Services/account.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  model: any = {};
  
  user: any = {};

  constructor(public accountService: AccountService) {
  }

  ngOnInit(): void {
  }

  LoginMethod() {
    this.accountService.Login(this.model).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error)
    });
  }

  LogOut() {
    this.accountService.Logout();
  }

  

}
