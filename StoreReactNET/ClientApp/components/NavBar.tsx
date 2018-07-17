import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import "./Navbar.css"
import { User } from '../classess/User';

export class NavBar extends React.Component
{
    constructor()
    {
        super();
        this.state =
        {
            user: null
        }
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
                                    <NavLink to={'/login'} activeClassName="active" >
                                        Log in
                                    </NavLink>
                                </li>
                                    <li>
                                    <NavLink to={'/register'} activeClassName="active">
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
            /*userNavbar = 
            (
                <
            )
            */
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