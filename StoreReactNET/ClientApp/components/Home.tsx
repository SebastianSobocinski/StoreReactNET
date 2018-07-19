import * as React from 'react';
import { RouteComponentProps } from 'react-router';

export class Home extends React.Component<RouteComponentProps<{}>, {}>
{
    constructor(props)
    {
        super(props);
        console.log(this.props);
    }


    public render()
    {
        return <h1>Home page!</h1>

    }
}
