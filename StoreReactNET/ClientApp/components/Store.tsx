import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link, NavLink } from 'react-router-dom';

import { User } from '../classess/User';


export class Store extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            categoryID: null,
            productID: null
        }
        if (this.props.match != null)
        {
            if (this.props.match.params.productID != null)
            {
                this.state = 
                {
                    categoryID: this.props.match.params.categoryID,
                    productID: this.props.match.params.productID
                };
            }
            else
            {
                this.state =
                {
                    categoryID: this.props.match.params.categoryID,
                    productID: null
                };
            }
        }
    }

    componentWillReceiveProps(nextProps)
    {
        if (nextProps.match != null)
        {
            if (nextProps.match.params.productID != null)
            {
                this.setState(
                {
                    categoryID: nextProps.match.params.categoryID,
                    productID: nextProps.match.params.productID
                });
            }
            else
            {
                this.setState(
                {
                    categoryID: nextProps.match.params.categoryID,
                    productID: null
                });
            }

        }
        
    }

    render()
    {
        let View;
        if (this.state.categoryID != null)
        {
            View = (<h1>{this.state.categoryID}{this.state.productID}</h1>);
        }
        else
        {
            View = (<NavLink to={'/Store/2/10'}> Store page </NavLink>);
        }
        return View;

    }
}