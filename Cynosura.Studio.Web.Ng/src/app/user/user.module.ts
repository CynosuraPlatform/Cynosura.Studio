import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { UserCoreModule } from "../user-core/user-core.module";
import { RoleCoreModule } from "../role-core/role-core.module";

import { UserListComponent } from "./user-list.component";
import { UserEditComponent } from "./user-edit.component";
import { MaterialModule } from "../material.module";

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
        RoleCoreModule,
        MaterialModule
    ],
    exports: [
        MaterialModule
    ],
    providers: [
    ]
})
export class UserModule {

}
