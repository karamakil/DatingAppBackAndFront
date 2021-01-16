import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_Models/Member';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  public MemberProp: Member;

  public galleryOptions: NgxGalleryOptions[];
  public galleryImages: NgxGalleryImage[];

  constructor(private memberService: MembersService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.LoadMember();
  }

  GetImages(): NgxGalleryImage[] {
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

  LoadMember() {
    let member = this.memberService.GetMember(this.route.snapshot.paramMap.get("username"))
      .subscribe(mem => {
        this.MemberProp = mem;
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
      });
  }

}
