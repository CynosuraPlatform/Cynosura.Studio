import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { PageEvent } from "@angular/material/paginator";
import { MatSnackBar } from "@angular/material";

import { Entity } from "../entity-core/entity.model";
import { EntityFilter } from "../entity-core/entity-filter.model";
import { EntityService } from "../entity-core/entity.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";
import { NoticeHelper } from "../core/notice.helper";

class EntityListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new EntityFilter();
}

@Component({
    selector: "app-entity-list",
    templateUrl: "./entity-list.component.html",
    styleUrls: ["./entity-list.component.scss"]
})
export class EntityListComponent implements OnInit {
    content: Page<Entity>;
    state: EntityListState;
    pageSizeOptions = [10, 20];
    columns = [
        "name",
        "pluralName",
        "displayName",
        "pluralDisplayName",
        "isAbstract",
        "baseEntity",
        "action"
    ];
    private innerSolutionId: number;
    get solutionId(): number {
        if (!this.innerSolutionId) {
            this.innerSolutionId = this.storeService.get("entitiesSolutionId", 0);
        }
        return this.innerSolutionId;
    }
    set solutionId(val: number) {
        this.innerSolutionId = val;
        this.storeService.set("entitiesSolutionId", this.innerSolutionId);
        this.getEntities();
    }

    constructor(
        private modalHelper: ModalHelper,
        private entityService: EntityService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService,
        private noticeHelper: NoticeHelper
        ) {
        this.state = this.storeService.get("entityListState", new EntityListState());
    }

    ngOnInit(): void {
        this.getEntities();
    }

    getEntities(): void {
        if (this.solutionId) {
            this.entityService.getEntities({ solutionId: this.solutionId, pageIndex: this.state.pageIndex, pageSize: this.state.pageSize,
                filter: this.state.filter })
                .then(content => {
                    this.content = content;
                })
                .catch(error => this.onError(error));
        } else {
            this.content = null;
        }
    }

    reset(): void {
        this.state.filter.text = null;
        this.getEntities();
    }

    delete(id: string): void {
        this.modalHelper.confirmDelete()
            .subscribe(() => {
                this.entityService.deleteEntity({ solutionId: this.solutionId, id })
                    .then(() => {
                        this.getEntities();
                    })
                    .catch(error => this.onError(error));
            });
    }

    onPage(page: PageEvent) {
        this.state.pageIndex = page.pageIndex;
        this.state.pageSize = page.pageSize;
        this.getEntities();
    }

    onError(error: Error) {
        if (error) {
            this.noticeHelper.showError(error);
        }
    }
}
