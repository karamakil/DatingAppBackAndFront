import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
  Ousername: any;

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(n => {
      this.Ousername = n?.userName
    });

  }

  LoginMethod() {
    this.accountService.Login(this.model).subscribe(response => {
      console.log(response);
      this.router.navigateByUrl("/members")
      this.toastr.success('Hello world!', 'Toastr fun!');
    });
  }

  LogOut() {
    this.accountService.Logout();
    this.router.navigateByUrl("/")
  }



}
