import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Menu } from "../models/menu.model";

@Injectable()
export class MenuService {
    private menuUrl = "/api/menu";

    constructor(private httpClient: HttpClient) { }

    getMenu(): Promise<Menu> {
        return this.httpClient.get<Menu>(this.menuUrl)
            .toPromise();
    }
}
