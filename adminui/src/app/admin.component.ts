import { Component, OnInit } from "@angular/core";
import { ApiService } from "./api.service";

@Component({
    moduleId: module.id,
    selector: 'admin-info',
    templateUrl: './restricted.component.html'
})
export class AdminComponent implements OnInit {
    values: number[] = [];

    constructor(private apiService: ApiService) { }

    ngOnInit(): void {
	this.apiService.getAdmin()
	    .then(result => this.values = result);
    }
}