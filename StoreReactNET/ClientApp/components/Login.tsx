import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Redirect } from 'react-router-dom';
import { Popup } from './Popup';

import $ from 'jquery';
import './Login.css'

export class Login extends React.Component
{
    constructor(props)
    {
        super(props);
        const queryString = require('query-string');
        let parsed = queryString.parse(this.props.location.search);

        this.state =
        {
            user: this.props.data.user || null,  
            failed: Boolean(parsed.failed) || false
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
            currentState.failed = Boolean(parsed.failed) || false
        }
        this.setState(currentState);
    }

    render()
    {
        if (this.state.user != null)
        {
            return <Redirect to={'/'} />
        }
        else
        {
            let popup = null
            if (this.state.failed)
            {
                popup = <Popup title="Wrong email or password." />
            }
            return (

                <div id="loginContainer" className="col-lg-8">

                    <h2>Please sign in</h2>
                    <div id="loginForm" name="loginForm">
                        <form method="POST" action="/Account/Login">
                            <div className="col-lg-12">
                                <input type="email" id="emailLogin" name="Email" className="form-control input-lg" placeholder="Email address" />
                            </div>
                            <div className="col-lg-12">
                                <input type="password" id="passwordLogin" name="Password" className="form-control input-lg" placeholder="Password" />
                            </div>
                            <button type="submit" id="submitLogin" className="btn btn-warning btn-lg">Log in </button>
                        </form>
                    </div>
                    {popup}
                </div>

            )
        }

    }
}