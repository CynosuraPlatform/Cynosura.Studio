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
        CoreModule,
        EnumCoreModule,
        EnumValueCoreModule
    ],
    providers: [
    ],
    exports: [
        EnumValueListComponent
    ]
})
export class EnumValueModule {

}
