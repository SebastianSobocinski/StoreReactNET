import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Redirect } from 'react-router-dom';

import $ from 'jquery';
import './Login.css'

export class Login extends React.Component
{
    constructor(props)
    {
        super(props);

        if (this.props.data.user != null)
        {
            this.state =
            {
                user: this.props.data.user,
                redirect: true
            }
        }
        else
        {
            this.state =
            {
                user: this.props.data.user,
                redirect: false
            }
        }
    }

    componentWillReceiveProps(nextProps)
    {
        if (nextProps.data.user != null)
        {
            this.setState({ user: nextProps.data.user, redirect: true });

        } 
    }

    componentDidMount()
    {
        let button = document.getElementById('submitLogin');
        if (button != null)
        {
            button.addEventListener('click', () =>
            {
                let userEmail = document.getElementById('emailLogin').value;
                let userPassword = document.getElementById('passwordLogin').value;
                $.ajax(
                    {
                        type: "POST",
                        url: "/Account/Login",
                        data:
                        {
                            Email: userEmail,
                            Password: userPassword
                        },
                        success: (respond) =>
                        {
                            if (respond.success)
                            {
                                window.location.href = "/";
                            }
                            else
                            {
                                this.renderFailedLogin();
                            }

                        }
                    });
            });
        }
        
    }
    renderFailedLogin()
    {

        let alertBackground = document.createElement('div');
        alertBackground.id = "loginAlertBackground";

        let alertDiv = document.createElement('div');
        alertDiv.id = "loginAlertDiv";

        let message = document.createElement('p');
        message.id = "loginAlertMessage";
        message.innerHTML = "Wrong email or password! Please try again.";

        let alertButton = document.createElement('button');
        alertButton.id = "loginAlertButton";
        alertButton.className = "btn btn-danger btn-lg";
        alertButton.innerHTML = "OK";
        alertButton.addEventListener('click', () =>
        {
            alertBackground.remove();
            alertDiv.remove();
        });

        alertDiv.appendChild(message);
        alertDiv.appendChild(alertButton);

        document.getElementById('react-app').appendChild(alertBackground);
        document.getElementById('react-app').appendChild(alertDiv);
    }
    render()
    {
        if (this.state.redirect)
        {
            return <Redirect to={'/'} />
        }
        return (

            <div id="loginContainer" className="col-lg-8">

                <h2>Please sign in</h2>
                <div id="loginForm" name="loginForm">

                    <div className="col-lg-12">
                        <input type="email" id="emailLogin" name="Email" className="form-control input-lg" placeholder="Email address" />
                    </div>
                    <div className="col-lg-12">
                        <input type="password" id="passwordLogin" name="Password" className="form-control input-lg" placeholder="Password" />
                    </div>
                    <button type="submit" id="submitLogin" className="btn btn-warning btn-lg">Log in </button>
                </div>
            </div>

        )
    }
}