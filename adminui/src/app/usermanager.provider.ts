import { UserManager, UserManagerSettings } from "oidc-client";
import { environment } from "environments/environment";


export function newManager() {
    return new UserManager({
	authority: environment.authUri,
	client_id: 'js',
	redirect_uri: environment.appUri + 'login',
	post_logout_redirect_uri: environment.appUri,
	response_type: 'id_token token',
	scope: 'openid adminapi',
	silent_redirect_uri: environment.appUri + 'login',
	automaticSilentRenew: true,
	filterProtocolClaims: true,
	loadUserInfo: true
    });
}

export let userManagerProvider =
    {
	provide: UserManager,
	/*
	Registering UserManger like this because the UserManager constructor takes an interface, 
	which Angular's DI cannot handle without modification to UserMananger itself.
	https://angular.io/docs/ts/latest/guide/dependency-injection.html#!#injection-token
	*/
	useFactory: newManager
    };