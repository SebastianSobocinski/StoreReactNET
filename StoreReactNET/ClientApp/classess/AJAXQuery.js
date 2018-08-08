import $ from 'jquery';
import { User } from './User';

export class AjaxQuery
{
    static async getUserSession()
    {
        return new Promise((resolve) =>
        {
            let result;
            $.ajax(
                {
                    type: "GET",
                    url: "Session/GetUserSession",
                    success: (respond) =>
                    {
                        if (respond.isEstablished)
                        {
                            result = new User(respond.data)
                        }
                        resolve(result);
                    }
                });
        });
    }
    static async getAllCategories()
    {
        return new Promise((resolve) =>
        {
            let result = [];
            $.ajax(
                {
                    type: "GET",
                    url: "Product/GetAllCategories",
                    success: (respond) =>
                    {
                        if (respond.success)
                        {

                            result = JSON.parse(respond.categories);

                        }
                        resolve(result);
                    }
                });
        });
    }
    static async getProducts(categoryID, page, filtersString)
    {
        return new Promise((resolve) =>
        {
            let result = [];
            $.ajax(
                {
                    type: "GET",
                    url: "Product/GetProducts",
                    data:
                    {
                        CategoryID: categoryID,
                        Page: page,
                        Filters: filtersString
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            result = JSON.parse(respond.products);
                        }
                        resolve(result);
                    }
                });
        })
    }
    static async getAllFiltersFromCategory(categoryID)
    {
        return new Promise((resolve) =>
        {
            let result;
            $.ajax(
                {
                    type: "GET",
                    url: "Product/GetAllFiltersFromCategory",
                    data:
                    {
                        CategoryID: categoryID,
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            result = respond.filters
                        }
                        resolve(result);
                    }
                });
        })
    }
}