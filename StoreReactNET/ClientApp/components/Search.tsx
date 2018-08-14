import * as React from 'react';
import { SearchBar } from './SearchBar';
import { Product } from './Product'; 

import './Search.css';

import { AjaxQuery } from "../classess/AJAXQuery";

export class Search extends React.Component
{
    constructor(props)
    {
        super(props)

        const queryString = require('query-string');
        var parsed = queryString.parse(this.props.location.search);
        if (parsed.query == "")
        {
            parsed.query = null;
        }

        this.state =
        {
            query: parsed.query,
            page: 1,
            productsList: [],
            orderBy: "relevance"
        }
        this.loadResults();
    }
    loadResults()
    {
        let currentState = this.state;

        let queryString = currentState.query;
        let queryArray = [];
        if (queryString != null)
        {
            queryArray = queryString.split(" ") || [];
        }

        AjaxQuery.getAllSearchedProducts(JSON.stringify(queryArray), this.state.page, this.state.orderBy)
            .then((value) =>
            {
                currentState.productsList = value;
                this.setState(currentState);
            })
        
    }

    async sortProducts()
    {
        let currentState = this.state;

        let orderBy = document.getElementById("searchOrderBySelect").value;

        let queryString = currentState.query;
        let queryArray = [];
        if (queryString != null)
        {
            queryArray = queryString.split(" ") || [];
        }
        currentState.orderBy = orderBy;
        currentState.productsList = await AjaxQuery.getAllSearchedProducts(JSON.stringify(queryArray), currentState.page, currentState.orderBy)
        this.setState(currentState);
    }

    renderProducts()
    {
        return this.state.productsList.map((obj) =>
        {
            return <Product key={obj.productID} addToCart={this.props.data.addToCart} data={obj} />
        });
    }

    render()
    {
        return (
            <div id="searchContainer" className="col-xs-12 container">
                <div id="searchTopBar" className="col-xs-12 container">
                    <SearchBar />
                    <div id="searchOrderBar" className="col-md-2 col-xs-12">
                        <select onChange={this.sortProducts.bind(this)} id="searchOrderBySelect" className="form-control">
                            <optgroup label="Order by">
                                <option value="relevance">Relevance</option>
                                <option value="toLower">Price (higher - lower)</option>
                                <option value="toHigher">Price (lower - higher)</option>
                            </optgroup>
                        </select>
                    </div>
                </div>
                <div id="searchMain" className="col-xs-12">
                    {this.renderProducts()}
                </div>
            </div>
            )
    }
}