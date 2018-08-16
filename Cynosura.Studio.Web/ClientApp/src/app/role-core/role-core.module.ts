import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { RoleService } from "./role.service";

@NgModule({
    declarations: [
    ],
    imports: [
        CoreModule
    ],
    providers: [
        RoleService
    ]
})
export class RoleCoreModule {

}
