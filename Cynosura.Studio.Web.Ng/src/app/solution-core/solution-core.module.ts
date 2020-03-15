import { NgModule } from '@angular/core';

import { CoreModule } from '../core/core.module';

import { SolutionService } from './solution.service';
import { SolutionSelectComponent } from './solution-select.component';
import { SolutionShowComponent } from './solution-show.component';
import { TemplateService } from './template-service';

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
        TemplateService
    ],
    exports: [
        SolutionSelectComponent,
        SolutionShowComponent
    ]
})
export class SolutionCoreModule {

}
