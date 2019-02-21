
import { EnumValue } from "../enumValue-core/enumValue.model";

export class Enum {
    id: string;
    name: string;
    displayName: string;
    values: EnumValue[];

    constructor() {
        this.values = new Array<EnumValue>();
    }
}
