import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';

import { ProfileService } from './profile.service';
import { ProfileEditComponent } from './edit.component';

@NgModule({
    declarations: [
        ProfileEditComponent,
    ],
    imports: [
        CoreModule,
        RouterModule,
    ],
    providers: [
        ProfileService
    ],
    exports: [
        ProfileEditComponent
    ]
})
export class ProfileCommonModule {

}
