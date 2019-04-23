import { FieldFilter } from "./field-filter.model";
import { OrderDirection } from "../core/models/order-direction.model";

export class UpdateField {
    id: string;
    name: string;
    displayName: string;
    size: number;
    entityId: number;
    isRequired: boolean;
    enumId: number;
    isSystem: boolean;
}

export class CreateField {
    name: string;
    displayName: string;
    size: number;
    entityId: number;
    isRequired: boolean;
    enumId: number;
    isSystem: boolean;
}
