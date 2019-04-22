import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Solution } from "../solution-core/solution.model";
import { SolutionService } from "../solution-core/solution.service";

import { Error } from "../core/error.model";


@Component({
    selector: "app-solution-edit",
    templateUrl: "./solution-edit.component.html"
})
export class SolutionEditComponent implements OnInit {
    solution: Solution;
    error: Error;

    constructor(private solutionService: SolutionService,
                private route: ActivatedRoute,
                private router: Router) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id: number = params.id === "0" ? null : params.id;
            this.getSolution(id);
        });
    }

    private getSolution(id: number): void {
        if (!id) {
            this.solution = new Solution();
        } else {
            this.solutionService.getSolution({ id }).then(solution => {
                this.solution = solution;
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveSolution();
    }

    private saveSolution(): void {
        if (this.solution.id) {
            this.solutionService.updateSolution(this.solution)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        } else {
            this.solutionService.createSolution(this.solution)
                .then(
                    () => window.history.back(),
                    error => this.error = error
                );
        }
    }

    generate(): void {
        this.error = null;
        this.solutionService.generateSolution({ id: this.solution.id })
            .then(
                () => {},
                error => this.error = error
            );
    }

    upgrade(): void {
        this.error = null;
        this.solutionService.upgradeSolution({ id: this.solution.id })
            .then(
                () => { },
                error => this.error = error
            );
    }
}
