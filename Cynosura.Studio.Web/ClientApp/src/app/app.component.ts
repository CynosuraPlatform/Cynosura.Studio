import { Component, OnInit } from "@angular/core";

import { AuthService } from "./core/services/auth.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit {
    title = "app";

    constructor(private authService: AuthService) { }

    ngOnInit(): void {
        this.authService.init()
            .subscribe(
                () => { console.info("Auth init success"); },
                error => console.warn(error)
            );
    }
}
