import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link, NavLink, Redirect } from 'react-router-dom';
import { Product } from './Product';

import { User } from '../classess/User';

import './Store.css';

import $ from 'jquery';


export class Store extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            categoryID: 1,
            page: 1,
            productsList: [],
        }
        this.updateState(this.props);
    }

    private async updateState(nextProps)
    {
        let currentState = this.state;

        if (nextProps.match != null)
        {
            let categoryProps = nextProps.match.params.categoryID;
            if (categoryProps != null)
            {
                currentState.categoryID = categoryProps;
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
        currentState.productsList = await this.getProducts();
        this.setState(currentState);
    }

    async componentWillReceiveProps(nextProps)
    {
        await this.updateState(nextProps);  
    }

    async getProducts()
    {
        let result = [];
        await new Promise((resolve) =>
        {
            $.ajax(
                {
                    type: "GET",
                    url: "Product/GetProductsByPage",
                    data:
                    {
                        CategoryID: this.state.categoryID,
                        Page: this.state.page
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            result = JSON.parse(respond.products);
                        }
                        resolve();
                    }
                });
        })
        return result;
        
    }

    renderProducts()
    {
        return this.state.productsList.map((obj) =>
        {
            return <Product key={obj.ProductID} data={obj} />
        });
        
    }


    render()
    {
        let View = (
            <div id="storeContainer" className="container">
                <aside className="col-md-3">

                </aside>
                <div id="productsContainer" className="col-md-9">
                    {this.renderProducts()}
                </div>
            </div>
            
        );
        return View;
    }
}