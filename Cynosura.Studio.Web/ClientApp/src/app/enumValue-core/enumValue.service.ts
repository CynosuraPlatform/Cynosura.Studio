import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import "rxjs/add/operator/toPromise";

import { EnumValue } from "./enumValue.model";
import { Page } from "../core/page.model";

@Injectable()
export class EnumValueService {
    private enumValueUrl = "/api/enumValue";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient) { }

    getEnumValues(pageIndex?: number, pageSize?: number): Promise<Page<EnumValue>> {
        const url = this.enumValueUrl;

        let params = new HttpParams();

        if (pageIndex != undefined)
            params = params.set("pageIndex", pageIndex.toString());

        if (pageSize != undefined)
            params = params.set("pageSize", pageSize.toString());

        return this.httpClient.get<Page<EnumValue>>(url, {
            params: params
        }).toPromise();
    }

    getEnumValue(id: number): Promise<EnumValue> {
        const url = `${this.enumValueUrl}/${id}`;
        return this.httpClient.get<EnumValue>(url)
            .toPromise();
    }

    updateEnumValue(enumValue: EnumValue): Promise<EnumValue> {
        const url = `${this.enumValueUrl}/${enumValue.id}`;
        return this.httpClient.put<EnumValue>(url, JSON.stringify(enumValue), { headers: this.headers })
            .toPromise();
    }

    createEnumValue(enumValue: EnumValue): Promise<EnumValue> {
        return this.httpClient.post<EnumValue>(this.enumValueUrl, JSON.stringify(enumValue), { headers: this.headers })
            .toPromise();
    }

    deleteEnumValue(id: number): Promise<{}> {
        const url = `${this.enumValueUrl}/${id}`;
        return this.httpClient.delete(url)
            .toPromise();
    }
}