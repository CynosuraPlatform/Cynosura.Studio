import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { Router } from "@angular/router";
import { MatSidenav } from "@angular/material/sidenav";
import { MediaObserver } from "@angular/flex-layout";
import { Observable } from "rxjs";
import { debounceTime, map, tap, first } from "rxjs/operators";

import { LoadingService } from "./core/loading.service";

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

    constructor(private loadingService: LoadingService,
                private cdRef: ChangeDetectorRef,
                private media: MediaObserver,
                private changeDetectorRef: ChangeDetectorRef,
                private router: Router) {
        loadingService
            .onLoadingChanged
            .pipe(debounceTime(500))
            .subscribe((isLoading: boolean) => {
                this.isLoading = isLoading;
                this.cdRef.detectChanges();
            });
    }

    ngOnInit(): void {
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
