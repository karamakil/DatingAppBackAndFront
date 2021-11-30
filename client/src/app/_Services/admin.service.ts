import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_Models/User';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  GetUsersWithRoles() {
    return this.http.get<Partial<User[]>>(this.baseUrl + "admin/users-with-roles")
  }

  UpdateUserRoles(userName :string, roles : string[]) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + userName + '?roles=' + roles, {});
  }

}
