import { Injectable } from "@angular/core";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { tap } from "rxjs/operators/tap";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request)
            .pipe(tap(
                    (event: HttpEvent<any>) => {

                    }, (err: any) => {
                        if (err instanceof HttpErrorResponse) {
                            throw err.error;
                        }
                    })
            );
    }
}
