import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Entity } from "../entity-core/entity.model";
import { EntityService } from "../entity-core/entity.service";

import { Error } from "../core/error.model";


@Component({
    selector: "app-entity-edit",
    templateUrl: "./entity-edit.component.html"
})
export class EntityEditComponent implements OnInit {
    entity: Entity;
    error: Error;
    solutionId: number;

    constructor(private entityService: EntityService,
                private route: ActivatedRoute,
                private router: Router) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id: string = params.id === "0" ? null : params.id;
            this.solutionId = this.route.snapshot.queryParams.solutionId;
            this.getEntity(id);
        });
    }

    private getEntity(id: string): void {
        if (!id) {
            this.entity = new Entity();
        } else {
            this.entityService.getEntity({ solutionId: this.solutionId, id }).then(entity => {
                this.entity = entity;
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveEntity();
    }

    private saveEntity(): void {
        if (this.entity.id) {
            this.entityService.updateEntity({ ...this.entity, solutionId: this.solutionId })
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.entityService.createEntity({ ...this.entity, solutionId: this.solutionId })
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }

    generate(): void {
        this.entityService.generateEntity({ solutionId: this.solutionId, id: this.entity.id })
            .then(
                () => { },
                error => this.error = error
            );
    }
}
