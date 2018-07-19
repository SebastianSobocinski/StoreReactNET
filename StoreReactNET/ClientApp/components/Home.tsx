import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { User } from '../classess/User';


export class Home extends React.Component
{
    constructor(props)
    {
        super(props);
    }


    public render()
    {
        return <h1>Home page!</h1>

    }
}
