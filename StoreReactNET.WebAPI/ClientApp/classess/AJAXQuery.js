import $ from 'jquery';

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
                            result = respond.user;
                        }
                        resolve(result);
                    }
                });
        });
    
    }
    static async getCartSession()
    {
        return new Promise((resolve) =>
        {
            let result;
            $.ajax(
                {
                    type: "GET",
                    url: "Session/GetCartSession",
                    success: (respond) =>
                    {
                        if (respond.isEstablished)
                        {
                            result = respond.cart;
                        }
                        else 
                        {
                            result = [];
                        }
                        resolve(result);
                    }
                });
        });
    }
    static async addToCartSession(productID)
    {
        return new Promise((resolve) =>
        {
            let result;
            $.ajax(
                {
                    type: "POST",
                    url: "Session/AddToCartSession",
                    data:
                    {
                        ProductID: productID
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            result = respond.cart
                        }
                        alert(respond.message)

                        resolve(result);
                    }
                });
        });
    }
    static async updateCartSession(cartArray)
    {
        return new Promise((resolve) =>
        {
            let result;
            $.ajax(
                {
                    type: "POST",
                    url: "Session/UpdateCartSession",
                    data:
                    {
                        Cart: JSON.stringify(cartArray)
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            result = respond.cart;
                        }
                        alert(respond.message);
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
    static async getProducts(categoryID, page, filtersString, orderBy)
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
                        Filters: filtersString,
                        OrderBy: orderBy
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            result = respond.products || [];
                        }
                        else
                        {
                            result = [];
                        }
                        resolve(result);
                    }
                });
        })
    }
    static async getClickedProduct(productID)
    {
        return new Promise((resolve) =>
        {
            let result = null;
            $.ajax(
                {
                    type: "GET",
                    url: "Product/GetClickedProduct",
                    data:
                    {
                        ProductID: productID,
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            result = respond.product || null;
                        }
                        else
                        {
                            result = null;
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
    static async getAllSearchedProducts(query, page, orderBy)
    {
        return new Promise((resolve) =>
        {
            let result;
            $.ajax(
                {
                    type: "GET",
                    url: "Product/GetSearchedProducts",
                    data:
                    {
                        Query: query,
                        Page: page,
                        OrderBy: orderBy
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            result = respond.products || [];
                        }
                        else
                        {
                            result = [];
                        }
                        resolve(result);
                    }
                });
        });
    }

    static async getUserDetails()
    {
        return new Promise((resolve) =>
        {
            let result = null;
            $.ajax(
                {
                    type: "GET",
                    url: "Account/GetUserDetails",
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            let time = respond.userDetails.dateOfBirth.split("T")[0];
                            respond.userDetails.dateOfBirth = time;
                            result = respond.userDetails
                        }
                        resolve(result);
                    }
                });
        });
    }
    static async getUserAddresses()
    {
        return new Promise((resolve) =>
        {
            let result = null;
            $.ajax(
                {
                    type: "GET",
                    url: "Account/GetUserAddresses",
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            if (respond.userAddresses.length > 0)
                            {
                                result = respond.userAddresses
                            }
                            else
                            {
                                result = null
                            }

                        }
                        resolve(result);
                    }
                });
        });
    }
    static async getUserLatestOrders()
    {
        return new Promise((resolve) =>
        {
            let result = null;
            $.ajax(
                {
                    type: "GET",
                    url: "Account/GetUserLatestOrders",
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            if (respond.userOrders.length > 0)
                            {
                                result = respond.userOrders
                            }
                            else
                            {
                                result = null
                            }

                        }
                        resolve(result);
                    }
                });
        });
    }
    static async removeUserAddress(id)
    {
        return new Promise((resolve) =>
        {
            $.ajax(
                {
                    type: "POST",
                    url: "Account/RemoveUserAddress",
                    data: 
                    {
                        Id: id
                    },
                    success: (respond) =>
                    {
                        if (respond.success)
                        {
                            window.location = window.location.origin;
                            resolve();

                        }
                        else
                        {
                            alert("Something went wrong");
                            resolve();
                        }
                    }
                });
        });
    }
}