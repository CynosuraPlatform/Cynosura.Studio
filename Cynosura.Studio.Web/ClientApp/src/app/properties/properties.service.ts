import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
    providedIn: "root"
})
export class PropertiesService {
    private propertiesUrl = "/api/properties";

    constructor(private httpClient: HttpClient) { }

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
