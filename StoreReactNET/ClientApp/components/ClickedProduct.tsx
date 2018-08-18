import * as React from 'react';
import { Link, NavLink, Redirect } from 'react-router-dom';

import { AjaxQuery } from '../classess/AjaxQuery';
import { Product } from './Product';


import './ClickedProduct.css';

export class ClickedProduct extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            product: null
        }
        this.getProduct();
    }
    async getProduct()
    {
        let currentState = this.state;
        currentState.product = await AjaxQuery.getClickedProduct(this.props.match.params.productID)
        this.setState(currentState);
    }
    renderDetails()
    {
        return this.state.product.productDetailsList.map((obj) =>
        {
            return (
                <div key={obj[0]} className="details-item col-xs-12 container">
                    <div className="details-item-header col-xs-12">
                        {obj[0]}
                    </div>
                    <div className="details-item-content col-xs-12">
                        {obj[1]}
                    </div>
                </div>
                )
        });
    }
    render()
    {
        if (this.state.product == null)
        {
            return null;
        }
        else
        {
            let details = null;
            if (this.state.product.productDetailsList.length > 0)
            {
                details = (
                    <div id="clickedProductBottom" className="col-xs-12 container">
                        {this.renderDetails()}
                    </div>
                );
            }
            return (
                <div id="clickedProductContainer" className="col-xs-12 container">
                    <div id="clickedProductBar" className="col-xs-12">
                        <NavLink to={'/Store'}>Store</NavLink>
                        <span> > </span>
                        <NavLink to={'/Store/' + this.state.product.productCategoryID}>{this.state.product.productCategoryName}</NavLink>
                        <span> > </span>
                        <NavLink to={'/Products/' + this.state.product.productID}>{this.state.product.productName}</NavLink>
                    </div>
                    <div id="clickedProductTop" className="col-xs-12 container">
                        <Product data={this.state.product} addToCart={this.props.data.addToCart}/>
                    </div>
                    {details}
                </div>

            )
        }
        
    }
}