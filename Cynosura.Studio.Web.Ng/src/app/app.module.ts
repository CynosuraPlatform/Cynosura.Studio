// import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule, APP_INITIALIZER } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule, Route } from "@angular/router";

import { ModalModule } from "ngx-modialog";
import { BootstrapModalModule } from "ngx-modialog/plugins/bootstrap";

import { AuthModule } from "./auth/auth.module";
import { CoreModule } from "./core/core.module";

import { ConfigService } from "./config/config.service";
import { MenuService } from "./nav-menu/menu.service";
import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent
    ],
    imports: [
        // BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
        BrowserAnimationsModule,
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: "", component: HomeComponent, pathMatch: "full" },
// ADD ROUTES HERE
            { path: "entity", loadChildren: "./entity/entity.module#EntityModule" },
            { path: "enum", loadChildren: "./enum/enum.module#EnumModule" },
            { path: "solution", loadChildren: "./solution/solution.module#SolutionModule" }
        ]),
        ModalModule.forRoot(),
        BootstrapModalModule,
        CoreModule,
        AuthModule
    ],
    providers: [
        ConfigService,
        MenuService,
        {
            provide: APP_INITIALIZER,
            useFactory: (configService: ConfigService) => {
                return () => configService.load();
            },
            multi: true,
            deps: [ConfigService]
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
