import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { MatSnackBar } from "@angular/material";

import { Role } from "../role-core/role.model";
import { RoleService } from "../role-core/role.service";

import { Error } from "../core/error.model";


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
                private snackBar: MatSnackBar) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id = +params.id;
            this.getRole(id);
        });
    }

    private getRole(id: number): void {
        this.id = id;
        if (id === 0) {
            this.role = new Role();
            this.roleForm.patchValue(this.role);
        } else {
            this.roleService.getRole({ id }).then(role => {
                this.role = role;
                this.roleForm.patchValue(this.role);
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveRole();
    }

    private saveRole(): void {
        if (this.id) {
            this.roleService.updateRole(this.roleForm.value)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        } else {
            this.roleService.createRole(this.roleForm.value)
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
            Error.setFormErrors(this.roleForm, error);
        }
    }
}
