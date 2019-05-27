import { Component, Input } from "@angular/core";
import { trigger, state, style, transition, animate } from "@angular/animations";

@Component({
    selector: "app-table-detail",
    templateUrl: "table-detail.component.html",
    styleUrls: ["table-detail.component.scss"],
    animations: [
        trigger("expandCollapse", [
            state("collapsed", style({ height: "0px", minHeight: "0", display: "none" })),
            state("expanded", style({ height: "*" })),
            transition("expanded <=> collapsed", animate("225ms cubic-bezier(0.4, 0.0, 0.2, 1)")),
        ])
    ]
})
export class TableDetailComponent {
    @Input()
    expanded = false;

    constructor() { }
}
