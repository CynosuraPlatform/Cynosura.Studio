export class Error {
    constructor(message?: string) {
        this.message = message;
    }
    message?: string;
    modelState: { [key: string]: ModelStateError };
    exceptionMessage: string;
    exceptionType: string;
    errors: any[];

    httpStatus: number;
}

export class ModelStateError {
    errorMessage: string;
    exceptionMessage: string;
    exceptionSource: string;
}
