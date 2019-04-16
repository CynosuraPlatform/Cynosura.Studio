import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { ConfigService } from "../config/config.service";
import { Solution } from "./solution.model";
import { SolutionFilter } from "./solution-filter.model";
import { Page } from "../core/page.model";

@Injectable()
export class SolutionService {
    private solutionUrl = this.configService.config.apiBaseUrl + "/api/solution";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getSolutions(pageIndex?: number, pageSize?: number, filter?: SolutionFilter): Promise<Page<Solution>> {
        const url = this.solutionUrl;

        let params = new HttpParams();

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

        return this.httpClient.get<Page<Solution>>(url, {
            params: params
        }).toPromise();
    }

    getSolution(id: number): Promise<Solution> {
        const url = `${this.solutionUrl}/${id}`;
        return this.httpClient.get<Solution>(url)
            .toPromise();
    }

    updateSolution(solution: Solution): Promise<Solution> {
        const url = `${this.solutionUrl}/${solution.id}`;
        return this.httpClient.put<Solution>(url, JSON.stringify(solution), { headers: this.headers })
            .toPromise();
    }

    createSolution(solution: Solution): Promise<Solution> {
        return this.httpClient.post<Solution>(this.solutionUrl, JSON.stringify(solution), { headers: this.headers })
            .toPromise();
    }

    deleteSolution(id: number): Promise<{}> {
        const url = `${this.solutionUrl}/${id}`;
        return this.httpClient.delete(url)
            .toPromise();
    }


    generateSolution(id: number): Promise<{}> {
        const url = `${this.solutionUrl}/${id}/generate`;
        return this.httpClient.post(url, null)
            .toPromise();
    }

    upgradeSolution(id: number): Promise<{}> {
        const url = `${this.solutionUrl}/${id}/upgrade`;
        return this.httpClient.post(url, null)
            .toPromise();
    }

    openSolution(solution: Solution): Promise<Solution> {
        const url = `${this.solutionUrl}/open`;
        return this.httpClient.post<Solution>(url, JSON.stringify(solution), { headers: this.headers })
            .toPromise();
    }
}
