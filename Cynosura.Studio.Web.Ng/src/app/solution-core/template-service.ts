import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ConfigService } from "../config/config.service";

@Injectable()
export class TemplateService {

    private apiUrl = this.configService.config.apiBaseUrl + "/api";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(
        private configService: ConfigService,
        private httpClient: HttpClient) {
    }

    getTemplates(): Promise<TemplateModel> {
        const url = `${this.apiUrl}/GetTemplates`;
        return this.httpClient.post<TemplateModel>(url, null, { headers: this.headers })
            .toPromise();
    }
}

export interface TemplateModel {
    name: string;
}
