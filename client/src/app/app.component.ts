import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';

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
  }

  constructor(private http: HttpClient) { };

  getUsers() {
    this.http.get("http://localhost:5000/Users").subscribe(
      Response => { this.users = Response; },
      error => { console.log(error); });
  }


}
