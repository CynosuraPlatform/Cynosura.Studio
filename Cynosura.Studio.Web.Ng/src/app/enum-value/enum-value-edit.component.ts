import { Component, Input, OnInit, Output, EventEmitter, Inject } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

import { EnumValue } from "../enum-value-core/enum-value.model";

import { Error } from "../core/error.model";
import { NoticeHelper } from "../core/notice.helper";


class DialogData {
    enumValue: EnumValue;
    solutionId: number;
}

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

    error: Error;

    constructor(public dialogRef: MatDialogRef<EnumValueEditComponent>,
                @Inject(MAT_DIALOG_DATA) public data: DialogData,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {
        this.solutionId = data.solutionId;
        this.enumValue = data.enumValue;
    }

    ngOnInit(): void {

    }

    save(): void {
        this.saveEnumValue();
    }

    private saveEnumValue(): void {
        const enumValue: EnumValue = this.enumValueForm.value;
        enumValue.properties = this.enumValue.properties;
        this.dialogRef.close(enumValue);
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.enumValueForm, error);
        }
    }
}
