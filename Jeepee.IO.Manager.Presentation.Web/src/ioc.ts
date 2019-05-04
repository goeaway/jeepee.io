import { Container } from "inversify";
import { IOC } from "./consts";
import { IAPIService } from "./service-types";
import APIService from "./services/api-service"

export const container = new Container();
container.bind<IAPIService>(IOC.ApiService).to(APIService).inSingletonScope();