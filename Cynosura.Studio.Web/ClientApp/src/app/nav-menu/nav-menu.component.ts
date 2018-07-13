import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';

import { AuthService } from "../core/services/auth.service";
import { MenuService } from "../core/services/menu.service";
import { Menu } from "../core/models/menu.model";

@Component({
    selector: "app-nav-menu",
    templateUrl: "./nav-menu.component.html",
    styleUrls: ["./nav-menu.component.css"]
})
export class NavMenuComponent implements OnInit {
    menu: Menu;
    isExpanded = false;
    loggedIn = false;

    constructor(private menuService: MenuService,
        private authService: AuthService,
        private router: Router) { }

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
