import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { Modal } from "ngx-modialog/plugins/bootstrap";

import { User } from "./user.model";
import { UserService } from "./user.service";

import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "user-list",
    templateUrl: "./list.component.html"
})
export class UserListComponent implements OnInit {
    content: Page<User>;
    error: Error;
    pageSize = 10;

    constructor(
        private modal: Modal,
        private userService: UserService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService
        ) {}

    ngOnInit(): void {
        let pageIndex = this.storeService.get("usersPageIndex");
        if (!pageIndex) pageIndex = 0;
        this.getUsers(pageIndex);
    }

    getUsers(pageIndex: number): void {
        this.userService.getUsers(pageIndex, this.pageSize)
            .then(content => {
                if (content.pageItems.length == 0 && content.totalItems != 0) {
                    this.content.currentPageIndex--;
                    this.storeService.set("usersPageIndex", this.content.currentPageIndex);
                    this.getUsers(this.content.currentPageIndex);
                }
                this.content = content;
            })
            .catch(error => this.error = error);
    }

    edit(id: number): void {
        this.router.navigate([id], { relativeTo: this.route });
    }

    add(): void {
        this.router.navigate([0], { relativeTo: this.route });
    }

    delete(id: number): void {
        const dialogRef = this.modal
            .confirm()
            .size("sm")
            .keyboard(27)
            .title("Delete?")
            .body("Are you sure you want to delete?")
            .okBtn("Delete")
            .cancelBtn("Cancel")
            .open();
        dialogRef.result
            .then(() => {
                this.userService.deleteUser(id)
                    .then(() => {
                        this.getUsers(this.content.currentPageIndex);
                    })
                    .catch(error => this.error = error);
            })
            .catch(() => {});
    }

    onPageSelected(pageIndex: number) {
        this.storeService.set("usersPageIndex", pageIndex);
        this.getUsers(pageIndex);
    }
}
