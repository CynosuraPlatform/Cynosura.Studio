import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Enum } from "../enum-core/enum.model";
import { EnumService } from "../enum-core/enum.service";
import { UpdateEnum, CreateEnum } from "../enum-core/enum-request.model";

import { Error } from "../core/error.model";
import { NoticeHelper } from "../core/notice.helper";
import { ConvertStringTo } from "../core/converter.helper";

@Component({
    selector: "app-enum-edit",
    templateUrl: "./enum-edit.component.html",
    styleUrls: ["./enum-edit.component.scss"]
})
export class EnumEditComponent implements OnInit {
    id: string;
    enumForm = this.fb.group({
        id: [],
        name: [],
        displayName: []
    });
    enum: Enum;
    error: Error;
    solutionId: number;

    constructor(private enumService: EnumService,
                private route: ActivatedRoute,
                private router: Router,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id: string = params.id === "0" ? null : params.id;
            this.solutionId = ConvertStringTo.number(this.route.snapshot.queryParams.solutionId);
            this.getEnum(id);
        });
    }

    private async getEnum(id: string) {
        this.id = id;
        if (!id) {
            this.enum = new Enum();
        } else {
            this.enum = await this.enumService.getEnum({ solutionId: this.solutionId, id });
        }
        this.enumForm.patchValue(this.enum);
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveEnum();
    }

    private async saveEnum() {
        try {
            if (this.id) {
                const updateEnum: UpdateEnum = this.enumForm.value;
                updateEnum.solutionId = this.solutionId;
                updateEnum.properties = this.enum.properties;
                updateEnum.values = this.enum.values;
                await this.enumService.updateEnum(updateEnum);
            } else {
                const createEnum: CreateEnum = this.enumForm.value;
                createEnum.solutionId = this.solutionId;
                createEnum.properties = this.enum.properties;
                createEnum.values = this.enum.values;
                await this.enumService.createEnum(createEnum);
            }
            window.history.back();
        } catch (error) {
            this.onError(error);
        }
    }

    async generate() {
        try {
            await this.enumService.generateEnum({ solutionId: this.solutionId, id: this.enum.id });
            this.noticeHelper.showMessage("Generation completed");
        } catch (error) {
            this.onError(error);
        }
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.enumForm, error);
        }
    }
}
