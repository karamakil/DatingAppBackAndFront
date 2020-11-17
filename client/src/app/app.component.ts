import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';
import { User } from './_Models/User';
import { AccountService } from './_Services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  Name = "Karam akil";

  users: any;

  ngOnInit() {
    this.getUsers();

    this.setCurrentUser();
  }

  constructor(private http: HttpClient, private accountService : AccountService) { };

  getUsers() {
    this.http.get("http://localhost:5000/api/Users").subscribe(
      Response => { this.users = Response; },
      error => { console.log(error); });
  }

  setCurrentUser()
  {
    const user : User = JSON.parse(localStorage.getItem("User"));
    this.accountService.SetCurrentUser(user);
  }


}
