import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_Models/Member';
import { MembersService } from 'src/app/_Services/members.service';
import { TimeagoModule } from 'ngx-timeago';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Message } from 'src/app/_Models/Message';
import { MessageService } from 'src/app/_Services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs',{static:true}) memberTabs: TabsetComponent;
  public MemberProp: Member;
  public galleryOptions: NgxGalleryOptions[];
  public galleryImages: NgxGalleryImage[];
  public activeTab: TabDirective;
  public messages: Message[] = [];

  constructor(private messageService: MessageService, private memberService: MembersService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      debugger;
      this.MemberProp = data.member;
    });
    this.route.queryParams.subscribe(p => {
      p.tab ? this.SelectTab(p.tab) : this.SelectTab(0);
    })

     this.galleryOptions = [
      {
        width: "500px",
        height: "500px",
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      }
    ];
    this.galleryImages = this.GetImages();

  }

  GetImages(): NgxGalleryImage[] {
    debugger;
    const imgUrl = [];
    for (let photo of this.MemberProp.photos) {
      imgUrl.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url,
      });
    }
    return imgUrl;
  }


  LoadMessages() {
    this.messageService.getMessageThread(this.MemberProp.userName).subscribe(msg => {
      this.messages = msg;
    });
  }

  SelectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.LoadMessages();
    }
  }

}
