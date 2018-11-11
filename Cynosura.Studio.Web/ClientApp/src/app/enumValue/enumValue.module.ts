import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { EnumValueCoreModule } from "../enumValue-core/enumValue-core.module";
import { EnumCoreModule } from "../enum-core/enum-core.module";

import { EnumValueListComponent } from "./list.component";
import { EnumValueEditComponent } from "./edit.component";

@NgModule({
    declarations: [
        EnumValueListComponent,
        EnumValueEditComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "", component: EnumValueListComponent },
            { path: ":id", component: EnumValueEditComponent }
        ]),
        CoreModule,
        EnumCoreModule,
        EnumValueCoreModule
    ],
    providers: [
    ]
})
export class EnumValueModule {

}