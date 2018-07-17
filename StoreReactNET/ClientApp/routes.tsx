import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

export const routes = <Layout>
    <Route path='/' exact component={ Home } />

</Layout>;
