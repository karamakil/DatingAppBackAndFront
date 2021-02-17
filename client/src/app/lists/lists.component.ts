import { Component, OnInit } from '@angular/core';
import { Member } from '../_Models/Member';
import { PaginatedResult, Pagination } from '../_Models/Pagination';
import { MembersService } from '../_Services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  public member: Partial<Member[]>;
  public predicate = "liked";
  public pageNumber = 1;
  public pageSize = 5;
  public pagination: Pagination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.memberService.GetLikes(this.predicate, this.pageNumber, this.pageSize).subscribe(response => {
      this.member = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadLikes();
  }

}
