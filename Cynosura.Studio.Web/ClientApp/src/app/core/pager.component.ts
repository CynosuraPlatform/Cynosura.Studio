import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "app-pager",
    templateUrl: "./pager.component.html"
})

export class PagerComponent {
    pages: number[] = [0];
    innerTotalItems = 0;
    innerPageSize = 10;
    innerCurrentPage = 0;
    innerButtonsMaxRange = 3;

    @Input()
    set currentPage(currentPage: number) {
        this.innerCurrentPage = currentPage;
        this.updatePages();
    }

    get currentPage() { return this.innerCurrentPage; }

    @Input()
    set buttonsMaxRange(buttonsMaxRange: number) {
        this.innerButtonsMaxRange = buttonsMaxRange;
        this.updatePages();
    }

    get buttonsMaxRange() { return this.innerButtonsMaxRange; }

    @Output()
    currentPageChange = new EventEmitter<number>();

    onChangeObj(page: number) {
        this.currentPage = page;
        this.currentPageChange.emit(page);
    }

    @Input()
    set pageSize(pageSize: number) {
        this.innerPageSize = pageSize;
        this.updatePages();
    }

    get pageSize() { return this.innerPageSize; }

    @Input()
    set totalItems(totalItems: number) {
        this.innerTotalItems = totalItems;
        this.updatePages();
    }

    get totalItems() { return this.innerTotalItems; }

    private updatePages() {
        const totalPages = Math.ceil(this.innerTotalItems / this.innerPageSize);

        if (this.innerCurrentPage >= totalPages && totalPages > 0) {
            this.onChangeObj(totalPages - 1);
            return;
        }

        const temp: number[] = [];
        this.pages = [];

        temp[0] = 0;
        temp[totalPages - 1] = totalPages - 1;

        for (let i = this.innerCurrentPage - this.innerButtonsMaxRange; i <= this.innerCurrentPage + this.innerButtonsMaxRange; i++) {
            temp[i] = i;
        }

        let k = 0;
        for (let i = 0; i < temp.length; i++) {
            if ((temp[i] >= 0 && temp[i] < totalPages || temp[i] === null) && !(temp[i] === null && temp[i - 1] === null)) {
                this.pages[k] = temp[i];
                k++;
            }
        }
    }
}
