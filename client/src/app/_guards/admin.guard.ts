import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_Models/User';
import { AccountService } from '../_Services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private AccountService: AccountService, private toaster : ToastrService){}

  canActivate(): Observable<boolean>  {
    return this.AccountService.currentUser$
    .pipe(map( (u : User)  => {

      if (u.roles.includes("Admin") || u.roles.includes("Moderator")) {
        return true;
      }
      this.toaster.error("Cant enter this area")
      return false;

    }));
  }
  
}
