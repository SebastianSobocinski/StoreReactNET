import * as React from 'react';
import './Popup.css';

export class Popup extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state =
        {
            title: this.props.title,
            type: this.props.type,
            show: true
        }
        this.close = this.close.bind(this);
    }
    close()
    {
        let currentState = this.state;
        currentState.show = false;
        this.setState(currentState);
    }
    render()
    {
        if (this.state.show)
        {
            let buttonType = null;
            if (this.state.type == "alertError")
            {
                buttonType = "btn-danger";
            }
            else if (this.state.type == "alertSuccess")
            {
                buttonType = "btn-success";
            }
            return (
                <div>
                    <div className="alertBackground">
                    </div>
                    <div className={"alertDiv " + this.state.type + " col-xs-12 container"}>
                        <p className="alertMessage col-xs-12">
                            {this.state.title}
                        </p>
                        <button className={"alertButton btn " + buttonType + " btn-lg col-xs-12" } onClick={this.close}>
                            OK
                    </button>
                    </div>
                </div>
            );
        }
        else
        {
            return null;
        }
       
    }
}