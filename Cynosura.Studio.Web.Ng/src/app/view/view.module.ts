import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { ViewCoreModule } from '../view-core/view-core.module';
import { SolutionCoreModule } from '../solution-core/solution-core.module';

import { ViewListComponent } from './view-list.component';
import { ViewEditComponent } from './view-edit.component';
import { ViewViewComponent } from './view-view.component';

@NgModule({
    declarations: [
        ViewListComponent,
        ViewEditComponent,
        ViewViewComponent,
    ],
    imports: [
        RouterModule.forChild([
            { path: '', component: ViewListComponent },
            { path: ':id', component: ViewViewComponent }
        ]),
        CoreModule,
        ViewCoreModule,
        SolutionCoreModule,
    ],
    providers: [
    ],
    entryComponents: [
        ViewEditComponent
    ]
})
export class ViewModule {

}
