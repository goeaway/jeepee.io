import "reflect-metadata";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { BrowserRouter as Router, Route, Link, Redirect } from "react-router-dom";
import Home from "./components/pages/home";
import About from "./components/pages/about";
import Login from "./components/pages/login";
import Watch from "./components/pages/watch";
import Play from "./components/pages/play";
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
        const authed = false;

        return (
            <Router>
                <div className="app">
                    <nav className="nav">
                        <ul className="nav__list">
                            <li className="nav__list__item">
                                <Link to="/">Home</Link>
                            </li>
                            <li className="nav__list__item">
                                <Link to="/watch">Watch</Link>
                            </li>
                            <li className="nav__list__item">
                                <Link to="/play">Play</Link>
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
                        <Route path="/watch" component={Watch} />
                        <Route path="/play" render={() => (authed ? <Play /> : <Redirect to="/login" />)} />
                    </div>
                </div>
            </Router>
        );
    }
}

ReactDOM.render(<App />, document.getElementById("react-root"))