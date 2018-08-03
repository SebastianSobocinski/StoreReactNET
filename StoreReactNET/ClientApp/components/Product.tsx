import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';

import $ from 'jquery';
import './Product.css';

export class Product extends React.Component
{
    constructor(props)
    {
        super(props);

        this.state =
        {
            productID: this.props.data.ProductID,
            categoryID: this.props.data.ProductCategoryID,
            categoryName: this.props.data.ProductCategoryName,
            productName: this.props.data.ProductName,
            productDescription: this.props.data.ProductDescription,
            productPrice: this.props.data.ProductPrice,
            productImages: this.props.data.ProductImages
        }
    }
    formatPrice()
    {
        let startPriceVAT = this.state.productPrice;
        let startPriceGross = this.state.productPrice * 1.23;
        return {
            VAT: startPriceVAT.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$& '),
            Gross: startPriceGross.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$& ');
        };

    }
    render()
    {
        let imageSrc;
        if (this.state.productImages.length > 0)
        {
            imageSrc = require('../images/' + this.state.categoryID + '/' + this.state.productID + '/' + this.state.productImages[0]);
        }
        else
        {
            imageSrc = require('../images/null.png');
        }
        let navDirectory = '/Products/' + this.state.productID;
        let prices = this.formatPrice();
        return (
            <div className="productDiv col-md-12">
                <NavLink to={navDirectory} className="productHeader">{this.state.productName}</NavLink>
                <img className="col-md-4 col-xs-8 productImage" src={imageSrc} />
                <div className="col-md-6 col-xs-12 productContent">
                    <p className="productDescription"> {this.state.productDescription}</p>
                </div>
                <div className="col-md-2 col-xs-12 productSides">
                    <p className="productPriceGross"> {prices.Gross + ' PLN'} </p>
                    <p className="productPriceVAT"> {prices.VAT + ' VAT'} </p>
                    <button onClick={()=> this.test()} className="productAdd btn btn-warning">Add to cart</button>
                </div>
            </div>
            )
    }
}