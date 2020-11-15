import { Component, OnInit } from '@angular/core';

import { StoreService } from '../core/store.service';
import { EntityListState } from '../entity-core/entity.model';

@Component({
    selector: 'app-entities',
    templateUrl: './entities.component.html',
    styleUrls: ['./entities.component.scss']
})
export class EntitiesComponent {

    state: EntityListState;

    constructor(private storeService: StoreService) {
        this.state = this.storeService.get('entityListState', new EntityListState());
    }

}
