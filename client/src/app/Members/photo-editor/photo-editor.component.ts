import { Component, Input, OnInit } from '@angular/core';
import { FileItem, FileUploader } from 'ng2-file-upload';
import { Member } from 'src/app/_Models/Member';
import { Photo } from 'src/app/_Models/Photo';
import { User } from 'src/app/_Models/User';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member;

  uploader: FileUploader;
  hasBasedDropZone = false;
  baseUrl = environment.apiUrl;
  user: User;

  constructor(private accountService: AccountService, private memberService: MembersService) {
    this.accountService.currentUser$.subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.InitializeUploader();
  }

  FileOverBase(e: any) {
    this.hasBasedDropZone = e;
  }

  InitializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    })

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, Response, status, headers) => {
      if (Response) {
        const photo: Photo = JSON.parse(Response);
        this.member.photos.push(photo)
        if (photo.isMain) {
          this.user.photoUrl = photo.url;
          this.member.photoUrl = photo.url
          this.accountService.SetCurrentUser(this.user);
        }
      }
    }
  }

  SetMainPhoto(photo: Photo) {
    this.memberService.SetMainPhoto(photo.id).subscribe((x) => {
      this.user.photoUrl = photo.url;
      this.accountService.SetCurrentUser(this.user);
      this.member.photoUrl = photo.url;
      this.member.photos.forEach(p => {
        if (p.isMain) p.isMain = false;
        if (p.id == photo.id) p.isMain = true;
      })
    });
  }

  DeletePhoto(photoId: number) {
    this.memberService.DeletePhoto(photoId).subscribe(() => {
      this.member.photos = this.member.photos.filter(x => x.id != photoId);
    })
  }

}
