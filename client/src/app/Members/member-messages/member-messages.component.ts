import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/_Models/Message';
import { MessageService } from 'src/app/_Services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input()  messages: Message[];

  constructor() { }

  ngOnInit(): void {
  }


}
