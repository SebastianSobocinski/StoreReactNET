import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import { Login } from './Login';


import { NavBar } from './NavBar';
import { Footer } from './Footer';
import { Home } from './Home';
import { User } from '../classess/User';

import $ from 'jquery';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class App extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            user: null
        }

        this.getUser = this.getUser.bind(this);
        this.setUser = this.setUser.bind(this);

    }
    componentDidMount()
    {
        $.ajax(
            {
                type: "GET",
                url: "api/Session/GetUserSession",
                success: (respond) =>
                {
                    console.log(respond);
                    if (respond.isEstablished)
                    {
                        let user = new User(respond.data)
                        this.setUser(user);
                    }

                }
            });
    }
    getUser()
    {
        return this.state.user;
    }
    setUser(_user)
    {
        this.setState({ user: _user });
    }
    renderChildren()
    {
        return React.Children.map(this.props.children, child =>
        {
            console.log(child);
            return React.cloneElement(child, {name: "test"})
        })
    }
    public render()
    {
        let mainProps =
        {
            user: this.state.user,
            getUser: this.getUser,
            setUser: this.setUser
        }

        return <div className='container-fluid'>
            <NavBar data={mainProps} />
            <div id="mainBody" className="container">
                <Switch>
                    <Route exact path="/" render={() => <Home />} />
                    <Route path='/Account/Login' render={() => <Login data={mainProps}/>} />
                </Switch>
            </div>
            <Footer />
        </div>

    }
}
