import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { EnumValue } from "../enum-value-core/enum-value.model";

import { Error } from "../core/error.model";


@Component({
    selector: "app-enum-value-edit",
    templateUrl: "./enum-value-edit.component.html"
})
export class EnumValueEditComponent implements OnInit {
    @Input()
    solutionId: number;

    innerEnumValue: EnumValue;

    private localEnumValue: EnumValue;

    get enumValue(): EnumValue {
        return this.localEnumValue;
    }

    @Input()
    set enumValue(value: EnumValue) {
        this.localEnumValue = value;
        this.innerEnumValue = { ...value };
    }

    @Output()
    enumValueSave = new EventEmitter<EnumValue>();
    error: Error;

    constructor() {
    }

    ngOnInit(): void {

    }

    onSubmit(): void {
        this.saveEnumValue();
    }

    private saveEnumValue(): void {
        this.enumValueSave.emit(this.innerEnumValue);
    }

}
