import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { RegisterComponent } from "./register.component";
import { AccountService } from "./account.service";


@NgModule({
    declarations: [
        RegisterComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "register", component: RegisterComponent },
        ]),
        CoreModule
    ],
    providers: [
        AccountService
    ],
    exports: [
    ]
})
export class AccountModule {

}
