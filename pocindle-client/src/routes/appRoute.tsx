import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import { TestPage } from '../pages';

const AppRouter: React.FC = () => {
  return (
    <Switch>
      <Route exact path="/" component={TestPage} />
      <Route path="/test" component={TestPage} />
      <Redirect to="/" />
    </Switch>
  );
};

export default AppRouter;
