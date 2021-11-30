import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_Models/Member';
import { PaginatedResult, Pagination } from 'src/app/_Models/Pagination';
import { User } from 'src/app/_Models/User';
import { UserParams } from 'src/app/_Models/UserParams';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  // public membersListObservable$: Observable<Member[]>;
  public members: Member[];
  public pagination: Pagination;
  public userParams: UserParams;
  public user: User;
  public memArray: Member[] = [];
  public genderList = [{ value: "male", display: "Male" }, { value: "female", display: "Female" }];

  constructor(private memberService: MembersService) {
    debugger;
    this.userParams = memberService.GetUserParams();
  }

  ngOnInit(): void {
    // this.membersListObservable$ = this.memberService.GetMembers();
    // debugger
    // this.loadMembers();
    // console.log(this.membersListObservable$);
    this.loadMembers();
  }


  loadMembers() {
    // this.memberService.GetMembers().subscribe(members => {
    //   this.memArray = members;
    // })..
    this.memberService.SetUserParams(this.userParams);
    this.memberService.GetMembers(this.userParams)
      .subscribe(
        res => {
          this.members = res.result;
          this.pagination = res.pagination;
        }
      );
  }

  public pageChanged(event) {
    this.userParams.pageNumber = event.page;
    this.memberService.SetUserParams(this.userParams);
    this.loadMembers();
  }

  public resetFilters() {
    this.userParams = this.memberService.ResetUserParams();
    this.loadMembers();
  }

}
