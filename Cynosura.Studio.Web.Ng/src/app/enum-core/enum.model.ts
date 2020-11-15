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
    pageSize = 10;
    pageIndex = 0;
    filter = new EnumFilter();
}
