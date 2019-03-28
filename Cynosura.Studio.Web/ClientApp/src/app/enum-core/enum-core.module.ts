import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";

import { EnumService } from "./enum.service";
import { EnumSelectComponent } from "./enum-select.component";
import { EnumShowComponent } from "./enum-show.component";

@NgModule({
    declarations: [
        EnumSelectComponent,
        EnumShowComponent
    ],
    imports: [
        CoreModule
    ],
    providers: [
        EnumService
    ],
    exports: [
        EnumSelectComponent,
        EnumShowComponent
    ]
})
export class EnumCoreModule {

}
