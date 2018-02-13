import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute,  Params } from "@angular/router";
import { AuthService } from "./auth.service";

@Component({
    moduleId: module.id,
    selector: 'login',
    template: ''
})
export class LoginComponent implements OnInit {
    values: number[] = [];

    constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) { }

    ngOnInit(): void {
	this.authService.signInRedirect()
	    .then(user => {
		let url = user.state.nav.url; 
		this.router.navigateByUrl(url);
	    });
    }
}