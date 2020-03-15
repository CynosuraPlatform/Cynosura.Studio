import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { mergeMap } from 'rxjs/operators';

import { ModalHelper } from '../core/modal.helper';
import { StoreService } from '../core/store.service';
import { Error } from '../core/error.model';
import { Page } from '../core/page.model';
import { NoticeHelper } from '../core/notice.helper';

import { Solution } from '../solution-core/solution.model';
import { SolutionFilter } from '../solution-core/solution-filter.model';
import { SolutionService } from '../solution-core/solution.service';
import { SolutionEditComponent } from './solution-edit.component';
import { SolutionOpenComponent } from './solution-open.component';

class SolutionListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new SolutionFilter();
}

@Component({
    selector: 'app-solution-list',
    templateUrl: './solution-list.component.html',
    styleUrls: ['./solution-list.component.scss']
})
export class SolutionListComponent implements OnInit {
    content: Page<Solution>;
    state: SolutionListState;
    pageSizeOptions = [10, 20];
    columns = [
        'name',
        'path',
        'templateName',
        'templateVersion',
        'action'
    ];

    constructor(
        private modalHelper: ModalHelper,
        private solutionService: SolutionService,
        private storeService: StoreService,
        private dialog: MatDialog,
        private noticeHelper: NoticeHelper
        ) {
        this.state = this.storeService.get('solutionListState', new SolutionListState());
    }

    ngOnInit() {
        this.getSolutions();
    }

    private getSolutions() {
        this.solutionService.getSolutions({
            pageIndex: this.state.pageIndex,
            pageSize: this.state.pageSize,
            filter: this.state.filter
        }).subscribe(content => this.content = content);
    }

    onSearch() {
        this.getSolutions();
    }

    onReset() {
        this.state.filter.text = null;
        this.getSolutions();
    }

    onCreate() {
        SolutionEditComponent.show(this.dialog, 0).subscribe(() => {
            this.getSolutions();
        });
    }

    onEdit(id: number) {
        SolutionEditComponent.show(this.dialog, id).subscribe(() => {
            this.getSolutions();
        });
    }

    onDelete(id: number) {
        this.modalHelper.confirmDelete()
            .pipe(
                mergeMap(() => this.solutionService.deleteSolution({ id }))
            )
            .subscribe(() => this.getSolutions(),
                error => this.onError(error));
    }

    onPage(page: PageEvent) {
        this.state.pageIndex = page.pageIndex;
        this.state.pageSize = page.pageSize;
        this.getSolutions();
    }

    onError(error: Error) {
        if (error) {
            this.noticeHelper.showError(error);
        }
    }

    open() {
        this.dialog.open(SolutionOpenComponent, {
            width: '600px'
        });
    }
}
