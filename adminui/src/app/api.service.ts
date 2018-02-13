import { Injectable } from '@angular/core';
import { Http, RequestOptionsArgs, Headers } from '@angular/http';
import { AuthService } from './auth.service';
import { environment } from 'environments/environment';
import { Settings } from './settings'

import 'rxjs/add/operator/toPromise';

@Injectable()
export class ApiService {
    private apiUri: string;

    constructor(private http: Http, private authService: AuthService) {
	this.apiUri = environment.apiUri;
    }

    getRestricted(): Promise<number[]> {

	return this.authService.getUser()
	    .then(user => {
		if (user) {
		    const url = `${this.apiUri}restricted`;
		    let headers: Headers = new Headers();
		    headers.append("Authorization", "Bearer " + user.access_token);
		    let options: RequestOptionsArgs = {
			headers: headers
		    };
		    return this.http.get(url, options)
			.toPromise()
			.then(response => response.json() as number[])
			.catch(reason => { /* ??? */ });
		} else {
		    this.authService.signIn();
		}
	    });
    }

    getAdmin(): Promise<number[]> {

		return this.authService.getUser()
			.then(user => {
			if (user) {
				const url = `${this.apiUri}admin`;
				let headers: Headers = new Headers();
				headers.append("Authorization", "Bearer " + user.access_token);
				let options: RequestOptionsArgs = {
				headers: headers
				};
				return this.http.get(url, options)
				.toPromise()
				.then(response => response.json() as number[])
				.catch(reason => { 403 /* show 403 for the error */ });
			} else {
				this.authService.signIn();
			}
			});
		}

		getClerk(): Promise<number[]> {
			
			return this.authService.getUser()
				.then(user => {
				if (user) {
					const url = `${this.apiUri}clerk`;
					let headers: Headers = new Headers();
					headers.append("Authorization", "Bearer " + user.access_token);
					let options: RequestOptionsArgs = {
					headers: headers
					};
					return this.http.get(url, options)
					.toPromise()
					.then(response => response.json() as number[])
					.catch(reason => { 403 /* show 403 for the error */ });
				} else {
					this.authService.signIn();
				}
				});
			}
				
    getUnrestricted(): Promise<any> {
	const url = `${this.apiUri}settings`;
	return this.http.get(url)
	    .toPromise()
	    .then(response => response.json())
	    .catch(reason => { /* ??? */ });
    }
}