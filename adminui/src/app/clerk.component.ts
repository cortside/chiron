import { Component, OnInit } from "@angular/core";
import { ApiService } from "./api.service";

@Component({
    moduleId: module.id,
    selector: 'clerk-info',
    templateUrl: './restricted.component.html'
})
export class ClerkComponent implements OnInit {
    values: number[] = [];

    constructor(private apiService: ApiService) { }

    ngOnInit(): void {
	this.apiService.getClerk()
	    .then(result => this.values = result);
    }
}