import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import "./Navbar.css"
import { User } from '../classess/User';

import $ from 'jquery';

export class NavBar extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            user: this.props.data.user,
            categories: []
        }
        this.init();
    }
    async init()
    {
        let currentState = this.state;
        currentState.categories = await this.getCategories();
        this.setState(currentState);
    }
    componentWillReceiveProps(nextProps)
    {
        this.setState({ user: nextProps.data.user })
    }

    componentDidUpdate()
    {
        let logout = document.getElementById('accountLogout');
        if (logout != null)
        {
            logout.addEventListener('click', () =>
            {
                $.ajax(
                    {
                        type: "POST",
                        url: "Session/ClearSession",
                        success: (respond) =>
                        {
                            if (respond.success)
                            {
                                window.location.href = "/";
                            }
                        }
                    });
            });
        }
    }

    async getCategories()
    {
        let result = [];
        await new Promise((resolve) =>
        {
            $.ajax(
                {
                    type: "GET",
                    url: "Product/GetAllCategories",
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            
                            result = JSON.parse(respond.categories);
                            
                        }
                        resolve();
                    }
                });

        });
        return result;
    }

    renderCategories()
    {
        return this.state.categories.map((obj) =>
        {
            return <li key={obj.Id}><NavLink to={'/Store/' +  obj.Id }>{obj.CategoryName}</NavLink></li>
        });
    }
    render()
    {
        let userNavbar;
        if (this.state.user == null || this.state.user.userID == 0)
        {
            userNavbar = 
            (
                <div className="navbar-collapse collapse">
                    <ul id="navOptions" className="nav navbar-nav">
                        <li>
                            <a data-toggle="dropdown">Account</a>
                            <ul className="dropdown-menu" id="accountDropdown">
                                <li>
                                    <NavLink to={'/Account/Login'} activeClassName="active" >
                                        Log in
                                    </NavLink>
                                </li>
                                <li>
                                    <NavLink to={'/Account/Register'} activeClassName="active">
                                        Register
                                    </NavLink>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <a data-toggle="dropdown"> Products </a>
                            <ul className="dropdown-menu" id="productsDropdown">
                                {this.renderCategories()}
                            </ul>
                        </li>
                    </ul>
                </div>
            )
        }
        else 
        {
            userNavbar = 
            (
                <div className="navbar-collapse collapse">
                    <ul id="navOptions" className="nav navbar-nav">
                        <li>
                            <NavLink to={'/Account/Profile'}>
                                Welcome, {this.state.user.firstName}
                            </NavLink>
                        </li>
                        <li>
                            <a data-toggle="dropdown"> Products </a>
                            <ul className="dropdown-menu" id="productsDropdown">
                                {this.renderCategories()}
                            </ul>
                        </li>
                        <li>
                            <NavLink to={'/Account/Cart'} activeClassName="active">
                                <span id="shoppingCartIcon" className="navIcon glyphicon glyphicon-shopping-cart"><span id="shoppingCartCount">0</span></span><span id="cartValue">0,00 PLN</span>
                            </NavLink>
                        </li>
                        <li>
                            <a id="accountLogout">
                                <span className="navIcon glyphicon glyphicon-log-out"></span>
                            </a>
                        </li>
                    </ul>
                </div>
            )
            
        }
        return (
        <div id="navigationBar" className="navbar navbar-inverse navbar-fixed-top">
            <div id="navContainer" className="container">
                <div id="navHeader" className="navbar-header">
                    <NavLink to={'/'} exact className="navbar-brand">
                            StoreReactNET
                    </NavLink>

                    <button type="button" className="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                    </button>

                </div>
                {userNavbar}
            </div>
        </div>
        )


    }
}