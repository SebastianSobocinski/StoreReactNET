import * as React from 'react';
import { Redirect } from 'react-router';
import { Popup } from './Popup';

import './Register.css';

const errosEnum = Object.freeze(
    {
        0: "User already exists.",
        1: "Passwords must be the same.",
        2: "Invalid e-mail address."
    })

export class Register extends React.Component 
{
    constructor(props)
    {
        super(props);

        const queryString = require('query-string');
        let parsed = queryString.parse(this.props.location.search);
        if (isNaN(parsed.failCode))
        {
            parsed.failCode = null;
        }
        this.state = 
        {
            user: this.props.data.user || null,
            failCode: parsed.failCode
        }
    }

    componentWillReceiveProps(nextProps)
    {
        let currentState = this.state;
        if (nextProps.data.user != null)
        {
            currentState.user = nextProps.data.user;
        }
        else
        {
            const queryString = require('query-string');
            let parsed = queryString.parse(nextProps.location.search);
            if (isNaN(parsed.failCode))
            {
                parsed.failCode = null;
            }
            currentState.failCode = parsed.failCode;
        }
        this.setState(currentState);
    }
    componentDidMount()
    {
        let inputs = document.getElementsByClassName("registerInput");
        for (let i = 0; i < inputs.length; i++)
        {
            inputs[i].addEventListener("click", () =>
            {
                for (let j = 0; j < inputs.length; j++)
                {
                    inputs[j].classList.remove("error");
                }
            })
        }
        
    }
    validateRegister(event)
    {
        let form = event.target;

        let emailValue = form["Email"].value;
        let passwordValue = form["Password"].value;
        let rePasswordValue = form["RePassword"].value;

        let syntax = /\S+@\S+/;

        if (!syntax.test(emailValue))
        {
            form["Email"].classList.add("error");
            event.preventDefault();
        }
        if (passwordValue.length < 6 || passwordValue.length > 25)
        {
            form["Password"].classList.add("error");
            event.preventDefault();
        }
        if (rePasswordValue.length < 6 || rePasswordValue.length > 25)
        {
            form["RePassword"].classList.add("error");
            event.preventDefault();
        }
        if (passwordValue != rePasswordValue)
        {
            form["RePassword"].classList.add("error");
            event.preventDefault();
        }

    }

    render()
    {
        if (this.state.user != null)
        {
            return <Redirect to="/" />
        }
        else
        {
            let popup;
            if (this.state.failCode != null)
            {
                popup = <Popup type="alertError" title={errosEnum[this.state.failCode]} />
            }
            return (
                <div id="registerContainer" className="col-lg-8">
                    <h2>Please register</h2>
                    <div id="registerForm" name="registerForm">
                        <form method="POST" action="/Account/Register" onSubmit={this.validateRegister}>
                            <div className="col-lg-12 registerInputDiv">
                                <input type="text" id="emailRegister" name="Email" className="form-control input-lg registerInput" placeholder="Email address" />
                            </div>
                            <div className="col-lg-12 registerInputDiv">
                                <input type="password" id="passwordRegister" name="Password" className="form-control input-lg registerInput" placeholder="Password" />
                            </div>
                            <div className="col-lg-12 registerInputDiv">
                                <input type="password" id="rePasswordRegister" name="RePassword" className="form-control input-lg registerInput" placeholder="Re-type password" />
                            </div>
                            <button type="submit" id="submitRegister" className="btn btn-warning btn-lg">Register</button>
                        </form>
                    </div>
                    {popup}
                </div>
                )
        }
    }
}