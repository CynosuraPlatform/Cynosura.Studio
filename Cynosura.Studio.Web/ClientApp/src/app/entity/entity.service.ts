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

    getEntities(solutionId: number, pageIndex?: number, pageSize?: number): Promise<Page<Entity>> {
        const url = this.entityUrl;

        let params = new HttpParams();

        params = params.set("solutionId", solutionId.toString());

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

    getEntity(solutionId: number, id: string): Promise<Entity> {
        const url = `${this.entityUrl}/${id}`;
        return this.httpClient.get<Entity>(url,
            {
                params: { "solutionId": solutionId.toString() }
            })
            .toPromise();
    }

    updateEntity(solutionId: number, entity: Entity): Promise<Entity> {
        const url = `${this.entityUrl}/${entity.id}`;
        return this.httpClient.put<Entity>(url,
            JSON.stringify(entity),
            {
                headers: this.headers,
                params: { "solutionId": solutionId.toString() }
            })
            .toPromise();
    }

    createEntity(solutionId: number, entity: Entity): Promise<Entity> {
        return this.httpClient.post<Entity>(this.entityUrl, JSON.stringify(entity),
            {
                headers: this.headers,
                params: { "solutionId": solutionId.toString() }
            })
            .toPromise();
    }

    deleteEntity(solutionId: number, id: number): Promise<{}> {
        const url = `${this.entityUrl}/${id}`;
        return this.httpClient.delete(url,
            {
                params: { "solutionId": solutionId.toString() }
            })
            .toPromise();
    }
}
