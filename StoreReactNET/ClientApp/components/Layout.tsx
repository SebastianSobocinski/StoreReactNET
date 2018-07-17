import * as React from 'react';
import { NavBar } from './NavBar';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component
{
    public render()
    {
        return <div className='container-fluid'>
            <NavBar />
            {this.props.children}
        </div>

    }
}
