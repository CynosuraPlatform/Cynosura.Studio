import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { MatSnackBar } from "@angular/material";

import { User } from "../user-core/user.model";
import { CreateUser, UpdateUser } from "../user-core/user-request.model";
import { UserService } from "../user-core/user.service";

import { Role } from "../role-core/role.model";
import { RoleService } from "../role-core/role.service";

import { Error } from "../core/error.model";


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
    roles: Role[] = [];
    error: Error;

    constructor(private userService: UserService,
                private roleService: RoleService,
                private route: ActivatedRoute,
                private router: Router,
                private fb: FormBuilder,
                private snackBar: MatSnackBar) { }

    ngOnInit(): void {
        this.roleService.getRoles({}).then(roles => this.roles = roles.pageItems).then(() =>
            this.route.params.forEach((params: Params) => {
                const id = +params.id;
                this.getUser(id);
            }));
    }

    private getUser(id: number): void {
        this.id = id;
        if (id === 0) {
            this.userForm.patchValue(new User());
        } else {
            this.userService.getUser({ id }).then(user => {
                this.userForm.patchValue(user);
                for (const role of this.roles) {
                    if (user.roleIds.indexOf(role.id) !== -1) {
                        role.isSelected = true;
                    }
                }
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveUser();
    }

    private saveUser(): void {
        const user = this.userForm.value;
        user.roleIds = this.roles
            .filter(role => role.isSelected)
            .map(role => role.id);
        if (this.id) {
            this.userService.updateUser(user)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        } else {
            this.userService.createUser(user)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        }
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.snackBar.open(error.message, "Ok");
            Error.setFormErrors(this.userForm, error);
        }
    }
}
