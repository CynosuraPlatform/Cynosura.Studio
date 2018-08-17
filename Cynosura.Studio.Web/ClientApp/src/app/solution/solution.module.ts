import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { SolutionCoreModule } from "../solution-core/solution-core.module";

import { SolutionListComponent } from "./list.component";
import { SolutionEditComponent } from "./edit.component";

@NgModule({
    declarations: [
        SolutionListComponent,
        SolutionEditComponent
    ],
    imports: [
		RouterModule.forChild([
            { path: "", component: SolutionListComponent },
            { path: ":id", component: SolutionEditComponent }
        ]),
        CoreModule,
        SolutionCoreModule
    ],
    providers: [
    ],
    exports: [
    ]
})
export class SolutionModule {

}
