import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { PageEvent } from "@angular/material/paginator";
import { MatSnackBar, MatDialog } from "@angular/material";

import { Solution } from "../solution-core/solution.model";
import { SolutionFilter } from "../solution-core/solution-filter.model";
import { SolutionService } from "../solution-core/solution.service";
import { SolutionOpenComponent } from "./solution-open.component";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

class SolutionListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new SolutionFilter();
}

@Component({
    selector: "app-solution-list",
    templateUrl: "./solution-list.component.html",
    styleUrls: ["./solution-list.component.scss"]
})
export class SolutionListComponent implements OnInit {
    content: Page<Solution>;
    state: SolutionListState;
    pageSizeOptions = [10, 20];
    columns = [
        "name",
        "path",
        "templateName",
        "templateVersion"
    ];

    constructor(
        private modalHelper: ModalHelper,
        private solutionService: SolutionService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService,
        private dialog: MatDialog,
        private snackBar: MatSnackBar
        ) {
        this.state = this.storeService.get("solutionListState", new SolutionListState());
    }

    ngOnInit(): void {
        this.getSolutions();
    }

    getSolutions(): void {
        this.solutionService.getSolutions({ pageIndex: this.state.pageIndex, pageSize: this.state.pageSize, filter: this.state.filter })
            .then(content => {
                this.content = content;
            })
            .catch(error => this.onError(error));
    }

    reset(): void {
        this.state.filter.text = null;
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
            .subscribe(() => {
                this.solutionService.deleteSolution({ id })
                    .then(() => {
                        this.getSolutions();
                    })
                    .catch(error => this.onError(error));
            });
    }

    onPage(page: PageEvent) {
        this.state.pageIndex = page.pageIndex;
        this.state.pageSize = page.pageSize;
        this.getSolutions();
    }

    onError(error: Error) {
        if (error) {
            this.snackBar.open(error.message, "Ok");
        }
    }

    open() {
        this.dialog.open(SolutionOpenComponent, {
            width: "600px"
        });
    }
}
