import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { MaterialModule } from "../material.module";
import { EnumValueShowComponent } from "./enum-value-show.component";

@NgModule({
    declarations: [
        EnumValueShowComponent
    ],
    imports: [
        CoreModule,
        MaterialModule
    ],
    providers: [
    ],
    exports: [
        MaterialModule,
        EnumValueShowComponent
    ]
})
export class EnumValueCoreModule {

}

