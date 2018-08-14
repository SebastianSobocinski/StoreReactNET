import * as React from 'react';

import { CartItem } from './CartItem';

import './Cart.css';

export class Cart extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            cart: this.props.data.cart
        }
        this.setQuantity = this.setQuantity.bind(this);
    }
    componentWillReceiveProps(nextProps)
    {
        let currentState = this.state;
        currentState.cart = nextProps.data.cart;
        this.setState(currentState);
    }

    renderCartItems()
    {
        return this.state.cart.map((obj) =>
        {
            return <CartItem key={obj.productID} data={obj} setQuantity={this.setQuantity} />

        })
    }

    setQuantity(el, quantity)
    {
        let cart = this.state.cart;
        let pos = cart.indexOf(el);
        cart[pos].quantity = quantity;
    }
    render()
    {
        let View = null
        if (this.state.cart.length > 0)
        {
            View = (
                <div id="cartContainer" className="col-xs-12 container">
                    <div id="cartItemsList" className="col-xs-12 container">
                        {this.renderCartItems()}
                    </div>
                    <div className="cartBottom col-xs-12 container">
                        <button id="cartApply" onClick={() => this.props.cart.reCalculate()} className="btn btn-warning">Apply</button>
                    </div>
                </div>
            )
        }
        return View;
    }
}