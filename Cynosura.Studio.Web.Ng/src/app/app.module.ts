import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Route } from '@angular/router';
import { MatPaginatorIntl } from '@angular/material/paginator';

import { MaterialModule } from './material.module';
import { CoreModule } from './core/core.module';

import { ConfigService } from './config/config.service';
import { MenuService } from './nav-menu/menu.service';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { MatPaginatorIntlCustom } from './mat-paginator-intl';
import { TranslocoRootModule } from './transloco-root.module';

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
        loadChildren: () => import('./view/view-routed.module').then(m => m.ViewRoutedModule)
      },
      {
        path: 'entity',
        loadChildren: () => import('./entity/entity-routed.module').then(m => m.EntityRoutedModule)
      },
      {
        path: 'enum',
        loadChildren: () => import('./enum/enum-routed.module').then(m => m.EnumRoutedModule)
      },
      {
        path: 'solution',
        loadChildren: () => import('./solution/solution-routed.module').then(m => m.SolutionRoutedModule)
      },
      {
        path: 'worker-schedule-task',
        loadChildren: () => import('./worker-schedule-task/worker-schedule-task-routed.module').then(m => m.WorkerScheduleTaskRoutedModule)
      },
      {
        path: 'worker-run',
        loadChildren: () => import('./worker-run/worker-run-routed.module').then(m => m.WorkerRunRoutedModule)
      },
      {
        path: 'worker-info',
        loadChildren: () => import('./worker-info/worker-info-routed.module').then(m => m.WorkerInfoRoutedModule)
      },
      {
        path: 'file',
        loadChildren: () => import('./file/file-routed.module').then(m => m.FileRoutedModule)
      },
      {
        path: 'file-group',
        loadChildren: () => import('./file-group/file-group-routed.module').then(m => m.FileGroupRoutedModule)
      },
      {
        path: 'profile',
        loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule)
      },
      {
        path: 'role',
        loadChildren: () => import('./role/role-routed.module').then(m => m.RoleRoutedModule)
      },
      {
        path: 'user',
        loadChildren: () => import('./user/user-routed.module').then(m => m.UserRoutedModule)
      },
    ], { relativeLinkResolution: 'legacy' }),
    MaterialModule,
    CoreModule,
    TranslocoRootModule,
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
    },
    { provide: MatPaginatorIntl, useClass: MatPaginatorIntlCustom }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
