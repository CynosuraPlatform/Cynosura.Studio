export class Field {
    id: string;
    name: string;
    displayName: string;
    type: FieldType;
    entityId: string;
    size: number;
    isRequired: boolean;
    enumId: string;
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
    Guid
}
