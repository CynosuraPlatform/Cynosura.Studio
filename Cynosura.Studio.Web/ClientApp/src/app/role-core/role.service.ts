import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { Role } from "./role.model";
import { Page } from "../core/page.model";

@Injectable()
export class RoleService {
    private roleUrl = "/api/role";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient) { }

    getRoles(pageIndex?: number, pageSize?: number): Promise<Page<Role>> {
        const url = this.roleUrl;

        let params = new HttpParams();

        if (pageIndex != undefined)
            params = params.set("pageIndex", pageIndex.toString());

        if (pageSize != undefined)
            params = params.set("pageSize", pageSize.toString());

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
