import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { PageEvent } from "@angular/material/paginator";
import { MatSnackBar } from "@angular/material";

import { User } from "../user-core/user.model";
import { UserFilter } from "../user-core/user-filter.model";
import { UserService } from "../user-core/user.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

class UserListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new UserFilter();
}

@Component({
    selector: "app-user-list",
    templateUrl: "./user-list.component.html",
    styleUrls: ["./user-list.component.scss"]
})
export class UserListComponent implements OnInit {
    content: Page<User>;
    state: UserListState;
    pageSizeOptions = [10, 20];
    columns = [
        "userName",
        "email",
    ];

    constructor(
        private modalHelper: ModalHelper,
        private userService: UserService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService,
        private snackBar: MatSnackBar
        ) {
        this.state = this.storeService.get("userListState", new UserListState());
    }

    ngOnInit(): void {
        this.getUsers();
    }

    getUsers(): void {
        this.userService.getUsers({ pageIndex: this.state.pageIndex, pageSize: this.state.pageSize, filter: this.state.filter })
            .then(content => {
                this.content = content;
            })
            .catch(error => this.onError(error));
    }

    reset(): void {
        this.state.filter.text = null;
        this.getUsers();
    }

    delete(id: number): void {
        this.modalHelper.confirmDelete()
            .subscribe(() => {
                this.userService.deleteUser({ id })
                    .then(() => {
                        this.getUsers();
                    })
                    .catch(error => this.onError(error));
            });
    }

    onPage(page: PageEvent) {
        this.state.pageIndex = page.pageIndex;
        this.state.pageSize = page.pageSize;
        this.getUsers();
    }

    onError(error: Error) {
        if (error) {
            this.snackBar.open(error.message, "Ok");
        }
    }
}
