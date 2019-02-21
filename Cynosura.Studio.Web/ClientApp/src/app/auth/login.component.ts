import { Component } from "@angular/core";
import { OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "./auth.service";

@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["login.component.css"]
})
export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    error: any;
    returnUrl: string;

    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private router: Router,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";
        this.loginForm = this.formBuilder.group({
            username: ["", [Validators.required]],
            password: ["", [Validators.required]],
        });
    }

    onSubmit() {
        this.authService.login(this.loginForm.value)
            .subscribe(() => {
                this.router.navigateByUrl(this.returnUrl);
            },
            error => this.error = error);
    }

}
