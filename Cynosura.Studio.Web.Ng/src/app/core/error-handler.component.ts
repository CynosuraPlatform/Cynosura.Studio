import { Component, Input, isDevMode, OnInit } from "@angular/core";

@Component({
    selector: "app-error-handler",
    templateUrl: "./error-handler.component.html"
})

export class ErrorHandlerComponent implements OnInit {
    isDev = false;

    @Input()
    error: object | null = null;

    ngOnInit(): void {
        this.isDev = isDevMode();
    }
}
