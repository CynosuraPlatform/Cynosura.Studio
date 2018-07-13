import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { Modal } from "ngx-modialog/plugins/bootstrap";

import { Role } from "./role.model";
import { RoleService } from "./role.service";

import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "role-list",
    templateUrl: "./list.component.html"
})
export class RoleListComponent implements OnInit {
    content: Page<Role>;
    error: Error;
    pageSize = 10;

    constructor(
        private modal: Modal,
        private roleService: RoleService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService
        ) {}

    ngOnInit(): void {
        let pageIndex = this.storeService.get("rolesPageIndex");
        if (!pageIndex) pageIndex = 0;
        this.getRoles(pageIndex);
    }

    getRoles(pageIndex: number): void {
        this.roleService.getRoles(pageIndex, this.pageSize)
            .then(content => {
                if (content.pageItems.length == 0 && content.totalItems != 0) {
                    this.content.currentPageIndex--;
                    this.storeService.set("rolesPageIndex", this.content.currentPageIndex);
                    this.getRoles(this.content.currentPageIndex);
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
                this.roleService.deleteRole(id)
                    .then(() => {
                        this.getRoles(this.content.currentPageIndex);
                    })
                    .catch(error => this.error = error);
            })
            .catch(() => {});
    }

    onPageSelected(pageIndex: number) {
        this.storeService.set("rolesPageIndex", pageIndex);
        this.getRoles(pageIndex);
    }
}
