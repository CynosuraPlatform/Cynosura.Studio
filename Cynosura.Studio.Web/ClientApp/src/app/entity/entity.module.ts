import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { SolutionModule } from "../solution/solution.module";
import { FieldModule } from "../field/field.module";

import { EntityListComponent } from "./list.component";
import { EntityEditComponent } from "./edit.component";

import { EntityService } from "./entity.service";

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
        SolutionModule,
        FieldModule
    ],
    providers: [
        EntityService
    ]
})
export class EntityModule {

}
