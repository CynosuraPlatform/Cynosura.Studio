import { Component, Input, OnInit, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

import { User } from "./user.model";
import { UserService } from "./user.service";

@Component({
    selector: "app-user-select",
    templateUrl: "./user-select.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => UserSelectComponent),
            multi: true
        }
    ]
})

export class UserSelectComponent implements OnInit, ControlValueAccessor {
    constructor(private userService: UserService) { }

    users: User[] = [];

    @Input()
    value: number | null = null;

    get innerValue() {
        return this.value;
    }

    set innerValue(val) {
        this.value = val;
        this.onChange(val);
        this.onTouched();
    }

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    readonly = false;

    onChange: any = () => { };
    onTouched: any = () => { };

    ngOnInit(): void {
        this.userService.getUsers().then(users => this.users = users.pageItems);
    }

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.innerValue = value;
    }
}
