import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { EnumValue } from "../enum-value-core/enum-value.model";

import { Error } from "../core/error.model";
import { NoticeHelper } from "../core/notice.helper";
import { ConvertStringTo } from "../core/converter.helper";


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
                private noticeHelper: NoticeHelper) {
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
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.enumValueForm, error);
        }
    }
}
