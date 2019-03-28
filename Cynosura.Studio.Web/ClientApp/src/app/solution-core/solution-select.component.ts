import { Component, Input, OnInit, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

import { Solution } from "./solution.model";
import { SolutionService } from "./solution.service";

@Component({
    selector: "app-solution-select",
    templateUrl: "./solution-select.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SolutionSelectComponent),
            multi: true
        }
    ]
})

export class SolutionSelectComponent implements OnInit, ControlValueAccessor {
    constructor(private solutionService: SolutionService) { }

    solutions: Solution[] = [];

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
        this.solutionService.getSolutions().then(solutions => this.solutions = solutions.pageItems);
    }

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.value = value;
    }
}
