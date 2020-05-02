import { NgModule } from '@angular/core';

import { CoreModule } from '../core/core.module';

import { SolutionService } from './solution.service';
import { SolutionSelectComponent } from './solution-select.component';
import { SolutionShowComponent } from './solution-show.component';

@NgModule({
    declarations: [
        SolutionSelectComponent,
        SolutionShowComponent
    ],
    imports: [
        CoreModule,
    ],
    providers: [
        SolutionService,
    ],
    exports: [
        SolutionSelectComponent,
        SolutionShowComponent
    ]
})
export class SolutionCoreModule {

}
