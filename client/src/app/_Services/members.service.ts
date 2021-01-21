import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_Models/Member';

// was used to add the token to the request and send it by the request in get methods
// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization: "Bearer " + JSON.parse(localStorage.getItem("User"))?.token
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private baseUrl = environment.apiUrl

  public members: Member[] = [];

  constructor(private http: HttpClient) { }

  GetMembers(): Observable<Member[]> {
    //the of return variable as observable
    if (this.members.length > 0) { return of(this.members) };
    return this.http.get<Member[]>(this.baseUrl + "users").pipe(
      map(mem => {
        this.members = mem;
        return this.members;
      })
    );
  }

  GetMember(userName: string): Observable<Member> {
    let mem = this.members.find(x => x.userName === userName);
    if (mem !== undefined) {
      return of(mem);
    }
    return this.http.get<Member>(this.baseUrl + "users/" + userName);
  }

  UpdateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(map(
      () => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      }
    ));
  }

  SetMainPhoto(photoId: number)
  {
    return this.http.put(this.baseUrl + "users/set-main-photo/"+photoId,{});
  }

  DeletePhoto(photoId : number)
  {
    return this.http.delete(this.baseUrl + "users/delete-photo/"+ photoId);
  }


}
