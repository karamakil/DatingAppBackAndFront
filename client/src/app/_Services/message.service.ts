import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as internal from 'events';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Message } from '../_Models/Message';
import { GetPaginationHeaders, getPaginationResult } from './PaginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  messages: Message[];
  constructor(private httpClient: HttpClient) {

  }

  public getMessages(pageNumber, pageSize, container) {
    let params = GetPaginationHeaders(pageNumber, pageSize);
    params = params.append("Container", container);
    return getPaginationResult<Message[]>(this.baseUrl + 'messages', params, this.httpClient);
  }

  public getMessageThread(userName: string) {
    return this.httpClient.get<Message[]>(this.baseUrl + "messages/thread/" + userName);
  }

}
