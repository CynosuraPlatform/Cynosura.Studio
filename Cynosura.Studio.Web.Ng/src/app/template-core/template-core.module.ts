import { NgModule } from '@angular/core';

import { CoreModule } from '../core/core.module';

import { TemplateService } from '../template-core/template-service';
import { TemplateSelectComponent } from './template-select.component';
import { TemplateVersionSelectComponent } from './template-version-select.component';

@NgModule({
    declarations: [
        TemplateSelectComponent,
        TemplateVersionSelectComponent
    ],
    imports: [
        CoreModule,
    ],
    providers: [
        TemplateService
    ],
    exports: [
        TemplateSelectComponent,
        TemplateVersionSelectComponent
    ]
})
export class TemplateCoreModule {

}
