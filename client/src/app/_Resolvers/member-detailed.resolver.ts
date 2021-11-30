import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Member } from "../_Models/Member";
import { MembersService } from "../_Services/members.service";

@Injectable({
    providedIn: 'root'
})
export class MemberDetailResolver implements Resolve<Member>{

    constructor(private memberService: MembersService) {
    }


    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Member> {
        return this.memberService
            .GetMember(route.paramMap.get('username'));
    }

}