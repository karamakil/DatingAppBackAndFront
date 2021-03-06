import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_Services/account.service';
import { User } from '../_Models/User';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User;
    this.accountService.currentUser$.subscribe(user => currentUser = user);
    if (currentUser) {
      request = request.clone({
        setHeaders: {
          Authorization: 'Bearer ' + currentUser.token,
        }
      })
    }

    return next.handle(request);
  }
}


