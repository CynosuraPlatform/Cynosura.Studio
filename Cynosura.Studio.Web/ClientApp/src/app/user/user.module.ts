import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";

import { UserListComponent } from "./list.component";
import { UserEditComponent } from "./edit.component";

import { UserService } from "./user.service";

@NgModule({
    declarations: [
        UserListComponent,
        UserEditComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "user", component: UserListComponent },
            { path: "user/:id", component: UserEditComponent }
        ]),
        CoreModule
    ],
    providers: [
        UserService
    ]
})
export class UserModule {

}
