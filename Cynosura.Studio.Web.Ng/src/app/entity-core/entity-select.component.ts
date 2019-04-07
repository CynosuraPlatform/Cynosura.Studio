import { Component, Input, OnInit, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

import { Entity } from "./entity.model";
import { EntityService } from "./entity.service";

@Component({
    selector: "app-entity-select",
    templateUrl: "./entity-select.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EntitySelectComponent),
            multi: true
        }
    ]
})

export class EntitySelectComponent implements OnInit, ControlValueAccessor {
    constructor(private entityService: EntityService) { }

    entities: Entity[] = [];

    @Input()
    solutionId: number;

    @Input()
    value: number | null = null;

    get innerValue() {
        return this.value;
    }

    set innerValue(val) {
        this.value = val;
        this.onChange(val);
        this.onTouched();
    }

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    readonly = false;

    onChange: any = () => { };
    onTouched: any = () => { };

    ngOnInit(): void {
        this.entityService.getEntities(this.solutionId).then(entities => this.entities = entities.pageItems);
    }

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.innerValue = value;
    }
}
