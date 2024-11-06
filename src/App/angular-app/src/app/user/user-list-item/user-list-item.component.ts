import { Component, Input, OnInit } from '@angular/core'
import { UsersClient } from '../../api/api-reference'
import { RouterModule } from '@angular/router'
import { MatButton } from '@angular/material/button'
import { MatListItem, MatList } from '@angular/material/list'
import { MatLine } from '@angular/material/core'
import { MatDivider } from '@angular/material/divider'

@Component({
    selector: 'app-user-list-item',
    standalone: true,
    imports: [
        RouterModule,
        MatList,
        MatListItem,
        MatLine,
        MatDivider,
        MatButton,
    ],
    templateUrl: './user-list-item.component.html',
    styleUrl: './user-list-item.component.scss',
})
export class UserListItemComponent implements OnInit {
    @Input() id: string = ''
    emailAddress: string = ''
    fullName: string = ''
    userStatusName: string = ''

    constructor(private usersClient: UsersClient) {}

    ngOnInit() {
        this.usersClient.get(this.id).subscribe((user) => {
            this.emailAddress = user.emailAddress ?? ''
            this.fullName = user.fullName ?? ''
            this.userStatusName = user.userStatusName ?? ''
        })
    }
}