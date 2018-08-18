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
            return (
                <div>
                    <div className="alertBackground">
                    </div>
                    <div className="alertDiv col-xs-12 container">
                        <p className="alertMessage col-xs-12">
                            {this.state.title}
                        </p>
                        <button className="alertButton btn btn-danger btn-lg col-xs-12" onClick={this.close}>
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