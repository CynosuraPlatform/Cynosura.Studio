import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { MaterialModule } from "../material.module";

import { SolutionService } from "./solution.service";
import { SolutionSelectComponent } from "./solution-select.component";
import { SolutionShowComponent } from "./solution-show.component";
import { TemplateService } from "./template-service";

@NgModule({
    declarations: [
        SolutionSelectComponent,
        SolutionShowComponent
    ],
    imports: [
        CoreModule,
        MaterialModule
    ],
    providers: [
        SolutionService,
        TemplateService
    ],
    exports: [
        MaterialModule,
        SolutionSelectComponent,
        SolutionShowComponent
    ]
})
export class SolutionCoreModule {

}
