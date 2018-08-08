import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { EntityCoreModule } from "../entity-core/entity-core.module";

import { FieldListComponent } from "./list.component";
import { FieldEditComponent } from "./edit.component";

@NgModule({
    declarations: [
        FieldListComponent,
        FieldEditComponent
    ],
    imports: [
        CoreModule,
        EntityCoreModule
    ],
    providers: [
    ],
    exports: [
        FieldListComponent
    ]
})
export class FieldModule {

}
