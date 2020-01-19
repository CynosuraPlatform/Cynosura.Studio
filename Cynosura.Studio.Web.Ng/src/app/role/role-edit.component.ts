import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Role } from "../role-core/role.model";
import { RoleService } from "../role-core/role.service";

import { Error } from "../core/error.model";
import { NoticeHelper } from "../core/notice.helper";


@Component({
    selector: "app-role-edit",
    templateUrl: "./role-edit.component.html",
    styleUrls: ["./role-edit.component.scss"]
})
export class RoleEditComponent implements OnInit {
    id: number;
    roleForm = this.fb.group({
        id: [],
        name: []
    });
    role: Role;
    error: Error;

    constructor(private roleService: RoleService,
                private route: ActivatedRoute,
                private router: Router,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id = +params.id;
            this.getRole(id);
        });
    }

    private async getRole(id: number) {
        this.id = id;
        if (id === 0) {
            this.role = new Role();
        } else {
            this.role = await this.roleService.getRole({ id });
        }
        this.roleForm.patchValue(this.role);
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveRole();
    }

    private async saveRole() {
        try {
            if (this.id) {
                await this.roleService.updateRole(this.roleForm.value);
            } else {
                await this.roleService.createRole(this.roleForm.value);
            }
            window.history.back();
        } catch (error) {
            this.onError(error);
        }
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.roleForm, error);
        }
    }
}
