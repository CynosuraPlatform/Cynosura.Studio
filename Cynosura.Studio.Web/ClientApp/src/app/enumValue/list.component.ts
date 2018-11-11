import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";

import { EnumValue } from "../enumValue-core/enumValue.model";
import { EnumValueService } from "../enumValue-core/enumValue.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "enumValue-list",
    templateUrl: "./list.component.html"
})
export class EnumValueListComponent implements OnInit {
    content: Page<EnumValue>;
    error: Error;
    pageSize = 10;
    private _pageIndex: number;
    get pageIndex(): number {
        if (!this._pageIndex) {
            this._pageIndex = this.storeService.get("enumValuesPageIndex") | 0;
        }
        return this._pageIndex;
    }
    set pageIndex(value: number) {
        this._pageIndex = value;
        this.storeService.set("enumValuesPageIndex", value);
    }

    constructor(
        private modalHelper: ModalHelper,
        private enumValueService: EnumValueService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService
        ) {}

    ngOnInit(): void {
        this.getEnumValues();
    }

    getEnumValues(): void {        
        this.enumValueService.getEnumValues(this.pageIndex, this.pageSize)
            .then(content => {
                this.content = content;
            })
            .catch(error => this.error = error);
    }

    reset(): void {
        this.enumValueService.getEnumValues(this.content.currentPageIndex, this.pageSize)
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
                this.enumValueService.deleteEnumValue(id)
                    .then(() => {
                        this.getEnumValues();
                    })
                    .catch(error => this.error = error);
            })
            .catch(() => { });
    }

    onPageSelected(pageIndex: number) {
        this.pageIndex = pageIndex;
        this.getEnumValues();
    }
}