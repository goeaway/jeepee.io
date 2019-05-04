import * as React from "react";
import * as ReactDOM from "react-dom";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import Home from "./components/pages/home";
import About from "./components/pages/about";
import Login from "./components/pages/login";
import "./styles/app.less";
import { container } from "./ioc";
import { IOC } from "./consts";
import getContainer from "inversify-inject-decorators";
import { IAuthService } from "./service-types";
import { IControllerService } from "./service-types";

let {lazyInject} = getContainer(container);

export default class App extends React.Component {

    @lazyInject(IOC.AuthService)
    private _authService: IAuthService;
    @lazyInject(IOC.ControllerService)
    private _controllerService: IControllerService;

    render() {
        return (
            <Router>
                <div className="app">
                    <nav className="nav">
                        <ul className="nav__list">
                            <li className="nav__list__item">
                                <Link to="/">Home</Link>
                            </li>
                            <li className="nav__list__item">
                                <Link to="/about">About</Link>
                            </li>
                            <li className="nav__list__item">
                                <Link to="/login">Login</Link>
                            </li>
                        </ul>
                    </nav>

                    <div className="content">
                        <Route path="/" exact component={Home} />
                        <Route path="/about" component={About} />
                        <Route path="/login" component={Login} />
                    </div>
                </div>
            </Router>
        );
    }
}

ReactDOM.render(<App />, document.getElementById("react-root"))