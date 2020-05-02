import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { FieldCoreModule } from '../field-core/field-core.module';
import { EntityCoreModule } from '../entity-core/entity-core.module';
import { EnumCoreModule } from '../enum-core/enum-core.module';
import { PropertiesModule } from '../properties/properties.module';

import { FieldListComponent } from './field-list.component';
import { FieldEditComponent } from './field-edit.component';
import { FieldViewComponent } from './field-view.component';

@NgModule({
    declarations: [
        FieldListComponent,
        FieldEditComponent,
        FieldViewComponent,
    ],
    imports: [
        CoreModule,
        EnumCoreModule,
        FieldCoreModule,
        EntityCoreModule,
        PropertiesModule
    ],
    providers: [
    ],
    exports: [
        FieldListComponent
    ],
    entryComponents: [
        FieldEditComponent
    ]
})
export class FieldModule {

}

