import { Component, inject, OnInit } from '@angular/core'
import { CommonModule } from '@angular/common'
import { GetUserDetailsResponse, UsersClient } from '../../api/api-reference'
import { ActivatedRoute, RouterModule } from '@angular/router'
import { MaterialModule } from '../../shared/material.module'

@Component({
    selector: 'app-user-details',
    standalone: true,
    imports: [CommonModule, RouterModule, MaterialModule],
    templateUrl: './user-details.component.html',
    styleUrl: './user-details.component.scss',
})
export class UserDetailsComponent implements OnInit {
    route: ActivatedRoute = inject(ActivatedRoute)
    user!: GetUserDetailsResponse

    constructor(private usersClient: UsersClient) {}

    ngOnInit(): void {
        this.usersClient
            .get(this.route.snapshot.params['id'])
            .subscribe((user) => (this.user = user))
    }
}
