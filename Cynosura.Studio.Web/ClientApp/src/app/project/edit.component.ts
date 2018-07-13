import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Project } from "./project.model";
import { ProjectService } from "./project.service";

import { Error } from "../core/error.model";


@Component({
    selector: "project-edit",
    templateUrl: "./edit.component.html"
})
export class ProjectEditComponent implements OnInit {
    project: Project;
    error: Error;

    constructor(private projectService: ProjectService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params["id"];
            this.getProject(id);
        });
    }

    private getProject(id: number): void {
        if (id === 0) {
            this.project = new Project();
        } else {
            this.projectService.getProject(id).then(project => {
                this.project = project;
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveProject();
    }

    private saveProject(): void {
        if (this.project.id) {
            this.projectService.updateProject(this.project)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.projectService.createProject(this.project)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }

}
