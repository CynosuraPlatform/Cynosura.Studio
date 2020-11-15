import { Component, OnInit } from '@angular/core';

import { StoreService } from '../core/store.service';
import { ViewListState } from '../view-core/view.model';

@Component({
    selector: 'app-views',
    templateUrl: './views.component.html',
    styleUrls: ['./views.component.scss']
})
export class ViewsComponent {

    state: ViewListState;

    constructor(private storeService: StoreService) {
        this.state = this.storeService.get('viewListState', new ViewListState());
    }

}
