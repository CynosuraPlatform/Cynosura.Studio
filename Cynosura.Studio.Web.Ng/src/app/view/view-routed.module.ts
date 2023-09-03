import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ViewsComponent } from './views.component';
import { ViewViewComponent } from './view-view.component';
import { ViewModule } from '../view/view.module';

@NgModule({
  declarations: [
  ],
  imports: [
    RouterModule.forChild([
      { path: '', component: ViewsComponent },
      { path: ':id', component: ViewViewComponent }
    ]),
    ViewModule,
  ],
  providers: [
  ],
  entryComponents: [
  ]
})
export class ViewRoutedModule {

}
