import * as React from 'react';
import { RouteComponentProps } from 'react-router';

import $ from 'jquery';
import './Login.css'

export class Login extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            user: this.props.data.user
        }

    }
    componentDidMount()
    {
        let button = document.getElementById('submitLogin');
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

                    }
                    
                    
                }
            });
        }
    }
    render()
    {
        return (

            <div id="loginContainer" className="col-lg-8">

                <h2>Please sign in</h2>
                <div id="loginForm" name="loginForm" method="POST">

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