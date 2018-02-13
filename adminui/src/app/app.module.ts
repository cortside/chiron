import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppConfig } from './app.config';
import { HttpModule } from '@angular/http';

import { AppConfigInitializerProvider } from './appconfig.provider';

import { AppComponent } from './app.component';
import { UnrestrictedComponent } from './unrestricted.component';
import { RestrictedComponent } from './restricted.component';
import { AdminComponent } from './admin.component';
import { ClerkComponent } from './clerk.component';
import { LoginComponent } from './login.component';
import { LogoutComponent } from './logout.component';
import { UserComponent } from './user.component';
import { TestingComponent } from './testing.component';

import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import { UserManager } from "oidc-client";

import { AppRoutingModule } from './app-routing.module';

@NgModule({
    declarations: [
	AppComponent,
	UnrestrictedComponent,
	RestrictedComponent,
	AdminComponent,
	ClerkComponent,
	UserComponent,
	LoginComponent,
	LogoutComponent,
	TestingComponent
    ],
    imports: [
	BrowserModule,
	FormsModule,
	HttpModule,
	AppRoutingModule
    ],
    providers: [
	ApiService,
	AuthService,
	AppConfig,
	AppConfigInitializerProvider
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
