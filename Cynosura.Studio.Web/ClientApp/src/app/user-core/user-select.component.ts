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

    onChange: any = () => { };
    onTouched: any = () => { };

    @Input("value")
    val: number | null = null;

    get value() {
        return this.val;
    }

    set value(val) {
        this.val = val;
        this.onChange(val);
        this.onTouched();
    }

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    readonly = false;

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
        if (value) {
            this.value = value;
        }
    }
}
