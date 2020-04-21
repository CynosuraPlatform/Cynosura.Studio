import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Route } from '@angular/router';

import { MaterialModule } from './material.module';
import { CoreModule } from './core/core.module';
import { AccountModule } from './account/account.module';

import { ConfigService } from './config/config.service';
import { MenuService } from './nav-menu/menu.service';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent
    ],
    imports: [
        BrowserAnimationsModule,
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [] },
// ADD ROUTES HERE
            {
                path: 'view',
                canActivate: [],
                loadChildren: () => import('./view/view.module').then(m => m.ViewModule)
            },
            {
                path: 'entity',
                canActivate: [],
                loadChildren: () => import('./entity/entity.module').then(m => m.EntityModule)
            },
            {
                path: 'enum',
                canActivate: [],
                loadChildren: () => import('./enum/enum.module').then(m => m.EnumModule)
            },
            {
                path: 'solution',
                canActivate: [],
                loadChildren: () => import('./solution/solution.module').then(m => m.SolutionModule)
            },
            {
                path: 'role',
                canActivate: [],
                loadChildren: () => import('./role/role.module').then(m => m.RoleModule)
            },
            {
                path: 'user',
                canActivate: [],
                loadChildren: () => import('./user/user.module').then(m => m.UserModule)
            },
        ]),
        MaterialModule,
        CoreModule,
        AccountModule,
    ],
    exports: [
        MaterialModule
    ],
    providers: [
        ConfigService,
        MenuService,
        {
            provide: APP_INITIALIZER,
            useFactory: (configService: ConfigService) => {
                return () => configService.load();
            },
            multi: true,
            deps: [ConfigService]
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
