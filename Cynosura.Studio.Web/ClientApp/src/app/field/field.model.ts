export class Field
{
    id: string;
    name: string;
    displayName: string;
    type: FieldType;
    size: number;
    isRequired: boolean;
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
