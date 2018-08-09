import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link, NavLink, Redirect } from 'react-router-dom';
import MaterialIcon, { colorPalette } from 'material-icons-react';

import { Product } from './Product';
import { User } from '../classess/User';
import { AjaxQuery } from '../classess/AjaxQuery';

import './Store.css';
import '../visuals/StoreVisuals.js';

import $ from 'jquery';
import { NavBar } from './NavBar';

export class Store extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            categoryID: 1,
            categoryName: "",
            page: 1,
            allCategories: [],
            productsList: [],
            productFilters: [],
            selectedFilters: null
        }
        this.updateState(this.props);
    }
    async updateState(nextProps)
    {
        let currentState = this.state;

        if (nextProps.match != null)
        {
            let categoryProps = nextProps.match.params.categoryID;
            if (categoryProps != null)
            {
                currentState.categoryID = categoryProps;
                currentState.selectedFilters = null;
            }

            let pageProps = nextProps.match.params.page;
            if (pageProps != null)
            {
                currentState.page = pageProps;
            }
        }
        else
        {
            currentState.categoryID = 1;
            currentState.page = 1;
        }

        currentState.productsList = await AjaxQuery.getProducts(currentState.categoryID, currentState.page, JSON.stringify(currentState.selectedFilters));
        currentState.allCategories = await AjaxQuery.getAllCategories();
        currentState.categoryName = await this.getSelectedCategoryName(currentState);
        currentState.productFilters = await AjaxQuery.getAllFiltersFromCategory(currentState.categoryID);
        this.setState(currentState);
        
    }
    async componentWillReceiveProps(nextProps)
    {
        this.updateState(nextProps);
    }

    async getSelectedCategoryName(state)
    {
        return new Promise((resolve) =>
        {
            state.allCategories.forEach((el) =>
            {
                if (el.Id == state.categoryID)
                {
                    resolve(el.CategoryName);
                }
            })
        })
    }
    async setFilters()
    {
        
        let currentState = this.state;
        currentState.selectedFilters = [];  
        let maxPriceValue = parseFloat(document.getElementById("sidePricePickerMax").value.replace(",", "."));
        if (!isNaN(maxPriceValue))
        {
            let entry =
                {
                    type: "maxPrice",
                    value: [maxPriceValue]
                };
            currentState.selectedFilters.push(entry);
        }
        
        let allFilters = currentState.productFilters;
        allFilters.forEach((el) => currentState.selectedFilters.push({type: el.type, value: []}))
        let checkboxes = Array.from(document.getElementsByClassName('filterItemCheck'));
        checkboxes.forEach((el) =>
        {
            if (el.checked)
            {
                let entry = el.value;
                let type = el.dataset.filterType;
                this.checkAndAdd(type, entry, currentState.selectedFilters);
            }
        })
        document.getElementById("orderBySelect").value = "relevance";
        currentState.page = 1;
        currentState.productsList = await AjaxQuery.getProducts(currentState.categoryID, currentState.page, JSON.stringify(currentState.selectedFilters));
        this.setState(currentState);
        
    }
    private checkAndAdd(type, entry, array)
    {
        
        array.forEach((el) =>
        {
            if (el.type == type)
            {
                el.value.push(entry);
                return;
            }
        })

    }

    async sortProducts()
    {
        let currentState = this.state;
        let initProducts = currentState.productsList;
        let sortedProducts;

        let orderType = document.getElementById("orderBySelect").value;
        switch (orderType)
        {
            case 'relevance':
                sortedProducts = initProducts.sort((a, b) => { return b.ProductID - a.ProductID });
                break;
            case 'toLower':
                sortedProducts = initProducts.sort((a, b) => { return b.ProductPrice - a.ProductPrice })
                break;
            case 'toHigher':
                sortedProducts = initProducts.sort((a, b) => { return a.ProductPrice - b.ProductPrice })
                break;
            default:
                sortedProducts = initProducts;
                break;
        }
        currentState.productsList = sortedProducts;
        this.setState(currentState);
    }

    renderProducts()
    {

        return this.state.productsList.map((obj) =>
        {
            return <Product key={obj.ProductID} data={obj} />
        });
        
    }
    renderCategories()
    {
        return this.state.allCategories.map((obj) =>
        {
            return <li key={obj.Id}><NavLink to={'/Store/'+obj.Id}> {obj.CategoryName} </NavLink></li>
        })
    }
    renderFilters()
    {
        let filters = this.state.productFilters;
        if (filters != null)
        {
            return filters.map((obj) =>
            {
                let View;
                View =
                    (
                        <div key={obj.type} className="sideFilter col-xs-12 container">
                            <a className="sideFilterHeader col-xs-12">{obj.displayName}</a>
                            {
                                obj.variables.map((el) =>
                                {
                                    return (<div key={el} className="filterItem col-xs-12"><input className="filterItemCheck form-check-input" type="checkbox" value={el} data-filter-type={obj.type} /><p className="filterItemText">{el}</p></div>)
                                }
                                )}
                        </div>
                    )
                return View;
            });
        }
        

       
            
        
        /*return toDraw.map((obj) =>
        {
            let entry;
            if (obj.display == "header")
            {
                return <a key={obj.value} className="sideHeader col-xs-12">{obj.value}</a>
            }
            else
            {
                return (<div key={obj.value} className="filterItem col-xs-12"><input className="filterItemCheck form-check-input" type="checkbox" value={obj.value} data-filter-type={obj.type} /><p className="filterItemText">{obj.value}</p></div>)
            }


        });
        */

    }

    render()
    {
        let View = (

            <div id="storeContainer" className="container">
                <div id="topBar" className="col-md-12">
                    <div id="searchBar" className="col-md-5 col-xs-12">
                        <form action="/Search/" method="GET">
                            <button id="searchBarButton"><MaterialIcon icon="search" /></button>
                            <input type="text" id="searchBarInput" className="form-control input-lg" name="query" placeholder="Search products..." />
                        </form>
                    </div>
                    <div id="bottomBar" className="col-md-12 container">
                        <div id="categoryBar" className="col-sm-10 col-xs-12"><NavLink to={'/Store'}>Store</NavLink> > <NavLink to={'/Store/' + this.state.categoryID}> {this.state.categoryName} </NavLink></div>
                        <div id="orderBar" className="col-sm-2 col-xs-12">
                            <select onChange={this.sortProducts.bind(this)} id="orderBySelect" className="form-control">
                                <optgroup label="Order by">
                                    <option value="relevance">Relevance</option>
                                    <option value="toLower">Price (higher - lower)</option>
                                    <option value="toHigher">Price to (lower to higher)</option>
                                </optgroup>
                            </select>
                        </div>
                    </div>
                </div>
                <aside className="col-md-3 container">
                    <div id="sideCategoryTitle" className="col-md-12 container">
                        <a id="sideCategoryTitleHeader" className="sideHeader"> Categories </a>
                    </div>
                    <ul id="sideCategoryList" className="col-md-12 container">
                        {this.renderCategories()}
                    </ul>
                    <div id="sidePricePicker" className="col-md-12 container">
                        <a className="sideHeader col-xs-12"> Max price </a>
                        <input type="text" id="sidePricePickerMax" className="form-control input-sm" />
                    </div>
                    <div id="sideAllFilters" className="col-md-12 container">
                        {this.renderFilters()}
                    </div>
                    <button id="updateFiltersButton" className="btn btn-warning col-xs-8 col-xs-offset-2" onClick={this.setFilters.bind(this)}> Filter </button>
                </aside>
                <div id="productsContainer" className="col-md-9">
                    {this.renderProducts()}
                </div>

            </div>
            
        );
        return View;
    }
}