import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";

import { ProjectListComponent } from "./list.component";
import { ProjectEditComponent } from "./edit.component";
import { ProjectSelectComponent } from "./select.component";

import { ProjectService } from "./project.service";

@NgModule({
    declarations: [
        ProjectListComponent,
        ProjectEditComponent,
        ProjectSelectComponent
    ],
    imports: [
		RouterModule.forChild([
            { path: "project", component: ProjectListComponent },
            { path: "project/:id", component: ProjectEditComponent }
        ]),
		CoreModule
    ],
    providers: [
        ProjectService
    ],
    exports: [
        ProjectSelectComponent
    ]
})
export class ProjectModule {

}
