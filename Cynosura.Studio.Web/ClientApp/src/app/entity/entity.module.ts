import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { SolutionModule } from "../solution/solution.module";

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
        SolutionModule
    ],
    providers: [
        EntityService
    ]
})
export class EntityModule {

}
