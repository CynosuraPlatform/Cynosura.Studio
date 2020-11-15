import { Component, Inject } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Error } from '../core/error.model';

import { Solution } from '../solution-core/solution.model';
import { SolutionService } from '../solution-core/solution.service';

@Component({
    templateUrl: 'solution-open.component.html',
    styleUrls: ['solution-open.component.scss']
})
export class SolutionOpenComponent {
    path: string;
    error: Error;

    constructor(
        private solutionService: SolutionService,
        public dialogRef: MatDialogRef<SolutionOpenComponent>,
        private router: Router,
        private route: ActivatedRoute,
        private snackBar: MatSnackBar,
        @Inject(MAT_DIALOG_DATA) public data: {}) { }

    onNoClick(): void {
        this.dialogRef.close();
    }

    edit(id: number): void {
        this.router.navigate(['solution', id], { relativeTo: this.route });
    }

    open() {
        const solution = {
            path: this.path
        } as Solution;
        this.solutionService.openSolution(solution)
            .subscribe((result) => {
                this.dialogRef.close();
                this.edit(result.id);
            }, error => this.onError(error));
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.snackBar.open(error.message, 'Ok');
        }
    }
}
