import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { ConfigService } from "../../config/config.service";
import { Menu } from "../models/menu.model";

@Injectable()
export class MenuService {
    private menuUrl = this.configService.config.apiBaseUrl + "/api/menu";

    constructor(private httpClient: HttpClient,
                private configService: ConfigService) { }

    getMenu(): Promise<Menu> {
        return this.httpClient.get<Menu>(this.menuUrl)
            .toPromise();
    }
}
