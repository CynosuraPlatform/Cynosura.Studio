import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { ConfigService } from '../config/config.service';

@Injectable({
    providedIn: 'root'
})
export class PropertiesService {
    private propertiesUrl = this.configService.config.apiBaseUrl + '/api/properties';

    constructor(
        private httpClient: HttpClient,
        private configService: ConfigService) { }

    loaded: { [k: string]: any };
    getProperties(): Promise<{ [k: string]: any }> {
        if (this.loaded) {
            return Promise.resolve(this.loaded);
        }
        const url = `${this.propertiesUrl}`;
        return this.httpClient.get<{ [k: string]: any }>(url)
            .toPromise()
            .then((data) => {
                this.loaded = data;
                return data;
            });
    }
}
