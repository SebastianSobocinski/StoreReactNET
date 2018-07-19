import * as React from 'react';
import { Route, Switch } from 'react-router-dom';

import { NavBar } from './NavBar';
import { Footer } from './Footer';
import { Home } from './Home';
import { User } from '../classess/User';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class App extends React.Component<LayoutProps, {}>
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
        let navbarProps =
        {
            user: this.state.user,
            getUser: this.getUser,
            setUser: this.setUser
        }

        return <div className='container-fluid'>
            <NavBar data={navbarProps} />
            <div id="mainBody" className="container">
                <Switch>
                    <Route exact path="/" render={() => <Home /> } />
                </Switch>
            </div>
            <Footer />
        </div>

    }
}
