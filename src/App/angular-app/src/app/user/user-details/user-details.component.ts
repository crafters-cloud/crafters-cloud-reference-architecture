import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { CreateOrUpdateUserCommand, GetUserDetailsResponse, LookupResponseOfUserStatusId, UsersClient } from '../../api/api-reference';
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

  user!: GetUserDetailsResponse;
  statuses: LookupResponseOfUserStatusId[] = [];

  editForm = new FormGroup({
    fullName: new FormControl<string>(''),
    emailAddress: new FormControl('', [Validators.email, Validators.required]),  
    userStatus: new FormControl<LookupResponseOfUserStatusId | null>(null),
  })
   
  constructor(private usersClient: UsersClient, public location: Location){}

  ngOnInit(): void {    
    const client = this.usersClient;

    client.get(this.route.snapshot.params['id'])
          .subscribe(user => this.user = user);    
    
    client.getStatusesLookup()
          .subscribe(statuses => {this.statuses = statuses});
  }

  submitEditedUser() {  
    const controls = this.editForm.controls;
    
    const command = new CreateOrUpdateUserCommand({
      id: this.user.id,
      emailAddress: controls.emailAddress.value!,
      fullName: controls.fullName.value!,
      roleId: this.user.roleId,
      userStatusId: controls.userStatus.value?.value
    });

    this.usersClient.post(command)
    .subscribe({
      next: () => this.location.back()
  });
  }
}
