import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { EntitiesComponent } from './entities.component';
import { EntityViewComponent } from './entity-view.component';
import { EntityModule } from '../entity/entity.module';

@NgModule({
  declarations: [
  ],
  imports: [
    RouterModule.forChild([
      { path: '', component: EntitiesComponent },
      { path: ':id', component: EntityViewComponent }
    ]),
    EntityModule,
  ],
  providers: [
  ]
})
export class EntityRoutedModule {

}
