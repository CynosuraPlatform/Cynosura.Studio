import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "pager",
    templateUrl: "./pager.component.html"
})

export class PagerComponent {
    pages: number[] = [0];
    _totalItems = 0;
    _pageSize = 10;
    _currentPage = 0;
    _buttonsMaxRange = 3;

    @Input()
    set currentPage(currentPage: number) {
        this._currentPage = currentPage;
        this.updatePages();
    }

    get currentPage() { return this._currentPage; }

    @Input()
    set buttonsMaxRange(buttonsMaxRange: number) {
        this._buttonsMaxRange = buttonsMaxRange;
        this.updatePages();
    }

    get buttonsMaxRange() { return this._buttonsMaxRange; }

    @Output()
    currentPageChange = new EventEmitter<number>();

    onChangeObj(page: number) {
        this.currentPage = page;
        this.currentPageChange.emit(page);
    }

    @Input()
    set pageSize(pageSize: number) {
        this._pageSize = pageSize;
        this.updatePages();
    }

    get pageSize() { return this._pageSize; }

    @Input()
    set totalItems(totalItems: number) {
        this._totalItems = totalItems;
        this.updatePages();
    }

    get totalItems() { return this._totalItems; }

    private updatePages() {
        const totalPages = Math.ceil(this._totalItems / this._pageSize);

        if (this._currentPage >= totalPages && totalPages > 0) {
            this.onChangeObj(totalPages - 1);
            return;
        }

        let temp: number[] = [];
        this.pages = [];

        temp[0] = 0;
        temp[totalPages - 1] = totalPages - 1;

        for (let i = this._currentPage - this._buttonsMaxRange; i <= this._currentPage + this._buttonsMaxRange; i++) {
            temp[i] = i;
        }

        let k = 0;
        for (let i = 0; i < temp.length; i++) {
            if ((temp[i] >= 0 && temp[i] < totalPages || temp[i] == null) && !(temp[i] == null && temp[i - 1] == null)) {
                this.pages[k] = temp[i];
                k++;
            }
        }
    }
}
