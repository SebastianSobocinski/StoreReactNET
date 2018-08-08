import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import { Login } from './Login';
import ResizeObserver from 'resize-observer-polyfill';


import { NavBar } from './NavBar';
import { Footer } from './Footer';
import { Home } from './Home';
import { Store } from './Store';
import { User } from '../classess/User';
import { AjaxQuery } from '../classess/AjaxQuery';

import $ from 'jquery';
import './App.css';



export class App extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            user: null
        }

        this.setUser = this.setUser.bind(this);

    }

    async componentDidMount()
    {
        let _user = await AjaxQuery.getUserSession();
        if (_user != null)
        {
            this.setUser(_user);
        }

        let navBar = document.getElementById('navigationBar');
        new ResizeObserver(() =>
        {
            let navBarHeight = navBar.clientHeight;
            document.getElementById('mainBody').style.marginTop = navBarHeight + 'px';

        }).observe(navBar);

    }

    setUser(_user)
    {
        this.setState({ user: _user });
    }

    addProductToCart(_product)
    {

    }

    public render()
    {
        let mainProps =
        {
            user: this.state.user,
            setUser: this.setUser
        }

        return <div className='container-fluid'>
            <NavBar data={mainProps} />
            <div id="mainBody" className="container">
                <Switch>
                    <Route exact path="/" render={() => <Store />} />
                    <Route exact path='/Account/Login' render={() => <Login data={mainProps} />} />
                    <Route exact path="/Store" render={() => <Store />} />
                    <Route path="/Store/:categoryID?/:page?" render={(props) => <Store data={mainProps} {...props} />} />
                </Switch>
            </div>
            <Footer data={mainProps}/>
        </div>

    }
}
