import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { PageEvent } from "@angular/material/paginator";

import { Role } from "../role-core/role.model";
import { RoleFilter } from "../role-core/role-filter.model";
import { RoleService } from "../role-core/role.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";
import { NoticeHelper } from "../core/notice.helper";

class RoleListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new RoleFilter();
}

@Component({
    selector: "app-role-list",
    templateUrl: "./role-list.component.html",
    styleUrls: ["./role-list.component.scss"]
})
export class RoleListComponent implements OnInit {
    content: Page<Role>;
    state: RoleListState;
    pageSizeOptions = [10, 20];
    columns = [
        "name",
        "action"
    ];

    constructor(
        private modalHelper: ModalHelper,
        private roleService: RoleService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService,
        private noticeHelper: NoticeHelper
        ) {
        this.state = this.storeService.get("roleListState", new RoleListState());
    }

    ngOnInit(): void {
        this.getRoles();
    }

    async getRoles() {
        this.content = await this.roleService.getRoles({
            pageIndex: this.state.pageIndex,
            pageSize: this.state.pageSize,
            filter: this.state.filter
        });
    }

    reset(): void {
        this.state.filter.text = null;
        this.getRoles();
    }

    delete(id: number): void {
        this.modalHelper.confirmDelete()
            .subscribe(async () => {
                try {
                    await this.roleService.deleteRole({ id });
                    this.getRoles();
                } catch (error) {
                    this.onError(error);
                }
            });
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
