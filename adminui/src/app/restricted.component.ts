import { Component, OnInit } from "@angular/core";
import { ApiService } from "./api.service";

@Component({
    moduleId: module.id,
    selector: 'restricted-info',
    templateUrl: './restricted.component.html'
})
export class RestrictedComponent implements OnInit {
    values: number[] = [];

    constructor(private apiService: ApiService) { }

    ngOnInit(): void {
	this.apiService.getRestricted()
	    .then(result => this.values = result);
    }
}