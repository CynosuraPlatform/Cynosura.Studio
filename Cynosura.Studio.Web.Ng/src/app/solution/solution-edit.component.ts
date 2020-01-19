import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { Solution } from "../solution-core/solution.model";
import { SolutionService } from "../solution-core/solution.service";

import { Error } from "../core/error.model";
import { TemplateModel, TemplateService } from "../solution-core/template-service";
import { NoticeHelper } from "../core/notice.helper";
import { ConvertStringTo } from "../core/converter.helper";


@Component({
    selector: "app-solution-edit",
    templateUrl: "./solution-edit.component.html",
    styleUrls: ["./solution-edit.component.scss"]
})
export class SolutionEditComponent implements OnInit {
    id: number;
    solutionForm = this.fb.group({
        id: [],
        name: [],
        path: [],
        templateName: [],
        templateVersion: []
    });
    solution: Solution;
    templates: TemplateModel;
    error: Error;

    constructor(
        private solutionService: SolutionService,
        private templateService: TemplateService,
        private route: ActivatedRoute,
        private router: Router,
        private fb: FormBuilder,
        private noticeHelper: NoticeHelper) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id: number = params.id === "0" ? null : ConvertStringTo.number(params.id);
            this.getSolution(id);
        });
        this.templateService.getTemplates()
            .then((templates) => this.templates = templates);
    }

    private async getSolution(id: number) {
        this.id = id;
        if (!id) {
            this.solution = new Solution();
        } else {
            this.solution = await this.solutionService.getSolution({ id });
        }
        this.solutionForm.patchValue(this.solution);
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveSolution();
    }

    private async saveSolution() {
        try {
            if (this.id) {
                await this.solutionService.updateSolution(this.solutionForm.value);
            } else {
                await this.solutionService.createSolution(this.solutionForm.value);
            }
            window.history.back();
        } catch (error) {
            this.onError(error);
        }
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.solutionForm, error);
        }
    }
    async generate() {
        try {
            this.error = null;
            await this.solutionService.generateSolution({ id: this.solution.id });
            this.noticeHelper.showMessage("Generation completed");
        } catch (error) {
            this.onError(error);
        }
    }

    async upgrade() {
        try {
            this.error = null;
            await this.solutionService.upgradeSolution({ id: this.solution.id });
            this.noticeHelper.showMessage("Upgrade completed");
        } catch (error) {
            this.onError(error);
        }
    }
}
