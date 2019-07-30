import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { PageEvent } from "@angular/material/paginator";
import { MatSnackBar } from "@angular/material";

import { Role } from "../role-core/role.model";
import { RoleFilter } from "../role-core/role-filter.model";
import { RoleService } from "../role-core/role.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

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
    ];

    constructor(
        private modalHelper: ModalHelper,
        private roleService: RoleService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService,
        private snackBar: MatSnackBar
        ) {
        this.state = this.storeService.get("roleListState", new RoleListState());
    }

    ngOnInit(): void {
        this.getRoles();
    }

    getRoles(): void {
        this.roleService.getRoles({ pageIndex: this.state.pageIndex, pageSize: this.state.pageSize, filter: this.state.filter })
            .then(content => {
                this.content = content;
            })
            .catch(error => this.onError(error));
    }

    reset(): void {
        this.state.filter.text = null;
        this.getRoles();
    }

    delete(id: number): void {
        this.modalHelper.confirmDelete()
            .subscribe(() => {
                this.roleService.deleteRole({ id })
                    .then(() => {
                        this.getRoles();
                    })
                    .catch(error => this.onError(error));
            });
    }

    onPage(page: PageEvent) {
        this.state.pageIndex = page.pageIndex;
        this.state.pageSize = page.pageSize;
        this.getRoles();
    }

    onError(error: Error) {
        if (error) {
            this.snackBar.open(error.message, "Ok");
        }
    }
}
