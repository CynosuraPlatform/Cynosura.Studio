import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { SolutionCoreModule } from '../solution-core/solution-core.module';

import { SolutionListComponent } from './solution-list.component';
import { SolutionEditComponent } from './solution-edit.component';
import { SolutionOpenComponent } from './solution-open.component';
import { SolutionViewComponent } from './solution-view.component';

@NgModule({
    declarations: [
        SolutionListComponent,
        SolutionEditComponent,
        SolutionOpenComponent,
        SolutionViewComponent,
    ],
    imports: [
        RouterModule.forChild([
            { path: '', component: SolutionListComponent },
            { path: ':id', component: SolutionViewComponent }
        ]),
        CoreModule,
        SolutionCoreModule
    ],
    providers: [
    ],
    entryComponents: [
        SolutionEditComponent,
        SolutionOpenComponent
    ],
})
export class SolutionModule {

}
