import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { Solution } from "./solution.model";
import { Page } from "../core/page.model";

@Injectable()
export class SolutionService {
    private solutionUrl = "/api/solution";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient) { }

    getSolutions(pageIndex?: number, pageSize?: number): Promise<Page<Solution>> {
        const url = this.solutionUrl;

        let params = new HttpParams();

        if (pageIndex != undefined)
            params = params.set("pageIndex", pageIndex.toString());

        if (pageSize != undefined)
            params = params.set("pageSize", pageSize.toString());

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
}
