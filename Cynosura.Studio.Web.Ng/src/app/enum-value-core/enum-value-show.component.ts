import { Component, Input } from '@angular/core';

import { EnumValue } from './enum-value.model';

@Component({
    selector: 'app-enum-value-show',
    templateUrl: './enum-value-show.component.html'
})

export class EnumValueShowComponent {
    @Input()
    value: EnumValue;
}
