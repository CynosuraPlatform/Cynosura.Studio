import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { RoleCoreModule } from '../role-core/role-core.module';

import { RoleListComponent } from './role-list.component';
import { RoleEditComponent } from './role-edit.component';
import { RoleViewComponent } from './role-view.component';

@NgModule({
    declarations: [
        RoleListComponent,
        RoleEditComponent,
        RoleViewComponent,
    ],
    imports: [
        RouterModule.forChild([
            { path: '', component: RoleListComponent },
            { path: ':id', component: RoleViewComponent }
        ]),
        CoreModule,
        RoleCoreModule,
    ],
    exports: [
    ],
    providers: [
    ],
    entryComponents: [
        RoleEditComponent
    ]
})
export class RoleModule {

}
