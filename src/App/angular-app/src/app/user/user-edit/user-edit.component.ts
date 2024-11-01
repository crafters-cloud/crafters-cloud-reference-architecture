import { Component, inject, OnInit, signal } from '@angular/core'
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
    GetUserDetailsResponse,
    LookupResponseOfUserStatusId,
    UsersClient,
} from '../../api/api-reference'
import { ActivatedRoute, RouterModule } from '@angular/router'
import { MaterialModule } from '../../shared/material.module'
import { takeUntilDestroyed } from '@angular/core/rxjs-interop'
import { merge } from 'rxjs'

@Component({
    selector: 'app-user-edit',
    standalone: true,
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        RouterModule,
        MaterialModule,
    ],
    templateUrl: './user-edit.component.html',
    styleUrl: './user-edit.component.scss',
})
export class UserEditComponent implements OnInit {
    route: ActivatedRoute = inject(ActivatedRoute)
    user!: GetUserDetailsResponse
    statuses: LookupResponseOfUserStatusId[] = []
    errorMessage = signal('')

    editForm = new FormGroup({
        fullName: new FormControl<string>('', Validators.required),
        emailAddress: new FormControl('', [
            Validators.email,
            Validators.required,
        ]),
        userStatus: new FormControl<LookupResponseOfUserStatusId | null>(null),
    })

    constructor(
        private usersClient: UsersClient,
        public location: Location
    ) {
        merge(
            this.editForm.controls.emailAddress.statusChanges,
            this.editForm.controls.emailAddress.valueChanges
        )
            .pipe(takeUntilDestroyed())
            .subscribe(() => this.UpdateErrorMessage())
    }

    UpdateErrorMessage() {
        if (this.editForm.controls.emailAddress.hasError('email'))
            this.errorMessage.set('Must be a valid email address.')
        else if (this.editForm.controls.emailAddress.hasError('required'))
            this.errorMessage.set('Email address field must not be empty.')
    }

    ngOnInit(): void {
        const client = this.usersClient

        client
            .get(this.route.snapshot.params['id'])
            .subscribe((user) => (this.user = user))

        client.getStatusesLookup().subscribe((statuses) => {
            this.statuses = statuses
        })
    }

    submitEditedUser() {
        const controls = this.editForm.controls
        const command = new CreateOrUpdateUserCommand({
            id: this.user.id,
            emailAddress: controls.emailAddress.value!,
            fullName: controls.fullName.value!,
            roleId: this.user.roleId,
            userStatusId: controls.userStatus.value?.value,
        })

        this.usersClient.post(command).subscribe({
            next: () => this.location.back(),
        })
    }
}
