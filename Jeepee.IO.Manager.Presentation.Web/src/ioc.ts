import { Container } from "inversify";
import { IOC } from "./consts";
import { IAuthService, IControllerService } from "./service-types";
import AuthService from "./services/auth-service"
import ControllerService from "./services/controller-service";

export const container = new Container();
container.bind<IAuthService>(IOC.AuthService).to(AuthService).inSingletonScope();
container.bind<IControllerService>(IOC.ControllerService).to(ControllerService).inSingletonScope();