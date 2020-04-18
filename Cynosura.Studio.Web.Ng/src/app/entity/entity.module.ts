import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { EntityCoreModule } from '../entity-core/entity-core.module';
import { SolutionCoreModule } from '../solution-core/solution-core.module';
import { FieldModule } from '../field/field.module';
import { PropertiesModule } from '../properties/properties.module';

import { EntityListComponent } from './entity-list.component';
import { EntityEditComponent } from './entity-edit.component';
import { EntityViewComponent } from './entity-view.component';

@NgModule({
    declarations: [
        EntityListComponent,
        EntityEditComponent,
        EntityViewComponent,
    ],
    imports: [
        RouterModule.forChild([
            { path: '', component: EntityListComponent },
            { path: ':id', component: EntityViewComponent }
        ]),
        CoreModule,
        EntityCoreModule,
        SolutionCoreModule,
        FieldModule,
        PropertiesModule
    ],
    providers: [
    ],
    entryComponents: [
        EntityEditComponent
    ],
    exports: [
    ]
})
export class EntityModule {

}

