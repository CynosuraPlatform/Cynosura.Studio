import { Component, OnInit, Input } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { mergeMap } from 'rxjs/operators';

import { ModalHelper } from '../core/modal.helper';
import { StoreService } from '../core/store.service';
import { Error } from '../core/error.model';
import { Page } from '../core/page.model';
import { NoticeHelper } from '../core/notice.helper';

import { Solution, SolutionListState } from '../solution-core/solution.model';
import { SolutionService } from '../solution-core/solution.service';
import { SolutionEditComponent } from './solution-edit.component';
import { SolutionOpenComponent } from './solution-open.component';

@Component({
    selector: 'app-solution-list',
    templateUrl: './solution-list.component.html',
    styleUrls: ['./solution-list.component.scss']
})
export class SolutionListComponent implements OnInit {
    content: Page<Solution>;
    pageSizeOptions = [10, 20];
    columns = [
        'name',
        'path',
        'templateName',
        'templateVersion',
        'action'
    ];

    @Input()
    state: SolutionListState;

    @Input()
    baseRoute = '/solution';

    constructor(
        private dialog: MatDialog,
        private modalHelper: ModalHelper,
        private solutionService: SolutionService,
        private noticeHelper: NoticeHelper
        ) {
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
        SolutionEditComponent.show(this.dialog, null).subscribe(() => {
            this.getSolutions();
        });
    }

    onExport(): void {
        this.solutionService.exportSolutions({ filter: this.state.filter })
            .subscribe(file => {
                file.download();
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
