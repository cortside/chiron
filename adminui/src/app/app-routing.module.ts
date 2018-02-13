import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UnrestrictedComponent } from './unrestricted.component';
import { RestrictedComponent } from './restricted.component';
import { AdminComponent } from './admin.component';
import { ClerkComponent } from './clerk.component';
import { LoginComponent } from './login.component';
import { LogoutComponent } from './logout.component';
import { UserComponent } from './user.component';
import { TestingComponent } from './testing.component';

const routes: Routes = [
    {
	path: '',
	redirectTo: '/unrestricted',
	pathMatch: 'full'
    },
    {
	path: 'user',
	component: UserComponent
    }, {
	path: 'testing',
	component: TestingComponent
    },
    {
	path: 'login',
	component: LoginComponent
    }, {
	path: 'logout',
	component: LogoutComponent
    },
    {
	path: 'unrestricted',
	component: UnrestrictedComponent
    },
    {
	path: 'restricted',
	component: RestrictedComponent
    },
    {
	path: 'admin',
	component: AdminComponent
    },
    {
	path: 'clerk',
	component: ClerkComponent
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }