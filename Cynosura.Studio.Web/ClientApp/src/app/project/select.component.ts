import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";

import { Project } from "./project.model";
import { ProjectService } from "./project.service";

@Component({
    selector: "project-select",
    templateUrl: "./select.component.html"
})

export class ProjectSelectComponent implements OnInit {
    constructor(private projectService: ProjectService) { }

    projects: Project[] = [];

    @Input()
    selectedProjectId: number | null = null;

    @Output()
    selectedProjectIdChange = new EventEmitter<number>();

    @Input()
    readonly = false;

    ngOnInit(): void {
        this.projectService.getProjects().then(projects => this.projects = projects.pageItems);
    }
}
