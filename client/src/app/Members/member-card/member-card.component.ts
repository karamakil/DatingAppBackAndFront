import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_Models/Member';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() memberInput: Member;

  constructor(private memberService: MembersService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  public AddLike(member: Member) {
    this.memberService.AddLike(member.userName).subscribe(() => {
      this.toastr.success("You have like" + member.knownAs);
    })
  }

}
