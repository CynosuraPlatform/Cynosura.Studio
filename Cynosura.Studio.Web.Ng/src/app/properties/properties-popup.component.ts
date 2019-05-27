import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

import { PropertiesService } from "./properties.service";

export interface DialogData {
    properties: { [k: string]: any };
    target: string;
}

@Component({
    templateUrl: "properties-popup.component.html",
    styleUrls: ["properties-popup.component.scss"]
})
export class PropertiesPopupComponent implements OnInit {

    target: string;

    set properties(properties) {
        this.innerProperties = properties;
        this.boolProperties = {};
        Object.keys(properties)
            .filter((f) => typeof properties[f] === "boolean")
            .reduce((prev, cur) => {
                prev[cur] = properties[cur];
                return prev;
            }, this.boolProperties);
    }
    get properties() {
        return this.innerProperties;
    }

    defaults: { [k: string]: any };
    innerProperties: { [k: string]: any };

    boolProperties: { [k: string]: any };

    newProperty = "";

    constructor(
        public dialogRef: MatDialogRef<PropertiesPopupComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private propertiesService: PropertiesService) {
        this.properties = data.properties;
        this.target = data.target;
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    ngOnInit() {
        this.propertiesService.getProperties()
            .then((data) => {
                this.defaults = data;
                Object.keys(data)
                    .filter((f) => typeof data[f] === "boolean")
                    .reduce((boolProperties, cur) => {
                        if (boolProperties[cur] === undefined || boolProperties[cur] === null) {
                            boolProperties[cur] = data[cur];
                        }
                        return boolProperties;
                    }, this.boolProperties);
            });
    }

    addBool() {
        if (this.newProperty && this.newProperty.length > 0 && this.properties[this.newProperty] === undefined) {
            this.boolProperties[this.newProperty] = false;
            this.properties[this.newProperty] = false;
            this.newProperty = "";
        }
    }

    handleChange(key, value) {
        if (this.defaults[key] === value) {
            delete this.properties[key];
        } else {
            this.properties[key] = value;
        }
    }
}
