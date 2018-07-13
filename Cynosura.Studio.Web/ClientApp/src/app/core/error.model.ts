export class Error {
    constructor(message?: string) {
        this.message = message;
    }
    message?: string;
    modelState: Object;
    exceptionMessage: string;
    exceptionType: string;
    errors: Object[];

    httpStatus: number;
}