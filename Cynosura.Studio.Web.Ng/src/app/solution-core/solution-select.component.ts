import { Component, Input, OnInit, forwardRef, OnDestroy, ElementRef, Optional, Self, DoCheck } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NgControl } from "@angular/forms";
import { MatFormFieldControl } from "@angular/material";
import { FocusMonitor } from "@angular/cdk/a11y";
import { coerceBooleanProperty } from "@angular/cdk/coercion";

import { Subject } from "rxjs";

import { Solution } from "./solution.model";
import { SolutionService } from "./solution.service";

@Component({
    selector: "app-solution-select",
    templateUrl: "./solution-select.component.html",
    providers: [
        { provide: MatFormFieldControl, useExisting: SolutionSelectComponent }
    ]
})

export class SolutionSelectComponent implements OnInit, ControlValueAccessor, MatFormFieldControl<number | null>, OnDestroy, DoCheck {

    static nextId = 0;

    stateChanges = new Subject<void>();
    focused = false;
    controlType = "app-solution-select";
    id = `solution-select-${SolutionSelectComponent.nextId++}`;
    describedBy = "";

    errorState = false;

    get empty() {
        return !this.value;
    }

    get shouldLabelFloat() { return this.focused || !this.empty; }

    Solution = Solution;

    solutions: Solution[] = [];

    @Input()
    value: number | null = null;

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    placeholder: string;

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

    constructor(private solutionService: SolutionService,
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
        this.solutionService.getSolutions({}).then(solutions => this.solutions = solutions.pageItems);
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

    ngDoCheck(): void {
        if (this.ngControl) {
            this.errorState = this.ngControl.invalid;
            this.stateChanges.next();
        }
    }
}
