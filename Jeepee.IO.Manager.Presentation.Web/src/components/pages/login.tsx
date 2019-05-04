import * as React from "react";

export default class Login extends React.Component {
    render() {
        return (
            <div className="page page--center">
                <div className="card card--accent-top">
                    <div className="card__header">
                        <h3>Login</h3>
                    </div>
                    <div className="card__body">
                        <div className="form-item form-item--accent">
                            <label className="form-item__label">Username/Email</label>
                            <input type="text" className="form-item__input" />
                        </div>
                        <div className="form-item form-item--accent">
                            <label className="form-item__label">Password</label>
                            <input type="password" className="form-item__input" />
                        </div>
                    </div>
                    <div className="card__footer">
                        <button type="submit" className="btn btn--accent">Login</button>
                    </div>
                </div>
            </div>
        )
    }
}