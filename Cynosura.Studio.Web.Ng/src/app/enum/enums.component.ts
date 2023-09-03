import { Component, OnInit } from '@angular/core';

import { StoreService } from '../core/store.service';
import { EnumListState } from '../enum-core/enum.model';

@Component({
  selector: 'app-enums',
  templateUrl: './enums.component.html',
  styleUrls: ['./enums.component.scss']
})
export class EnumsComponent {

  state: EnumListState;

  constructor(private storeService: StoreService) {
    this.state = this.storeService.get('enumListState', new EnumListState());
  }

}
