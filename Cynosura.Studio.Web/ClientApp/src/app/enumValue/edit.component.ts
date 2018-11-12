import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { EnumValue } from "../enumValue-core/enumValue.model";

import { Error } from "../core/error.model";


@Component({
    selector: "enumValue-edit",
    templateUrl: "./edit.component.html"
})
export class EnumValueEditComponent implements OnInit {
    @Input()
    solutionId: number;

    innerEnumValue: EnumValue;

    private _enumValue: EnumValue;

    get enumValue(): EnumValue {
        return this._enumValue;
    }

    @Input()
    set enumValue(value: EnumValue) {
        this._enumValue = value;
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
