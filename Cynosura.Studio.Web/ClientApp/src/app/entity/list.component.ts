import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";

import { Entity } from "../entity-core/entity.model";
import { EntityService } from "../entity-core/entity.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "entity-list",
    templateUrl: "./list.component.html"
})
export class EntityListComponent implements OnInit {
    content: Page<Entity>;
    error: Error;
    pageSize = 10;
    private _pageIndex: number;
    get pageIndex(): number {
        if (!this._pageIndex) {
            this._pageIndex = this.storeService.get("entitiesPageIndex") | 0;
        }
        return this._pageIndex;
    }
    set pageIndex(value: number) {
        this._pageIndex = value;
        this.storeService.set("entitiesPageIndex", value);
    }
    private _solutionId: number;
    get solutionId(): number {
        if (!this._solutionId) {
            this._solutionId = this.storeService.get("entitiesSolutionId") | 0;
        }
        return this._solutionId;
    }
    set solutionId(val: number) {
        this._solutionId = val;
        this.storeService.set("entitiesSolutionId", this._solutionId);
        this.getEntities();
    }

    constructor(
        private modalHelper: ModalHelper,
        private entityService: EntityService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService
        ) {}

    ngOnInit(): void {
        this.getEntities();
    }

    getEntities(): void {
        if (this.solutionId) {
            this.entityService.getEntities(this.solutionId, this.pageIndex, this.pageSize)
                .then(content => {
                    this.content = content;
                })
                .catch(error => this.error = error);
        } else {
            this.content = null;
        }
    }

    reset(): void {
        this.entityService.getEntities(this.content.currentPageIndex, this.pageSize)
            .then(content => { this.content = content; },
                error => this.error = error.json() as Error);
    }

    edit(id: string): void {
        this.router.navigate([id], { relativeTo: this.route, queryParams: { solutionId: this.solutionId } });
    }

    add(): void {
        this.router.navigate([0], { relativeTo: this.route, queryParams: { solutionId: this.solutionId } });
    }

    delete(id: string): void {
        this.modalHelper.confirmDelete()
            .then(() => {
                this.entityService.deleteEntity(this.solutionId, id)
                    .then(() => {
                        this.getEntities();
                    })
                    .catch(error => this.error = error);
            });
    }

    onPageSelected(pageIndex: number) {
        this.pageIndex = pageIndex;
        this.getEntities();
    }
}
