import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Enum } from "../enum-core/enum.model";
import { EnumService } from "../enum-core/enum.service";

import { Error } from "../core/error.model";


@Component({
    selector: "app-enum-edit",
    templateUrl: "./enum-edit.component.html"
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
            const id: string = params.id === "0" ? null : params.id;
            this.solutionId = this.route.snapshot.queryParams.solutionId;
            this.getEnum(id);
        });
    }

    private getEnum(id: string): void {
        if (!id) {
            this.enum = new Enum();
        } else {
            this.enumService.getEnum({ solutionId: this.solutionId, id }).then(enumModel => {
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
            this.enumService.updateEnum({ ...this.enum,  solutionId: this.solutionId })
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.enumService.createEnum({ ...this.enum,  solutionId: this.solutionId })
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }

    generate(): void {
        this.enumService.generateEnum({ solutionId: this.solutionId, id: this.enum.id })
            .then(
                () => { },
                error => this.error = error
            );
    }
}
