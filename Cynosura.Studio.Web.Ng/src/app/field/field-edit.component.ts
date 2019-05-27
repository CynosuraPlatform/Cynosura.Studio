import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { MatSnackBar } from "@angular/material";

import { Field, FieldType } from "../field-core/field.model";

import { Error } from "../core/error.model";


@Component({
    selector: "app-field-edit",
    templateUrl: "./field-edit.component.html",
    styleUrls: ["./field-edit.component.scss"]
})
export class FieldEditComponent implements OnInit {

    FieldType = FieldType;

    @Input()
    solutionId: number;
    fieldForm = this.fb.group({
        id: [],
        name: [],
        displayName: [],
        type: [],
        size: [],
        entityId: [],
        isRequired: [],
        enumId: [],
        isSystem: []
    });
    private localField: Field;

    get field(): Field {
        return this.localField;
    }

    @Input()
    set field(value: Field) {
        this.localField = value;
        this.fieldForm.reset();
        this.fieldForm.patchValue(value);
    }

    @Output()
    fieldSave = new EventEmitter<Field>();
    error: Error;

    constructor(private route: ActivatedRoute,
                private router: Router,
                private fb: FormBuilder,
                private snackBar: MatSnackBar) {
    }

    ngOnInit(): void {

    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveField();
    }

    private saveField(): void {
        const field: Field = this.fieldForm.value;
        field.properties = this.localField.properties;
        this.fieldSave.emit(field);
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.snackBar.open(error.message, "Ok");
            Error.setFormErrors(this.fieldForm, error);
        }
    }
}
