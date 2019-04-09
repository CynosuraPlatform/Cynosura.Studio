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
        this.roleService.getRoles().then(roles => this.roles = roles.pageItems);
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
