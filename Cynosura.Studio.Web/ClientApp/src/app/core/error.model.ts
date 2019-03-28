export class Error {
    constructor(message?: string) {
        this.message = message;
    }
    message?: string;
    modelState: object;
    exceptionMessage: string;
    exceptionType: string;
    errors: object[];

    httpStatus: number;
}
