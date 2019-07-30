import { FormGroup } from "@angular/forms";

export class Error {
    constructor(message?: string) {
        this.message = message;
    }
    message?: string;
    modelState: { [key: string]: ModelStateItem };
    exceptionMessage: string;
    exceptionType: string;
    errors: any[];

    httpStatus: number;

    static setFormErrors(group: FormGroup, error: Error) {
        if (error.modelState) {
            for (const key in error.modelState) {
                const errors = error.modelState[key].errors.reduce((o, e) => {
                    o[e.errorMessage] = true;
                    return o;
                }, {});
                if (group.controls[key]) {
                    group.controls[key].setErrors(errors);
                }
            }
        }
    }
}

export class ModelStateItem {
    errors: ModelStateError[];
}

export class ModelStateError {
    errorMessage: string;
    exceptionMessage: string;
    exceptionSource: string;
}
