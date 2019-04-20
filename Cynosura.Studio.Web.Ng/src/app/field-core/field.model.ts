
export class Field {
    id: string;
    name: string;
    displayName: string;
    type: FieldType;
    entityId: string;
    size: number;
    isSystem: boolean;

    isRequired: boolean;
    enumId: string;

    properties: { [k: string]: any};

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
