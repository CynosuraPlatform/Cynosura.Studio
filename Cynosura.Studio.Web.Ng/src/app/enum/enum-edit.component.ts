import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { MatSnackBar } from "@angular/material";

import { Enum } from "../enum-core/enum.model";
import { EnumService } from "../enum-core/enum.service";
import { UpdateEnum, CreateEnum } from "../enum-core/enum-request.model";

import { Error } from "../core/error.model";

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
                private snackBar: MatSnackBar) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id: string = params.id === "0" ? null : params.id;
            this.solutionId = this.route.snapshot.queryParams.solutionId;
            this.getEnum(id);
        });
    }

    private getEnum(id: string): void {
        this.id = id;
        if (!id) {
            this.enum = new Enum();
            this.enumForm.patchValue(this.enum);
        } else {
            this.enumService.getEnum({ solutionId: this.solutionId, id }).then(enumModel => {
                this.enum = enumModel;
                this.enumForm.patchValue(this.enum);
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveEnum();
    }

    private saveEnum(): void {
        if (this.id) {
            const updateEnum: UpdateEnum = this.enumForm.value;
            updateEnum.solutionId = this.solutionId;
            updateEnum.properties = this.enum.properties;
            updateEnum.values = this.enum.values;
            this.enumService.updateEnum(updateEnum)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        } else {
            const createEnum: CreateEnum = this.enumForm.value;
            createEnum.solutionId = this.solutionId;
            createEnum.properties = this.enum.properties;
            createEnum.values = this.enum.values;
            this.enumService.createEnum(createEnum)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        }
    }

    generate(): void {
        this.enumService.generateEnum({ solutionId: this.solutionId, id: this.enum.id })
            .then(
                () => { },
                error => this.onError(error)
            );
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.snackBar.open(error.message, "Ok");
            Error.setFormErrors(this.enumForm, error);
        }
    }
}
