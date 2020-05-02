import { EnumValueFilter } from './enum-value-filter.model';
import { OrderDirection } from '../core/models/order-direction.model';

export class UpdateEnumValue {
    id: string;
    name: string;
    displayName: string;
    value: number;
    enumId: number;
}

export class CreateEnumValue {
    name: string;
    displayName: string;
    value: number;
    enumId: number;
}
