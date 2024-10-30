import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CreateOrUpdateUserCommand, LookupResponseOfGuid, LookupResponseOfUserStatusId, UsersClient, UserStatusId } from '../../api/api-reference';
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
  isEmailAddressValid: boolean = true;

  id: string = "";
  emailAddress: string = "";
  fullName: string  = "";
  userStatusName: string = "";
  createdOn?: Date;
  updatedOn?: Date;
  userStatusDescription?: string;
  userStatusId: UserStatusId | undefined;
  userRoleId: string | undefined = "";
  roles : LookupResponseOfGuid[] = [];
  statuses: LookupResponseOfUserStatusId[] = [];
  editForm = new FormGroup({
    fullName: new FormControl(''),
    emailAddress: new FormControl(''),  
    userStatus: new FormControl(''),
  })
   
  constructor(private usersClient: UsersClient){}

  ngOnInit(): void {    
    this.id = this.route.snapshot.params['id'];
    this.usersClient.get(this.route.snapshot.params['id']).subscribe(user => {
      this.emailAddress = user.emailAddress ?? ""; 
      this.fullName = user.fullName ?? "";
      this.userStatusName = user.userStatusName ?? "";
      this.userStatusId = user.userStatusId;
      this.userRoleId = user.roleId;
      this.createdOn = user.createdOn;
      this.updatedOn = user.updatedOn;
    })
    
    this.usersClient.getRolesLookup().subscribe(roles => {this.roles = roles;});
    this.usersClient.getStatusesLookup().subscribe(statuses => {this.statuses = statuses});
  }

  submitEditedUser() {  
    this.userStatusId = this.statuses.find(s => s.label == this.editForm.controls.userStatus.value)?.value;

    const command = new CreateOrUpdateUserCommand({
      id: this.id,
      emailAddress: this.editForm.controls.emailAddress.value?.toString(),
      fullName: this.editForm.controls.fullName.value?.toString(),
      roleId: this.userRoleId,
      userStatusId: this.userStatusId
    });

    this.usersClient.post(command).subscribe(result => console.log(result));
  }
}
