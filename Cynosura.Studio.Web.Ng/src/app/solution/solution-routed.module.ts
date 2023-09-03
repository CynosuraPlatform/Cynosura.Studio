import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SolutionsComponent } from './solutions.component';
import { SolutionViewComponent } from './solution-view.component';
import { SolutionModule } from '../solution/solution.module';

@NgModule({
  declarations: [
  ],
  imports: [
    RouterModule.forChild([
      { path: '', component: SolutionsComponent },
      { path: ':id', component: SolutionViewComponent }
    ]),
    SolutionModule,
  ],
  providers: [
  ],
  entryComponents: [
  ]
})
export class SolutionRoutedModule {

}
