import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";

import { Solution } from "./solution.model";
import { SolutionService } from "./solution.service";

@Component({
    selector: "solution-select",
    templateUrl: "./select.component.html"
})

export class SolutionSelectComponent implements OnInit {
    constructor(private solutionService: SolutionService) { }

    solutions: Solution[] = [];

    @Input()
    selectedSolutionId: number | null = null;

    @Output()
    selectedSolutionIdChange = new EventEmitter<number>();

    @Input()
    readonly = false;

    ngOnInit(): void {
        this.solutionService.getSolutions().then(solutions => this.solutions = solutions.pageItems);
    }
}
