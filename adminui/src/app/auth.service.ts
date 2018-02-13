import { Injectable } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { UserManager, User, UserManagerSettings } from "oidc-client";
import { AppConfig } from "./app.config";

@Injectable()
export class AuthService {
    private userManager: UserManager;
    private nav: any;

    constructor(private route: ActivatedRoute, private router: Router, private config: AppConfig) {
	this.userManager = new UserManager({
	    authority: config.getConfig('authUri'),
	    client_id: 'js',
	    redirect_uri: config.getConfig('appUri') + 'login',
	    post_logout_redirect_uri: config.getConfig('appUri'),
	    response_type: 'id_token token',
	    scope: 'openid adminapi',
	    silent_redirect_uri: config.getConfig('appUri') + 'login',
	    automaticSilentRenew: true,
	    filterProtocolClaims: true,
	    loadUserInfo: true
	});
	this.router.events.subscribe((nav: any) => this.nav = nav);
    }

    signIn(state?: any): void {
	state = state || {};
	state.nav = this.nav;
	this.userManager.signinRedirect({ state: state });
    }

    signInRedirect(url?: string): Promise<User> {
	return this.userManager.signinRedirectCallback();
    }

    getUser(): Promise<User> {
	return this.userManager.getUser();
    }

    signOut(): void {
	this.userManager.signoutRedirect();
    }
}