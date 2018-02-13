// https://gist.github.com/fernandohu/122e88c3bcd210bbe41c608c36306db9
import { Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from "environments/environment";
import 'rxjs/Rx';

@Injectable()
export class AppConfig {

    private config: Object = null;

    constructor(private http: Http) { }

    /**
     * Use to get the data found in the second file (config file)
     */
    public getConfig(key: any) {
	return this.config[key];
    }

    public load() {
	this.config = environment;
	return new Promise((resolve, reject) => {
	    this.http.get('config.json').map(res => res.json()).catch((error: any): any => {
		console.log('Configuration file "config.json" could not be read');
		resolve(true);
	    }).subscribe((response) => {
		// Compatibility issues with older browsers. See:
		// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/assign
		Object.assign(this.config, response);
		console.log('Configuration file "config.json" loaded.', this.config);
		resolve(true);
	    });
	});
    }
}