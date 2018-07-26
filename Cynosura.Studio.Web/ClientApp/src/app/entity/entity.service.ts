import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import "rxjs/add/operator/toPromise";

import { Entity } from "./entity.model";
import { Page } from "../core/page.model";

@Injectable()
export class EntityService {
    private entityUrl = "/api/entity";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient) { }

    getEntities(projectId: number, pageIndex?: number, pageSize?: number): Promise<Page<Entity>> {
        const url = this.entityUrl;

        let params = new HttpParams();

        params = params.set("projectId", projectId.toString());

        if (pageIndex != undefined)
            params = params.set("pageIndex", pageIndex.toString());

        if (pageSize != undefined)
            params = params.set("pageSize", pageSize.toString());

        return this.httpClient.get<Page<Entity>>(url,
            {
                params: params
            })
            .toPromise();
    }

    getEntity(projectId: number, id: string): Promise<Entity> {
        const url = `${this.entityUrl}/${id}`;
        return this.httpClient.get<Entity>(url,
            {
                params: { "projectId": projectId.toString() }
            })
            .toPromise();
    }

    updateEntity(projectId: number, entity: Entity): Promise<Entity> {
        const url = `${this.entityUrl}/${entity.id}`;
        return this.httpClient.put<Entity>(url,
            JSON.stringify(entity),
            {
                headers: this.headers,
                params: { "projectId": projectId.toString() }
            })
            .toPromise();
    }

    createEntity(projectId: number, entity: Entity): Promise<Entity> {
        return this.httpClient.post<Entity>(this.entityUrl, JSON.stringify(entity),
            {
                headers: this.headers,
                params: { "projectId": projectId.toString() }
            })
            .toPromise();
    }

    deleteEntity(projectId: number, id: number): Promise<{}> {
        const url = `${this.entityUrl}/${id}`;
        return this.httpClient.delete(url,
            {
                params: { "projectId": projectId.toString() }
            })
            .toPromise();
    }
}
