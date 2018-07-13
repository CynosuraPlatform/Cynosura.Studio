import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import "rxjs/add/operator/toPromise";

import { Project } from "./project.model";
import { Page } from "../core/page.model";

@Injectable()
export class ProjectService {
    private projectUrl = "/api/project";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient) { }

    getProjects(pageIndex?: number, pageSize?: number): Promise<Page<Project> > {
        const url = this.projectUrl;

        const params = new HttpParams();

        if (pageIndex != undefined)
            params.set("pageIndex", pageIndex.toString());

        if (pageSize != undefined)
            params.set("pageSize", pageSize.toString());

        return this.httpClient.get<Page<Project> >(url, {
            params: params
        }).toPromise();
    }

    getProject(id: number): Promise<Project> {
        const url = `${this.projectUrl}/${id}`;
        return this.httpClient.get<Project>(url)
            .toPromise();
    }

    updateProject(project: Project): Promise<Project> {
        const url = `${this.projectUrl}/${project.id}`;
        return this.httpClient.put<Project>(url, JSON.stringify(project), { headers: this.headers })
            .toPromise();
    }

    createProject(project: Project): Promise<Project> {
        return this.httpClient.post<Project>(this.projectUrl, JSON.stringify(project), { headers: this.headers })
            .toPromise();
    }

    deleteProject(id: number): Promise<{}> {
        const url = `${this.projectUrl}/${id}`;
        return this.httpClient.delete(url)
            .toPromise();
    }
}