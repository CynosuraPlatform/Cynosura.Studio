import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { ConfigService } from "../config/config.service";
import { CreatedEntity } from "../core/models/created-entity.model";
import { Role } from "./role.model";
import { GetRoles, GetRole, UpdateRole, CreateRole, DeleteRole } from "./role-request.model";
import { Page } from "../core/page.model";

@Injectable()
export class RoleService {
    private apiUrl = this.configService.config.apiBaseUrl + "/api";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getRoles(getRoles: GetRoles): Promise<Page<Role>> {
        const url = `${this.apiUrl}/GetRoles`;
        return this.httpClient.post<Page<Role>>(url, JSON.stringify(getRoles), { headers: this.headers })
            .toPromise();
    }

    getRole(getRole: GetRole): Promise<Role> {
        const url = `${this.apiUrl}/GetRole`;
        return this.httpClient.post<Role>(url, JSON.stringify(getRole), { headers: this.headers })
            .toPromise();
    }

    updateRole(updateRole: UpdateRole): Promise<{}> {
        const url = `${this.apiUrl}/UpdateRole`;
        return this.httpClient.post(url, JSON.stringify(updateRole), { headers: this.headers })
            .toPromise();
    }

    createRole(createRole: CreateRole): Promise<CreatedEntity<number>> {
        const url = `${this.apiUrl}/CreateRole`;
        return this.httpClient.post<CreatedEntity<number>>(url, JSON.stringify(createRole), { headers: this.headers })
            .toPromise();
    }

    deleteRole(deleteRole: DeleteRole): Promise<{}> {
        const url = `${this.apiUrl}/DeleteRole`;
        return this.httpClient.post(url, JSON.stringify(deleteRole), { headers: this.headers })
            .toPromise();
    }
}
