import { Component, Input } from '@angular/core';

import { Enum } from './enum.model';

@Component({
    selector: 'app-enum-show',
    templateUrl: './enum-show.component.html'
})

export class EnumShowComponent {
    @Input()
    value: Enum;
}
