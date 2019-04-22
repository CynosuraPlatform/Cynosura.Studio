import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { ConfigService } from "../config/config.service";
import { CreatedEntity } from "../core/models/created-entity.model";
import { Solution } from "./solution.model";
import { GetSolutions, GetSolution, UpdateSolution, CreateSolution, DeleteSolution,
         GenerateSolution, UpgradeSolution } from "./solution-request.model";
import { SolutionFilter } from "./solution-filter.model";
import { Page } from "../core/page.model";

@Injectable()
export class SolutionService {
    private apiUrl = this.configService.config.apiBaseUrl + "/api";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getSolutions(getSolutions: GetSolutions): Promise<Page<Solution>> {
        const url = `${this.apiUrl}/GetSolutions`;
        return this.httpClient.post<Page<Solution>>(url, JSON.stringify(getSolutions), { headers: this.headers })
            .toPromise();
    }

    getSolution(getSolution: GetSolution): Promise<Solution> {
        const url = `${this.apiUrl}/GetSolution`;
        return this.httpClient.post<Solution>(url, JSON.stringify(getSolution), { headers: this.headers })
            .toPromise();
    }

    updateSolution(updateSolution: UpdateSolution): Promise<{}> {
        const url = `${this.apiUrl}/UpdateSolution`;
        return this.httpClient.post(url, JSON.stringify(updateSolution), { headers: this.headers })
            .toPromise();
    }

    createSolution(createSolution: CreateSolution): Promise<CreatedEntity<number>> {
        const url = `${this.apiUrl}/CreateSolution`;
        return this.httpClient.post<CreatedEntity<number>>(url, JSON.stringify(createSolution), { headers: this.headers })
            .toPromise();
    }

    deleteSolution(deleteSolution: DeleteSolution): Promise<{}> {
        const url = `${this.apiUrl}/DeleteSolution`;
        return this.httpClient.post(url, JSON.stringify(deleteSolution), { headers: this.headers })
            .toPromise();
    }


    generateSolution(generateSolution: GenerateSolution): Promise<{}> {
        const url = `${this.apiUrl}/GenerateSolution`;
        return this.httpClient.post(url, JSON.stringify(generateSolution), { headers: this.headers })
            .toPromise();
    }

    upgradeSolution(upgradeSolution: UpgradeSolution): Promise<{}> {
        const url = `${this.apiUrl}/UpgradeSolution`;
        return this.httpClient.post(url, JSON.stringify(upgradeSolution), { headers: this.headers })
            .toPromise();
    }
}
