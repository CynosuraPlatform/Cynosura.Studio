import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { HTTP_INTERCEPTORS } from "@angular/common/http";

import { AuthInterceptor } from "./auth.interceptor";
import { ErrorInterceptor } from "./error.interceptor";
import { LoadingInterceptor } from "./loading.interceptor";

import { BoolPipe } from "./pipes/bool.pipe";

import { ErrorHandlerComponent } from "./error-handler.component";
import { ModelValidatorComponent } from "./model-validator.component";
import { PagerComponent } from "./pager.component";
import { TextEditComponent } from "./controls/text.edit.component";
import { TextViewComponent } from "./controls/text.view.component";
import { NumberEditComponent } from "./controls/number.edit.component";
import { NumberViewComponent } from "./controls/number.view.component";
import { BoolEditComponent } from "./controls/bool.edit.component";
import { BoolViewComponent } from "./controls/bool.view.component";
import { DateTimeEditComponent } from "./controls/datetime.edit.component";
import { DateTimeViewComponent } from "./controls/datetime.view.component";
import { DateEditComponent } from "./controls/date.edit.component";
import { DateViewComponent } from "./controls/date.view.component";
import { TimeEditComponent } from "./controls/time.edit.component";
import { TimeViewComponent } from "./controls/time.view.component";

import { AuthService } from "./services/auth.service";
import { MenuService } from "./services/menu.service";
import { LoadingService } from "./loading.service";
import { StoreService } from "./store.service";

import { ModalHelper } from "./modal.helper";

@NgModule({
    declarations: [
        BoolPipe,
        ErrorHandlerComponent,
        ModelValidatorComponent,
        PagerComponent,
        TextEditComponent,
        TextViewComponent,
        NumberEditComponent,
        NumberViewComponent,
        BoolEditComponent,
        BoolViewComponent,
        DateTimeEditComponent,
        DateTimeViewComponent,
        DateEditComponent,
        DateViewComponent,
        TimeEditComponent,
        TimeViewComponent
    ],
    imports: [
        CommonModule,
        FormsModule
    ],
    providers: [
        StoreService,
        AuthService,
        MenuService,
        LoadingService,
        ModalHelper,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: LoadingInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        }
    ],
    exports: [
        CommonModule,
        FormsModule,
        BoolPipe,
        ErrorHandlerComponent,
        ModelValidatorComponent,
        PagerComponent,
        TextEditComponent,
        TextViewComponent,
        NumberEditComponent,
        NumberViewComponent,
        BoolEditComponent,
        BoolViewComponent,
        DateTimeEditComponent,
        DateTimeViewComponent,
        DateEditComponent,
        DateViewComponent,
        TimeEditComponent,
        TimeViewComponent
    ]
})
export class CoreModule {

}
