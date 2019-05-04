export interface IAuthService {
    login: () => void;
    logout: () => void;
    requestControl: () => void;
}

export interface IControllerService {
    sendCommand: () => void;
}