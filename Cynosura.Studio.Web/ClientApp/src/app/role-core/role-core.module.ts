import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";

import { RoleService } from "./role.service";
import { RoleSelectComponent } from "./select.component";

@NgModule({
    declarations: [
        RoleSelectComponent
    ],
    imports: [
        CoreModule
    ],
    providers: [
        RoleService
    ],
    exports: [
        RoleSelectComponent
    ]
})
export class RoleCoreModule {

}