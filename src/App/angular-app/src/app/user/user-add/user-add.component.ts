import { Component, OnInit, signal } from '@angular/core'
import { CommonModule, Location } from '@angular/common'
import {
    FormControl,
    FormGroup,
    FormsModule,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms'
import {
    CreateOrUpdateUserCommand,
    LookupResponseOfGuid,
    LookupResponseOfUserStatusId,
    UsersClient,
} from '../../api/api-reference'
import { RouterModule } from '@angular/router'
import { MaterialModule } from '../../shared/material.module'
import { takeUntilDestroyed } from '@angular/core/rxjs-interop'
import { merge } from 'rxjs'

@Component({
  selector: 'app-user-add',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    RouterModule,
    MaterialModule,],
  templateUrl: './user-add.component.html',
  styleUrl: './user-add.component.scss'
})
export class UserAddComponent implements OnInit {
  errorMessage = signal('')
  statuses: LookupResponseOfUserStatusId[] = []
  roles: LookupResponseOfGuid[] = []
  
  addForm = new FormGroup({
    fullName: new FormControl<string>('', Validators.required),
    emailAddress: new FormControl('', [
        Validators.email, Validators.required
    ]),
    userStatus: new FormControl<LookupResponseOfUserStatusId | null>(null),
    userRole: new FormControl<LookupResponseOfGuid | null>(null)
})

  constructor(private usersClient: UsersClient, public location: Location){
    merge(
      this.addForm.controls.emailAddress.statusChanges,
      this.addForm.controls.emailAddress.valueChanges
  )
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.UpdateErrorMessage())
  }

  UpdateErrorMessage() {
    if (this.addForm.controls.emailAddress.hasError('email'))
        this.errorMessage.set('Must be a valid email address.')
    else if (this.addForm.controls.emailAddress.hasError('required'))
        this.errorMessage.set('Email address field must not be empty.')
}

  ngOnInit(): void {
    const client = this.usersClient
    client
        .getStatusesLookup()
        .subscribe((statuses) => {
        this.statuses = statuses
    })

    client.getRolesLookup()
        .subscribe((roles) => {
            this.roles = roles
        })
  }
  
  submitUser(){
    const controls = this.addForm.controls
    const command = new CreateOrUpdateUserCommand({
        emailAddress: controls.emailAddress.value!,
        fullName: controls.fullName.value!,
        roleId: controls.userRole.value?.value,
        userStatusId: controls.userStatus.value?.value,
    })

    this.usersClient.post(command).subscribe() 
  }

  // TO DO: 
  // Add notification if user was successfully added 

}
