import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";

import { EntitySelectComponent } from "./select.component";

import { EntityService } from "./entity.service";

@NgModule({
    declarations: [
        EntitySelectComponent
    ],
    imports: [
        CoreModule
    ],
    providers: [
        EntityService
    ],
    exports: [
        EntitySelectComponent
    ]
})
export class EntityCoreModule {

}
