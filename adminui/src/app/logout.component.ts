import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { AuthService } from "./auth.service";

@Component({
    moduleId: module.id,
    selector: 'logout',
    template: ''
})
export class LogoutComponent implements OnInit {
    values: number[] = [];

    constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) { }

    ngOnInit(): void {
	this.authService.signOut();
    }
}