import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { mergeMap } from 'rxjs/operators';

import { ModalHelper } from '../core/modal.helper';
import { StoreService } from '../core/store.service';
import { Error } from '../core/error.model';
import { Page } from '../core/page.model';
import { NoticeHelper } from '../core/notice.helper';

import { Role } from '../role-core/role.model';
import { RoleFilter } from '../role-core/role-filter.model';
import { RoleService } from '../role-core/role.service';
import { RoleEditComponent } from './role-edit.component';

class RoleListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new RoleFilter();
}

@Component({
    selector: 'app-role-list',
    templateUrl: './role-list.component.html',
    styleUrls: ['./role-list.component.scss']
})
export class RoleListComponent implements OnInit {
    content: Page<Role>;
    state: RoleListState;
    pageSizeOptions = [10, 20];
    columns = [
        'name',
        'action'
    ];

    constructor(
        private dialog: MatDialog,
        private modalHelper: ModalHelper,
        private roleService: RoleService,
        private storeService: StoreService,
        private noticeHelper: NoticeHelper
        ) {
        this.state = this.storeService.get('roleListState', new RoleListState());
    }

    ngOnInit() {
        this.getRoles();
    }

    private getRoles() {
        this.roleService.getRoles({
            pageIndex: this.state.pageIndex,
            pageSize: this.state.pageSize,
            filter: this.state.filter
        }).subscribe(content => this.content = content);
    }

    onSearch() {
        this.getRoles();
    }

    onReset(): void {
        this.state.filter.text = null;
        this.getRoles();
    }

    onCreate(): void {
        RoleEditComponent.show(this.dialog, 0).subscribe(() => {
            this.getRoles();
        });
    }

    onEdit(id: number) {
        RoleEditComponent.show(this.dialog, id).subscribe(() => {
            this.getRoles();
        });
    }

    onDelete(id: number): void {
        this.modalHelper.confirmDelete()
            .pipe(
                mergeMap(() => this.roleService.deleteRole({ id }))
            )
            .subscribe(() => this.getRoles(),
                error => this.onError(error));
    }

    onPage(page: PageEvent) {
        this.state.pageIndex = page.pageIndex;
        this.state.pageSize = page.pageSize;
        this.getRoles();
    }

    onError(error: Error) {
        if (error) {
            this.noticeHelper.showError(error);
        }
    }
}
