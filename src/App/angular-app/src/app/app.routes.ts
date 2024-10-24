import { Routes } from '@angular/router';
import { UsersListComponent } from './user/users-list/users-list.component';
import { UserDetailsComponent } from './user/user-details/user-details.component';


export const routes: Routes = [
    { path: '', redirectTo: 'users', pathMatch: 'full' },
    {
        path: 'users',
        component: UsersListComponent,
        title: 'User List'
    },
    {
        path: 'user/:id',
        component: UserDetailsComponent,
        title: 'User details'
    }
];
