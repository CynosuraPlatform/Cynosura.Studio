import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

import { LoginComponent } from "./login.component";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule.forChild([
            { path: "login", component: LoginComponent }
        ])
    ],
    declarations: [
        LoginComponent
    ],
    schemas: [

    ]
})
export class AuthModule { }
