import { Component, Input, OnInit, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

import { Enum } from "./enum.model";
import { EnumService } from "./enum.service";

@Component({
    selector: "app-enum-select",
    templateUrl: "./enum-select.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EnumSelectComponent),
            multi: true
        }
    ]
})

export class EnumSelectComponent implements OnInit, ControlValueAccessor {
    constructor(private enumService: EnumService) { }

    enums: Enum[] = [];

    @Input()
    solutionId: number;

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
        this.enumService.getEnums(this.solutionId).then(enums => this.enums = enums.pageItems);
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
