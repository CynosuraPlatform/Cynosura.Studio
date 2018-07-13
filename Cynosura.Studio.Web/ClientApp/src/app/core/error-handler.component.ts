import { Component, Input, isDevMode, OnInit } from "@angular/core";

@Component({
    selector: "error-handler",
    templateUrl: "./error-handler.component.html"
})

export class ErrorHandlerComponent implements OnInit{
    isDev: boolean = false;

    @Input()
    error: Object | null = null;

    ngOnInit(): void {
        this.isDev = isDevMode();
    }
}
