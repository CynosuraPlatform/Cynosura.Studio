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
    pageSize = 10;
    pageIndex = 0;
    filter = new EnumValueFilter();
}
