import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { FormBuilder, FormGroup } from "@angular/forms";

import { Error } from "../core/error.model";
import { ProfileService } from "./profile.service";
import { Profile } from "./profile.model";
import { UpdateProfile } from "./profile-request.model";
import { NoticeHelper } from "../core/notice.helper";

@Component({
    selector: "app-profile-edit",
    templateUrl: "./edit.component.html",
    styleUrls: ["./edit.component.scss"]
})
export class ProfileEditComponent implements OnInit {
    profile: Profile;
    profileForm = this.fb.group({
        id: [],
        email: [],
        currentPassword: [],
        newPassword: [],
        confirmPassword: []
    });
    error: Error;

    constructor(
        private profileService: ProfileService,
        private fb: FormBuilder,
        private noticeHelper: NoticeHelper) {
    }

    ngOnInit(): void {
        this.getProfile();
    }

    private getProfile(): void {
        this.profileService.getProfile({})
            .then((profile) => {
                this.profile = profile;
                this.profileForm.reset();
                this.profileForm.patchValue(this.profile);
            });
    }

    onSubmit(): void {
        this.error = null;
        this.saveProfile();
    }

    private saveProfile(): void {
        const profile: UpdateProfile = this.profileForm.value;
        this.profileService.updateProfile(profile)
            .then(
                () => {
                   // this.authService.refreshTokens()
                        // .subscribe((token) => {
                            this.noticeHelper.showMessage("Profile saved!");
                            this.getProfile();
                        // });
                },
                (error) => {
                    this.onError(error);
                }
            );
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.profileForm, error);
        }
    }

}
