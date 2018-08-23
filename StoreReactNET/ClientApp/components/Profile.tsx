﻿import * as React from 'react';
import * as ReactDOM from 'react-dom';

import './Profile.css';
import { Redirect } from 'react-router';
import ReactLoading from 'react-loading';

import Countries from 'react-select-country';
import MaterialIcon from 'material-icons-react';
import { AjaxQuery } from '../classess/AJAXQuery';
import { NavLink } from 'react-router-dom';

export class Profile extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            user: this.props.data.user,
            userDetails: null,
            userAddresses: null,
            loading: true
        };

    }
    componentWillMount()
    {
        let currentState = this.state;

        let p1 = AjaxQuery.getUserDetails().then((value) =>
        {
            currentState.userDetails = value;
        })
        let p2 = AjaxQuery.getUserAddresses().then((value) =>
        {
            currentState.userAddresses = value;
        })

        Promise.all([p1, p2]).then(() =>
        {
            currentState.loading = false;
            this.setState(currentState);
        })
    }
    componentWillReceiveProps(nextProps)
    {
        let currentState = this.state;
        if (nextProps.data.user != null)
        {
            currentState.user = nextProps.data.user;
        }
        this.setState(currentState);
    }

    static validateDetails(event)
    {
        let form = event.target;
        
        let reg = /^[a-zA-Z]{3,35}$/;

        let firstName = form["FirstName"];
        let lastName = form["LastName"];

        if (!reg.test(firstName.value))
        {
            firstName.classList.add("error");
            event.preventDefault();
        }
        if (!reg.test(lastName.value))
        {
            lastName.classList.add("error");
            event.preventDefault();
        }
        for (let i = 0; i < form.length; i++)
        {
            form[i].onclick = () =>
            {
                for (let j = 0; j < form.length; j++)
                {
                    form[j].classList.remove("error");
                }
            })
        }

    }
    static validateAddress(event)
    {
        let form = event.target;

        let reg = /^[a-zA-Z]+$/;
        let reg2 = /^[\d]{3,10}$/;

        let zipCode = form["ZipCode"];
        let zipCodeValue = parseInt(zipCode.value);
        let streetName = form["StreetName"]
        let homeNr = form["HomeNr"]
        let appratmentNr = form["AppartmentNr"]
        let city = form["City"];

        if (streetName.value.length < 3)
        {
            streetName.classList.add("error");
            event.preventDefault();
        }
        if (homeNr.value.length < 1)
        {
            homeNr.classList.add("error");
            event.preventDefault();
        }
        if (appartmentNr.value.length < 1)
        {
            appartmentNr.classList.add("error");
            event.preventDefault();
        }
        if (!reg2.test(zipCodeValue))
        {

            zipCode.classList.add("error");
            event.preventDefault();
        }
        if (!reg.test(city.value))
        {
            city.classList.add("error");
            event.preventDefault();
        }
        for (let i = 0; i < form.length; i++)
        {
            form[i].onclick = () =>
            {
                for (let j = 0; j < form.length; j++)
                {
                    form[j].classList.remove("error");
                }
            })
        }

        



    }
    openSelectedAddress(event)
    {
        let addressID = event.target.value;
        let form = document.getElementById("userAddressesForm");
        let address = null;

        this.state.userAddresses.some((obj) =>
        {
            if (obj.id == addressID)
            {
                address = obj;
                return;
            }
        })
        if (address != null)
        {
            form["Id"].value = address.id;
            form["StreetName"].value = address.streetName;
            form["HomeNr"].value = address.homeNr;
            form["AppartmentNr"].value = address.appartmentNr;
            form["ZipCode"].value = address.zipcode;
            form["City"].value = address.city;
            form["Country"].value = address.country;
            document.getElementById("countriesValue").value = address.country || "";
        }
        form.style.display = "block";
    }
    updateCountry(event)
    {
        document.forms["userAddressesForm"]["Country"].value = event.target.value;
    }
    renderAddressesList()
    {
        return this.state.userAddresses.map((obj) =>
        {
            return <option key={obj.id} value={obj.id}>{obj.streetName + " " + obj.homeNr +"/" + obj.appartmentNr + ", " + obj.city + ", "+ obj.country}</option>
        })
    }
    render()
    {
        if (this.state.loading)
        {
            return <ReactLoading type="bubbles" color="#333"/>
        }

        if (this.state.user != null)
        {
            let details = null;
            let addresses = null;
            if (this.state.userDetails != null)
            {
                details = (
                    <div className="profileSection col-xs-12">
                        <form method="POST" action="/Account/SetDetails" name="userDetailsForm" onSubmit={Profile.validateDetails}>

                            <label className="col-xs-12">
                                E-mail
                            </label>
                            <div className="col-xs-12 col-sm-4">
                                <input type="text" value={this.state.user.email} readOnly className="form-control" />
                            </div>

                            <label className="col-xs-12">
                                First Name
                            </label>
                            <div className="col-xs-12 col-sm-4">
                                <input type="text" name="FirstName" defaultValue={this.state.userDetails.firstName} className="form-control" />
                            </div>

                            <label className="col-xs-12">
                                Last Name
                            </label>
                            <div className="col-xs-12 col-sm-4">
                                <input type="text" name="LastName" defaultValue={this.state.userDetails.lastName} className="form-control" />
                            </div>

                            <label className="col-xs-12">
                                Date of birth
                            </label>
                            <div className="col-xs-12 col-sm-4">
                                <input type="date" name="DateOfBirth" defaultValue={this.state.userDetails.dateOfBirth} className="form-control" />
                            </div>

                            <div className="col-xs-12">
                                <button type="submit" className="btn btn-primary">Apply</button>
                            </div>

                        </form>
                    </div>
                )
            }
            else
            {
                details = (
                    <NavLink to={'/Account/Profile/AddDetails'}>
                        <div className="profileSectionNull col-xs-12 col-sm-6 col-sm-offset-3">
                            <p className="profileSectionNullText">
                                Please update your details!
                            </p>
                            <MaterialIcon icon="add_circle_outline" size="large" color="#333" />
                        </div>
                    </NavLink>
                );
            }

            if (this.state.userAddresses != null)
            {
                addresses = (
                    <div className="profileSection col-xs-12 container">
                        <div className="col-xs-10 col-sm-4">
                            <select defaultValue="null" className="form-control" onChange={this.openSelectedAddress.bind(this)}>

                                <option value="null" disabled>Choose address</option>
                                {this.renderAddressesList()}

                            </select>
                            
                        </div>
                        <NavLink to={'/Account/Profile/AddAddress'}>
                            <div className="col-xs-2" id="profileAddNewAddressIcon" onClick={this.openAddTab}>
                                 <MaterialIcon icon="add_circle_outline" color="#333" />
                            </div>
                        </NavLink>
                        <div id="userAddressesFormContainer" className="col-xs-12">
                            <form method="POST" action={"/Account/SetAddress"} id="userAddressesForm" name="userAddressesForm" onSubmit={Profile.validateAddress}>

                                <input type="text" name="Id" style={{ display: "none" }} />
                                <label className="col-xs-12">
                                    Street name
                                </label>
                                <div className="col-xs-12 col-sm-4">
                                    <input type="text" name="StreetName" className="form-control" />
                                </div>

                                <label className="col-xs-12">
                                    Home nr
                                </label>
                                <div className="col-sm-1 col-xs-4">
                                    <input type="text" name="HomeNr" className="form-control" />
                                </div>

                                <label className="col-xs-12">
                                    Appartment nr
                                </label>
                                <div className="col-sm-1 col-xs-4">
                                    <input type="text" name="AppartmentNr" className="form-control" />
                                </div>

                                <label className="col-xs-12">
                                    Zip code
                                </label>
                                <div className="col-sm-1 col-xs-4">
                                    <input type="text" name="ZipCode" className="form-control" />
                                </div>

                                <label className="col-xs-12">
                                    City
                                </label>
                                <div className="col-xs-12 col-sm-4">
                                    <input type="text" name="City" className="form-control" />
                                </div>

                                <label className="col-xs-12">
                                    Country
                                </label>
                                <div className="col-xs-12 col-sm-4">
                                    <input type="text" name="Country" style={{ display: "none" }} />
                                    <Countries id="countriesValue" className="form-control" empty="Select Country" onChange={this.updateCountry} />
                                </div>

                                <div className="col-xs-12">
                                    <button className="btn btn-primary"> Apply </button>
                                </div>

                            </form>
                        </div>
                            
                    </div>
                    )
            }
            else
            {
                addresses = (
                    <NavLink to={'/Account/Profile/AddAddress'}>
                        <div className="profileSectionNull col-xs-12 col-sm-6 col-sm-offset-3">
                            <p className="profileSectionNullText">
                                Please update your addresses list!
                            </p>
                            <MaterialIcon icon="add_circle_outline" size="large" color="#333" />
                        </div>
                    </NavLink>
                );
            }

            return (
                <div id="profileContainer" className="col-xs-12 container">
                    <div id="profileDetails" className="col-xs-12 container">
                        <h2 id="profileDetailsHeader" className="col-xs-12">
                            Profile Details
                        </h2>
                        <div id="profileDetailsList" className="col-xs-12 container">
                            {details}
                        </div>
                    </div>
                    <div id="profileAddresses" className="col-xs-12 container">
                        <h2 id="profileAddressesHeader" className="col-xs-12">
                            Your addresses
                        </h2>
                        <div id="profileAddressesList" className="col-xs-12 container">
                            {addresses}
                        </div>
                    </div>
                </div>

            );
        }
        return <Redirect to={"/"} />


    }
}
export class ProfileAddDetails extends React.Component
{
    constructor(props)
    {
        super(props);

        this.state =
        {
            user: this.props.data.user,
        }

    }
    componentWillReceiveProps(nextProps)
    {
        let currentState = this.state;
        if (nextProps.data.user != null)
        {
            currentState.user = nextProps.data.user;
        }
        this.setState(currentState);
    }
    render()
    {
        if (this.state.user == null)
        {
            return <Redirect to={'/'} />
        }
        return (
            <div id="profileAddDetailsContainer" className="col-xs-12 container">
                <form method="POST" action="/Account/SetDetails" name="AddDetailsForm" onSubmit={Profile.validateDetails}>

                    <label className="col-xs-12">
                        E-mail
                    </label>
                    <div className="col-xs-12 col-sm-4">
                        <input type="text" value={this.state.user.email} readOnly className="form-control" />
                    </div>

                    <label className="col-xs-12">
                        First Name
                            </label>
                    <div className="col-xs-12 col-sm-4">
                        <input type="text" name="FirstName" className="form-control" />
                    </div>

                    <label className="col-xs-12">
                        Last Name
                            </label>
                    <div className="col-xs-12 col-sm-4">
                        <input type="text" name="LastName" className="form-control" />
                    </div>

                    <label className="col-xs-12">
                        Date of birth
                            </label>
                    <div className="col-xs-12 col-sm-4">
                        <input type="date" name="DateOfBirth" className="form-control" />
                    </div>

                    <div className="col-xs-12">
                        <button type="submit" className="btn btn-primary">Add</button>
                    </div>

                </form>

            </div>
            )
    }
}
export class ProfileAddAddress extends React.Component
{
    constructor(props)
    {
        super(props);

        this.state =
        {
            user: this.props.data.user;
        }
    }
    componentWillReceiveProps(nextProps)
    {
        let currentState = this.state;
        if (nextProps.data.user != null)
        {
            currentState.user = nextProps.data.user;
        }
        this.setState(currentState);
    }
    updateCountry(event)
    {
        document.forms["AddAddressForm"]["Country"].value = event.target.value;
    }
    render()
    {
        if (this.state.user == null)
        {
            return <Redirect to={'/'} />
        }
        return (
            <div id="profileAddAddressContainer" className="col-xs-12 container">
                <form method="POST" action={"/Account/SetAddress"} id="profileAddAddressForm" name="AddAddressForm" onSubmit={Profile.validateAddress}>

                    <label className="col-xs-12">
                        Street name
                    </label>
                    <div className="col-xs-12 col-sm-4">
                        <input type="text" name="StreetName" className="form-control" />
                    </div>

                    <label className="col-xs-12">
                        Home nr
                    </label>
                    <div className="col-sm-1 col-xs-4">
                        <input type="text" name="HomeNr" className="form-control" />
                    </div>

                    <label className="col-xs-12">
                        Appartment nr
                    </label>
                    <div className="col-sm-1 col-xs-4">
                        <input type="text" name="AppartmentNr" className="form-control" />
                    </div>

                    <label className="col-xs-12">
                        Zip code
                    </label>
                    <div className="col-sm-1 col-xs-4">
                        <input type="text" name="ZipCode" className="form-control" />
                    </div>

                    <label className="col-xs-12">
                        City
                    </label>
                    <div className="col-xs-12 col-sm-4">
                        <input type="text" name="City" className="form-control" />
                    </div>

                    <label className="col-xs-12">
                        Country
                    </label>
                    <div className="col-xs-12 col-sm-4">
                        <input type="text" name="Country" style={{ display: "none" }} />
                        <Countries className="form-control" empty="Select Country" onChange={this.updateCountry} />
                    </div>
                    <div className="col-xs-12">
                        <button className="btn btn-primary"> Apply </button>
                    </div>

                </form>
            </div>
            )
    }
}

