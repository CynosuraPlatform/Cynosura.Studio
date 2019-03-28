import { Component, Input, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
    selector: "app-text-edit",
    templateUrl: "./text.edit.component.html",
    styleUrls: ["text.edit.component.css"],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TextEditComponent),
            multi: true
        }
    ]
})
export class TextEditComponent implements ControlValueAccessor {

    onChange: any = () => { };
    onTouched: any = () => { };

    @Input("value")
    val: string;

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    type = "text";

    @Input()
    multiline = false;

    @Input()
    readonly = false;

    get value() {
        return this.val;
    }

    set value(val) {
        this.val = val;
        this.onChange(val);
        this.onTouched();
    }

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.value = value;
    }
}
