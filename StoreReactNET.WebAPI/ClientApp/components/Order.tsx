import * as React from 'react';
import { Redirect } from 'react-router-dom';

import ReactLoading from 'react-loading';
import { AjaxQuery } from '../classess/AjaxQuery';
import './Order.css';
import { CartItem } from './CartItem';

export class Order extends React.Component
{
    constructor(props)
    {
        super(props);

        this.state =
            {
                user: this.props.data.user,
                cart: null,
                userDetails: null,
                userAddresses: null,
                loading: true
            }
    }
    componentWillMount()
    {
        let currentState = this.state;

        let p1 = AjaxQuery.getCartSession().then((value) =>
        {
            currentState.cart = value;
        });
        let p2 = AjaxQuery.getUserDetails().then((value) =>
        {
            currentState.userDetails = value;
        })
        let p3 = AjaxQuery.getUserAddresses().then((value) =>
        {
            currentState.userAddresses = value;
        })

        Promise.all([p1, p2, p3]).then(() =>
        {
            currentState.loading = false;
            this.setState(currentState);
        })
    }
    calculateTotalValue()
    {
        let price = 0;
        this.state.cart.forEach((el) =>
        {
            price += el.productPrice * el.quantity;
        })
        price = price * 1.23;
        return price.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$& ') + " PLN";
    }
    validateOrder(event)
    {
        let form = event.target;
        if (form["Address"].value == "null")
        {
            event.preventDefault();
        }

    }

    renderAddressesList()
    {
        return this.state.userAddresses.map((obj) =>
        {
            if (obj.appartmentNr == null)
            {
                return <option key={obj.id} value={obj.id}>{obj.streetName + " " + obj.homeNr + ", " + obj.city + ", " + obj.country}</option>
            }
            else
            {
                return <option key={obj.id} value={obj.id}>{obj.streetName + " " + obj.homeNr + "/" + obj.appartmentNr + ", " + obj.city + ", " + obj.country}</option>
            }

        })
    }
    renderProducts()
    {
        return this.state.cart.map((obj) =>
        {
            return (
                <tr>
                    <td className="orderTableName"> {obj.productName} </td>
                    <td> {obj.quantity} </td>
                    <td> {(obj.productPrice * obj.quantity * 1.23).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$& ') + " PLN" } </td>
                </tr>
                )
        })

    }
    render()
    {
        if (this.state.loading)
        {
            return <ReactLoading type="bubbles" color="#333" />
        }
        if (this.state.userDetails == null || this.state.userAddresses == null)
        {
            return <Redirect to={"/Account/Profile"} />
        }
        if (this.state.user != null && this.state.cart.length > 0)
        {
            return (
                <div id="orderContainer">
                    <table id="orderTable" className="table">
                        <thead>
                            <tr>
                                <th id="orderNameHeader" scope="col">Product</th>
                                <th className="orderHeaderMiddle" scope="col">Quantity</th>
                                <th className="orderHeaderMiddle" scope="col">Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.renderProducts()}
                        </tbody>
                    </table>
                    <div id="orderTotalPrice" className="col-xs-12 col-sm-3 col-sm-offset-9">
                        <p>Total: {this.calculateTotalValue()}</p>
                    </div>
                    <div id="orderAddress" className="col-xs-12 container">
                        <h4 className="col-xs-12">Select Address </h4>
                        <form name="orderForm" method="POST" action="/Account/SubmitOrder" onSubmit={this.validateOrder}> 
                            <div className="col-xs-12 col-sm-4">
                                <select defaultValue="null" className="form-control" name="AddressID">

                                    <option value="null" disabled>Choose address</option>
                                    {this.renderAddressesList()}

                                </select>
                            </div>
                            <div style={{marginTop:"20px"}}className="col-xs-12 col-sm-3 col-sm-offset-9">
                                <button type="submit" className="btn btn-primary">Confirm </button>
                            </div>
                        </form>
                    </div>
                </div>
                )
        }

        return <Redirect to={"/"} />
    }
}