import { NgModule } from "@angular/core";

import { CoreModule } from "../core/core.module";
import { PropertiesComponent } from "./properties.component";
import { PropertiesService } from "./properties.service";

@NgModule({
    declarations: [
        PropertiesComponent
    ],
    imports: [
        CoreModule
    ],
    providers: [
        PropertiesService
    ],
    exports: [
        PropertiesComponent
    ]
})
export class PropertiesModule {

}

