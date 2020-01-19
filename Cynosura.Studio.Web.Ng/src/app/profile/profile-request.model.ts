export class GetProfile {
}

export class UpdateProfile {
    email: string;
    phoneNumber?: string;
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
}
