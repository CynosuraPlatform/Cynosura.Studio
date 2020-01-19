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

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getEntities(getEntities: GetEntities): Promise<Page<Entity>> {
        const url = `${this.apiUrl}/GetEntities`;
        return this.httpClient.post<Page<Entity>>(url, getEntities)
            .toPromise();
    }

    getEntity(getEntity: GetEntity): Promise<Entity> {
        const url = `${this.apiUrl}/GetEntity`;
        return this.httpClient.post<Entity>(url, getEntity)
            .toPromise();
    }

    updateEntity(updateEntity: UpdateEntity): Promise<{}> {
        const url = `${this.apiUrl}/UpdateEntity`;
        return this.httpClient.post(url, updateEntity)
            .toPromise();
    }


    createEntity(createEntity: CreateEntity): Promise<CreatedEntity<string>> {
        const url = `${this.apiUrl}/CreateEntity`;
        return this.httpClient.post<CreatedEntity<string>>(url, createEntity)
            .toPromise();
    }

    deleteEntity(deleteEntity: DeleteEntity): Promise<{}> {
        const url = `${this.apiUrl}/DeleteEntity`;
        return this.httpClient.post(url, deleteEntity)
            .toPromise();
    }

    generateEntity(generateEntity: GenerateEntity): Promise<{}> {
        const url = `${this.apiUrl}/GenerateEntity`;
        return this.httpClient.post(url, generateEntity)
            .toPromise();
    }
}
