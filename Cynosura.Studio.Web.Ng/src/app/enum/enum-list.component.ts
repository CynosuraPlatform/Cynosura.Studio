import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { mergeMap } from 'rxjs/operators';

import { ModalHelper } from '../core/modal.helper';
import { StoreService } from '../core/store.service';
import { Error } from '../core/error.model';
import { Page } from '../core/page.model';
import { NoticeHelper } from '../core/notice.helper';

import { Enum } from '../enum-core/enum.model';
import { EnumFilter } from '../enum-core/enum-filter.model';
import { EnumService } from '../enum-core/enum.service';
import { EnumEditComponent } from './enum-edit.component';

class EnumListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new EnumFilter();
}

@Component({
    selector: 'app-enum-list',
    templateUrl: './enum-list.component.html',
    styleUrls: ['./enum-list.component.scss']
})
export class EnumListComponent implements OnInit {
    content: Page<Enum>;
    state: EnumListState;
    pageSizeOptions = [10, 20];
    columns = [
        'name',
        'displayName',
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
        this.getEnums();
    }

    constructor(
        private dialog: MatDialog,
        private modalHelper: ModalHelper,
        private enumService: EnumService,
        private storeService: StoreService,
        private noticeHelper: NoticeHelper
        ) {
            this.state = this.storeService.get('enumListState', new EnumListState());
    }

    ngOnInit(): void {
        this.getEnums();
    }

    private getEnums() {
        if (this.solutionId) {
            this.enumService.getEnums({
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
        this.getEnums();
    }

    onReset() {
        this.state.filter.text = null;
        this.getEnums();
    }

    onCreate() {
        EnumEditComponent.show(this.dialog, this.solutionId, '').subscribe(() => {
            this.getEnums();
        });
    }

    onEdit(id: string) {
        EnumEditComponent.show(this.dialog, this.solutionId, id).subscribe(() => {
            this.getEnums();
        });
    }

    onDelete(id: string) {
        this.modalHelper.confirmDelete()
            .pipe(
                mergeMap(() => this.enumService.deleteEnum({ solutionId: this.solutionId, id }))
            )
            .subscribe(() => this.getEnums(),
                error => this.onError(error));
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
