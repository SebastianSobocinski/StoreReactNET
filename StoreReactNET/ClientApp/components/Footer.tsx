import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import "./Footer.css";



export class Footer extends React.Component
{
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
        return (
            <footer>
                <div className="container">
                    <div id="footer-top">
                        <div className="col-md-3 col-sm-4 col-xs-12 footer-box">
                            <p className="footer-header">Links</p>
                            <ol className="col-lg-12">
                                <li><NavLink to={'/login'}>Login</NavLink></li>
                                <li><NavLink to={'/register'}>Register</NavLink></li>
                                <li><NavLink to={'/store'}>Products</NavLink></li>
                                <li><NavLink to={'/'}></NavLink></li>
                            </ol>
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