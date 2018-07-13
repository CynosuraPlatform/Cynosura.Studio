import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";

import { RoleListComponent } from "./list.component";
import { RoleEditComponent } from "./edit.component";

import { RoleService } from "./role.service";

@NgModule({
    declarations: [
        RoleListComponent,
        RoleEditComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "role", component: RoleListComponent },
            { path: "role/:id", component: RoleEditComponent }
        ]),
        CoreModule
    ],
    providers: [
        RoleService
    ]
})
export class RoleModule {

}
