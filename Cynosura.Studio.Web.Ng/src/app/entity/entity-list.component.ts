import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";

import { Entity } from "../entity-core/entity.model";
import { EntityFilter } from "../entity-core/entity-filter.model";
import { EntityService } from "../entity-core/entity.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "app-entity-list",
    templateUrl: "./entity-list.component.html"
})
export class EntityListComponent implements OnInit {
    content: Page<Entity>;
    error: Error;
    pageSize = 10;
    filter = new EntityFilter();
    private innerPageIndex: number;
    get pageIndex(): number {
        if (!this.innerPageIndex) {
            this.innerPageIndex = this.storeService.get("entitiesPageIndex", 0);
        }
        return this.innerPageIndex;
    }
    set pageIndex(value: number) {
        this.innerPageIndex = value;
        this.storeService.set("entitiesPageIndex", value);
    }
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
        private storeService: StoreService
        ) {}

    ngOnInit(): void {
        this.getEntities();
    }

    getEntities(): void {
        if (this.solutionId) {
            this.entityService.getEntities(this.solutionId, this.pageIndex, this.pageSize, this.filter)
                .then(content => {
                    this.content = content;
                })
                .catch(error => this.error = error);
        } else {
            this.content = null;
        }
    }

    reset(): void {
        this.filter.text = null;
        this.getEntities();
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
            })
            .catch(() => { });
    }

    onPageSelected(pageIndex: number) {
        this.pageIndex = pageIndex;
        this.getEntities();
    }
}
