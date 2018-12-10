import * as React from 'react';
import MaterialIcon from 'material-icons-react';

import './SearchBar.css';


export class SearchBar extends React.Component
{

    constructor()
    {
        super();
    }
    render()
    {
        return (
            <div id="searchBar" className="col-md-5 col-xs-12">
                <form action="/Search/" method="GET">
                    <button id="searchBarButton"><MaterialIcon icon="search" /></button>
                    <input type="text" id="searchBarInput" className="form-control input-lg" name="query" placeholder="Search products..." />
                </form>
            </div>
            )
    }
}