import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { ProjectModule } from "../project/project.module";

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
        ProjectModule
    ],
    providers: [
        EntityService
    ]
})
export class EntityModule {

}
