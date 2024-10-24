import { CommonModule } from '@angular/common';
import { Component, OnInit, Input } from '@angular/core';
import { GetUsersResponseItem, UsersClient } from '../../api/api-reference';
import { FormsModule } from '@angular/forms';
import { UserListItemComponent } from "../user-list-item/user-list-item.component";

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule, FormsModule, UserListItemComponent],
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.scss'
})
export class UsersListComponent implements OnInit {
  usersList: GetUsersResponseItem[] = [];
  query: string = "";

  constructor(private usersClient: UsersClient) {
  }

  ngOnInit(): void {
    this.usersClient.search(this.query, undefined, undefined, undefined, undefined, undefined).subscribe(users => this.usersList = users.items ?? []);
  }

  filterResults(event: any) {
    this.usersClient.search(event.target.value, undefined, undefined, undefined, undefined, undefined).subscribe(users => this.usersList = users.items ?? []);
  }

}
