import { Component, Input } from '@angular/core';

import { Entity } from './entity.model';

@Component({
    selector: 'app-entity-show',
    templateUrl: './entity-show.component.html'
})

export class EntityShowComponent {
    @Input()
    value: Entity;
}
