import { Component, OnInit } from "@angular/core";
import { ApiService } from "./api.service";

@Component({
    moduleId: module.id,
    selector: 'other-info',
    templateUrl: './other.component.html'
})
export class OtherComponent implements OnInit {
    values: number[] = [];

    constructor(private apiService: ApiService) { }

    ngOnInit(): void {
	this.apiService.getOther()
	    .then(result => this.values = result);
    }
}