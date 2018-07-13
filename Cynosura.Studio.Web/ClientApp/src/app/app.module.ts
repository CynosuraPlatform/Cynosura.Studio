import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule, Route } from "@angular/router";

import { ModalModule } from "ngx-modialog";
import { BootstrapModalModule } from "ngx-modialog/plugins/bootstrap";

import { AuthModule } from "./auth/auth.module";
import { RoleModule } from "./role/role.module";
import { UserModule } from "./user/user.module";
import { ProjectModule } from "./project/project.module";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";

var routes: Route[] = [
    { path: "", component: HomeComponent, pathMatch: "full" }
];

//routes.push({ path: "user", loadChildren: "./user/user.module#UserModule" });
//routes.push({ path: "role", loadChildren: "./role/role.module#RoleModule" });

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot(routes),
        ModalModule.forRoot(),
        BootstrapModalModule,
        AuthModule,
        RoleModule,
        UserModule,
        ProjectModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
