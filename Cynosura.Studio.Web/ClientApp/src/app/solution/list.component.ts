import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";

import { Solution } from "../solution-core/solution.model";
import { SolutionService } from "../solution-core/solution.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "solution-list",
    templateUrl: "./list.component.html"
})
export class SolutionListComponent implements OnInit {
    content: Page<Solution>;
    error: Error;
    pageSize = 10;
    private _pageIndex: number;
    get pageIndex(): number {
        if (!this._pageIndex) {
            this._pageIndex = this.storeService.get("solutionsPageIndex") | 0;
        }
        return this._pageIndex;
    }
    set pageIndex(value: number) {
        this._pageIndex = value;
        this.storeService.set("solutionsPageIndex", value);
    }

    constructor(
        private modalHelper: ModalHelper,
        private solutionService: SolutionService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService
        ) {}

    ngOnInit(): void {
        this.getSolutions();
    }

    getSolutions(): void {        
        this.solutionService.getSolutions(this.pageIndex, this.pageSize)
            .then(content => {
                this.content = content;
            })
            .catch(error => this.error = error);
    }

    reset(): void {
        this.solutionService.getSolutions(this.content.currentPageIndex, this.pageSize)
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
        this.modalHelper.confirmDelete()
            .then(() => {
                this.solutionService.deleteSolution(id)
                    .then(() => {
                        this.getSolutions();
                    })
                    .catch(error => this.error = error);
            });
    }

    onPageSelected(pageIndex: number) {
        this.pageIndex = pageIndex;
        this.getSolutions();
    }
}