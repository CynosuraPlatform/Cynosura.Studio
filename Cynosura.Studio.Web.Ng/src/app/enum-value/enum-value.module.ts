import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { TranslocoRootModule } from '../transloco-root.module';
import { EnumValueCoreModule } from '../enum-value-core/enum-value-core.module';
import { EnumCoreModule } from '../enum-core/enum-core.module';
import { PropertiesModule } from '../properties/properties.module';

import { EnumValueListComponent } from './enum-value-list.component';
import { EnumValueEditComponent } from './enum-value-edit.component';
import { EnumValueViewComponent } from './enum-value-view.component';

@NgModule({
    declarations: [
        EnumValueListComponent,
        EnumValueEditComponent,
        EnumValueViewComponent,
    ],
    imports: [
        RouterModule,
        CoreModule,
        TranslocoRootModule,
        EnumCoreModule,
        EnumValueCoreModule,
        PropertiesModule
    ],
    exports: [
        EnumValueListComponent,
    ],
    providers: [
    ],
    entryComponents: [
        EnumValueEditComponent
    ]
})
export class EnumValueModule {

}

