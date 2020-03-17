import { Component, Input, OnInit, forwardRef, OnDestroy, ElementRef, Optional, Self, DoCheck, OnChanges,
    SimpleChanges } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NgControl } from '@angular/forms';
import { MatFormFieldControl } from '@angular/material/form-field';
import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { Subject } from 'rxjs';

import { TemplateModel } from './template.model';
import { TemplateService } from './template-service';

@Component({
    selector: 'app-template-version-select',
    templateUrl: './template-version-select.component.html',
    providers: [
        { provide: MatFormFieldControl, useExisting: TemplateVersionSelectComponent }
    ]
})

export class TemplateVersionSelectComponent implements OnInit, ControlValueAccessor,
    MatFormFieldControl<number | null>, OnDestroy, DoCheck, OnChanges {

    static nextId = 0;

    stateChanges = new Subject<void>();
    focused = false;
    controlType = 'app-template-version-select';
    id = `template-version-select-${TemplateVersionSelectComponent.nextId++}`;
    describedBy = '';

    errorState = false;

    get empty() {
        return !this.value;
    }

    get shouldLabelFloat() { return this.focused || !this.empty; }

    templateVersions: string[] = [];

    @Input()
    templateName: string;

    @Input()
    value: number | null = null;

    @Input()
    name: string;

    @Input()
    placeholder: string;

    @Input()
    get required(): boolean { return this.innerRequired; }
    set required(value: boolean) {
        this.innerRequired = coerceBooleanProperty(value);
        this.stateChanges.next();
    }
    private innerRequired = false;

    @Input()
    get disabled(): boolean { return this.innerDisabled; }
    set disabled(value: boolean) {
        this.innerDisabled = coerceBooleanProperty(value);
        this.stateChanges.next();
    }
    private innerDisabled = false;

    get innerValue() {
        return this.value;
    }

    set innerValue(val) {
        this.value = val;
        this.onChange(val);
        this.onTouched();
    }

    onChange: any = () => { };
    onTouched: any = () => { };

    constructor(private templateService: TemplateService,
                private fm: FocusMonitor, private elRef: ElementRef<HTMLElement>,
                @Optional() @Self() public ngControl: NgControl) {
        fm.monitor(elRef, true).subscribe(origin => {
            this.focused = !!origin;
            this.stateChanges.next();
        });
        if (this.ngControl !== null) {
            this.ngControl.valueAccessor = this;
        }
    }

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.innerValue = value;
    }

    private load() {
        if (this.templateName) {
            this.templateService.getTemplateVersions({ templateName: this.templateName }).subscribe((templateVersions) => {
                this.templateVersions = templateVersions;
            });
        } else {
            this.templateVersions = [];
        }
    }

    ngOnInit(): void {
        this.load();
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.templateName.currentValue !== changes.templateName.previousValue && !changes.templateName.isFirstChange()) {
            this.load();
        }
    }

    ngOnDestroy() {
        this.stateChanges.complete();
        this.fm.stopMonitoring(this.elRef);
    }

    setDescribedByIds(ids: string[]) {
        this.describedBy = ids.join(' ');
    }

    onContainerClick(event: MouseEvent) {
    }

    ngDoCheck(): void {
        if (this.ngControl) {
            this.errorState = this.ngControl.invalid;
            this.stateChanges.next();
        }
    }
}
