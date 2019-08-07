import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { MatSnackBar } from "@angular/material";

import { Entity } from "../entity-core/entity.model";
import { EntityService } from "../entity-core/entity.service";
import { UpdateEntity, CreateEntity } from "../entity-core/entity-request.model";

import { Error } from "../core/error.model";

@Component({
    selector: "app-entity-edit",
    templateUrl: "./entity-edit.component.html",
    styleUrls: ["./entity-edit.component.scss"]
})
export class EntityEditComponent implements OnInit {
    id: string;
    regExp: RegExp = /^[A-Z][a-zA-Z0-9]{2,}$/;
    entityForm = this.fb.group({
        id: [],
        name: ["", [Validators.required, Validators.pattern(this.regExp)]],
        pluralName: ["", [Validators.required, Validators.pattern(this.regExp)]],
        displayName: [],
        pluralDisplayName: [],
        isAbstract: [],
        baseEntityId: []
    });
    entity: Entity;
    error: Error;
    solutionId: number;

    constructor(private entityService: EntityService,
                private route: ActivatedRoute,
                private router: Router,
                private fb: FormBuilder,
                private snackBar: MatSnackBar) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id: string = params.id === "0" ? null : params.id;
            this.solutionId = this.route.snapshot.queryParams.solutionId;
            this.getEntity(id);
        });
    }

    private getEntity(id: string): void {
        this.id = id;
        if (!id) {
            this.entity = new Entity();
            this.entityForm.patchValue(this.entity);
        } else {
            this.entityService.getEntity({ solutionId: this.solutionId, id }).then(entity => {
                this.entity = entity;
                this.entityForm.patchValue(this.entity);
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
        if (this.id) {
            const updateEntity: UpdateEntity = this.entityForm.value;
            updateEntity.solutionId = this.solutionId;
            updateEntity.properties = this.entity.properties;
            updateEntity.fields = this.entity.fields;
            this.entityService.updateEntity(updateEntity)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        } else {
            const createEntity: CreateEntity = this.entityForm.value;
            createEntity.solutionId = this.solutionId;
            createEntity.properties = this.entity.properties;
            createEntity.fields = this.entity.fields;
            this.entityService.createEntity(createEntity)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        }
    }

    generate(): void {
        this.entityService.generateEntity({ solutionId: this.solutionId, id: this.id })
            .then(
                () => { },
                error => this.onError(error)
            );
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.snackBar.open(error.message, "Ok");
            Error.setFormErrors(this.entityForm, error);
        }
    }
}
