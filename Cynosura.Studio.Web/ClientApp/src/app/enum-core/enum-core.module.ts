import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";

import { EnumService } from "./enum.service";
import { EnumSelectComponent } from "./select.component";

@NgModule({
    declarations: [
        EnumSelectComponent
    ],
    imports: [
        CoreModule
    ],
    providers: [
        EnumService
    ],
    exports: [
        EnumSelectComponent
    ]
})
export class EnumCoreModule {

}