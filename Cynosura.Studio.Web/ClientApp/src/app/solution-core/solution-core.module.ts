import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";

import { SolutionSelectComponent } from "./select.component";

import { SolutionService } from "./solution.service";

@NgModule({
    declarations: [
        SolutionSelectComponent
    ],
    imports: [
		CoreModule
    ],
    providers: [
        SolutionService
    ],
    exports: [
        SolutionSelectComponent
    ]
})
export class SolutionCoreModule {

}
