import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import {
  TestPage,
  AuthorizationPage,
  AuthorizationFinishedPage,
} from '../pages';
import PrivateRoute from './privateRoute';
import { postAuthRequest } from '../api/apiRequests';

const AppRouter: React.FC = () => {
  const handleAuthorization = async () => {
    const { data } = await postAuthRequest();
    console.log(data);
    window.location.assign(data.redirectUrl);
  };

  return (
    <Switch>
      <Route
        path="/auth"
        render={() => <AuthorizationPage onAuthorize={handleAuthorization} />}
      />
      <Route
        path="/authorizationFinished/:requestToken"
        component={AuthorizationFinishedPage}
      />
      <PrivateRoute exact path="/" component={TestPage} redirectPath="/auth" />
      <Redirect to="/" />
    </Switch>
  );
};

export default AppRouter;
