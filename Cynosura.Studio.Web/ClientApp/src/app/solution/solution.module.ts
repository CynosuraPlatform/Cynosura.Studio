import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";

import { SolutionListComponent } from "./list.component";
import { SolutionEditComponent } from "./edit.component";
import { SolutionSelectComponent } from "./select.component";

import { SolutionService } from "./solution.service";

@NgModule({
    declarations: [
        SolutionListComponent,
        SolutionEditComponent,
        SolutionSelectComponent
    ],
    imports: [
		RouterModule.forChild([
            { path: "solution", component: SolutionListComponent },
            { path: "solution/:id", component: SolutionEditComponent }
        ]),
		CoreModule
    ],
    providers: [
        SolutionService
    ],
    exports: [
        SolutionSelectComponent
    ]
})
export class SolutionModule {

}
