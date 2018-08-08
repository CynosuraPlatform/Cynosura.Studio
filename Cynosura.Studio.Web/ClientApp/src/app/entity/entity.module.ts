import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { EntityCoreModule } from "../entity-core/entity-core.module";
import { SolutionModule } from "../solution/solution.module";
import { FieldModule } from "../field/field.module";

import { EntityListComponent } from "./list.component";
import { EntityEditComponent } from "./edit.component";

@NgModule({
    declarations: [
        EntityListComponent,
        EntityEditComponent
    ],
    imports: [
		RouterModule.forChild([
            { path: "entity", component: EntityListComponent },
            { path: "entity/:id", component: EntityEditComponent }
        ]),
        CoreModule,
        EntityCoreModule,
        SolutionModule,
        FieldModule
    ],
    providers: [
        
    ],
    exports: [
        
    ]
})
export class EntityModule {

}
