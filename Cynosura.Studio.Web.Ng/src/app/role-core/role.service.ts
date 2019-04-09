import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { ConfigService } from "../config/config.service";
import { Role } from "./role.model";
import { RoleFilter } from "./role-filter.model";
import { Page } from "../core/page.model";

@Injectable()
export class RoleService {
    private roleUrl = this.configService.config.apiBaseUrl + "/api/role";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getRoles(pageIndex?: number, pageSize?: number, filter?: RoleFilter): Promise<Page<Role>> {
        const url = this.roleUrl;

        let params = new HttpParams();

        if (pageIndex !== undefined && pageIndex !== null) {
            params = params.set("pageIndex", pageIndex.toString());
        }

        if (pageSize !== undefined && pageSize !== null) {
            params = params.set("pageSize", pageSize.toString());
        }

        if (filter) {
            params = Object.keys(filter).reduce((prev, cur) => {
                if (filter[cur] !== undefined && filter[cur] !== null) {
                    prev = prev.set(`filter.${cur}`, filter[cur]);
                }
                return prev;
            }, params);
        }

        return this.httpClient.get<Page<Role>>(url, {
            params: params
        }).toPromise();
    }

    getRole(id: number): Promise<Role> {
        const url = `${this.roleUrl}/${id}`;
        return this.httpClient.get<Role>(url)
            .toPromise();
    }

    updateRole(role: Role): Promise<Role> {
        const url = `${this.roleUrl}/${role.id}`;
        return this.httpClient.put<Role>(url, JSON.stringify(role), { headers: this.headers })
            .toPromise();
    }

    createRole(role: Role): Promise<Role> {
        return this.httpClient.post<Role>(this.roleUrl, JSON.stringify(role), { headers: this.headers })
            .toPromise();
    }

    deleteRole(id: number): Promise<{}> {
        const url = `${this.roleUrl}/${id}`;
        return this.httpClient.delete(url)
            .toPromise();
    }
}
