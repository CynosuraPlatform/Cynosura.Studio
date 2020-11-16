import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { ConfigService } from '../config/config.service';
import { CreatedEntity } from '../core/models/created-entity.model';
import { Page } from '../core/page.model';
import { FileResult } from '../core/file-result.model';

import { Solution } from './solution.model';
import { GetSolutions, GetSolution, ExportSolutions,
    UpdateSolution, CreateSolution, DeleteSolution, GenerateSolution, UpgradeSolution } from './solution-request.model';

@Injectable({ providedIn: 'root' })
export class SolutionService {
    private apiUrl = this.configService.config.apiBaseUrl + '/api';

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getSolutions(getSolutions: GetSolutions): Observable<Page<Solution>> {
        const url = `${this.apiUrl}/GetSolutions`;
        return this.httpClient.post<Page<Solution>>(url, getSolutions);
    }

    getSolution(getSolution: GetSolution): Observable<Solution> {
        const url = `${this.apiUrl}/GetSolution`;
        return this.httpClient.post<Solution>(url, getSolution);
    }

    exportSolutions(exportSolutions: ExportSolutions): Observable<FileResult> {
        const url = `${this.apiUrl}/ExportSolutions`;
        return this.httpClient.post(url, exportSolutions, {
            responseType: 'blob' as 'json',
            observe: 'response',
        }).pipe(map((response => new FileResult(response))));
    }

    updateSolution(updateSolution: UpdateSolution): Observable<{}> {
        const url = `${this.apiUrl}/UpdateSolution`;
        return this.httpClient.post(url, updateSolution);
    }

    createSolution(createSolution: CreateSolution): Observable<CreatedEntity<number>> {
        const url = `${this.apiUrl}/CreateSolution`;
        return this.httpClient.post<CreatedEntity<number>>(url, createSolution);
    }

    deleteSolution(deleteSolution: DeleteSolution): Observable<{}> {
        const url = `${this.apiUrl}/DeleteSolution`;
        return this.httpClient.post(url, deleteSolution);
    }


    generateSolution(generateSolution: GenerateSolution): Observable<{}> {
        const url = `${this.apiUrl}/GenerateSolution`;
        return this.httpClient.post(url, generateSolution);
    }

    upgradeSolution(upgradeSolution: UpgradeSolution): Observable<{}> {
        const url = `${this.apiUrl}/UpgradeSolution`;
        return this.httpClient.post(url, upgradeSolution);
    }

    openSolution(solution: Solution): Observable<Solution> {
        const url = `${this.apiUrl}/OpenSolution`;
        return this.httpClient.post<Solution>(url, solution);
    }
}
