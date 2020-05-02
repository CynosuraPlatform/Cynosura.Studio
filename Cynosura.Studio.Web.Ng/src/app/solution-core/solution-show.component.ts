import { Component, Input } from '@angular/core';

import { Solution } from './solution.model';

@Component({
    selector: 'app-solution-show',
    templateUrl: './solution-show.component.html'
})

export class SolutionShowComponent {
    @Input()
    value: Solution;
}
