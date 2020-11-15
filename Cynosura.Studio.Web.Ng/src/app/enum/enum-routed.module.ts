import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { EnumsComponent } from './enums.component';
import { EnumViewComponent } from './enum-view.component';
import { EnumModule } from '../enum/enum.module';

@NgModule({
    declarations: [
    ],
    imports: [
        RouterModule.forChild([
            { path: '', component: EnumsComponent },
            { path: ':id', component: EnumViewComponent }
        ]),
        EnumModule,
    ],
    providers: [
    ],
    entryComponents: [
    ]
})
export class EnumRoutedModule {

}
