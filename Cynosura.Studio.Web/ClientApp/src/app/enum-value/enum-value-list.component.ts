import { Component, OnInit, Input } from "@angular/core";

import { EnumValue } from "../enum-value-core/enum-value.model";

import { ModalHelper } from "../core/modal.helper";
import { Guid } from "../core/guid";
import { Error } from "../core/error.model";

@Component({
    selector: "app-enum-value-list",
    templateUrl: "./enum-value-list.component.html"
})
export class EnumValueListComponent implements OnInit {
    @Input()
    solutionId: number;

    @Input()
    enumValues: EnumValue[];

    enumValue: EnumValue;
    error: Error;

    constructor(
        private modalHelper: ModalHelper
        ) {}

    ngOnInit(): void {
        
    }

    findEnumValue(id: string): EnumValue {
        return this.enumValues.find(v => v.id === id);
    }

    edit(id: string): void {
        this.enumValue = this.findEnumValue(id);
    }

    add(): void {
        this.enumValue = new EnumValue();
    }

    enumValueSave(enumValue: EnumValue): void {
        if (enumValue.id) {
            const foundEnumValue = this.findEnumValue(enumValue.id);
            if (foundEnumValue) {
                const index = this.enumValues.indexOf(foundEnumValue);
                this.enumValues[index] = enumValue;
            }
        } else {
            enumValue.id = Guid.newGuid();
            this.enumValues.push(enumValue);
        }

        this.enumValue = null;
    }

    delete(id: string): void {
this.modalHelper.confirmDelete()
            .then(() => {
                const foundEnumValue = this.findEnumValue(id);
                const index = this.enumValues.indexOf(foundEnumValue);
                this.enumValues.splice(index, 1);
            })
            .catch(() => { });
    }
}

