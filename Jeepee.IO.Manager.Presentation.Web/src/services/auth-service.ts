import { IAuthService } from "../service-types";
import { injectable } from "inversify";

@injectable()
export default class AuthService implements IAuthService {
    // maintain state in here by keeping track of user's Authorisation header 
    // each request made must take that header


    login = () => {
        // request jwt token by sending request to auth server with jwt of implicit type with user's user and password?
    }

    logout = () => {
        // request auth server revokes our token
    }

    requestControl = () => {

    }
}