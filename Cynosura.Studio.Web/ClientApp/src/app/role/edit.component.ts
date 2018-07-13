import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Role } from "./role.model";
import { RoleService } from "./role.service";

import { Error } from "../core/error.model";


@Component({
    selector: "role-edit",
    templateUrl: "./edit.component.html"
})
export class RoleEditComponent implements OnInit {
    role: Role;
    error: Error;

    constructor(private roleService: RoleService,
        private route: ActivatedRoute,
        private router: Router) { }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params["id"];
            this.getRole(id);
        });
    }

    private getRole(id: number): void {
        if (id === 0) {
            this.role = new Role();
        } else {
            this.roleService.getRole(id).then(role => this.role = role);
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveRole();
    }

    private saveRole(): void {
        if (this.role.id) {
            this.roleService.updateRole(this.role)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.roleService.createRole(this.role)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }
}
