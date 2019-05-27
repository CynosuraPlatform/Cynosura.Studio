import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { HTTP_INTERCEPTORS } from "@angular/common/http";

import { AuthInterceptor } from "./auth.interceptor";

import { LoginComponent } from "./login.component";

import { AuthService } from "./auth.service";
import { MaterialModule } from "../material.module";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule.forChild([
            { path: "login", component: LoginComponent }
        ]),
        MaterialModule
    ],
    exports: [
        MaterialModule
    ],
    declarations: [
        LoginComponent
    ],
    providers: [
        AuthService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        }
    ],
    schemas: [

    ]
})
export class AuthModule { }
