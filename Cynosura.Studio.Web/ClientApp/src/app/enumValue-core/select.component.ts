import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";

import { EnumValue } from "./enumValue.model";
import { EnumValueService } from "./enumValue.service";

@Component({
    selector: "enumValue-select",
    templateUrl: "./select.component.html"
})

export class EnumValueSelectComponent implements OnInit {
    constructor(private enumValueService: EnumValueService) { }

    enumValues: EnumValue[] = [];

    @Input()
    selectedEnumValueId: number | null = null;

    @Output()
    selectedEnumValueIdChange = new EventEmitter<number>();

    @Input()
    readonly = false;

    ngOnInit(): void {
        this.enumValueService.getEnumValues().then(enumValues => this.enumValues = enumValues.pageItems);
    }
}