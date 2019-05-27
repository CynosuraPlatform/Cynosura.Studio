import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { MaterialModule } from "../material.module";

import { RoleService } from "./role.service";
import { RoleSelectComponent } from "./role-select.component";
import { RoleShowComponent } from "./role-show.component";

@NgModule({
    declarations: [
        RoleSelectComponent,
        RoleShowComponent
    ],
    imports: [
        CoreModule,
        MaterialModule
    ],
    providers: [
        RoleService
    ],
    exports: [
        MaterialModule,
        RoleSelectComponent,
        RoleShowComponent
    ]
})
export class RoleCoreModule {

}
