import { PageSettings } from '../core/page.model';
import { FieldFilter } from './field-filter.model';

export class Field {
  id: string;
  name: string;
  displayName: string;
  type: FieldType;
  entityId: string;
  size: number;
  isSystem = false;

  isRequired = false;
  enumId: string;

  properties: { [k: string]: any };

  constructor() {
    this.properties = {};
  }
}

export enum FieldType {
  String,
  Int32,
  Int64,
  Decimal,
  Double,
  Boolean,
  DateTime,
  Date,
  Time,
  Guid,
  Blob
}

export class FieldListState {
  pageSize = PageSettings.pageSize;
  pageIndex = 0;
  filter = new FieldFilter();
}
