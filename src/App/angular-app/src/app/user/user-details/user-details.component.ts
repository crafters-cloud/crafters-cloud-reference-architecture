import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CreateOrUpdateUserCommand, UsersClient } from '../../api/api-reference';
import { ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, FormsModule],
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.scss'
})

export class UserDetailsComponent implements OnInit{

  route: ActivatedRoute = inject(ActivatedRoute);
  editMode: boolean = false;

  @Input() id: string = "";
  emailAddress: string = "";
  fullName: string = "";
  userStatusName: string = "";
  createdOn?: Date;
  updatedOn?: Date;
  userStatusDescription?: string;


  editForm = new FormGroup({
    fullName: new FormControl(''),
    emailAddress: new FormControl(''),  
    userStatus: new FormControl(''),
  })


  constructor(private usersClient: UsersClient){}

  ngOnInit(): void {    
    this.usersClient.get(this.route.snapshot.params['id']).subscribe(user => {
      this.emailAddress = user.emailAddress ?? ""; 
      this.fullName = user.fullName ?? "";
      this.userStatusName = user.userStatusName ?? "";
      this.userStatusDescription = user.userStatusDescription ?? "";
      this.createdOn = user.createdOn;
      this.updatedOn = user.updatedOn;
    })
  }

  submitEditedUser() {
    const command = new CreateOrUpdateUserCommand({
      id : this.id,
      emailAddress: this.emailAddress,
      fullName: this.fullName
    });

    this.usersClient.post(command);
  }
}
