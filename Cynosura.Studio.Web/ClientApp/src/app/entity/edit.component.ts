import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Entity } from "./entity.model";
import { EntityService } from "./entity.service";

import { Error } from "../core/error.model";


@Component({
    selector: "entity-edit",
    templateUrl: "./edit.component.html"
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
            let id = params["id"];
            this.solutionId = this.route.snapshot.queryParams["solutionId"];
            this.getEntity(id);
        });
    }

    private getEntity(id: string): void {
        if (id === "0") {
            this.entity = new Entity();
        } else {
            this.entityService.getEntity(this.solutionId, id).then(entity => {
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
            this.entityService.updateEntity(this.solutionId, this.entity)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.entityService.createEntity(this.solutionId, this.entity)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }

    generate(): void {
        this.entityService.generateEntity(this.solutionId, this.entity.id)
            .then(
                () => { },
                error => this.error = error
            );
    }
}
