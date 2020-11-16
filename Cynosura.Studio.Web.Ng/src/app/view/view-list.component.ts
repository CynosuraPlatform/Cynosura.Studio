import { Component, OnInit, Input } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { mergeMap } from 'rxjs/operators';

import { ModalHelper } from '../core/modal.helper';
import { StoreService } from '../core/store.service';
import { Error } from '../core/error.model';
import { Page } from '../core/page.model';
import { NoticeHelper } from '../core/notice.helper';

import { View, ViewListState } from '../view-core/view.model';
import { ViewService } from '../view-core/view.service';
import { ViewEditComponent } from './view-edit.component';

@Component({
    selector: 'app-view-list',
    templateUrl: './view-list.component.html',
    styleUrls: ['./view-list.component.scss']
})
export class ViewListComponent implements OnInit {
    content: Page<View>;
    pageSizeOptions = [10, 20];
    columns = [
        'name',
        'action'
    ];
    private innerSolutionId: number;
    get solutionId(): number {
        if (!this.innerSolutionId) {
            this.innerSolutionId = this.storeService.get('solutionId', 0);
        }
        return this.innerSolutionId;
    }
    set solutionId(val: number) {
        this.innerSolutionId = val;
        this.storeService.set('solutionId', this.innerSolutionId);
        this.getViews();
    }

    @Input()
    state: ViewListState;

    @Input()
    baseRoute = '/view';

    constructor(
        private dialog: MatDialog,
        private modalHelper: ModalHelper,
        private viewService: ViewService,
        private storeService: StoreService,
        private noticeHelper: NoticeHelper
        ) {
    }

    ngOnInit() {
        this.getViews();
    }

    private getViews() {
        if (this.solutionId) {
            this.viewService.getViews({
                solutionId: this.solutionId,
                pageIndex: this.state.pageIndex,
                pageSize: this.state.pageSize,
                filter: this.state.filter
            }).subscribe(content => this.content = content);
        } else {
            this.content = null;
        }
    }

    onSearch() {
        this.getViews();
    }

    onReset() {
        this.state.filter.text = null;
        this.getViews();
    }

    onCreate() {
        ViewEditComponent.show(this.dialog, this.solutionId, null).subscribe(() => {
            this.getViews();
        });
    }

    onExport(): void {
        this.viewService.exportViews({ solutionId: this.solutionId, filter: this.state.filter })
            .subscribe(file => {
                file.download();
            });
    }

    onEdit(id: string) {
        ViewEditComponent.show(this.dialog, this.solutionId, id).subscribe(() => {
            this.getViews();
        });
    }

    onDelete(id: string) {
        this.modalHelper.confirmDelete()
            .pipe(
                mergeMap(() => this.viewService.deleteView({ solutionId: this.solutionId, id }))
            )
            .subscribe(() => this.getViews(),
                error => this.onError(error));
    }

    onPage(page: PageEvent) {
        this.state.pageIndex = page.pageIndex;
        this.state.pageSize = page.pageSize;
        this.getViews();
    }

    onError(error: Error) {
        if (error) {
            this.noticeHelper.showError(error);
        }
    }
}
