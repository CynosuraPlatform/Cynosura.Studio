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
        this.solutionService.getSolutions().then(solutions => this.solutions = solutions.pageItems);
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
