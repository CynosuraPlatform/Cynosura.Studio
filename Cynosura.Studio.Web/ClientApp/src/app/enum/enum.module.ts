import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { EnumCoreModule } from "../enum-core/enum-core.module";
import { SolutionCoreModule } from "../solution-core/solution-core.module";
import { EnumValueModule } from "../enumValue/enumValue.module";

import { EnumListComponent } from "./list.component";
import { EnumEditComponent } from "./edit.component";

@NgModule({
    declarations: [
        EnumListComponent,
        EnumEditComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "", component: EnumListComponent },
            { path: ":id", component: EnumEditComponent }
        ]),
        CoreModule,
        EnumCoreModule,
        SolutionCoreModule,
        EnumValueModule
    ],
    providers: [
    ]
})
export class EnumModule {

}
