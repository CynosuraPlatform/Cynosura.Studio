import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { EnumCoreModule } from "../enum-core/enum-core.module";
import { SolutionCoreModule } from "../solution-core/solution-core.module";
import { EnumValueModule } from "../enum-value/enum-value.module";
import { PropertiesModule } from "../properties/properties.module";

import { EnumListComponent } from "./enum-list.component";
import { EnumEditComponent } from "./enum-edit.component";

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
        EnumValueModule,
        PropertiesModule
    ],
    providers: [
    ]
})
export class EnumModule {

}

