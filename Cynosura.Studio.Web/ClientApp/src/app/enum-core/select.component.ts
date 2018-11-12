import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";

import { Enum } from "./enum.model";
import { EnumService } from "./enum.service";

@Component({
    selector: "enum-select",
    templateUrl: "./select.component.html"
})

export class EnumSelectComponent implements OnInit {
    constructor(private enumService: EnumService) { }

    enums: Enum[] = [];

    @Input()
    solutionId: number;

    @Input()
    selectedEnumId: number | null = null;

    @Output()
    selectedEnumIdChange = new EventEmitter<number>();

    @Input()
    readonly = false;

    ngOnInit(): void {
        this.enumService.getEnums(this.solutionId).then(enums => this.enums = enums.pageItems);
    }
}
