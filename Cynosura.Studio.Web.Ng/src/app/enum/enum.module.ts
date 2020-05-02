import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { EnumCoreModule } from '../enum-core/enum-core.module';
import { SolutionCoreModule } from '../solution-core/solution-core.module';
import { EnumValueModule } from '../enum-value/enum-value.module';
import { PropertiesModule } from '../properties/properties.module';

import { EnumListComponent } from './enum-list.component';
import { EnumEditComponent } from './enum-edit.component';
import { EnumViewComponent } from './enum-view.component';

@NgModule({
    declarations: [
        EnumListComponent,
        EnumEditComponent,
        EnumViewComponent,
    ],
    imports: [
        RouterModule.forChild([
            { path: '', component: EnumListComponent },
            { path: ':id', component: EnumViewComponent }
        ]),
        CoreModule,
        EnumCoreModule,
        SolutionCoreModule,
        EnumValueModule,
        PropertiesModule
    ],
    providers: [
    ],
    entryComponents: [
        EnumEditComponent
    ]
})
export class EnumModule {

}

