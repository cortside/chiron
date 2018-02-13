import { Component, OnInit } from '@angular/core';

@Component({
    moduleId: module.id,
    selector: 'testing',
    templateUrl: './testing.component.html'
})
export class TestingComponent implements OnInit {
    testValue: number;

    ngOnInit(): void {
	this.testValue = 100;
    }
}