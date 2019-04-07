import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { ConfigService } from "../config/config.service";
import { Entity } from "./entity.model";
import { EntityFilter } from "./entity-filter.model";
import { Page } from "../core/page.model";

@Injectable()
export class EntityService {
    private entityUrl = this.configService.config.apiBaseUrl + "/api/entity";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getEntities(solutionId: number, pageIndex?: number, pageSize?: number, filter?: EntityFilter): Promise<Page<Entity>> {
        const url = this.entityUrl;

        let params = new HttpParams();

        params = params.set("solutionId", solutionId.toString());

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

    deleteEntity(solutionId: number, id: string): Promise<{}> {
        const url = `${this.entityUrl}/${id}`;
        return this.httpClient.delete(url,
            {
                params: { "solutionId": solutionId.toString() }
            })
            .toPromise();
    }

    generateEntity(solutionId: number, id: string): Promise<{}> {
        const url = `${this.entityUrl}/${id}/generate`;
        return this.httpClient.post(url, null, {
                params: { "solutionId": solutionId.toString() }
            })
            .toPromise();
    }
}
