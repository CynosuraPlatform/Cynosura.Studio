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

    onChange: any = () => { };
    onTouched: any = () => { };

    @Input("value")
    val: string;

    private valueLocal: Date;

    get value(): Date {
        if (this.val && !this.valueLocal) {
            this.valueLocal = new Date(`2000-01-01T${this.val}`);
        }
        return this.valueLocal;
    }
    set value(val: Date) {
        this.valueLocal = val;
        if (val) {
            this.val = val.toTimeString().substring(0, 5);
        } else {
            this.val = null;
        }
        this.onChange(this.val);
        this.onTouched();
    }

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    readonly = false;

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.val = value;
        this.valueLocal = null;
        this.value = this.value;
    }
}
