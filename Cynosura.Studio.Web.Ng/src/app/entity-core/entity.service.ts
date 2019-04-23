import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { ConfigService } from "../config/config.service";
import { CreatedEntity } from "../core/models/created-entity.model";
import { Entity } from "./entity.model";
import { GetEntities, GetEntity, UpdateEntity, CreateEntity, DeleteEntity, GenerateEntity } from "./entity-request.model";
import { EntityFilter } from "./entity-filter.model";
import { Page } from "../core/page.model";

@Injectable()
export class EntityService {
    private apiUrl = this.configService.config.apiBaseUrl + "/api";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getEntities(getEntities: GetEntities): Promise<Page<Entity>> {
        const url = `${this.apiUrl}/GetEntities`;
        return this.httpClient.post<Page<Entity>>(url, JSON.stringify(getEntities), { headers: this.headers })
            .toPromise();
    }

    getEntity(getEntity: GetEntity): Promise<Entity> {
        const url = `${this.apiUrl}/GetEntity`;
        return this.httpClient.post<Entity>(url, JSON.stringify(getEntity), { headers: this.headers })
            .toPromise();
    }

    updateEntity(updateEntity: UpdateEntity): Promise<{}> {
        const url = `${this.apiUrl}/UpdateEntity`;
        return this.httpClient.post(url, JSON.stringify(updateEntity), { headers: this.headers })
            .toPromise();
    }


    createEntity(createEntity: CreateEntity): Promise<CreatedEntity<string>> {
        const url = `${this.apiUrl}/CreateEntity`;
        return this.httpClient.post<CreatedEntity<string>>(url, JSON.stringify(createEntity), { headers: this.headers })
            .toPromise();
    }

    deleteEntity(deleteEntity: DeleteEntity): Promise<{}> {
        const url = `${this.apiUrl}/DeleteEntity`;
        return this.httpClient.post(url, JSON.stringify(deleteEntity), { headers: this.headers })
            .toPromise();
    }

    generateEntity(generateEntity: GenerateEntity): Promise<{}> {
        const url = `${this.apiUrl}/GenerateEntity`;
        return this.httpClient.post(url, JSON.stringify(generateEntity), { headers: this.headers })
            .toPromise();
    }
}
