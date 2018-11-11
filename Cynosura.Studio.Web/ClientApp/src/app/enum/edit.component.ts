import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Enum } from "../enum-core/enum.model";
import { EnumService } from "../enum-core/enum.service";

import { Error } from "../core/error.model";


@Component({
    selector: "enum-edit",
    templateUrl: "./edit.component.html"
})
export class EnumEditComponent implements OnInit {
    enum: Enum;
    error: Error;

    constructor(private enumService: EnumService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params["id"];
            this.getEnum(id);
        });
    }

    private getEnum(id: number): void {
        if (id === 0) {
            this.enum = new Enum();
        } else {
            this.enumService.getEnum(id).then(enum => {
                this.enum = enum;
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
        if (this.enum.id) {
            this.enumService.updateEnum(this.enum)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.enumService.createEnum(this.enum)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }

}
