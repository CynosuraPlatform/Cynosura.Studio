import { NgModule } from '@angular/core';

import { CoreModule } from '../core/core.module';
import { FieldShowComponent } from './field-show.component';

@NgModule({
    declarations: [
        FieldShowComponent
    ],
    imports: [
        CoreModule,
    ],
    providers: [
    ],
    exports: [
        FieldShowComponent
    ]
})
export class FieldCoreModule {

}

