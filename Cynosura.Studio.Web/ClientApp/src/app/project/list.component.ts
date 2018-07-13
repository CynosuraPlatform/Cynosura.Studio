import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";

import { Modal } from "ngx-modialog/plugins/bootstrap";

import { Project } from "./project.model";
import { ProjectService } from "./project.service";

import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "project-list",
    templateUrl: "./list.component.html"
})
export class ProjectListComponent implements OnInit {
    content: Page<Project>;
    error: Error;
    pageSize = 10;

    constructor(
        private modal: Modal,
        private projectService: ProjectService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService
        ) {}

    ngOnInit(): void {
        let pageIndex = this.storeService.get("projectsPageIndex");
        if (!pageIndex) pageIndex = 0;
        this.getProjects(pageIndex);
    }

    getProjects(pageIndex: number): void {        
        this.projectService.getProjects(pageIndex, this.pageSize)
            .then(content => {
                if (content.pageItems.length == 0 && content.totalItems != 0) {
                    this.content.currentPageIndex--;
                    this.storeService.set("projectsPageIndex", this.content.currentPageIndex);
                    this.getProjects(this.content.currentPageIndex);
                }
                this.content = content;
            })
            .catch(error => this.error = error);
    }

    reset(): void {
        this.projectService.getProjects(this.content.currentPageIndex, this.pageSize)
            .then(content => { this.content = content; },
                error => this.error = error.json() as Error);
    }

    edit(id: number): void {
        this.router.navigate([id], { relativeTo: this.route });
    }

    add(): void {
        this.router.navigate([0], { relativeTo: this.route });
    }

    delete(id: number): void {
        const dialogRef = this.modal
            .confirm()
            .size("sm")
            .keyboard(27)
            .title("Удалить?")
            .body("Действительно хотите удалить?")
            .okBtn("Удалить")
            .cancelBtn("Отмена")
            .open();
		dialogRef.result.then(dialog => dialog.result)
            .then(() => {
                this.projectService.deleteProject(id)
                    .then(() => {
                        this.getProjects(this.content.currentPageIndex);
                    })
                    .catch(error => this.error = error)
            })
            .catch(() => {});
    }

    onPageSelected(pageIndex: number) {
        this.storeService.set("projectsPageIndex", pageIndex);
        this.getProjects(pageIndex);
    }
}