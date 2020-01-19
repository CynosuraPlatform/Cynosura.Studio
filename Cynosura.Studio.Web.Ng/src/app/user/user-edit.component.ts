import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { User } from "../user-core/user.model";
import { CreateUser, UpdateUser } from "../user-core/user-request.model";
import { UserService } from "../user-core/user.service";

import { Role } from "../role-core/role.model";
import { RoleService } from "../role-core/role.service";

import { Error } from "../core/error.model";
import { NoticeHelper } from "../core/notice.helper";


@Component({
    selector: "app-user-edit",
    templateUrl: "./user-edit.component.html",
    styleUrls: ["./user-edit.component.scss"]
})
export class UserEditComponent implements OnInit {
    id: number;
    userForm = this.fb.group({
        id: [],
        userName: [],
        email: [],
        password: [],
        confirmPassword: []
    });
    user: User;
    roles: Role[] = [];
    error: Error;

    constructor(private userService: UserService,
                private roleService: RoleService,
                private route: ActivatedRoute,
                private router: Router,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) { }

    async ngOnInit() {
        this.roles = (await this.roleService.getRoles({})).pageItems;
        this.route.params.forEach((params: Params) => {
            const id = +params.id;
            this.getUser(id);
        });
    }

    private async getUser(id: number) {
        this.id = id;
        if (id === 0) {
            this.user = new User();
        } else {
            this.user = await this.userService.getUser({ id });
        }
        this.userForm.patchValue(this.user);
        for (const role of this.roles) {
            if (this.user.roleIds && this.user.roleIds.indexOf(role.id) !== -1) {
                role.isSelected = true;
            }
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveUser();
    }

    private async saveUser() {
        try {
            const user = this.userForm.value;
            user.roleIds = this.roles
                .filter(role => role.isSelected)
                .map(role => role.id);
            if (this.id) {
                await this.userService.updateUser(user);
            } else {
                await this.userService.createUser(user);
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
            Error.setFormErrors(this.userForm, error);
        }
    }
}
