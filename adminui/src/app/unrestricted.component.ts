import { Component, OnInit } from "@angular/core";
import { ApiService } from "./api.service";
import { Settings } from './settings';

@Component({
    moduleId: module.id,
    selector: 'unrestricted-info',
    templateUrl: './unrestricted.component.html'
})
export class UnrestrictedComponent implements OnInit {
    settings: Settings;

    constructor(private apiService: ApiService) { }

    ngOnInit(): void {
	this.apiService.getUnrestricted()
	    .then(result => {
		this.settings = result
	    });
    }
}