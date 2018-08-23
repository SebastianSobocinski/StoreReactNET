import * as React from 'react';
import { Route, Switch } from 'react-router-dom';

import { NavBar } from './NavBar';
import { Footer } from './Footer';
import { Login } from './Login';
import { Register } from './Register';
import { Profile } from './Profile';
import { ProfileAddDetails } from './Profile';
import { ProfileAddAddress } from './Profile';
import { Cart } from './Cart';
import { Store } from './Store';
import { ClickedProduct } from './ClickedProduct';
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
            cart: [],
            loading: true
        }

        this.setUser = this.setUser.bind(this);
        this.addToCart = this.addToCart.bind(this);
        this.reCalculate = this.reCalculate.bind(this);
    }
    componentWillMount()
    {
        let currentState = this.state;
        let p1 = AjaxQuery.getUserSession().then((value) =>
        {
            currentState.user = value;
        });
        let p2 = AjaxQuery.getCartSession().then((value) =>
        {
            currentState.cart = value;
        });
        Promise.all([p1, p2]).then(() =>
        {
            currentState.loading = false;
            this.setState(currentState);
        })
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
        if (this.state.loading)
        {
            return null;
        }
        else
        {
            return (
                <div className='container-fluid'>
                    <NavBar data={mainProps} />
                    <div id="mainBody" className="container">
                        <Switch>
                            <Route exact path="/" render={() => <Store data={mainProps} />} />
                            <Route path="/Account/Login" render={(props) => <Login data={mainProps} {...props} />} />
                            <Route path="/Account/Register" render={(props) => <Register data={mainProps} {...props} />} />
                            <Route exact path="/Account/Profile" render={() => <Profile data={mainProps} />} />
                            <Route exact path="/Account/Profile/AddDetails" render={() => <ProfileAddDetails data={mainProps} />} />
                            <Route exact path="/Account/Profile/AddAddress" render={() => <ProfileAddAddress data={mainProps} />} />
                            <Route exact path="/Account/Cart" render={() => <Cart data={mainProps} cart={cartProps} />} />
                            <Route exact path="/Store" render={() => <Store data={mainProps} />} />
                            <Route path="/Store/:categoryID?/:page?" render={(props) => <Store data={mainProps} {...props} />} />
                            <Route path="/Products/:productID" render={(props) => <ClickedProduct data={mainProps} {...props} />} />
                            <Route path="/Search" render={(props) => <Search data={mainProps} {...props} />} />
                        </Switch>
                    </div>
                    <Footer data={mainProps} />
                </div>
                )
        }
        

    }
}
