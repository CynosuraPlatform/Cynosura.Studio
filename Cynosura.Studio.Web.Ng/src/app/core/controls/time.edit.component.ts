import { Component, Input, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
    selector: "app-time-edit",
    templateUrl: "./time.edit.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TimeEditComponent),
            multi: true
        }
    ]
})
export class TimeEditComponent implements ControlValueAccessor {

    @Input()
    value: string;

    private valueLocal: Date;

    get innerValue(): Date {
        if (this.value && !this.valueLocal) {
            this.valueLocal = new Date(`2000-01-01T${this.value}`);
        }
        return this.valueLocal;
    }
    set innerValue(val: Date) {
        this.valueLocal = val;
        if (val) {
            this.value = val.toTimeString().substring(0, 5);
        } else {
            this.value = null;
        }
        this.onChange(this.value);
        this.onTouched();
    }

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    readonly = false;

    onChange: any = () => { };
    onTouched: any = () => { };

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.value = value;
        this.valueLocal = null;
        this.innerValue = this.innerValue;
    }
}
