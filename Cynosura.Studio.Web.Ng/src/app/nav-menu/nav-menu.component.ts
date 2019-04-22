import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService } from "../auth/auth.service";
import { MenuService } from "./menu.service";
import { LoadingService } from "../core/loading.service";
import { Menu } from "./menu.model";

@Component({
    selector: "app-nav-menu",
    templateUrl: "./nav-menu.component.html",
    styleUrls: ["./nav-menu.component.scss"]
})
export class NavMenuComponent implements OnInit {
    menu: Menu;
    isExpanded = false;
    loggedIn = false;
    isLoading = false;

    constructor(private menuService: MenuService,
                private authService: AuthService,
                private loadingService: LoadingService,
                private router: Router) {
        loadingService
            .onLoadingChanged
            .subscribe(isLoading => this.isLoading = isLoading);
    }

    ngOnInit(): void {
        this.authService.loggedIn$.subscribe(loggedIn => this.loggedIn = loggedIn);
        this.authService.state$.subscribe(state => {
            this.menuService.getMenu().then(menu => {
                this.menu = menu;
            });
        });
    }

    logout() {
        this.authService.logout();
        this.router.navigate(["/"]);
    }

    collapse() {
        this.isExpanded = false;
    }

    toggle() {
        this.isExpanded = !this.isExpanded;
    }
}
