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
  private currentUserSource = new ReplaySubject<User>(null);
  
  public currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  Login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.SetCurrentUser(user);
          // this.currentUserSource.subscribe(res=> console.log(res.UserName+ "from login account service"));
        }
      })
    );
  }

  Logout() {
    localStorage.removeItem("User");
    this.currentUserSource.next(null);
  }

  Register(model: any) {
    return this.http.post(this.baseUrl + "account/register", model).pipe(
      map(
        (user: User) => {
          if (user) {
            this.SetCurrentUser(user);
          }
          return user;
        }
      )
    )
  }

  SetCurrentUser(user: User) {
    user.roles = [];
    const roles = this.GetDecodedToken(user.token).role;
    Array.isArray(roles)? user.roles = roles: user.roles.push(roles);
    localStorage.setItem('User', JSON.stringify(user));
    this.currentUserSource.next(user);
  }


  GetDecodedToken(token)
  {
    return JSON.parse(atob(token.split('.')[1]))
  }


}
