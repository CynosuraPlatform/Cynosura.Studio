import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { EnumCoreModule } from "../enum-core/enum-core.module";

import { EnumListComponent } from "./list.component";
import { EnumEditComponent } from "./edit.component";

@NgModule({
    declarations: [
        EnumListComponent,
        EnumEditComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "", component: EnumListComponent },
            { path: ":id", component: EnumEditComponent }
        ]),
        CoreModule,
        EnumCoreModule
    ],
    providers: [
    ]
})
export class EnumModule {

}