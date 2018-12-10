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
            productID: this.props.data.productID,
            categoryID: this.props.data.productCategoryID,
            categoryName: this.props.data.productCategoryName,
            productName: this.props.data.productName,
            productDescription: this.props.data.productDescription,
            productPrice: this.props.data.productPrice,
            productImages: this.props.data.productImages
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

    renderImage()
    {
        let imageSrc;
        if (this.state.productImages.length > 0)
        {
            try
            {
                imageSrc = require('../images/' + this.state.categoryID + '/' + this.state.productID + '/' + this.state.productImages[0]);
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
            return <div className="col-md-4 col-xs-8 productImage"></div>
        }
        else
        {
            return <img className="col-md-4 col-xs-8 productImage" src={imageSrc} />
        }
        

    }

    render()
    {
        
        let navDirectory = '/Products/' + this.state.productID;
        let prices = this.formatPrice();
        return (
            <div className="productDiv col-md-12">
                <NavLink to={navDirectory} className="productHeader">{this.state.productName}</NavLink>
                {this.renderImage()}
                <div className="col-md-6 col-xs-12 productContent">
                    <p className="productDescription"> {this.state.productDescription}</p>
                </div>
                <div className="col-md-2 col-xs-12 container productSides">
                    <p className="productPriceGross col-xs-12"> {prices.Gross + ' PLN'} </p>
                    <p className="productPriceVAT col-xs-12"> {prices.VAT + ' VAT'} </p>
                    <button onClick={()=> this.props.addToCart(this.state)} className="productAdd btn btn-warning">Add to cart</button>
                </div>
            </div>
            )
    }
}