import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { EnumValueCoreModule } from "../enum-value-core/enum-value-core.module";
import { EnumCoreModule } from "../enum-core/enum-core.module";

import { EnumValueListComponent } from "./enum-value-list.component";
import { EnumValueEditComponent } from "./enum-value-edit.component";

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

