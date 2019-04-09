import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { EntityCoreModule } from "../entity-core/entity-core.module";
import { SolutionCoreModule } from "../solution-core/solution-core.module";
import { FieldModule } from "../field/field.module";
import { PropertiesModule } from "../properties/properties.module";

import { EntityListComponent } from "./entity-list.component";
import { EntityEditComponent } from "./entity-edit.component";

@NgModule({
    declarations: [
        EntityListComponent,
        EntityEditComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "", component: EntityListComponent },
            { path: ":id", component: EntityEditComponent }
        ]),
        CoreModule,
        EntityCoreModule,
        SolutionCoreModule,
        FieldModule,
        PropertiesModule
    ],
    providers: [
    ],
    exports: [
    ]
})
export class EntityModule {

}

