import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import "./Navbar.css"
import { User } from '../classess/User';

export class NavBar extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            user: this.props.data.user
        }
    }
    componentWillReceiveProps(nextProps)
    {
        this.setState({ user: nextProps.data.user })
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
                            <NavLink to={'/store'} activeClassName="active">
                                Products
                            </NavLink>
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
                                Welcome {this.state.user.firstName}
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to={'/store'} activeClassName="active">
                                Products
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to={'/Cart/Show'} activeClassName="active">
                                <span id="shoppingCartIcon" className="navIcon glyphicon glyphicon-shopping-cart"><span id="shoppingCartCount">0</span></span><span id="cartValue">0,00 PLN</span>
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to={'/Account/Logout'} activeClassName="active">
                                <span className="navIcon glyphicon glyphicon-log-out"></span>
                            </NavLink>
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