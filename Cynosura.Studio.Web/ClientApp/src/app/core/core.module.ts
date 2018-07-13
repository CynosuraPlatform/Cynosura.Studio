import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { HTTP_INTERCEPTORS } from "@angular/common/http";

import { AuthInterceptor } from "./auth.interceptor";
import { ErrorInterceptor } from "./error.interceptor";

import { BoolPipe } from "./pipes/bool.pipe";

import { BoolComponent } from "./bool.component";
import { ErrorHandlerComponent } from "./error-handler.component";
import { ModelValidatorComponent } from "./model-validator.component";
import { PagerComponent } from "./pager.component";

import { AuthService } from "./services/auth.service";
import { MenuService } from "./services/menu.service";
import { StoreService } from "./store.service";

@NgModule({
    declarations: [
        BoolPipe,
        BoolComponent,
        ErrorHandlerComponent,
        ModelValidatorComponent,
        PagerComponent
    ],
    imports: [
        CommonModule,
        FormsModule
    ],
    providers: [
        StoreService,
        AuthService,
        MenuService,
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
        BoolComponent,
        ErrorHandlerComponent,
        ModelValidatorComponent,
        PagerComponent
    ]
})
export class CoreModule {

}
