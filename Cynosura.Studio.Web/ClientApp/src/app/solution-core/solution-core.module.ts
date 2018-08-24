import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";

import { SolutionService } from "./solution.service";
import { SolutionSelectComponent } from "./select.component";

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