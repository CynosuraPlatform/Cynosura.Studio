import { Component, Input } from '@angular/core';

import { View } from './view.model';

@Component({
    selector: 'app-view-show',
    templateUrl: './view-show.component.html'
})

export class ViewShowComponent {
    @Input()
    value: View;
}
