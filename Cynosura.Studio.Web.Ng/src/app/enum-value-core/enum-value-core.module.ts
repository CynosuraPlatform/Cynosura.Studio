import { NgModule } from '@angular/core';

import { CoreModule } from '../core/core.module';
import { EnumValueShowComponent } from './enum-value-show.component';

@NgModule({
    declarations: [
        EnumValueShowComponent
    ],
    imports: [
        CoreModule,
    ],
    providers: [
    ],
    exports: [
        EnumValueShowComponent
    ]
})
export class EnumValueCoreModule {

}

