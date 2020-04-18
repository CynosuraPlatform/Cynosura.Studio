import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { Menu } from './menu.model';

@Injectable()
export class MenuService {
    private menu: Menu;

    constructor() {
        this.menu = new Menu();
// ADD MENU ITEMS HERE
        this.menu.items.push({ route: '/entity', name: 'Entities', icon: 'notes', roles: [] });
        this.menu.items.push({ route: '/enum', name: 'Enums', icon: 'notes', roles: [''] });
        this.menu.items.push({ route: '/solution', name: 'Solutions', icon: 'folder', roles: [] });
    }

    getMenu(): Observable<Menu> {
        const menu: Menu = {
            items: this.menu.items.filter(item => {
                return true;
            })
        };
        return of(menu);
    }
}
