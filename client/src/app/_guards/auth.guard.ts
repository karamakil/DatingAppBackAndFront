import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map, skipWhile } from 'rxjs/operators';
import { AccountService } from '../_Services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private toastr: ToastrService) { };

  canActivate(): Observable<boolean> {
    // let retVal;
    // this.accountService.currentUser$.subscribe(x=> retVal = x);
    // if (retVal) {
    //   return true;
    // }
    // this.toastr.error("not auth");
    return this.accountService.currentUser$.pipe(
      skipWhile(u => !u),
      map(user => {
        if (user) { return true };
        this.toastr.error("not auth")
      })
    )


  }

}
