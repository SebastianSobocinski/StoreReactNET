import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import "./Footer.css";



export class Footer extends React.Component
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
        this.setState({ user: nextProps.data.user });
    }
    componentDidMount()
    {
        let footer = document.getElementsByTagName('footer')[0];
        let mainBody = document.getElementById('mainBody');
        mainBody.style.paddingBottom = footer.clientHeight+"px";

        window.addEventListener('resize', () =>
        {
            let footerHeight = (footer.clientHeight);
            mainBody.style.paddingBottom = footerHeight + "px";

        });
    }  
    
    render()
    {
        let spanStyle =
            {
                color: "white"
            }
        let menuOptions;
        if (this.state.user != null)
        {
            menuOptions =
                (
                <ol className="col-lg-12">
                    <li><NavLink to={'/'}>Home</NavLink></li>
                    <li><NavLink to={'/Account/Profile'}>My Profile</NavLink></li>
                    <li><NavLink to={'/Account/Cart'}>Cart</NavLink></li>
                    <li><NavLink to={'/Store'}>Products</NavLink></li> 
                </ol>
                )
        }
        else
        {
            menuOptions =
                (
                <ol className="col-lg-12">
                    <li><NavLink to={'/'}>Home</NavLink></li>
                    <li><NavLink to={'/Account/Login'}>Login</NavLink></li>
                    <li><NavLink to={'/Account/Register'}>Register</NavLink></li>
                    <li><NavLink to={'/Store'}>Products</NavLink></li>
                </ol>
                );
        }
        return (
            <footer>
                <div className="container">
                    <div id="footer-top">
                        <div className="col-md-3 col-sm-4 col-xs-12 footer-box">
                            <p className="footer-header">Navigate</p>
                            {menuOptions}
                        </div>
                        <div className="col-md-3 col-sm-4 col-xs-12 footer-box">
                            <p className="footer-header">Contact Us</p>
                            <ol className="col-lg-12">
                                <li><p>sebastian.sobocinski.7@gmail.com</p></li>
                            </ol>
                        </div>
                        <div className="col-md-3 col-sm-4 cols-xs-12 footer-box">
                            <p className="footer-header">Social Media</p>
                            <ol className="col-lg-12">
                                <li><a href="">Facebook</a></li>
                                <li><a href="">Twitter</a></li>
                                <li><a href="">Instagram</a></li>
                                <li><a href="">YouTube</a></li>
                            </ol>
                        </div>
                        <div className="col-md-3 col-sm-12 footer-box">
                            <p className="footer-header">Newsletter</p>
                            <div className="form-group">
                                <input type="text" className="form-control" /><input type="submit" value="Submit" className="btn btn-warning" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="footer-bottom">
                    <div className="container">
                        <p>Copyright - &copy; 2018 - <span style={spanStyle}>Sebastian Sobociński </span>- All rights reserved.</p>
                    </div>
                </div>
            </footer>
        )

    }
}