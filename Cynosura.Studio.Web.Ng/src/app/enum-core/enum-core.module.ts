import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { MaterialModule } from "../material.module";

import { EnumService } from "./enum.service";
import { EnumSelectComponent } from "./enum-select.component";
import { EnumShowComponent } from "./enum-show.component";

@NgModule({
    declarations: [
        EnumSelectComponent,
        EnumShowComponent
    ],
    imports: [
        CoreModule,
        MaterialModule
    ],
    providers: [
        EnumService
    ],
    exports: [
        MaterialModule,
        EnumSelectComponent,
        EnumShowComponent
    ]
})
export class EnumCoreModule {

}
