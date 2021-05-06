import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import { TestPage, AuthorizationPage } from '../pages';
import PrivateRoute from './privateRoute';

const AppRouter: React.FC = () => {
  return (
    <Switch>
      <Route path="/auth" component={AuthorizationPage} />
      <PrivateRoute exact path="/" component={TestPage} redirectPath="/auth" />
      <Redirect to="/" />
    </Switch>
  );
};

export default AppRouter;
