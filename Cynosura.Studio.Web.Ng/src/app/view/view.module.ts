import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { TranslocoRootModule } from '../transloco-root.module';
import { ViewCoreModule } from '../view-core/view-core.module';
import { SolutionCoreModule } from '../solution-core/solution-core.module';

import { ViewListComponent } from './view-list.component';
import { ViewEditComponent } from './view-edit.component';
import { ViewViewComponent } from './view-view.component';
import { ViewsComponent } from './views.component';

@NgModule({
    declarations: [
        ViewListComponent,
        ViewEditComponent,
        ViewViewComponent,
        ViewsComponent,
    ],
    imports: [
        RouterModule,
        CoreModule,
        TranslocoRootModule,
        ViewCoreModule,
        SolutionCoreModule,
    ],
    exports: [
        ViewListComponent,
    ],
    providers: [
    ]
})
export class ViewModule {

}
