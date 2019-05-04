export interface IAPIService {
    login: () => void;
    logout: () => void;
    sendControl: () => void;
    requestControl: () => void;
}