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

    getEnums(solutionId: number, pageIndex?: number, pageSize?: number): Promise<Page<Enum>> {
        const url = this.enumUrl;

        let params = new HttpParams();

        params = params.set("solutionId", solutionId.toString());

        if (pageIndex != undefined)
            params = params.set("pageIndex", pageIndex.toString());

        if (pageSize != undefined)
            params = params.set("pageSize", pageSize.toString());

        return this.httpClient.get<Page<Enum>>(url, {
            params: params
        }).toPromise();
    }

    getEnum(solutionId: number, id: string): Promise<Enum> {
        const url = `${this.enumUrl}/${id}`;
        return this.httpClient.get<Enum>(url, {
            params: { "solutionId": solutionId.toString() }
        }).toPromise();
    }

    updateEnum(solutionId: number, enumModel: Enum): Promise<Enum> {
        const url = `${this.enumUrl}/${enumModel.id}`;
        return this.httpClient.put<Enum>(url, JSON.stringify(enumModel), {
            headers: this.headers,
            params: { "solutionId": solutionId.toString() }
        }).toPromise();
    }

    createEnum(solutionId: number, enumModel: Enum): Promise<Enum> {
        return this.httpClient.post<Enum>(this.enumUrl, JSON.stringify(enumModel), {
            headers: this.headers,
            params: { "solutionId": solutionId.toString() }
        }).toPromise();
    }

    deleteEnum(solutionId: number, id: number): Promise<{}> {
        const url = `${this.enumUrl}/${id}`;
        return this.httpClient.delete(url, {
            params: { "solutionId": solutionId.toString() }
        }).toPromise();
    }

    generateEnum(solutionId: number, id: string): Promise<{}> {
        const url = `${this.enumUrl}/${id}/generate`;
        return this.httpClient.post(url, null, {
                params: { "solutionId": solutionId.toString() }
            })
            .toPromise();
    }
}
