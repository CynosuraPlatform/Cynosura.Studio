import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { UserService } from "./user.service";

@NgModule({
    declarations: [
    ],
    imports: [
        CoreModule
    ],
    providers: [
        UserService
    ]
})
export class UserCoreModule {

}
