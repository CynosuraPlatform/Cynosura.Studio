import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { SolutionCoreModule } from "../solution-core/solution-core.module";

import { SolutionListComponent } from "./solution-list.component";
import { SolutionEditComponent } from "./solution-edit.component";

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
    ]
})
export class SolutionModule {

}
