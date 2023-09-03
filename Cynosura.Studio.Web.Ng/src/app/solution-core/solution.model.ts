import { OrderDirection } from '../core/models/order-direction.model';
import { PageSettings } from '../core/page.model';
import { SolutionFilter } from './solution-filter.model';

export class Solution {
  id: number;
  creationDate: Date;
  modificationDate: Date;
  creationUserId: number;
  modificationUserId: number;
  name: string;
  path: string;
  templateName: string;
  templateVersion: string;
}

export class SolutionListState {
  pageSize = PageSettings.pageSize;
  pageIndex = 0;
  filter = new SolutionFilter();
  orderBy?: string;
  orderDirection?: OrderDirection;
}
