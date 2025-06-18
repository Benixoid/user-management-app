import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../model/ApiResponse';
import { User } from '../model/users';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private baseURI: string = 'https://localhost:7188/api/v1/Users';
  constructor(private http: HttpClient) {}

  public getusers(email: string, name: string) {
    let filter = '';

    if (email.length > 0) filter = '?EmailFilter=' + email;

    if (name.length > 0) {
      if (filter.length > 0) filter = filter + '&';
      else filter = '?';
      filter = filter + 'NameFilter=' + name;
    }
    return this.http.get<ApiResponse>(this.baseURI + filter);
  }

  public updateUser(user: User) {
    return this.http.put<ApiResponse>(this.baseURI + '/' + user.id, user);
  }

  public createUser(user: User) {
    //user.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<ApiResponse>(this.baseURI, user);
  }

  public deleteUser(id: string) {
    return this.http.delete<ApiResponse>(this.baseURI + '/' + id);
  }
}
