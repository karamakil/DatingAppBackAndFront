import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;

  constructor(public httpClient: HttpClient) { }

  ngOnInit(): void {
    // this.getUsers();
  }

  RegisterToggles() {
    this.registerMode = !this.registerMode;
  }

  // getUsers() {
  //   this.httpClient.get("http://localhost:5000/api/Users").subscribe(
  //     users => { this.users = users; },
  //     error => { console.log(error); });
  // }

  CancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }


}
