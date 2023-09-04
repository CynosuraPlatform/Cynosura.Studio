import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { TranslocoRootModule } from '../transloco-root.module';
import { EntityCoreModule } from '../entity-core/entity-core.module';
import { SolutionCoreModule } from '../solution-core/solution-core.module';
import { FieldModule } from '../field/field.module';
import { PropertiesModule } from '../properties/properties.module';

import { EntityListComponent } from './entity-list.component';
import { EntityEditComponent } from './entity-edit.component';
import { EntityViewComponent } from './entity-view.component';
import { EntitiesComponent } from './entities.component';

@NgModule({
    declarations: [
        EntityListComponent,
        EntityEditComponent,
        EntityViewComponent,
        EntitiesComponent,
    ],
    imports: [
        RouterModule,
        CoreModule,
        TranslocoRootModule,
        EntityCoreModule,
        SolutionCoreModule,
        FieldModule,
        PropertiesModule
    ],
    exports: [
        EntityListComponent,
    ],
    providers: [
    ]
})
export class EntityModule {

}

