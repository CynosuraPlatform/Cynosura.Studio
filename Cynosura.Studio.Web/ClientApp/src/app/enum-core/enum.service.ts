import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import "rxjs/add/operator/toPromise";

import { Enum } from "./enum.model";
import { Page } from "../core/page.model";

@Injectable()
export class EnumService {
    private enumUrl = "/api/enum";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient) { }

    getEnums(pageIndex?: number, pageSize?: number): Promise<Page<Enum>> {
        const url = this.enumUrl;

        let params = new HttpParams();

        if (pageIndex != undefined)
            params = params.set("pageIndex", pageIndex.toString());

        if (pageSize != undefined)
            params = params.set("pageSize", pageSize.toString());

        return this.httpClient.get<Page<Enum>>(url, {
            params: params
        }).toPromise();
    }

    getEnum(id: number): Promise<Enum> {
        const url = `${this.enumUrl}/${id}`;
        return this.httpClient.get<Enum>(url)
            .toPromise();
    }

    updateEnum(enum: Enum): Promise<Enum> {
        const url = `${this.enumUrl}/${enum.id}`;
        return this.httpClient.put<Enum>(url, JSON.stringify(enum), { headers: this.headers })
            .toPromise();
    }

    createEnum(enum: Enum): Promise<Enum> {
        return this.httpClient.post<Enum>(this.enumUrl, JSON.stringify(enum), { headers: this.headers })
            .toPromise();
    }

    deleteEnum(id: number): Promise<{}> {
        const url = `${this.enumUrl}/${id}`;
        return this.httpClient.delete(url)
            .toPromise();
    }
}