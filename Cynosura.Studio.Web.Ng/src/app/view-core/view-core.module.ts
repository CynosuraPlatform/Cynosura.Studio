import { NgModule } from '@angular/core';

import { CoreModule } from '../core/core.module';

import { ViewService } from './view.service';
import { ViewSelectComponent } from './view-select.component';
import { ViewShowComponent } from './view-show.component';

@NgModule({
    declarations: [
        ViewSelectComponent,
        ViewShowComponent
    ],
    imports: [
        CoreModule,
    ],
    providers: [
        ViewService
    ],
    exports: [
        ViewSelectComponent,
        ViewShowComponent
    ]
})
export class ViewCoreModule {

}
