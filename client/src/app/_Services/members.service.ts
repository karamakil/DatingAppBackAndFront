import { HttpClient, HttpHeaders, HttpParams, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_Models/Member';
import { PaginatedResult } from '../_Models/Pagination';
import { User } from '../_Models/User';
import { UserParams } from '../_Models/UserParams';
import { AccountService } from './account.service';
import { GetPaginationHeaders, getPaginationResult } from './PaginationHelper';

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
  public memberCache = new Map();
  public user: User;
  public userParams: UserParams;


  constructor(private http: HttpClient, private accountService: AccountService) {
    accountService.currentUser$.subscribe(x => {
      this.user = x;
      this.userParams = new UserParams(this.user);
    })
  }

  GetMembers(userParams: UserParams) {
    //the of return variable as observable 
    //used to make cache 
    // if (this.members.length > 0) { return of(this.members) };
    // return this.http.get<Member[]>(this.baseUrl + "users").pipe(
    //   map(mem => {
    //     this.members = mem;
    //     return this.members;
    //   })
    // );
    var response = this.memberCache.get(Object.values(userParams).join("-"));
    if (response) {
      return of(response);
    }
    let params = GetPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    params = params.append("minAge", userParams.minAge.toString());
    params = params.append("maxAge", userParams.maxAge.toString());
    params = params.append("Gender", userParams.gender);
    params = params.append("orderBy", userParams.orderBy);
    return getPaginationResult<Member[]>(this.baseUrl + 'users', params,this.http)
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join("-"), response);
        return response;
      }));
  }

 
 

  GetMember(userName: string): Observable<Member> {
    // let mem = this.members.find(x => x.userName === userName);
    // if (mem !== undefined) {
    //   return of(mem);
    // }
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.userName == userName);
    if (member) {
      return of(member);
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

  SetMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + "users/set-main-photo/" + photoId, {});
  }

  DeletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + "users/delete-photo/" + photoId);
  }

  public GetUserParams() {
    return this.userParams;
  }

  public SetUserParams(params: UserParams) {
    this.userParams = params;
  }

  public ResetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  //when sending post requets always send body 
  public AddLike(userName: string) {
    return this.http.post(this.baseUrl + "likes/" + userName, {});
  }

  public GetLikes(predicate: string, pageNumber, pageSize) {
    var params = GetPaginationHeaders(pageNumber, pageSize);
    params = params.append("predicate", predicate);
    // return this.http.get<Partial<Member[]>>(this.baseUrl + "likes?predicate=" + predicate);
    return getPaginationResult<Partial<Member[]>>(this.baseUrl + "likes", params,this.http);
  }

}
