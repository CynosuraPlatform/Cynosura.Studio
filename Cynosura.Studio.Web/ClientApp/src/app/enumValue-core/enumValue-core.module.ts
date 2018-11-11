import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";

import { EnumValueService } from "./enumValue.service";
import { EnumValueSelectComponent } from "./select.component";

@NgModule({
    declarations: [
        EnumValueSelectComponent
    ],
    imports: [
        CoreModule
    ],
    providers: [
        EnumValueService
    ],
    exports: [
        EnumValueSelectComponent
    ]
})
export class EnumValueCoreModule {

}