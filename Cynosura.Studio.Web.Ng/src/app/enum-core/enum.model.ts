import { OrderDirection } from '../core/models/order-direction.model';
import { PageSettings } from '../core/page.model';
import { EnumFilter } from './enum-filter.model';
import { EnumValue } from '../enum-value-core/enum-value.model';

export class Enum {

  id: string;
  name: string;
  displayName: string;
  values: EnumValue[];
  properties: { [k: string]: any };
  constructor() {
    this.values = new Array<EnumValue>();
    this.properties = {};
  }
}

export class EnumListState {
  pageSize = PageSettings.pageSize;
  pageIndex = 0;
  filter = new EnumFilter();
  orderBy?: string;
  orderDirection?: OrderDirection;
}
