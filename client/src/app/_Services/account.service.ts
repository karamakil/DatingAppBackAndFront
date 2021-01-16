import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { constants } from 'buffer';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_Models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  //environment is the file in the environments folder used to store values
  private baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>();
  public currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  //#region Authentication Methods
  Login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem("User", JSON.stringify(user));
          this.currentUserSource.next(user);

          // this.currentUserSource.subscribe(res=> console.log(res.UserName+ "from login account service"));
       
        }
      })
    );
  }

  SetCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  Logout() {
    localStorage.removeItem("User");
    this.currentUserSource.next(null);
  }
  //#endregion


  //#region Register User

  Register(model: any) {
    return this.http.post(this.baseUrl + "account/register", model).pipe(
      map(
        (user: User) => {
          if (user) {
            localStorage.setItem('User', JSON.stringify(user));
            this.currentUserSource.next(user);
          }
          return user;
        }
      )
    )
  }

  //#endregion

}
