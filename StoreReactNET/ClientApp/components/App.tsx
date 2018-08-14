import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import ResizeObserver from 'resize-observer-polyfill';


import { NavBar } from './NavBar';
import { Footer } from './Footer';
import { Login } from './Login';
import { Cart } from './Cart';
import { Store } from './Store';
import { Search } from './Search';
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
            user: null,
            cart: []
        }

        this.setUser = this.setUser.bind(this);
        this.addToCart = this.addToCart.bind(this);
        this.reCalculate = this.reCalculate.bind(this);
    }

    async componentDidMount()
    {
        let currentState = this.state;
        currentState.user = await AjaxQuery.getUserSession();
        currentState.cart = await AjaxQuery.getCartSession();
        this.setState(currentState);

        let navBar = document.getElementById('navigationBar');
        new ResizeObserver(() =>
        {
            let navBarHeight = navBar.clientHeight;
            document.getElementById('mainBody').style.marginTop = navBarHeight + 'px';

        }).observe(navBar);

    }

    async setUser(_user)
    {
        let currentState = this.state;
        currentState.user = _user;
        this.setState(currentState);
    }
    async addToCart(_product)
    {
        let currentState = this.state;
        currentState.cart = await AjaxQuery.addToCartSession(_product.productID);
        this.setState(currentState);
    }
    async reCalculate()
    {
        let currentState = this.state;
        currentState.cart = await AjaxQuery.updateCartSession(this.state.cart);
        this.setState(currentState);
    }
    render()
    {
        let mainProps =
        {
            user: this.state.user,
            cart: this.state.cart,
            setUser: this.setUser,
            addToCart: this.addToCart
        }
        let cartProps =
        {
            reCalculate: this.reCalculate
        }

        return <div className='container-fluid'>
            <NavBar data={mainProps} />
            <div id="mainBody" className="container">
                <Switch>
                    <Route exact path="/" render={() => <Store data={mainProps} />} />
                    <Route exact path="/Account/Login" render={() => <Login data={mainProps} />} />
                    <Route exact path="/Account/Cart" render={() => <Cart data={mainProps} cart={cartProps}/>} />
                    <Route exact path="/Store" render={() => <Store data={mainProps}/>} />
                    <Route path="/Store/:categoryID?/:page?" render={(props) => <Store data={mainProps} {...props} />} />
                    <Route path="/Search" render={(props) => <Search data={mainProps} {...props}/>} />
                </Switch>
            </div>
            <Footer data={mainProps}/>
        </div>

    }
}
