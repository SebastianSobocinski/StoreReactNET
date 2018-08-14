import * as React from 'react';

import './CartItem.css';

export class CartItem extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            productID: this.props.data.productID,
            productCategoryID: this.props.data.productCategoryID,
            productName: this.props.data.productName,
            productPrice: this.props.data.productPrice,
            productImages: this.props.data.productImages,
            quantity: this.props.data.quantity
        }
    }
    formatPrice()
    {
        let price = 0;
        price = this.state.productPrice * this.state.quantity;
        price = price * 1.23;
        return price.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$& ') + " PLN";
    }
    setQuantity(event)
    {
        let value = event.target.value;
        let currentState = this.state;
        currentState.quantity = value;
        this.props.setQuantity(this.props.data, value);
        this.setState(currentState);
    }
    renderImage()
    {
        let imageSrc;
        if (this.state.productImages.length > 0)
        {
            try
            {
                imageSrc = require('../images/' + this.state.productCategoryID + '/' + this.state.productID + '/' + this.state.productImages[0]);
            }
            catch (ex)
            {
                try
                {
                    imageSrc = require('../images/null.png')

                }
                catch (ex) { imageSrc = "null" };
            }
        }
        else
        {
            try
            {
                imageSrc = require('../images/null.png')
            }
            catch (ex) { imageSrc = "null" };
        }
        if (typeof (imageSrc) == 'object')
        {
            imageSrc = "null";
        }
        if (imageSrc == "null")
        {
            return <div className="col-xs-8 col-xs-offset-2 col-sm-4 col-sm-offset-0 col-md-2 cartItemImage"></div>
        }
        else
        {
            return <img className="col-xs-8 col-xs-offset-2 col-sm-4 col-sm-offset-0 col-md-2 cartItemImage" src={imageSrc} />
        }
    }
    render()
    {
        return (
            <div className="cartItem col-xs-12 container">
                {this.renderImage()}
                <div className="cartItemAside col-xs-12 col-sm-8 col-md-10 container">
                    <p className="cartItemTitle col-xs-6 col-xs-offset-3 col-sm-4 col-sm-offset-0">
                        {this.state.productName}
                    </p>
                    <div className="cartItemQuantity col-xs-6 col-xs-offset-3 col-sm-3 col-sm-offset-0">
                        <input type="number" onChange={this.setQuantity.bind(this)} min="0" max="10" placeholder={this.state.quantity} className="form-control cartItemQuantityInput" />
                    </div>
                    <div className="cartItemPrice col-xs-6 col-xs-offset-3 col-sm-5 col-sm-offset-0">
                        {this.formatPrice()}
                        </div>
                </div>
            </div>
            )
    }
}