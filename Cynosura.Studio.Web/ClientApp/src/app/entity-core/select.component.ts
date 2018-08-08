import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";

import { Entity } from "./entity.model";
import { EntityService } from "./entity.service";

@Component({
    selector: "entity-select",
    templateUrl: "./select.component.html"
})

export class EntitySelectComponent implements OnInit {
    constructor(private entityService: EntityService) { }

    entities: Entity[] = [];

    @Input()
    solutionId: number;

    @Input()
    selectedEntityId: string | null = null;

    @Output()
    selectedEntityIdChange = new EventEmitter<string>();

    @Input()
    readonly = false;

    ngOnInit(): void {
        this.entityService.getEntities(this.solutionId).then(entities => this.entities = entities.pageItems);
    }
}
