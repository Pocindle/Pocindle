import React, { useState } from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import {
  TestPage,
  AuthorizationPage,
  AuthorizationFinishedPage,
} from '../pages';
import PrivateRoute from './privateRoute';
import { postAuthRequest } from '../api/apiRequests';
import {
  getJwtFromLocalStorage,
  setJwtToLocalStorage,
} from '../utils/localStorage';

const AppRouter: React.FC = () => {
  const [jwt, setJwt] = useState<string>(getJwtFromLocalStorage() || '');

  const handleAuthorization = async () => {
    const { data } = await postAuthRequest();
    console.log(data);
    window.location.assign(data.redirectUrl);
  };

  const handleSuccessfulAuthorization = (jwtToken: string) => {
    setJwt(jwtToken);
    setJwtToLocalStorage(jwtToken);
    console.log(jwt);
  };

  return (
    <Switch>
      <Route
        path="/auth"
        render={() => <AuthorizationPage onAuthorize={handleAuthorization} />}
      />
      <Route
        path="/authorizationFinished/:requestToken"
        render={() => (
          <AuthorizationFinishedPage
            onSuccessfulAuthorization={handleSuccessfulAuthorization}
          />
        )}
      />
      <PrivateRoute
        exact
        path="/"
        isAuthorized={!!jwt}
        component={TestPage}
        redirectPath="/auth"
      />
      <Redirect to="/" />
    </Switch>
  );
};

export default AppRouter;
