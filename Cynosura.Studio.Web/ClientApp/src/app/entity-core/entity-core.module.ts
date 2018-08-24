import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";

import { EntityService } from "./entity.service";
import { EntitySelectComponent } from "./select.component";

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