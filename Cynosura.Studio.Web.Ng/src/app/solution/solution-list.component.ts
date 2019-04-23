import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";

import { Solution } from "../solution-core/solution.model";
import { SolutionFilter } from "../solution-core/solution-filter.model";
import { SolutionService } from "../solution-core/solution.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "app-solution-list",
    templateUrl: "./solution-list.component.html"
})
export class SolutionListComponent implements OnInit {
    content: Page<Solution>;
    error: Error;
    pageSize = 10;
    filter = new SolutionFilter();
    openLocation: string;
    private innerPageIndex: number;
    get pageIndex(): number {
        if (!this.innerPageIndex) {
            this.innerPageIndex = this.storeService.get("solutionsPageIndex", 0);
        }
        return this.innerPageIndex;
    }
    set pageIndex(value: number) {
        this.innerPageIndex = value;
        this.storeService.set("solutionsPageIndex", value);
    }

    constructor(
        private modalHelper: ModalHelper,
        private solutionService: SolutionService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService
    ) { }

    ngOnInit(): void {
        this.getSolutions();
    }

    getSolutions(): void {
        this.solutionService.getSolutions({ pageIndex: this.pageIndex, pageSize: this.pageSize, filter: this.filter })
            .then(content => {
                this.content = content;
            })
            .catch(error => this.error = error);
    }

    reset(): void {
        this.filter.text = null;
        this.getSolutions();
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
                this.solutionService.deleteSolution({ id })
                    .then(() => {
                        this.getSolutions();
                    })
                    .catch(error => this.error = error);
            })
            .catch(() => { });
    }

    onPageSelected(pageIndex: number) {
        this.pageIndex = pageIndex;
        this.getSolutions();
    }

    open() {
        const solution = {
            path: this.openLocation
        } as Solution;
        this.solutionService.openSolution(solution)
            .then((result) => this.edit(result.id))
            .catch((error) => console.log(this.error = error));
    }
}
