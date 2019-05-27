import { Component, Input, OnInit, forwardRef, OnDestroy, ElementRef, Optional, Self } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NgControl } from "@angular/forms";
import { MatFormFieldControl } from "@angular/material";
import { FocusMonitor } from "@angular/cdk/a11y";
import { coerceBooleanProperty } from "@angular/cdk/coercion";

import { Subject } from "rxjs";

import { Role } from "./role.model";
import { RoleService } from "./role.service";

@Component({
    selector: "app-role-select",
    templateUrl: "./role-select.component.html",
    providers: [
        { provide: MatFormFieldControl, useExisting: RoleSelectComponent }
    ]
})

export class RoleSelectComponent implements OnInit, ControlValueAccessor, MatFormFieldControl<number | null>, OnDestroy {

    static nextId = 0;

    stateChanges = new Subject<void>();
    focused = false;
    controlType = "app-role-select";
    id = `role-select-${RoleSelectComponent.nextId++}`;
    describedBy = "";

    get errorState(): boolean {
        return coerceBooleanProperty(this.ngControl.errors);
    }

    get empty() {
        return !this.value;
    }

    get shouldLabelFloat() { return this.focused || !this.empty; }

    Role = Role;

    constructor(private roleService: RoleService,
                private fm: FocusMonitor, private elRef: ElementRef<HTMLElement>,
                @Optional() @Self() public ngControl: NgControl) {
        fm.monitor(elRef, true).subscribe(origin => {
            this.focused = !!origin;
            this.stateChanges.next();
        });
        if (this.ngControl !== null) {
            this.ngControl.valueAccessor = this;
        }
    }

    roles: Role[] = [];

    @Input()
    value: number | null = null;

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    placeholder: string;

    @Input()
    readonly = false;

    @Input()
    get required(): boolean { return this.innerRequired; }
    set required(value: boolean) {
        this.innerRequired = coerceBooleanProperty(value);
        this.stateChanges.next();
    }
    private innerRequired = false;

    @Input()
    get disabled(): boolean { return this.innerDisabled; }
    set disabled(value: boolean) {
        this.innerDisabled = coerceBooleanProperty(value);
        this.stateChanges.next();
    }
    private innerDisabled = false;

    get innerValue() {
        return this.value;
    }

    set innerValue(val) {
        this.value = val;
        this.onChange(val);
        this.onTouched();
    }

    onChange: any = () => { };
    onTouched: any = () => { };

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.innerValue = value;
    }

    ngOnInit(): void {
        this.roleService.getRoles({}).then(roles => this.roles = roles.pageItems);
    }

    ngOnDestroy() {
        this.stateChanges.complete();
        this.fm.stopMonitoring(this.elRef);
    }

    setDescribedByIds(ids: string[]) {
        this.describedBy = ids.join(" ");
    }

    onContainerClick(event: MouseEvent) {
    }
}
