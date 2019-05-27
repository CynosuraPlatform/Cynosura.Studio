import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { MaterialModule } from "../material.module";

import { UserService } from "./user.service";
import { UserSelectComponent } from "./user-select.component";
import { UserShowComponent } from "./user-show.component";

@NgModule({
    declarations: [
        UserSelectComponent,
        UserShowComponent
    ],
    imports: [
        CoreModule,
        MaterialModule
    ],
    providers: [
        UserService
    ],
    exports: [
        MaterialModule,
        UserSelectComponent,
        UserShowComponent
    ]
})
export class UserCoreModule {

}
