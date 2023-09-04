import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { TranslocoRootModule } from '../transloco-root.module';
import { SolutionCoreModule } from '../solution-core/solution-core.module';
import { TemplateCoreModule } from '../template-core/template-core.module';

import { SolutionListComponent } from './solution-list.component';
import { SolutionEditComponent } from './solution-edit.component';
import { SolutionOpenComponent } from './solution-open.component';
import { SolutionViewComponent } from './solution-view.component';
import { SolutionsComponent } from './solutions.component';

@NgModule({
  declarations: [
    SolutionListComponent,
    SolutionEditComponent,
    SolutionOpenComponent,
    SolutionViewComponent,
    SolutionsComponent,
  ],
  imports: [
    RouterModule,
    CoreModule,
    TranslocoRootModule,
    SolutionCoreModule,
    TemplateCoreModule
  ],
  exports: [
    SolutionListComponent,
  ],
  providers: [
  ],
})
export class SolutionModule {

}
