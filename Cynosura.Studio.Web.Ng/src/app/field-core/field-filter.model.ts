import { EntityFilter } from "../core/models/entity-filter.model";

export class FieldFilter extends EntityFilter {
    name?: string;
    displayName?: string;
    sizeFrom?: number;
    sizeTo?: number;
    entityId?: number;
    isRequired?: boolean;
    enumId?: number;
    isSystem?: boolean;
}
