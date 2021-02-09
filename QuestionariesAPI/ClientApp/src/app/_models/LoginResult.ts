import { User } from "./user"

export class LoginResult {
    user: User;
    isAuthSuccess: Boolean;
    isPasswordSetupNeeded: Boolean;
}