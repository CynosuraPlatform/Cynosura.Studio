import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { EnumValue } from "../enumValue-core/enumValue.model";
import { EnumValueService } from "../enumValue-core/enumValue.service";

import { Error } from "../core/error.model";


@Component({
    selector: "enumValue-edit",
    templateUrl: "./edit.component.html"
})
export class EnumValueEditComponent implements OnInit {
    enumValue: EnumValue;
    error: Error;

    constructor(private enumValueService: EnumValueService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params["id"];
            this.getEnumValue(id);
        });
    }

    private getEnumValue(id: number): void {
        if (id === 0) {
            this.enumValue = new EnumValue();
        } else {
            this.enumValueService.getEnumValue(id).then(enumValue => {
                this.enumValue = enumValue;
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveEnumValue();
    }

    private saveEnumValue(): void {
        if (this.enumValue.id) {
            this.enumValueService.updateEnumValue(this.enumValue)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.enumValueService.createEnumValue(this.enumValue)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }

}
