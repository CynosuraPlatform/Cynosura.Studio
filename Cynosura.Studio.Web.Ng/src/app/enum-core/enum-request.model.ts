import { EnumFilter } from './enum-filter.model';
import { OrderDirection } from '../core/models/order-direction.model';
import { EnumValue } from '../enum-value-core/enum-value.model';

export class GetEnums {
    solutionId: number;
    pageIndex?: number;
    pageSize?: number;
    filter?: EnumFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class GetEnum {
    solutionId: number;
    id: string;
}

export class ExportEnums {
    filter?: EnumFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class UpdateEnum {
    solutionId: number;
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

export class CreateEnum {
    solutionId: number;
    name: string;
    displayName: string;
    values: EnumValue[];
    properties: { [k: string]: any };
    constructor() {
        this.values = new Array<EnumValue>();
        this.properties = {};
    }
}

export class DeleteEnum {
    solutionId: number;
    id: string;
}

export class GenerateEnum {
    solutionId: number;
    id: string;
}
