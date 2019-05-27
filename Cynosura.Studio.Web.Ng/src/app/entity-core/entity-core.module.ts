import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { MaterialModule } from "../material.module";

import { EntityService } from "./entity.service";
import { EntitySelectComponent } from "./entity-select.component";
import { EntityShowComponent } from "./entity-show.component";

@NgModule({
    declarations: [
        EntitySelectComponent,
        EntityShowComponent
    ],
    imports: [
        CoreModule,
        MaterialModule
    ],
    providers: [
        EntityService
    ],
    exports: [
        MaterialModule,
        EntitySelectComponent,
        EntityShowComponent
    ]
})
export class EntityCoreModule {

}
