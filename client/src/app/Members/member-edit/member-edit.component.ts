import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { ToastrIconClasses, ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_Models/Member';
import { User } from 'src/app/_Models/User';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild("formEdit") editForm: NgForm;

  //used to check on the browser level if the form is dirty before close or and event is fired
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  public MemberProp: Member;
  public user: User;

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.subscribe(user => {
      this.user = user;
    })
  }

  ngOnInit(): void {
    this.LoadMember();
  }

  LoadMember() {
    this.memberService.GetMember(this.user.userName).subscribe(mem => {
      this.MemberProp = mem;
    })
  }

  UpdateMember() {
    this.memberService.UpdateMember(this.MemberProp).subscribe( ()=> {
      console.log(this.MemberProp);
      this.toastr.success("updated");
      this.editForm.reset(this.MemberProp);
    });
  }

}
