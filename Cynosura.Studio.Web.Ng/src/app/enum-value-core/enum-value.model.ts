import { OrderDirection } from '../core/models/order-direction.model';
import { PageSettings } from '../core/page.model';
import { EnumValueFilter } from './enum-value-filter.model';

export class EnumValue {
  id: string;
  name: string;
  displayName: string;
  value: number;
  properties: { [k: string]: any };

  constructor() {
    this.properties = {};
  }
}

export class EnumValueListState {
  pageSize = PageSettings.pageSize;
  pageIndex = 0;
  filter = new EnumValueFilter();
  orderBy?: string;
  orderDirection?: OrderDirection;
}
