import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { AuthService } from "./auth.service";

@Component({
    moduleId: module.id,
    selector: 'user-info',
    templateUrl: './user.component.html'
})
export class UserComponent implements OnInit {
    values: number[] = [];
    user: Oidc.User;

    constructor(private authService: AuthService) { }

    ngOnInit(): void {
	this.authService.getUser()
	    .then(user => {
		if (user) {
		    this.user = user;
		} 
	    });
    }
}