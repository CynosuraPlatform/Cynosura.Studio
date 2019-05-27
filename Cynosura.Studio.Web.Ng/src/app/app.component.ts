import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { Observable } from "rxjs";
import { debounceTime, map, tap, first } from "rxjs/operators";
import { MediaObserver } from "@angular/flex-layout";

import { AuthService } from "./auth/auth.service";
import { LoadingService } from "./core/loading.service";
import { MatSidenav } from "@angular/material";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
    title = "app";
    isLoading = false;

    // FlexLayout
    isHandset$: Observable<boolean> = this.media.asObservable().pipe(
        map(
          () =>
            this.media.isActive("xs") ||
            this.media.isActive("sm") ||
            this.media.isActive("lt-md")
        ),
        tap(() => this.changeDetectorRef.detectChanges()));

    constructor(private authService: AuthService,
                private loadingService: LoadingService,
                private cdRef: ChangeDetectorRef,
                private media: MediaObserver,
                private changeDetectorRef: ChangeDetectorRef) {
        loadingService
            .onLoadingChanged
            .pipe(debounceTime(500))
            .subscribe((isLoading: boolean) => {
                this.isLoading = isLoading;
                this.cdRef.detectChanges();
            });
    }

    ngOnInit(): void {
        this.authService.init()
            .subscribe(
                () => { console.info("Auth init success"); },
                error => console.warn(error)
            );
        this.emitEventResize();
    }

    toggle(sidenav: MatSidenav): void {
        this.emitEventResize();
        this.isHandset$.pipe(
            first()
        ).subscribe(isHandset => {
            if (isHandset) {
                sidenav.toggle();
            }
        });
    }

    emitEventResize() {
        // fix for mat-sidenav-content not resizing
        window.dispatchEvent(new Event("resize"));
    }
}
