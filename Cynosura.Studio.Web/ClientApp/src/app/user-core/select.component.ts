import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";

import { User } from "./user.model";
import { UserService } from "./user.service";

@Component({
    selector: "user-select",
    templateUrl: "./select.component.html"
})

export class UserSelectComponent implements OnInit {
    constructor(private userService: UserService) { }

    users: User[] = [];

    @Input()
    selectedUserId: number | null = null;

    @Output()
    selectedUserIdChange = new EventEmitter<number>();

    @Input()
    readonly = false;

    ngOnInit(): void {
        this.userService.getUsers().then(users => this.users = users.pageItems);
    }
}
