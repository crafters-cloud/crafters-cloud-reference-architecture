import { Component, Input, OnInit } from '@angular/core';
import { UsersClient } from '../api/api-reference';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit {
  @Input() id: string = "";
  emailAddress: string = "";
  fullName: string = "";
  userStatusName: string = "";
  

  constructor(private usersClient: UsersClient) {}

  ngOnInit() {
    console.log(this.id);
    this.usersClient.get(this.id).subscribe(user => {
      this.emailAddress = user.emailAddress ?? ""; 
      this.fullName = user.fullName ?? "";
      this.userStatusName = user.userStatusName ?? "";
    })
  }
}
