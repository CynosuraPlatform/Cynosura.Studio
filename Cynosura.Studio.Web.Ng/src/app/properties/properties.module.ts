import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { PropertiesComponent } from "./properties.component";
import { PropertiesPopupComponent } from "./properties-popup.component";
import { PropertiesService } from "./properties.service";

@NgModule({
    declarations: [
        PropertiesComponent,
        PropertiesPopupComponent
    ],
    imports: [
        CoreModule
    ],
    providers: [
        PropertiesService
    ],
    exports: [
        PropertiesComponent
    ],
    entryComponents: [
        PropertiesPopupComponent
    ],
})
export class PropertiesModule {

}

