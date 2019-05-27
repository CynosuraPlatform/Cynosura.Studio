import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { MatSnackBar } from "@angular/material";

import { EnumValue } from "../enum-value-core/enum-value.model";

import { Error } from "../core/error.model";


@Component({
    selector: "app-enum-value-edit",
    templateUrl: "./enum-value-edit.component.html",
    styleUrls: ["./enum-value-edit.component.scss"]
})
export class EnumValueEditComponent implements OnInit {
    @Input()
    solutionId: number;
    enumValueForm = this.fb.group({
        id: [],
        name: [],
        displayName: [],
        value: [],
        enumId: []
    });
    private localEnumValue: EnumValue;
    get enumValue(): EnumValue {
        return this.localEnumValue;
    }

    @Input()
    set enumValue(value: EnumValue) {
        this.localEnumValue = value;
        this.enumValueForm.reset();
        this.enumValueForm.patchValue(value);
    }

    @Output()
    enumValueSave = new EventEmitter<EnumValue>();
    error: Error;

    constructor(private fb: FormBuilder,
                private snackBar: MatSnackBar) {
    }

    ngOnInit(): void {

    }

    onSubmit(): void {
        this.saveEnumValue();
    }

    private saveEnumValue(): void {
        const enumValue: EnumValue = this.enumValueForm.value;
        enumValue.properties = this.localEnumValue.properties;
        this.enumValueSave.emit(enumValue);
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.snackBar.open(error.message, "Ok");
            Error.setFormErrors(this.enumValueForm, error);
        }
    }
}
