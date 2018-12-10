import * as React from 'react';

import { Redirect, NavLink } from 'react-router-dom';
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
    calculateTotalValue()
    {
        let price = 0;
        this.state.cart.forEach((el) =>
        {
            price += el.productPrice * el.quantity;
        });
        price = price * 1.23;
        return price.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$& ') + " PLN";
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
                    <div id="cartBottom" className="col-xs-12 container">
                        <div id="cartTotalValue" className="col-xs-12 col-sm-3 col-sm-offset-9">Total: {this.calculateTotalValue()}</div>
                        <div id="cartButtons" className="col-xs-12 col-sm-3 col-sm-offset-9 container">

                            <button id="cartApply" onClick={() => this.props.cart.reCalculate()} className="btn btn-warning col-xs-5 col-xs-offset-1">Apply</button>
                            <NavLink to={"/Order"}>
                                <button id="cartSubmitOrder" className="btn btn-primary col-xs-5 col-xs-offset-1">Order</button>
                            </NavLink>
                        </div>


                    </div>
                </div>
            )
        }
        else
        {
            View = <Redirect to='/' />
        }
        return View;
    }
}