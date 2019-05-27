import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { MaterialModule } from "../material.module";
import { FieldShowComponent } from "./field-show.component";

@NgModule({
    declarations: [
        FieldShowComponent
    ],
    imports: [
        CoreModule,
        MaterialModule
    ],
    providers: [
    ],
    exports: [
        MaterialModule,
        FieldShowComponent
    ]
})
export class FieldCoreModule {

}

