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
    solutionId: number;

    constructor(private enumService: EnumService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            let id = params["id"];
            this.solutionId = this.route.snapshot.queryParams["solutionId"];
            this.getEnum(id);
        });
    }

    private getEnum(id: string): void {
        if (id === "0") {
            this.enum = new Enum();
        } else {
            this.enumService.getEnum(this.solutionId, id).then(enumModel => {
                this.enum = enumModel;
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
            this.enumService.updateEnum(this.solutionId, this.enum)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.enumService.createEnum(this.solutionId, this.enum)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }

    generate(): void {
        this.enumService.generateEnum(this.solutionId, this.enum.id)
            .then(
                () => { },
                error => this.error = error
            );
    }
}
