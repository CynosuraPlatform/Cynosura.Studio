import { Component, Input, OnInit, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

import { Role } from "./role.model";
import { RoleService } from "./role.service";

@Component({
    selector: "app-role-select",
    templateUrl: "./role-select.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => RoleSelectComponent),
            multi: true
        }
    ]
})

export class RoleSelectComponent implements OnInit, ControlValueAccessor {
    constructor(private roleService: RoleService) { }

    roles: Role[] = [];

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
        this.roleService.getRoles().then(roles => this.roles = roles.pageItems);
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
