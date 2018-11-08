import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";

import { Role } from "./role.model";
import { RoleService } from "./role.service";

@Component({
    selector: "role-select",
    templateUrl: "./select.component.html"
})

export class RoleSelectComponent implements OnInit {
    constructor(private roleService: RoleService) { }

    roles: Role[] = [];

    @Input()
    selectedRoleId: number | null = null;

    @Output()
    selectedRoleIdChange = new EventEmitter<number>();

    @Input()
    readonly = false;

    ngOnInit(): void {
        this.roleService.getRoles().then(roles => this.roles = roles.pageItems);
    }
}