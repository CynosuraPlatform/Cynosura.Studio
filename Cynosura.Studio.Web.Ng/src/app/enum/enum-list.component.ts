import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { PageEvent } from "@angular/material/paginator";

import { Enum } from "../enum-core/enum.model";
import { EnumFilter } from "../enum-core/enum-filter.model";
import { EnumService } from "../enum-core/enum.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";
import { NoticeHelper } from "../core/notice.helper";

class EnumListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new EnumFilter();
}

@Component({
    selector: "app-enum-list",
    templateUrl: "./enum-list.component.html",
    styleUrls: ["./enum-list.component.scss"]
})
export class EnumListComponent implements OnInit {
    content: Page<Enum>;
    state: EnumListState;
    pageSizeOptions = [10, 20];
    columns = [
        "name",
        "displayName",
        "action"
    ];
    private innerSolutionId: number;
    get solutionId(): number {
        if (!this.innerSolutionId) {
            this.innerSolutionId = this.storeService.get("enumsSolutionId", 0);
        }
        return this.innerSolutionId;
    }
    set solutionId(val: number) {
        this.innerSolutionId = val;
        this.storeService.set("enumsSolutionId", this.innerSolutionId);
        this.getEnums();
    }

    constructor(
        private modalHelper: ModalHelper,
        private enumService: EnumService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService,
        private noticeHelper: NoticeHelper
        ) {
        this.state = this.storeService.get("enumListState", new EnumListState());
    }

    ngOnInit(): void {
        this.getEnums();
    }

    async getEnums() {
        if (this.solutionId) {
            this.content = await this.enumService.getEnums({
                solutionId: this.solutionId,
                pageIndex: this.state.pageIndex,
                pageSize: this.state.pageSize,
                filter: this.state.filter
            });
        } else {
            this.content = null;
        }
    }

    reset(): void {
        this.state.filter.text = null;
        this.getEnums();
    }

    delete(id: string): void {
        this.modalHelper.confirmDelete()
            .subscribe(async () => {
                try {
                    await this.enumService.deleteEnum({ solutionId: this.solutionId, id });
                    this.getEnums();
                } catch (error) {
                    this.onError(error);
                }
            });
    }

    onPage(page: PageEvent) {
        this.state.pageIndex = page.pageIndex;
        this.state.pageSize = page.pageSize;
        this.getEnums();
    }

    onError(error: Error) {
        if (error) {
            this.noticeHelper.showError(error);
        }
    }
}
