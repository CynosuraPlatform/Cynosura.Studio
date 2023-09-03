import { Component, OnInit } from '@angular/core';

import { StoreService } from '../core/store.service';
import { SolutionListState } from '../solution-core/solution.model';

@Component({
  selector: 'app-solutions',
  templateUrl: './solutions.component.html',
  styleUrls: ['./solutions.component.scss']
})
export class SolutionsComponent {

  state: SolutionListState;

  constructor(private storeService: StoreService) {
    this.state = this.storeService.get('solutionListState', new SolutionListState());
  }

}
