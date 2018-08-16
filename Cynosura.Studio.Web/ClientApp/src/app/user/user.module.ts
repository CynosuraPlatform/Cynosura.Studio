import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { UserCoreModule } from "../user-core/user-core.module";
import { RoleCoreModule } from "../role-core/role-core.module";

import { UserListComponent } from "./list.component";
import { UserEditComponent } from "./edit.component";

@NgModule({
    declarations: [
        UserListComponent,
        UserEditComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "", component: UserListComponent },
            { path: ":id", component: UserEditComponent }
        ]),
        CoreModule,
        UserCoreModule,
        RoleCoreModule
    ],
    providers: [
    ]
})
export class UserModule {

}
