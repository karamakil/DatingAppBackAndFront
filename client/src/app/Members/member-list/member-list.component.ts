import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_Models/Member';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  public membersListObservable$: Observable<Member[]>;

  memArray: Member[] = [];

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.membersListObservable$ = this.memberService.GetMembers();
    // debugger
    // this.loadMembers();
    console.log(this.membersListObservable$);
  }

  // loadMembers() {
  //   this.memberService.GetMembers().subscribe(members => {
  //     this.memArray = members;
  //   })
  // }

}
